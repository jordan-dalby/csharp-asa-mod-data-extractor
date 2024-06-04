using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Versions;
using CUE4Parse.FileProvider.Vfs;
using Newtonsoft.Json;

namespace UtocDumper
{
    internal class Program
    {

        static void Main(string[] args)
        {
            AbstractVfsFileProvider Provider;

            // Where the server is stored on disk, needs to be the main directory so that it can find the globals.utoc file
            const string path = @"C:\Users\jorda\Desktop\TestServer\testserver";
            // The mod to look for, not strictly necessary but good for reducing required operations
            const string modOfInterest = "ResourceGatherers";

            // Create the default file provider from the base directory, basically does everything for you
            Provider = new DefaultFileProvider(directory: path, searchOption: SearchOption.AllDirectories, isCaseInsensitive: true, versions: new VersionContainer(EGame.GAME_UE5_0));

            // Initialize and mount the file provider
            Provider.Initialize();
            Provider.Mount();

            // Not necessary but will keep here in case it's useful in some way
            // IoStoreReader reader = new IoStoreReader(Directory.GetFiles(path, "global.utoc", SearchOption.AllDirectories)[0], CUE4Parse.UE4.IO.Objects.EIoStoreTocReadOptions.ReadDirectoryIndex, new VersionContainer(EGame.GAME_UE5_0));
            // IoGlobalData ioGlobalData = new IoGlobalData(reader);

            Console.WriteLine($"Found {Provider.Files.Count} files");

            foreach (var entry in Provider.Files.Values)
            {
                // perhaps not foolproof, relies on the name of the mod being in the path
                // this condition isn't strictly necessary, but it certainly cuts down the amount of data that needs to be combed through
                if (!entry.Path.Contains(modOfInterest))
                    continue;

                // not an exhaustive list of possibilities, again, just for cutting out the crap, like textures, this list will need properly populating
                // when building full items, things will need linking together, e.g. this EngramEntry relates to this PrimalItem:
                // Engram Entry [Blueprint Entry] -> PrimalItem
                if (entry.Name.Contains("PrimalItemStructure") || entry.Name.Contains("EngramEntry") || entry.Name.Contains("PrimalItemResource"))
                {
                    try
                    {
                        // Load all of the objects from the uasset file
                        var exports = Provider.LoadAllObjects(entry.Path);
                        // Serialize to string and write
                        Console.WriteLine($"{entry.Name} \n{JsonConvert.SerializeObject(exports, Formatting.Indented)}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load {entry.Name} {ex.Message}");
                    }
                }
            }
        }
    }
    
}