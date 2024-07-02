using System.Text.RegularExpressions;
using CUE4Parse.FileProvider;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.UE4.Versions;

namespace ModDataExtractor
{
    public class Extractor
    {
        public static void Extract(string[] args)
        {
            var options = CommandLine.Parser.Default.ParseArguments<Options>(args);
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
            AbstractVfsFileProvider provider = new DefaultFileProvider(directory: inputPath, searchOption: SearchOption.AllDirectories, isCaseInsensitive: true, versions: new VersionContainer(options.Value.UEVersion));

            // Initialize and mount the file provider
            provider.Initialize();
            provider.Mount();

            Parser parser = new Parser(provider);

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

                    string data = parser.ParseToJson(entry);
                    File.WriteAllText(filePath, data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception caused by {entry.Name} \nException: {ex.StackTrace}");
                }
            }
        }
    }
}