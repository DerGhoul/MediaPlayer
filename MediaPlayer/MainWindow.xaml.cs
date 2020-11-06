using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Windows.Media.Animation;
using System.Security.Policy;

namespace MediaPlayer
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

       // List<ListBoxItem> fileNames = new List<ListBoxItem>();
        
        List<String> files = new List<string>();
        List<string> fileNames = new List<string>();
        




        private List<Uri> getUriFile(String path)
        {   
            List<Uri> uris = new List<Uri>();
            foreach (string file in System.IO.Directory.GetFiles(path))
            {
                Uri uri;
                
                string a = path + System.IO.Path.GetFileNameWithoutExtension(file) +System.IO.Path.GetExtension(file);
                
                uri = new Uri(a);
                if (System.IO.Path.GetExtension(file) == ".mp3")
                {
                    //MessageBox.Show(a);
                    uris.Add(uri);
                }
                
            }
            //MessageBox.Show(uris.Count.ToString());
            return uris;

        }


        private void getListViewItems(String path)
        {
            foreach (string file in System.IO.Directory.GetFiles(path))
            {
                fileNames.Add(System.IO.Path.GetFileNameWithoutExtension(file));
            }           
        }

        private void fillListViewItems()
        {
            FileNames = fileNames;
            string mess = "";
            foreach (var item in fileNames)
            {
                mess += item;
            }
            //MessageBox.Show(mess);
            string mess1 = "";
            foreach (var item in FileNames)
            {
                mess1 += item;
            }
            //MessageBox.Show(mess1);
        }

        //Holds the list of file names which you want to display in your ListBox
        public IList<string> FileNames
        {
            get { return (IList<string>)GetValue(FileNamesProperty); }
            set { SetValue(FileNamesProperty, value); }
        }



        public static readonly DependencyProperty FileNamesProperty =
            DependencyProperty.Register("FileNames", typeof(IList<string>), typeof(MainWindow), new PropertyMetadata(null));

        //Holds the currently selected item from the ListBox
        public string SelectedFileName
        {
            get { return  (string)GetValue(SelectedFileNameProperty); }
            set { SetValue(SelectedFileNameProperty, value); }
        }
        public static readonly DependencyProperty SelectedFileNameProperty =
            DependencyProperty.Register("SelectedFileName", typeof(string), typeof(MainWindow), new PropertyMetadata(null, SelectedFileNameChanged));

        //This static method runs whenever the selected item changes (because SelectedItem is bound to SelectedFileName)
        private static void SelectedFileNameChanged(DependencyObject o,DependencyPropertyChangedEventArgs e)
        {
            var window = (MainWindow)o;
            
            Uri uri = new Uri(window.verzeichnis.Text +$"{o.GetValue(e.Property).ToString()}"+".mp4");
            
            
            //MessageBox.Show(uri.ToString());
            window.mediaPlayer.Source = uri;
            

            
            //Do something here to handle the change of selection
        }

        






        /*
            String[] folders = System.IO.Directory.GetDirectories(path);
            String[] files = System.IO.Directory.GetFiles(path);
            foreach (var item in files)
            {
                string[] name = item.Split('\\');
                string n = name[name.Length - 1];
                
                string fnames = System.IO.Path.GetFileNameWithoutExtension(item);



                ListBoxItem listBoxItem = new ListBoxItem()
                {
                    AllowDrop =false,
                    MinWidth = 10,                    
                };
                
                listBoxItem.Content = fnames;
                fileNames.Add(listBoxItem);                
            }

            
            otherMedia.ItemsSource = fileNames;
            

            
        }
        */

        private void mediaSearch_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Get the File"
            };
            String filePathAndName ="";


            Nullable<bool> result = openFileDialog.ShowDialog();
            String verzeichnisKette ="";
            Uri uri;

            if (result == true)
            {
                fileName.Text = openFileDialog.SafeFileName;
                filePathAndName = openFileDialog.FileName;
                String[] folderName = openFileDialog.FileName.Split('\\');
                int  items = folderName.Count<string>()-1;
                
                for (int i = 0; i < items; i++)
                {
                    verzeichnisKette += folderName[i] + "\\";
                }

                verzeichnis.Text = verzeichnisKette;
                uri = new Uri(filePathAndName);
            }
            else
            {
                uri = null;
            }
            

            if (uri != null && openFileDialog.FileName.EndsWith(".mp4"))
            {
                getListViewItems(verzeichnisKette);
                fillListViewItems();
                mediaPlayer.Source = uri;
                mediaPlayer.Play();
                
                
                FileInfo fi = new FileInfo(filePathAndName);
                //Element_MediaOpened();

            }
            else if (uri != null && openFileDialog.FileName.EndsWith(".mp3"))
            {
                var mP = new MusikPlayer(getUriFile(verzeichnisKette));
                mP.Show();
                Window window = this;
                mP.Top = window.Top;
                mP.Left = window.Left;
                mP.Height = window.Height;
                
                Close();             
            }
        }

        public void Element_MediaOpened(object sender, RoutedEventArgs e)
        {
            while (mediaPlayer.NaturalDuration.ToString() == "Automatic")
            {
                for (int i = 0; i < 20; i++)
                {
                    i++;
                }
            }
           

            try
            {
                string max = "" ;
                //timeToEnd.Text = mediaPlayer.NaturalDuration.ToString();
                int h = Convert.ToInt32(mediaPlayer.NaturalDuration.TimeSpan.TotalHours.ToString());
                int m = Convert.ToInt32(mediaPlayer.NaturalDuration.TimeSpan.TotalMinutes.ToString());
                int s = Convert.ToInt32(mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds.ToString());
                if (h != 0)
                {
                    MessageBox.Show(" h +m+ s");
                }
                else
                {
                    max += m + "." + s;
                }
                //timeToEnd.Text = Convert.ToString(Convert.ToDouble(mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds.ToString()) / 60);
                //slider.Maximum = Convert.ToDouble(Convert.ToString(Convert.ToDouble(mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds.ToString()) / 60));
            }
            catch (Exception)
            {
                MessageBox.Show("Can't get maximum for slider");
                //MessageBox.Show(mediaPlayer.NaturalDuration.ToString());
                //MessageBox.Show(Convert.ToString(Convert.ToDouble(mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds.ToString()) / 60));
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(seitenLeiste.ActualWidth == 0)
            {
                GridLength gridLength = new GridLength(seitenLeiste.MaxWidth);
                seitenLeiste.Width = gridLength;
                
                GridLength gridLength1 = new GridLength(topLeiste.MaxHeight);
                 topLeiste.Height = gridLength1;

                GridLength gridLength2 = new GridLength(controlLeiste.MaxHeight);
                controlLeiste.Height = gridLength2;

                
            }
            else
            {
                GridLength gridLength = new GridLength(seitenLeiste.MinWidth);
                seitenLeiste.Width = gridLength;

                GridLength gridLength1 = new GridLength(topLeiste.MinHeight);
                topLeiste.Height = gridLength1;

                GridLength gridLength2 = new GridLength(controlLeiste.MinHeight);
                controlLeiste.Height = gridLength2;

                
            }
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                mediaPlayer.Pause();
                

                /*
                timeSpan = mediaPlayer.Position;
                mediaPlayer.Stop();
                mediaPlayer.Position = timeSpan;               
                */
            }
            catch (Exception)
            {
                
                MessageBox.Show("The Media Player can't be paused");
            }
            
        }

        private void weiter_Click(object sender, RoutedEventArgs e)
        {
            // The Play method will begin the media if it is not currently active or
            // resume media if it is paused. This has no effect if the media is
            // already running.
            mediaPlayer.Play();

            InitializePropertyValues();


        }

        void InitializePropertyValues()
        {
            // Set the media's starting Volume and SpeedRatio to the current value of the
            // their respective slider controls.
            //mediaPlayer.Volume = (double)volumeSlider.Value;
            //mediaPlayer.SpeedRatio = (double)speedRatioSlider.Value;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            Visibility = Visibility.Visible;              
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {           
            //grid.Visibility = Visibility.Hidden;
        }

        private void forward_Click(object sender, RoutedEventArgs e)
        {            
            mediaPlayer.Position += TimeSpan.FromSeconds(10);
            TimeSpan timeSpan = mediaPlayer.Position;
            timeLineSlider.Value = timeSpan.TotalMilliseconds;

            int time = (int)(mediaPlayer.Position.TotalSeconds / 60);
            int mod = (int)(mediaPlayer.Position.TotalSeconds % 60);            

            timeFromStart.Text = time + "." + mod;
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Position -= TimeSpan.FromSeconds(10);
            TimeSpan timeSpan = mediaPlayer.Position;
            timeLineSlider.Value = timeSpan.TotalMilliseconds;
            int time = (int)(mediaPlayer.Position.TotalSeconds / 60);
            int mod = (int)(mediaPlayer.Position.TotalSeconds % 60);

            timeFromStart.Text = time + "." + mod;
        }

        private void mediaPlayer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mediaPlayer.Pause();
            
        }

        private void big_Click(object sender, RoutedEventArgs e)
        {
            //MediaTimeline mediaTimeline = mediaPlayer.NaturalDuration;
            //MediaClock mediaClock = new MediaClock(mediaTimeline);
        }
        
        private void timeLineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            

            int sliderValue = (int)e.NewValue;
            //MessageBox.Show(timeLineSlider.Value+"");

            TimeSpan ts = new TimeSpan(0, 0, 0, 0, sliderValue);
            mediaPlayer.Position = ts;
            mediaPlayer.Play();
        }

        private void mediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {           
            try
            {
                timeLineSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                
                int time = (int)(mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds / 60);
                int mod = (int)(mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds % 60);

                
                timeToEnd.Text = time+"."+mod;
                int timestart = (int)(mediaPlayer.Position.TotalSeconds / 60);
                int modstart = (int)(mediaPlayer.Position.TotalSeconds % 60);

                timeFromStart.Text = timestart + "." + modstart;

            }
            catch (Exception)
            {
                MessageBox.Show("Can't get maximum for timeLineSlider");
                //MessageBox.Show(mediaPlayer.NaturalDuration.ToString());
                //MessageBox.Show(Convert.ToString(Convert.ToDouble(mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds.ToString()) / 60));
            }
        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = (double)e.NewValue;
            
        }

        private void mediaPlayer_MouseWheel(object sender, MouseWheelEventArgs e)
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

        private void mediaPlayer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount  ==2)
            {
                if (WindowState != WindowState.Maximized || WindowStyle != WindowStyle.None|| seitenLeiste.ActualWidth != 0)
                {
                    WindowState = WindowState.Maximized;
                    WindowStyle = WindowStyle.None;
                    MouseButtonEventArgs f = null;
                    Button_Click(sender,f);
                    mediaPlayer.Play();
                }
                else
                {
                    WindowStyle = WindowStyle.SingleBorderWindow;
                } 
            }
            

                       
        }
       
    }
}

/*
public IList<string> FileNames
{
    get { return (IList<string>)GetValue(FileNamesProperty); }
    set { SetValue(FileNamesProperty, value); }

public static readonly DependencyProperty FileNamesProperty =
            DependencyProperty.Register("FileNames", typeof(IList<string>), typeof(MainWindow), new PropertyMetadata(null));

        //Holds the currently selected item from the ListBox
        public string SelectedFileName
        {
            get { return  (string)GetValue(SelectedFileNameProperty); }
            set { SetValue(SelectedFileNameProperty, value); }
        }
        public static readonly DependencyProperty SelectedFileNameProperty =
            DependencyProperty.Register("SelectedFileName", typeof(string), typeof(MainWindow), new PropertyMetadata(null, SelectedFileNameChanged));

        //This static method runs whenever the selected item changes (because SelectedItem is bound to SelectedFileName)
        private static void SelectedFileNameChanged(DependencyObject o,DependencyPropertyChangedEventArgs e)
        {
            var window = (MainWindow)o;
            
            Uri uri = new Uri(window.verzeichnis.Text +$"{o.GetValue(e.Property).ToString()}"+".mp4");
            
            
            //MessageBox.Show(uri.ToString());
            window.mediaPlayer.Source = uri;
            //Do something here to handle the change of selection
        }

}*/
