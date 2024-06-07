using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Versions;
using CUE4Parse.FileProvider.Vfs;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using CommandLine;
using Serilog.Debugging;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Assets;

namespace UtocDumper
{
    internal class Program
    {

        public class Options
        {
            [Option('i', "input", Required = true, HelpText = "The directory with all of the required game files")]
            public string InputDirectory { get; set; }

            [Option('o', "output", Required = true, HelpText = "The directory to output files to")]
            public string OutputDirectory { get; set; }

            [Option('b', "badfile", Required = false, HelpText = "Location of a file containing line-by-line items to skip")]
            public string BadFileDirectory { get; set; }

            [Option('d', "debug", Required = false, HelpText = "Set debug mode to true or false.", Default = false)]
            public bool Debug { get; set; }

            [Option('t', "targets", Required = false, HelpText = "Optional specific directories to extract data from, leave blank to extract everything")]
            public IEnumerable<string> Targets { get; set; }
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
                if (!entry.Path.Contains(".uasset", StringComparison.CurrentCultureIgnoreCase)) 
                { 
                    continue; 
                }

                if (targets.Any())
                {
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

               
                if (options.Value.Debug)
                {
                    Console.WriteLine(entry.Path);
                }

                try
                {
                    // Load all of the objects from the uasset file
                    var exports = provider.LoadAllObjects(entry.Path);
                    // Concat output file path
                    string filePath = outputPath + @"\Exports\" + entry.PathWithoutExtension + ".json";
                    // Create missing directories if not exists
                    string? directoryName = new FileInfo(filePath).DirectoryName;
                    if (directoryName == null)
                    {
                        Console.WriteLine($"Failed to create directory {filePath} because the DirectoryName was null");
                        continue;
                    }
                    Directory.CreateDirectory(directoryName);
                    // Serialize to string and write
                    File.WriteAllText(filePath, JsonConvert.SerializeObject(exports, Formatting.Indented));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception caused by {entry.Name} \nException: {ex.Message}");
                }
            }
        }
    }
}