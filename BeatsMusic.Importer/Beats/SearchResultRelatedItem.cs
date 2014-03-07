using Newtonsoft.Json;

namespace BeatsMusic.Importer.Beats
{
    public class SearchResultRelatedItem
    {
        /// <summary>
        /// The API object type of the referenced item. 
        /// </summary>
        [JsonProperty("ref_type")]
        public string ReferenceType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("display")]
        public string DisplayText { get; set; }
    }
}