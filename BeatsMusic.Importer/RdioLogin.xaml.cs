using System;
using System.Collections.Generic;
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
using RdioSharp;
using RdioSharp.Models;

namespace BeatsMusic.Importer
{
    public class RdioConstants
    {
        public const string ConsumerKey = "<rdio API consumer key here>";
        public const string ConsumerSecret = "<rdio Api Consumer Secret here>";
        public const string AccessKey = "";
        public const string AccessSecret = "";
    }

    /// <summary>
    /// Interaction logic for RdioLogin.xaml
    /// </summary>
    public partial class RdioLogin : Window
    {

        public RdioLogin()
        {
            InitializeComponent();
            grdAlbums.Visibility = Visibility.Hidden;
            this.StretchToMaximum();
            StartRdioLogin();
        }

        void webBrowser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            
            Debug.WriteLine("Navigating to " + e.Uri.OriginalString);
        }

        private RdioManager manager;
        private RdioUser user;
        private int pin;

        private void StartRdioLogin()
        {
            const string consumerKey = RdioConstants.ConsumerKey;
            const string consumerSecret = RdioConstants.ConsumerSecret;
            const string accessKey = RdioConstants.AccessKey;
            const string accessSecret = RdioConstants.AccessSecret;
            manager = new RdioManager(consumerKey, consumerSecret, accessKey, accessSecret);
            if (!manager.IsAuthorized)
            {
                manager.GenerateRequestTokenAndLoginUrl();
                System.Console.WriteLine(string.Format("Authorize at this url: {0}", manager.LoginUrl));
                webBrowser.Navigate(manager.LoginUrl);
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            pin = 0;
            if (!string.IsNullOrEmpty(txtPin.Text) && txtPin.Text.Length == 4 && int.TryParse(txtPin.Text, out pin))
            {
                LockUI();
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += bw_DoWork;
                bw.RunWorkerCompleted += bw_RunWorkerCompleted;
                bw.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Pin has to be 4 numbers long");
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Navigate(new CollectionImporter());
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            manager.Authorize(pin.ToString());
            user = manager.CurrentUser(new[] { "isSubscriber", "isTrial", "isUnlimited" });
            var albums = manager.GetAlbumsInCollection(user.Key, start: 0, count: int.MaxValue);
            App.AlbumsToImport = albums.Select(rdioAlbum => new ServiceAgnosticAlbum(rdioAlbum));
            e.Result = true;
        }

        private void LockUI()
        {
            this.IsEnabled = false;
            grdAlbums.Visibility = Visibility.Visible;
        }
    }
}
