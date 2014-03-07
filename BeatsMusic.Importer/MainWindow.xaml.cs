using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeatsMusic.Importer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.StretchToMaximum();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Navigate(new BeatsLogin());  
        }

        private void rdio_OnChecked(object sender, RoutedEventArgs e)
        {
            App.ImportSource = ImportSources.Rdio;
        }

        private void wmp_OnChecked(object sender, RoutedEventArgs e)
        {
            App.ImportSource = ImportSources.WindowsMediaPlayer;
        }

        private void itunes_OnChecked(object sender, RoutedEventArgs e)
        {
            App.ImportSource = ImportSources.iTunes;
        }
    }

    public static class WindowExtensions
    {
        public static void StretchToMaximum(this Window current)
        {
            current.WindowState = WindowState.Maximized;
            current.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            current.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            current.WindowStyle = WindowStyle.None;
            current.AllowsTransparency = false;
            current.ResizeMode = ResizeMode.CanResize;
        }

        public static void Navigate(this Window current, Window next)
        {
            Application.Current.MainWindow = next;
            next.Show();
            current.Close();
        }
    }
}
