using Newtonsoft.Json;

namespace Manager.MessageTypes
{
    public abstract class Message
    {
        [JsonProperty("MessageType")]
        public string MessageType;
        [JsonProperty("MessageData")]
        public string MessageData;
        
        
        public abstract void Execute();
    }
}