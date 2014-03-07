using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RdioSharp.Models;

namespace BeatsMusic.Importer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string BeatsAccessToken { get; set; }
        public static DateTime BeatsAccessTokenExpires { get; set; }
        public static IEnumerable<ServiceAgnosticAlbum> AlbumsToImport { get; set; }
        public static string BeatsCode { get; set; }
        public static ImportSources ImportSource { get; set; }
    }

    public enum ImportSources
    {
        None,
        Rdio,
        iTunes,
        WindowsMediaPlayer
    }

    public class ServiceAgnosticAlbum
    {
        public ServiceAgnosticAlbum(string albumName, string artistName)
        {
            AlbumName = albumName;
            ArtistName = artistName;
        }

        public ServiceAgnosticAlbum(RdioAlbum rdioAlbum)
        {
            AlbumName = rdioAlbum.Name;
            ArtistName = rdioAlbum.Artist;
        }

        public string AlbumName { get; set; }
        public string ArtistName { get; set; }

        public override string ToString()
        {
            return string.Format("AlbumName: {0}, ArtistName: {1}", AlbumName, ArtistName);
        }
    }
}
