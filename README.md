# CSharp ASA Mod Data Extractor  
This tool uses CUE4Parse to extract data from a specified ARK Survival Ascended mod, printing the contents of each uasset file in JSON format  
  
This tool can be used either with a server set up, or an ARK Survival Ascended full installation, change `path` to your desired choice  
  
## Tool usage  
  
  -i, --input      Required. The directory with all of the required game files  
  
  -o, --output     Required. The directory to output files to  
  
  -d, --debug      (Default: false) Set debug mode to true or false.  
  
  -t, --targets    Optional specific directories to extract data from, leave
                   blank to extract everything  
  
  -f, --file-types (Default: uasset) Optional, declare a list of file types of interest,
                   all other file types will be skipped  
  
  -v, --version    (Default: GAME_ARKSurvivalAscended) The UE version to use for parsing, see
                    https://github.com/FabianFG/CUE4Parse/blob/master/CUE4Parse/UE4/Versions/EGame.cs  
  
e.g. `mod_data_extractor.exe --debug --input "c:\my\path" --output "c:\my\other\path"`  
  
On Linux: `./mod_data_extractor --input /my/path --output /my/other/path`  
  
Windows and Linux executables found in the Releases  
https://github.com/jordan-dalby/csharp-asa-mod-data-extractor/releases