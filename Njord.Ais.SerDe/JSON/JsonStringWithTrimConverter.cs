using System.Text.Json;
using System.Text.Json.Serialization;

namespace Njord.Ais.SerDe.JSON
{
    /// <summary>
    /// A custom JSON converter that handles string values.
    /// Converts empty or whitespace-only strings to null during deserialization.
    /// Writes null for empty or whitespace-only strings during serialization.
    /// </summary>
    public class JsonStringWithTrimConverter : JsonConverter<string>
    {
        /// <summary>
        /// Reads and converts the JSON to a string.
        /// If the JSON string is empty or contains only whitespace, returns null.
        /// </summary>
        /// <param name="reader">The Utf8JsonReader to read from.</param>
        /// <param name="typeToConvert">The type to convert (string).</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <returns>The converted string or null if the input is empty or whitespace.</returns>
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return value.Trim();
        }

        /// <summary>
        /// Writes a string value as JSON.
        /// If the string is empty or contains only whitespace, writes null.
        /// </summary>
        /// <param name="writer">The Utf8JsonWriter to write to.</param>
        /// <param name="value">The string value to write.</param>
        /// <param name="options">Options to control the behavior during writing.</param>
        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.Trim());
        }
    }
}