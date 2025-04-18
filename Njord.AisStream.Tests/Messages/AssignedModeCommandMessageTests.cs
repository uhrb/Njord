using Njord.Ais.Extensions.Messages;
using Njord.Ais.Messages;
using Njord.AisStream.Converters;
using System.Text;
using System.Text.Json;
using Xunit.Abstractions;

namespace Njord.AisStream.Tests.Messages
{
    public class AssignedModeCommandMessageTests 
    {
        private readonly ITestOutputHelper _helper;

        public AssignedModeCommandMessageTests(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        [Theory]
        [MemberData(
            nameof(MessagesDataClass.GetMessages),
            "Data Source=messages.db",
            "AisStreamMessages",
            nameof(AisStreamMessageType.AssignedModeCommand),
            MemberType = typeof(MessagesDataClass)
        )]
        public void ConvertedWithoutExceptions(ulong id, string message, bool valid)
        {
            _helper.WriteLine($"Id={id}; Valid = {valid}");
            _helper.WriteLine(message);
            var converter = new JsonAisStreamEnvelopeConverter();
            var opts = new JsonSerializerOptions();
            var bytes = Encoding.UTF8.GetBytes(message);
            var reader = new Utf8JsonReader(bytes);
            var result = converter.Read(ref reader, typeof(AisStreamEnvelope), opts);
            Assert.NotNull(result);
            Assert.Equal(AisStreamMessageType.AssignedModeCommand, result.MessageType);
            Assert.NotNull(result.Metadata);
            Assert.NotNull(result.Message);
            var typedMessage = result.Message as IAssignedModeCommandMessage;
            Assert.NotNull(typedMessage);
            Assert.Equal(valid, typedMessage.IsValid());
        }

        [Theory]
        [MemberData(
           nameof(MessagesDataClass.GetMessages),
           "Data Source=messages.db",
           "AisStreamMessages",
           nameof(AisStreamMessageType.AssignedModeCommand),
           MemberType = typeof(MessagesDataClass)
        )]
        public void ConvertedBackAndForce(ulong id, string message, bool valid)
        {
            _helper.WriteLine($"Id={id}; Valid = {valid}");
            _helper.WriteLine(message);
            var converter = new JsonAisStreamEnvelopeConverter();
            var opts = new JsonSerializerOptions();
            var bytes = Encoding.UTF8.GetBytes(message);
            var reader = new Utf8JsonReader(bytes);
            var result = converter.Read(ref reader, typeof(AisStreamEnvelope), opts);
            Assert.NotNull(result);
            Assert.Equal(AisStreamMessageType.AssignedModeCommand, result.MessageType);
            string reConverter = string.Empty;
            using (var ms = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(ms))
                {
                    converter.Write(writer, result, opts);
                    writer.Flush();
                    reConverter = Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            Assert.NotEmpty(reConverter);
            bytes = Encoding.UTF8.GetBytes(reConverter);
            reader = new Utf8JsonReader(bytes);
            var result2 = converter.Read(ref reader, typeof(AisStreamEnvelope), opts);
            Assert.NotNull(result2);
            Assert.Equal(AisStreamMessageType.AssignedModeCommand, result2.MessageType);
            Assert.Equivalent(result, result2);
        }
    }
}
