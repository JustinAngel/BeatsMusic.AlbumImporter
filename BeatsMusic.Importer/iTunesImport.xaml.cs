using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using iTunesLib;

namespace BeatsMusic.Importer
{
    /// <summary>
    /// Interaction logic for WmpImport.xaml
    /// </summary>
    public partial class ITunesImport : Window
    {
        public ITunesImport()
        {
            InitializeComponent();
            this.StretchToMaximum();
            this.Loaded += WmpImport_Loaded;
        }

        void WmpImport_Loaded(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(GetWpAlbums);

        }

        private void GetWpAlbums(object state)
        {
           List<ServiceAgnosticAlbum> albums = new List<ServiceAgnosticAlbum>();

           IITPlaylist libraryPlaylist = GetLibraryPlaylist();
           if (libraryPlaylist != null)
           {
               IITTrackCollection tracks = libraryPlaylist.Tracks;
               int i = 1;
               foreach (IITFileOrCDTrack track in tracks)
               {
                   var artistName = track.Artist;
                   var albumName = track.Album;
                   if (!string.IsNullOrEmpty(artistName) &&
                       !string.IsNullOrEmpty(albumName) &&
                       !albums.Any(a => a.AlbumName == albumName && a.ArtistName == artistName))
                   {
                       albums.Add(new ServiceAgnosticAlbum(albumName, artistName));
                   }

                   i++;
               }
           }

            App.AlbumsToImport = albums;
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(() => this.Navigate(new CollectionImporter())));
        }

        private static IITPlaylist GetLibraryPlaylist()
        {
            IITPlaylist libraryPlaylist = null;
            IiTunes iTunes = new iTunesAppClass();
            IITSource librarySource = null;
            foreach (IITSource source in iTunes.Sources)
            {
                if (source.Kind == ITSourceKind.ITSourceKindLibrary)
                {
                    librarySource = source;
                    break;
                }
            }
            if (librarySource != null)
            {
                foreach (IITPlaylist pl in librarySource.Playlists)
                {
                    if (pl.Kind == ITPlaylistKind.ITPlaylistKindLibrary)
                    {
                        libraryPlaylist = pl;
                        break;
                    }
                }
            }
            return libraryPlaylist;
        }
    }
}
