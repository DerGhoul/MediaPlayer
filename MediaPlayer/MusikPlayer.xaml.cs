using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MediaPlayer
{
    /// <summary>
    /// Interaktionslogik für MusikPlayer.xaml
    /// </summary>
    public partial class MusikPlayer : Window
    {
        public MusikPlayer(List<Uri> uri)
        {
            InitializeComponent();
            this.uris = uri;
            musikPlayerSource();
            
        }
        /// <summary>
        /// Attributes
        /// </summary>
        /// <param name="uri"></param>
        List<Uri> uris;
        int title = 0;



        private void musikPlayerSource()
        {
            musikPlayer.Source = uris[title];
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                musikPlayer.Pause();
            }
            catch (Exception)
            {
                
            }
            
        }

        private void weiter_Click(object sender, RoutedEventArgs e)
        {
            musikPlayer.Play();
        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            musikPlayer.Volume = (double)e.NewValue;
            //MessageBox.Show(e.NewValue.ToString());
            string a = e.NewValue.ToString();
            //volV.Text = a;
            
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            {
                if (e.Delta > 0)
                {
                    volumeSlider.Value += 0.1;
                }
                else
                {
                    volumeSlider.Value -= 0.1;
                }
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if (title == 0)
            {
                title = uris.Count - 1;
            }
            else
            {
                title -- ;
                musikPlayerSource();

            }
        }

        private void forward_Click(object sender, RoutedEventArgs e)
        {
            if (title == uris.Count - 1)
            {
                title = 0;
            }
            else
            {
                title += 1;
                musikPlayerSource();
            }
            
        }
    }
}
