using CUE4Parse.FileProvider.Objects;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.UE4.AssetRegistry;
using CUE4Parse.UE4.Assets.Exports;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModDataExtractor
{
    public class Parser
    {
        private readonly AbstractVfsFileProvider Provider;

        public Parser(AbstractVfsFileProvider provider)
        {
            Provider = provider;
        }

        public string ParseToJson(GameFile gameFile)
        {
            switch (gameFile.Extension)
            {
                case "bin":
                    if (Provider.TryCreateReader(gameFile.Path, out var archive))
                    {
                        var registry = new FAssetRegistryState(archive);
                        return JsonConvert.SerializeObject(registry, Formatting.Indented);
                    }
                    break;
                default:
                    var exports = Provider.LoadAllObjects(gameFile.Path);

                    // UObjects themselves actually on contain the changes from the parent, so to get all values we need
                    // to get values from the parents, as deep as it goes, the following handles that
                    List<JObject> data = new List<JObject>();
                    foreach (UObject export in exports)
                    {
                        List<UObject> makeup = Helpers.GetClassMakeup(export);
                        JObject fullClassData = Helpers.CombineClassData(makeup);
                        if (makeup.First().ExportType == "DinoCharacterStatusComponent_BP_C")
                        {
                            List<PrimalDinoStat> stats = [];
                            stats.AddRange(PrimalDinoStat.GetStatsFromPrimalDino(export));
                            JObject? properties;
                            if (!fullClassData.ContainsKey("Properties"))
                            {
                                fullClassData.Add("Properties", new JObject());
                            }
                            properties = fullClassData["Properties"] as JObject;
                            foreach (PrimalDinoStat stat in stats)
                            {
                                Dictionary<string, object> props = new Dictionary<string, object>
                                {
                                    { "BaseValue", stat.Value },
                                    { "WildPerLevel", stat.WildPerLevel },
                                    { "TamedPerLevel", stat.TamedPerLevel },
                                    { "TamingReward", stat.TamingReward },
                                    { "EffectivenessReward", stat.EffectivenessReward },
                                    { "MaxGainedPerLevelUpIsPercent", stat.MaxGainedPerLevelUpValueIsPercent },
                                    { "CanLevelUpValue", stat.CanLevelUpValue },
                                    { "DontUseValue", stat.DontUseValue },
                                };
                                properties?.Add(stat.StatName.ToString(), JObject.FromObject(props));
                            }
                        }
                        else if (makeup.First().ExportType.Contains("PrimalItem"))
                        {
                            List<PrimalItemStat> stats = [];
                            stats.AddRange(PrimalItemStat.GetStatsFromPrimalItem(export));
                            JObject? properties;
                            if (!fullClassData.ContainsKey("Properties"))
                            {
                                fullClassData.Add("Properties", new JObject());
                            }
                            properties = fullClassData["Properties"] as JObject;
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
                                properties?.Add(stat.StatName.ToString(), JObject.FromObject(props));
                            }
                        }
                        data.Add(fullClassData);
                    }
                    return JsonConvert.SerializeObject(data, Formatting.Indented);
            }
            return "{}";
        }
    }
}