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
using WMPLib;

namespace BeatsMusic.Importer
{
    /// <summary>
    /// Interaction logic for WmpImport.xaml
    /// </summary>
    public partial class WmpImport: Window
    {
        public WmpImport()
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
            var wmp = new WindowsMediaPlayer();
            var collection = wmp.mediaCollection;
            this.authorIndex = collection.getMediaAtom("Author");
            this.albumArtistIndex = collection.getMediaAtom("WM/AlbumArtist");
            this.albumIndex = collection.getMediaAtom("Album");

            IWMPPlaylist allMedia = collection.getAll();
            List<ServiceAgnosticAlbum> albums = new List<ServiceAgnosticAlbum>();
            for (int i = 0; i < allMedia.count; i++)
            {
                var currentMedia = allMedia.get_Item(i);
                var artistName = GetArtist(currentMedia);
                var albumName = GetAlbum(currentMedia);

                if (!string.IsNullOrEmpty(artistName) &&
                    !string.IsNullOrEmpty(albumName) &&
                    !albums.Any(a => a.AlbumName == albumName && a.ArtistName == artistName))
                {
                    albums.Add(new ServiceAgnosticAlbum(albumName, artistName));
                }
            }
            App.AlbumsToImport = albums;

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(() => this.Navigate(new CollectionImporter())));
        }


        private int albumArtistIndex;
        private int authorIndex;
        private string GetArtist(IWMPMedia mediaItem)
        {
            string artist = mediaItem.getItemInfoByAtom(albumArtistIndex);
            if (string.IsNullOrEmpty(artist) || artist == "VARIOUS ARTISTS")
            {
                artist = mediaItem.getItemInfoByAtom(authorIndex);
            }

            return artist;
        }


        private int albumIndex;
        private string GetAlbum(IWMPMedia mediaItem)
        {
            return mediaItem.getItemInfoByAtom(albumIndex);
        }
    }
}
