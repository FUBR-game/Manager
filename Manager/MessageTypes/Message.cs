using Newtonsoft.Json;

namespace Manager.MessageTypes
{
    public class Message
    {
        [JsonProperty("MessageType")]
        public string MessageType;
        [JsonProperty("MessageData")]
        public string MessageData;
    }
}