using Newtonsoft.Json;

namespace BeatsMusic.Importer.Beats
{
    public class SearchResult
    {
        /// <summary>
        /// genre, artist, album, track, playlist or user.
        /// </summary>
        [JsonProperty("result_type")]
        public string ResultType { get; set; }

        /// <summary>
        /// The unique ID of the referenced item
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Text to be displayed when displaying a link to the item. 
        /// For albums this is album name.
        /// </summary>
        [JsonProperty("display")]
        public string DisplayText { get; set; }
        
        /// <summary>
        /// Additional text detail describing the item.
        /// For albums this is the artist name. 
        /// </summary>
        [JsonProperty("detail")]
        public string DetailText { get; set; }

        /// <summary>
        /// Object with keys corresponding to referenced object types, and values providing the information needed for linking to the objects.
        /// For album this is the artist info.  
        /// </summary>
        [JsonProperty("related")]
        public SearchResultRelatedItem SearchResultRelatedItem { get; set; }

        public override string ToString()
        {
            return string.Format("DisplayText: {0}, DetailText: {1}", DisplayText, DetailText);
        }
    }
}