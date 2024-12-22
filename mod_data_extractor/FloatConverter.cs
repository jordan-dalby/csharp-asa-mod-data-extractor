using Newtonsoft.Json;
using System;
using System.Globalization;

public class FloatConverter : JsonConverter<float>
{
    public override float ReadJson(JsonReader reader, Type objectType, float existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value == null) return 0.0f;
        return Convert.ToSingle(reader.Value);
    }

    public override void WriteJson(JsonWriter writer, float value, JsonSerializer serializer)
    {
        writer.WriteRawValue(value.ToString("F10", CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.'));
    }
} 