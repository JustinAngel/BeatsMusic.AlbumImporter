using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BeatsMusic.Importer.Beats;

namespace BeatsMusic.Importer
{
    /// <summary>
    /// Interaction logic for BeatsLogin.xaml
    /// </summary>
    public partial class BeatsLogin : Window
    {
        private BeatsClient client = new BeatsClient();
        public BeatsLogin()
        {
            InitializeComponent();
            this.StretchToMaximum();
            webBrowser.NavigateToString("<div backgrohd=\"#F2F3F5\" />");
            webBrowser.Source = client.GetAuthorizationUrl(BeatsClient.BeatsCallbackDomain);
            webBrowser.Navigating += webBrowser_Navigating;
        }

        private async void webBrowser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (e.Uri != null && e.Uri.Host.ToLower().Contains(BeatsClient.BeatsCallbackDomain.ToLower()))
            {
                webBrowser.NavigateToString(@"<html><body style=""background: #F2F3F5"" /></html>");

                var queryStringParams = HttpUtility.ParseQueryString(e.Uri.Query);
                if (queryStringParams.AllKeys.Contains("code"))
                {
                    App.BeatsCode = queryStringParams["code"];
                    App.BeatsAccessToken = await client.GetAccesstoken();

                    if (App.ImportSource == ImportSources.Rdio)
                        this.Navigate(new RdioLogin());
                    else if (App.ImportSource == ImportSources.WindowsMediaPlayer)
                        this.Navigate(new WmpImport());
                    else if (App.ImportSource == ImportSources.iTunes)
                        this.Navigate(new ITunesImport());
                }
            }
        }

        //private void navigateForTokenLogin(NameValueCollection queryStringParams)
        //{
        //    if (queryStringParams.AllKeys.Contains("access_token"))
        //    {
        //        App.BeatsAccessToken = queryStringParams["access_token"];
        //        App.BeatsAccessTokenExpires = DateTime.Now.AddSeconds(Convert.ToInt32(queryStringParams["expires_in"]));

        //        this.Navigate(new RdioLogin());
        //    }
        //}
    }
}
