using Newtonsoft.Json;

namespace BeatsMusic.Importer.Beats
{
    public class RequestTokenRootElement
    {
        [JsonProperty("jsonrpc")]
        public string jsonrpc { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("result")]
        public RequestTokenData Data { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}