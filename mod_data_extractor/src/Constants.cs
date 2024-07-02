using Newtonsoft.Json.Linq;

namespace ModDataExtractor
{
    public static class Constants
    {
        public readonly static JsonMergeSettings MERGE_SETTINGS = new JsonMergeSettings() { MergeArrayHandling = MergeArrayHandling.Replace };
    }
}