using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using BeatsMusic.Importer.Beats;
using SimMetricsMetricUtilities;

namespace BeatsMusic.Importer
{
    /// <summary>
    /// Interaction logic for CollectionImporter.xaml
    /// </summary>
    public partial class CollectionImporter : Window
    {
        private ObservableCollection<AlbumItemViewModel> AlbumsInUI = new ObservableCollection<AlbumItemViewModel>();
        private Queue<ServiceAgnosticAlbum> AlbumsToConvert = null;
        public CollectionImporter()
        {
            InitializeComponent();
            this.StretchToMaximum();
            grid.ItemsSource = AlbumsInUI;
            AlbumsToConvert = new Queue<ServiceAgnosticAlbum>(App.AlbumsToImport);
            progressBar.Maximum = AlbumsToConvert.Count;

            ThreadPool.QueueUserWorkItem(ConvertNextAlbumAndAddToUI);
        }

        private BeatsClient beatsClient = new BeatsClient();

        private async void ConvertNextAlbumAndAddToUI(object state)
        {
            if (!AlbumsToConvert.Any())
            {
                EnableImport();
                return;
            }
            
            var album = AlbumsToConvert.Dequeue();
            var albumSearchResults = await beatsClient.SearchStreamableAlbums(album.AlbumName, album.ArtistName);
            var bestSearchResult = FindBestMatch(album, albumSearchResults);

            if (bestSearchResult != null)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(() => AddItemToListOnScreen(bestSearchResult, albumSearchResults, album)));
            }

            //// TODO: delete this, testing only
            //if (progress >= 20)
            //    AlbumsToConvert.Clear();

            progress++;
            ConvertNextAlbumAndAddToUI(null);
        }

        private void AddItemToListOnScreen(SearchResult bestSearchResult, List<SearchResult> albumSearchResults, ServiceAgnosticAlbum originalSearch)
        {
            var itemViewModel = new AlbumItemViewModel(bestSearchResult.Id,
                bestSearchResult.DetailText,
                bestSearchResult.DisplayText,
                albumSearchResults,
                originalSearch);

            if (AlbumsInUI.FirstOrDefault(a => a.AlbumId == itemViewModel.AlbumId) == null)
            {
                AlbumsInUI.Add(itemViewModel);
                if (scrollViewer.VerticalOffset +200 >= scrollViewer.ViewportHeight)
                {
                    scrollViewer.ScrollToBottom();
                }
            }
            progressBar.Value = progress;
            progressText.Text = string.Format("{0} albums scanned (out of {1})", progress, progressBar.Maximum);
        }

        private void EnableImport()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(() =>
            {
                progressGrid.Visibility = Visibility.Hidden;
                txtImport.Text = "Import " + AlbumsInUI.Where(a => a.ShouldImport).Count() + " albums to beats music";
                btnImport.Visibility = Visibility.Visible;
            }));
        }

        private int progress = 1;
        private Levenstein textComparisonAlgo = new Levenstein();

        private SearchResult FindBestMatch(ServiceAgnosticAlbum album, List<SearchResult> albumSearchResults)
        {
            if (albumSearchResults == null || !albumSearchResults.Any())
                return null;

            // first try to match based on identical artist and album name
            var directMatch = albumSearchResults.Where(sr =>
                sr.DisplayText.ToLower() == album.AlbumName.ToLower()
                && sr.DetailText.ToLower() == album.ArtistName.ToLower());

            if (directMatch.Count() == 1)
                return directMatch.First();
            else if (directMatch.Count() > 1)
                albumSearchResults = directMatch.ToList();

            // next try to match just based on artist name
            var artistMatch = albumSearchResults.Where(sr => 
                sr.DetailText.ToLower() == album.ArtistName.ToLower());

            if (artistMatch.Count() == 1)
                return artistMatch.First();
            else if (artistMatch.Count() > 1)
                albumSearchResults = artistMatch.ToList();

            // next we'll try to zero down on textually similar artist and albums
            return albumSearchResults
                .Where(sr => 
                textComparisonAlgo.GetSimilarity(sr.DisplayText, album.AlbumName) +
                textComparisonAlgo.GetSimilarity(sr.DetailText, album.ArtistName) > 1)
                .OrderByDescending(sr =>
                textComparisonAlgo.GetSimilarity(sr.DisplayText, album.AlbumName) +
                textComparisonAlgo.GetSimilarity(sr.DetailText, album.ArtistName))
                .FirstOrDefault();

        }

        private void BtnImport_OnClick(object sender, RoutedEventArgs e)
        {
            btnImport.IsEnabled = false; 
            var ids = AlbumsInUI.Where(a => a.ShouldImport).Select(a => a.AlbumId).ToArray();
            beatsClient.BulkAddToCollection(ids);

            btnImport.Content = ids.Count() + " albums imported to beats music!";
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            txtImport.Text = "Import " + AlbumsInUI.Where(a => a.ShouldImport).Count() + " albums to beats music";
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            txtImport.Text = "Import " + AlbumsInUI.Where(a => a.ShouldImport).Count() + " albums to beats music";
            ((AlbumItemViewModel) ((CheckBox) sender).DataContext).PrintSearchResults();
        }

        private int FourOThree = 0;
        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (e.ErrorException.Message.Contains("403"))
            {
                Debug.WriteLine(++FourOThree + " image loading failed due to 403");    
            }

            Image image = (Image)sender;
            var dataContext = image.DataContext;
            image.Source = null;
            image.Source = new BitmapImage(((AlbumItemViewModel)(dataContext)).AlbumArtUri);
        }
    }
}
