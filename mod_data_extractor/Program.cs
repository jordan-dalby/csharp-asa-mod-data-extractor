﻿using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Versions;
using CUE4Parse.FileProvider.Vfs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using CommandLine;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Objects.Properties;
using CUE4Parse.UE4.AssetRegistry;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using CUE4Parse.UE4.Objects.UObject;
using Serilog;
using Serilog.Events;
using CUE4Parse.Compression;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace UtocDumper
{
    internal class Program
    {

        public class Options
        {
            [Option('i', "input", Required = true, HelpText = "The directory with all of the required game files")]
            public string InputDirectory { get; set; } = "";

            [Option('o', "output", Required = true, HelpText = "The directory to output files to")]
            public string OutputDirectory { get; set; } = "";

            [Option('d', "debug", Required = false, HelpText = "Set debug mode to true or false.", Default = false)]
            public bool Debug { get; set; } = false;

            [Option('t', "targets", Required = false, HelpText = "Optional specific directories to extract data from, leave blank to extract everything")]
            public IEnumerable<string> Targets { get; set; } = new List<string>();
            
            [Option('f', "file-types", Required = false, HelpText = "Optional argument to specify filetypes to search for", Default = new string[] { "uasset" })]
            public IEnumerable<string> FileTypes { get; set; } = ["uasset"];
            
            [Option('v', "version", Required = false, HelpText = "The UE version to use for parsing, see https://github.com/FabianFG/CUE4Parse/blob/master/CUE4Parse/UE4/Versions/EGame.cs", Default = EGame.GAME_ARKSurvivalAscended)]
            public EGame UEVersion { get; set; } = EGame.GAME_UE5_2;
        }

        private static JsonMergeSettings mergeSettings = new JsonMergeSettings() { MergeArrayHandling = MergeArrayHandling.Replace };
        private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings 
        { 
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter> { new FloatConverter() }
        };

        static void Main(string[] args)
        {
            // Very low level logging, only enable if necessary
            /*
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger();
            */

            var options = Parser.Default.ParseArguments<Options>(args);
            if (options.Errors.Any())
            {
                Console.WriteLine("There were errors parsing the command");
                return;
            }

            string inputPath = options.Value.InputDirectory;
            string outputPath = options.Value.OutputDirectory;

            List<string> targets = [.. options.Value.Targets];

            if (OodleHelper.DownloadOodleDll("./oo2core_9_win64.dll"))
            {
                OodleHelper.Initialize("./oo2core_9_win64.dll");
            }
            else
            {
                Console.WriteLine("Unable to initialise Oodle");
                return;
            }

            // Create the default file provider from the base directory, basically does everything for you
            AbstractVfsFileProvider provider = new DefaultFileProvider(directory: inputPath, searchOption: SearchOption.AllDirectories, versions: new VersionContainer(options.Value.UEVersion), pathComparer: StringComparer.OrdinalIgnoreCase);

            // Initialize and mount the file provider
            provider.Initialize();
            provider.Mount();

            foreach (var entry in provider.Files.Values)
            {
                // if the extension of the entry is not one we are looking for, skip this iteration
                if (!options.Value.FileTypes.Any(extension => entry.Extension.Contains(extension, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                // if the targets list has any values
                if (targets.Any())
                {
                    // evaluate if the current entry is a valid target, otherwise skip this iteration
                    bool isValidTarget = targets.Any(target => Regex.IsMatch(entry.Path, target));
                    if (!isValidTarget)
                    {
                        continue;
                    }
                }

                // debug
                if (options.Value.Debug)
                {
                    Console.WriteLine(entry.Path);
                }

                try
                {
                    // Concat output file path
                    string filePath = Path.Combine(outputPath, entry.PathWithoutExtension + ".json");
                    // Create missing directories if not exists
                    string? directoryName = new FileInfo(filePath).DirectoryName;
                    if (directoryName == null)
                    {
                        Console.WriteLine($"Failed to create directory {filePath} because the DirectoryName was null");
                        continue;
                    }
                    Directory.CreateDirectory(directoryName);

                    // Load all of the objects from the uasset file
                    if (entry.Extension == "bin")
                    {
                        if (provider.TryCreateReader(entry.Path, out var archive))
                        {
                            var registry = new FAssetRegistryState(archive);
                            // Write
                            File.WriteAllText(filePath, JsonConvert.SerializeObject(registry, jsonSettings));
                        }
                        continue;
                    }

                    var exports = provider.LoadPackage(entry.Path).GetExports();

                    // UObjects themselves actually on contain the changes from the parent, so to get all values we need
                    // to get values from the parents, as deep as it goes, the following handles that
                    List<JObject> data = new List<JObject>();
                    foreach (UObject export in exports)
                    {
                        List<UObject> makeup = GetClassMakeup(export);
                        JObject fullClassData = CombineClassData(makeup);
                        if (makeup.First().ExportType == "DinoCharacterStatusComponent_BP_C")
                        {
                            List<PrimalDinoStat> stats = [];
                            stats.AddRange(GetStatsFromPrimalDino(export));
                            JObject? properties;
                            if (!fullClassData.ContainsKey("Properties"))
                            {
                                fullClassData.Add("Properties", new JObject());
                            }
                            properties = (JObject)fullClassData["Properties"];
                            foreach (PrimalDinoStat stat in stats)
                            {
                                Dictionary<string, object> props = new Dictionary<string, object>
                                {
                                    { "BaseValue", stat.Value },
                                    { "WildPerLevel", stat.WildPerLevel },
                                    { "TamedPerLevel", stat.TamedPerLevel },
                                    { "TamingReward", stat.TamingReward },
                                    { "EffectivenessReward", stat.EffectivenessReward },
                                    { "MaxGainedPerLevelUpValueIsPercent", stat.MaxGainedPerLevelUpValueIsPercent },
                                    { "CanLevelUpValue", stat.CanLevelUpValue },
                                    { "DontUseValue", stat.DontUseValue },
                                };
                                properties.Add(stat.StatName.ToString(), JObject.FromObject(props));
                            }
                        }
                        else if (makeup.First().ExportType.Contains("PrimalItem"))
                        {
                            List<PrimalItemStat> stats = [];
                            stats.AddRange(GetStatsFromPrimalItem(export));
                            JObject? properties;
                            if (!fullClassData.ContainsKey("Properties"))
                            {
                                fullClassData.Add("Properties", new JObject());
                            }
                            properties = (JObject)fullClassData["Properties"];
                            foreach (PrimalItemStat stat in stats)
                            {
                                Dictionary<string, object> props = new Dictionary<string, object>
                                {
                                    { "Used", stat.Used },
                                    { "CalculateAsPercent", stat.CalculateAsPercent },
                                    { "DisplayAsPercent", stat.DisplayAsPercent },
                                    { "RequiresSubmerged", stat.RequiresSubmerged },
                                    { "PreventIfSubmerged", stat.PreventIfSubmerged },
                                    { "HideStatFromTooltip", stat.HideStatFromTooltip },
                                    { "DefaultModifierValue", stat.DefaultModifierValue },
                                    { "RandomizerRangeOverride", stat.RandomizerRangeOverride },
                                    { "RandomizerRangeMultiplier", stat.RandomizerRangeMultiplier },
                                    { "StateModifierScale", stat.StateModifierScale },
                                    { "InitialValueConstant", stat.InitialValueConstant },
                                    { "RatingValueMultiplier", stat.RatingValueMultiplier },
                                    { "AbsoluteMaxValue", stat.AbsoluteMaxValue },
                                };
                                properties.Add(stat.StatName.ToString(), JObject.FromObject(props));
                            }
                        }
                        data.Add(fullClassData);
                    }
                    // Serialize to string and write
                    File.WriteAllText(filePath, JsonConvert.SerializeObject(data, jsonSettings));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception caused by {entry.Name} \nException: {ex.StackTrace}");
                }
            }
        }

        public static List<UObject> GetClassMakeup(UObject export)
        {
            List<UObject> result = new List<UObject>() { export };
            // If the object has no parent, just return the result
            if (export.Template == null)
                return result;
            // Get the parent object
            ResolvedObject currentObject = export.Template;
            while (currentObject != null)
            {
                // Get the actual parent object
                UObject parent;
                if (currentObject.TryLoad(out parent))
                {
                    // Add it to the result
                    result.Add(parent);
                    // If this parent has no parent, it's the highest, so break
                    if (parent.Template == null)
                        break;
                    // Set the current object
                    currentObject = parent.Template;
                }
            }
            // Need to go in reverse order, starting with the highest parent, finishing with the object we care about
            result.Reverse();
            return result;
        }

        private static JObject CombineClassData(List<UObject> classData)
        {
            // If there's no class data, just return an empty object
            if (classData.Count == 0)
                return new JObject();

            // Create a JObject from the first object in the array, then loop everything else
            JObject result = JObject.FromObject(classData[0]);
            for (int i = 1; i < classData.Count; i++)
            {
                // Get the object & convert to JObject
                UObject obj = classData[i];
                JObject jsonObj = JObject.FromObject(obj);

                // Merge into the result
                result.Merge(jsonObj, mergeSettings);
            }
            return result;
        }

        private static List<PrimalDinoStat> GetStatsFromPrimalDino(UObject export)
        {
            List<UObject> makeup = GetClassMakeup(export);
            List<PrimalDinoStat> stats = PrimalDinoStat.BaseStats.Select(stat => new PrimalDinoStat
            {
                StatName = stat.StatName,
                Value = stat.Value,
                WildPerLevel = stat.WildPerLevel,
                TamedPerLevel = stat.TamedPerLevel,
                TamingReward = stat.TamingReward,
                EffectivenessReward = stat.EffectivenessReward,
                MaxGainedPerLevelUpValueIsPercent = stat.MaxGainedPerLevelUpValueIsPercent,
                CanLevelUpValue = stat.CanLevelUpValue,
                DontUseValue = stat.DontUseValue
            }).ToList();
            // Goes in backwards order so overrides should override
            foreach (UObject parentClass in makeup)
            {
                foreach (FPropertyTag tag in parentClass.Properties)
                {
                    if (tag.Tag == null)
                        continue;

                    PrimalDinoStatName statName = (PrimalDinoStatName)tag.ArrayIndex;
                    foreach (PrimalDinoStat stat in stats)
                    {
                        if (stat.StatName != statName)
                        {
                            continue;
                        }

                        if (tag.Tag.GetType() == typeof(FloatProperty))
                        {
                            float val = tag.Tag.GetValue<float>();
                            switch (tag.Name.PlainText)
                            {
                                case "MaxStatusValues":
                                    stat.Value = val;
                                    break;
                                case "AmountMaxGainedPerLevelUpValueTamed":
                                    stat.TamedPerLevel = val;
                                    break;
                                case "AmountMaxGainedPerLevelUpValue":
                                    stat.WildPerLevel = val;
                                    break;
                                case "TamingMaxStatMultipliers":
                                    stat.EffectivenessReward = val;
                                    break;
                                case "TamingMaxStatAdditions":
                                    stat.TamingReward = val;
                                    break;
                            }
                        }
                        else if (tag.Tag.GetType() == typeof(ByteProperty))
                        {
                            byte val = tag.Tag.GetValue<byte>();
                            switch (tag.Name.PlainText)
                            {                                
                                case "MaxGainedPerLevelUpValueIsPercent":
                                    stat.MaxGainedPerLevelUpValueIsPercent = val == 1;
                                    break;
                                case "CanLevelUpValue":
                                    stat.CanLevelUpValue = val == 1;
                                    break;
                                case "DontUseValue":
                                    stat.DontUseValue = val == 1;
                                    break;
                            }
                        }
                    }
                }
            }
            return stats;
        }

        private static List<PrimalItemStat> GetStatsFromPrimalItem(UObject export)
        {
            List<UObject> makeup = GetClassMakeup(export);
            List<PrimalItemStat> stats = PrimalItemStat.BaseStats.Select(stat => new PrimalItemStat
            {
                StatName = stat.StatName,
                Used = stat.Used,
                CalculateAsPercent = stat.CalculateAsPercent,
                DisplayAsPercent = stat.DisplayAsPercent,
                RequiresSubmerged = stat.RequiresSubmerged,
                PreventIfSubmerged = stat.PreventIfSubmerged,
                HideStatFromTooltip = stat.HideStatFromTooltip,
                DefaultModifierValue = stat.DefaultModifierValue,
                RandomizerRangeOverride = stat.RandomizerRangeOverride,
                RandomizerRangeMultiplier = stat.RandomizerRangeMultiplier,
                StateModifierScale = stat.StateModifierScale,
                InitialValueConstant = stat.InitialValueConstant,
                RatingValueMultiplier = stat.RatingValueMultiplier,
                AbsoluteMaxValue = stat.AbsoluteMaxValue
            }).ToList();
            // Goes in backwards order so overrides should override
            foreach (UObject parentClass in makeup)
            {
                foreach (FPropertyTag tag in parentClass.Properties)
                {
                    if (tag.Tag == null)
                        continue;

                    PrimalItemStatName statName = (PrimalItemStatName)tag.ArrayIndex;
                    foreach (PrimalItemStat stat in stats)
                    {
                        if (stat.StatName != statName)
                        {
                            continue;
                        }

                        if (tag.Tag.GetValue<FScriptStruct>() != null)
                        {
                            FScriptStruct data = tag.Tag.GetValue<FScriptStruct>();
                            if (data.StructType.GetType() != typeof(FStructFallback))
                                continue;

                            FStructFallback fallback = (FStructFallback)data.StructType;
                            foreach (FPropertyTag fallbackTag in fallback.Properties)
                            {
                                if (fallbackTag.Tag == null)
                                    continue;

                                if (fallbackTag.Tag.GetType() == typeof(FloatProperty))
                                {
                                    float val = fallbackTag.Tag.GetValue<float>();
                                    switch (fallbackTag.Name.PlainText)
                                    {
                                        case "RandomizerRangeMultiplier":
                                            stat.RandomizerRangeMultiplier = val;
                                            break;
                                        case "StateModifierScale":
                                            stat.StateModifierScale = val;
                                            break;
                                        case "InitialValueConstant":
                                            stat.InitialValueConstant = val;
                                            break;
                                        case "RatingValueMultiplier":
                                            stat.RatingValueMultiplier = val;
                                            break;
                                        case "AbsoluteMaxValue":
                                            stat.AbsoluteMaxValue = val;
                                            break;
                                    }
                                }
                                else if (fallbackTag.Tag.GetType() == typeof(BoolProperty))
                                {
                                    bool val = fallbackTag.Tag.GetValue<bool>();
                                    switch (fallbackTag.Name.PlainText)
                                    {
                                        case "bUsed":
                                            stat.Used = val;
                                            break;
                                        case "bCalculateAsPercent":
                                            stat.CalculateAsPercent = val;
                                            break;
                                        case "bDisplayAsPercent":
                                            stat.DisplayAsPercent = val;
                                            break;
                                        case "bRequiresSubmerged":
                                            stat.RequiresSubmerged = val;
                                            break;
                                        case "bPreventIfSubmerged":
                                            stat.PreventIfSubmerged = val;
                                            break;
                                        case "bHideStatFromTooltip":
                                            stat.HideStatFromTooltip = val;
                                            break;
                                    }
                                }
                                else if (fallbackTag.Tag.GetType() == typeof(IntProperty))
                                {
                                    int val = fallbackTag.Tag.GetValue<int>();
                                    switch (fallbackTag.Name.PlainText)
                                    {
                                        case "DefaultModifierValue":
                                            stat.DefaultModifierValue = val;
                                            break;
                                        case "RandomizerRangeOverride":
                                            stat.RandomizerRangeOverride = val;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return stats;
        }
    }
}
