using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Versions;
using CUE4Parse.FileProvider.Vfs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using CommandLine;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets;

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

            [Option('b', "badfile", Required = false, HelpText = "Location of a file containing line-by-line items to skip, regex enabled")]
            public string BadFileDirectory { get; set; } = "";

            [Option('d', "debug", Required = false, HelpText = "Set debug mode to true or false.", Default = false)]
            public bool Debug { get; set; } = false;

            [Option('t', "targets", Required = false, HelpText = "Optional specific directories to extract data from, leave blank to extract everything")]
            public IEnumerable<string> Targets { get; set; } = new List<string>();
            
            [Option('f', "file-types", Required = false, HelpText = "Optional argument to specify filetypes to search for", Default = new string[] { "uasset" })]
            public IEnumerable<string> FileTypes { get; set; } = ["uasset"];
        }

        static void Main(string[] args)
        {
            var options = Parser.Default.ParseArguments<Options>(args);
            if (options.Errors.Any())
            {
                Console.WriteLine("There were errors parsing the command");
                return;
            }

            string inputPath = options.Value.InputDirectory;
            string outputPath = options.Value.OutputDirectory;
            string badFilePath = options.Value.BadFileDirectory;

            List<string> badFiles = new List<string>();
            if (File.Exists(badFilePath))
            {
                badFiles.AddRange(File.ReadAllLines(badFilePath).ToList());
            }
            else
            {
                if (badFilePath.Length > 0)
                {
                    Console.WriteLine($"Couldn't find badfile at {badFilePath}");
                    return;
                }
            }

            List<string> targets = [.. options.Value.Targets];

            // Create the default file provider from the base directory, basically does everything for you
            AbstractVfsFileProvider provider = new DefaultFileProvider(directory: inputPath, searchOption: SearchOption.AllDirectories, isCaseInsensitive: true, versions: new VersionContainer(EGame.GAME_UE5_0));

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
                    bool isValidTarget = targets.Any(target => entry.Path.Contains(target, StringComparison.InvariantCultureIgnoreCase));
                    if (!isValidTarget)
                    {
                        continue;
                    }
                }

                // check if the file is a "bad file", i.e. one that is known to cause issues with CUE4Parse
                bool isBadFile = badFiles.Any(badFile => Regex.IsMatch(entry.Path, ".*" + badFile + ".*", RegexOptions.IgnoreCase));
                if (isBadFile)
                {
                    continue;
                }

                // debug
                if (options.Value.Debug)
                {
                    Console.WriteLine(entry.Path);
                }

                try
                {
                    // Load all of the objects from the uasset file
                    var exports = provider.LoadAllObjects(entry.Path);

                    // UObjects themselves actually on contain the changes from the parent, so to get all values we need
                    // to get values from the parents, as deep as it goes, the following handles that
                    List<JObject> data = new List<JObject>();
                    foreach (UObject export in exports)
                    {
                        List<UObject> makeup = GetClassMakeup(export);
                        JObject fullClassData = CombineClassData(makeup);
                        data.Add(fullClassData);
                    }

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
                    // Serialize to string and write
                    File.WriteAllText(filePath, JsonConvert.SerializeObject(data, Formatting.Indented));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception caused by {entry.Name} \nException: {ex.Message}");
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
                result.Merge(jsonObj);
            }
            return result;
        }
    }
}