using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeatsMusic.Importer.Beats
{
    public class SearchResultDataRoot
    {
        /// <summary>
        /// List of matching search results. Each element is a search_result object.
        /// </summary>
        [JsonProperty("data")]
        public List<SearchResult> Data { get; set; }

        /// <summary>
        /// Beats API response code. OK if successful; otherwise an error code is returned.
        /// </summary>
        [JsonProperty("code")]
        public string ResponseCode { get; set; }
    }
}