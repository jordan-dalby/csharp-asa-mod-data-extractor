using CommandLine;
using CUE4Parse.UE4.Versions;

namespace ModDataExtractor
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
        
        [Option('v', "version", Required = false, HelpText = "The UE version to use for parsing, see https://github.com/FabianFG/CUE4Parse/blob/master/CUE4Parse/UE4/Versions/EGame.cs", Default = EGame.GAME_UE5_2)]
        public EGame UEVersion { get; set; } = EGame.GAME_UE5_2;
    }
}