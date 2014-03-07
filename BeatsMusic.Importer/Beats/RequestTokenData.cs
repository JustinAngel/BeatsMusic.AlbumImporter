using Newtonsoft.Json;

namespace BeatsMusic.Importer.Beats
{
    public class RequestTokenData
    {
        public string return_type { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        public string token_type { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public object state { get; set; }
        public object uri { get; set; }
        public object extended { get; set; }
    }
}