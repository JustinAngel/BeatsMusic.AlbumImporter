using System;
using System.Collections.Generic;
using System.Diagnostics;
using BeatsMusic.Importer.Beats;

namespace BeatsMusic.Importer
{
    public class AlbumItemViewModel
    {
        public AlbumItemViewModel(string albumId, string artistName, string albumName, List<SearchResult> alternativeSearchResults, ServiceAgnosticAlbum originalSearch)
        {
            AlbumId = albumId;
            ArtistName = artistName;
            AlbumName = albumName;
            AlternativeSearchResults = alternativeSearchResults;
            OriginalSearch = originalSearch;
            ShouldImport = true;
        }


        public string AlbumId { get; set; }
        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public bool ShouldImport { get; set; }
        public List<SearchResult> AlternativeSearchResults { get; set; }
        public ServiceAgnosticAlbum OriginalSearch { get; set; }

        public Uri AlbumArtUri
        {
            get
            {
                return BeatsClient.GetDefaultAlbumImageUri(AlbumId);
            }
        }

        public void PrintSearchResults()
        {
            Debug.WriteLine("Options for " + OriginalSearch.ToString());
            foreach (var searchResult in AlternativeSearchResults)
            {
                Debug.WriteLine("\t" + searchResult.ToString());
            }
        }
    }
}