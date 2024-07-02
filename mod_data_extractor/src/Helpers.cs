using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Objects.Properties;
using Newtonsoft.Json.Linq;

namespace ModDataExtractor
{
    public static class Helpers
    {
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

        public static JObject CombineClassData(List<UObject> classData)
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
                result.Merge(jsonObj, Constants.MERGE_SETTINGS);
            }
            return result;
        }
    }
}