using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;
using System.IO;



namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool isPlaying = false;
        public bool isPaused = false;
        public bool isStopped = false;
        public bool isDragging = false;
        public DispatcherTimer timer;
        public Visibility imageOff = Visibility.Collapsed;
        public Visibility imageOn= Visibility.Visible;
        public Microsoft.Win32.OpenFileDialog dlg;

        int seconds;

        public MainWindow()
        {
            InitializeComponent();
            seconds = videoMediaElement.Position.Seconds;
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            dlg = new Microsoft.Win32.OpenFileDialog();

            //initially disable visibility of mp3 pic
            mp3Image.Visibility = imageOff;
            restoreButton.Visibility = imageOff;
        }

     
        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            // switches to turn other buttons off when one button is clicked
            isPlaying = true;
            isStopped = false;
            isPaused = false;
            
            // if text box for directory is empty display enter a valid media file
            if (browseTextBox.Text == "")
            {
                MessageBox.Show("Enter a valid media file");
            }

            // if text box is not empty play the current  media file
            else if (browseTextBox.Text != "")
            {
                videoMediaElement.Play();

                TimeSpan ts = videoMediaElement.NaturalDuration.TimeSpan;
                seekBarSlider.Maximum = ts.TotalSeconds;
            }

            //set the current position to update when the current position of media changes
           currentPositionLabel.Content = videoMediaElement.Position.ToString();
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            isPaused = true;
            isStopped = false;
            isPlaying = false;

            //pause the current video position
            videoMediaElement.Pause();
            seconds = videoMediaElement.Position.Seconds;
        }

      
       private void timer_Tick(object sender, EventArgs e)
    	{
           // if the seek bar is being dragged syncronize the current position with drag position
	        if (!isDragging)
	        {
	            seekBarSlider.Value = videoMediaElement.Position.TotalSeconds;                
	        }
	    }

        private void seekBarSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isDragging = true;

            //keeps track of the seekbar Slider's maximum as the seekbar is being dragged 
            TimeSpan ts = videoMediaElement.NaturalDuration.TimeSpan;
            seekBarSlider.Maximum = ts.TotalSeconds;
        }

        private void seekBarSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            // not being dragged
            isDragging = false;
            
            // set position equal to drag complete position
            videoMediaElement.Position = TimeSpan.FromSeconds(seekBarSlider.Value);
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            videoMediaElement.Volume = (double)volumeSlider.Value;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // escape program with escape key
            if (e.KeyboardDevice.GetKeyStates(Key.Escape)==KeyStates.Down)
            {
                Close();
            }
        }

        private void fullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            //disable when clicked
            fullScreenButton.IsEnabled = false;
            restoreButton.IsEnabled = true;
            restoreButton.Visibility = imageOn;
            fullScreenButton.Visibility = imageOff;


            // fully maxamize the form
            // hide task bar
            this.ShowInTaskbar = false;
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
           
           //adjust length and width of video element
           videoMediaElement.Height = 700;
           videoMediaElement.Width = this.Width-50;

           //seekBarSlider.Margin = new Thickness(150, 700, 500, 0);
           seekBarSlider.Margin = new Thickness(43, 714,100, 0);

           //change position of controls in full screen
           rewindButton1.Margin = new Thickness(157, 0, 0, 74);
           playButtonIcon.Margin = new Thickness(250, 0, 970, 71);
           pauseButtonIcon.Margin = new Thickness(352, 0, 0, 71);
           stopButton1.Margin = new Thickness(439, 0, 0, 70);
           fastFowardButton.Margin = new Thickness(520, 0,1090, 71);
           seekBarSlider.Margin = new Thickness(230, 760, 0, 0);
           volumeLabel.Margin = new Thickness(620, 794, 195, 0);
           volumeSlider.Margin = new Thickness(692, 799, 0, 0);
           currentPositionLabel.Margin = new Thickness(774, 828, 0, 0);
           restoreButton.Margin = new Thickness(800, 799, 0, 0);

            //resize video element height and width
            videoBorder.Height = videoMediaElement.Height;
            videoBorder.Width = videoMediaElement.Width-20;

            
        }

        private void seekBarSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan ts = TimeSpan.FromSeconds(e.NewValue);
             currentPositionLabel.Content=
               String.Format("{0:00}:{1:00}:{2:00}",
          
               ts.Hours, ts.Minutes, ts.Seconds);
        }

      
        private void restoreButton_Click(object sender, RoutedEventArgs e)
        {
            //disable button and set visibility
            restoreButton.IsEnabled = false;
            fullScreenButton.IsEnabled = true;
            fullScreenButton.Visibility = imageOn;
            restoreButton.Visibility = imageOff;
            

            //set the window to single border window in normal 
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.WindowState = WindowState.Normal;

            // set the new width of video width and height 
            videoMediaElement.Width = 860;
            videoMediaElement.Height = 534;
            videoBorder.Width = 860;
            videoBorder.Height = 534;

            //set original margin of controls
            seekBarSlider.Margin = new Thickness(43, 592, 0, 0);
            rewindButton1.Margin = new Thickness(157,0,0,236);
            playButtonIcon.Margin = new Thickness(261, 0, 0, 233);
            pauseButtonIcon.Margin = new Thickness(352, 0, 0, 233);
            stopButton1.Margin = new Thickness(439, 0, 0, 232);
            fastFowardButton.Margin = new Thickness(0, 0, 347, 233);
            volumeLabel.Margin = new Thickness(595, 622, 0, 0);
            volumeSlider.Margin = new Thickness(668, 622, 0, 0);
            currentPositionLabel.Margin = new Thickness(637, 652, 0, 0);
        }

        private void propertiesButton_Click(object sender, RoutedEventArgs e)
        {
            String location =dlg.FileName.ToString();
            FileInfo mp3Info = new FileInfo(location);
            long f = mp3Info.Length;
            
            TimeSpan ts=videoMediaElement.NaturalDuration.TimeSpan;
            // Title, Location, FileSize, Length, 
            MessageBox.Show("Title: "+mp3Info.Name + "\n" + "Location: "+ location + "\n"+"File Size: "+f +"\n"+ "Length:"+ String.Format("{0:00}:{1:00}:{2:00}",

                ts.Hours, ts.Minutes, ts.Seconds));
            
           // String author;
            //String Artist;
            //String Album;
        }

        private void playButtonIcon_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			BitmapImage image = new BitmapImage();
   			image.BeginInit();
			image.UriSource = new Uri(@"..\Icons\Play-Hot-icon.png", UriKind.Relative);
			image.EndInit();
			this.playButtonIcon.Source = image;
        }

        private void playButtonIcon_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			BitmapImage image = new BitmapImage();
   			image.BeginInit();
			image.UriSource = new Uri(@"..\Icons\Play-Normal-icon.png", UriKind.Relative);
			image.EndInit();
			this.playButtonIcon.Source = image;
        }

      

        private void playButtonIcon_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			
			BitmapImage image = new BitmapImage();
   			image.BeginInit();
			image.UriSource = new Uri(@"..\Icons\Play-Pressed-icon.png", UriKind.Relative);
			image.EndInit();
			this.playButtonIcon.Source = image;
			
			isPlaying = true;
            isStopped = false;
            isPaused = false;

            // checks empty text box display message box
            if (browseTextBox.Text == "")
            {
                MessageBox.Show("Enter a valid media file");
            }

            // play if file is entered
            else if (browseTextBox.Text != "")
            {
                videoMediaElement.Play();
            }

           currentPositionLabel.Content = videoMediaElement.Position.ToString();
        }

        private void pauseButtonIcon_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			 isPaused = true;
            isStopped = false;
            isPlaying = false;

            videoMediaElement.Pause();
            seconds = videoMediaElement.Position.Seconds;
			
        }

        private void stopButton1_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			isStopped = true;
            isPaused = false;
            isPlaying = false;

            // stops the video 
            videoMediaElement.Stop();
        }

        private void fastFowardButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	// TODO: Add event handler implementation here.

            // increments the current video position by 5 seconds
			videoMediaElement.Position = videoMediaElement.Position.Add(new TimeSpan(0, 0, 5));        
        }

        private void rewindButton1_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	  //TODO: Add event handler implementation here.

              // decrements the current video positon by 5 seconds
			  videoMediaElement.Position = videoMediaElement.Position.Add(new TimeSpan(0, 0, -5));
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // filters all media files
            dlg.Filter = "Media Files|*.wmv;*.mp3;*.flv;*";
            dlg.ShowDialog();
            
            //displays the file location in the text box
            browseTextBox.Text = dlg.FileName;

            // when the file is opened play the video or mp3
            if (browseTextBox.Text != "")
            {
                videoMediaElement.Source = new Uri(browseTextBox.Text);
                videoMediaElement.Play();

                currentPositionLabel.Content += videoMediaElement.Position.ToString();
                isPlaying = true;

                // if filename ends with mp3 show the mp3 image
                if (dlg.FileName.Substring(dlg.FileName.Length - 3) == "mp3")
                {
                    mp3Image.Visibility = imageOn;
                }

                //else turn the image off make invisible
                else
                {
                    mp3Image.Visibility = imageOff;
                }

            }
            else
            {
                //do nothing
            }


            if (videoMediaElement.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = videoMediaElement.NaturalDuration.TimeSpan;
                seekBarSlider.Maximum = ts.TotalSeconds;
                seekBarSlider.SmallChange = 1;
                seekBarSlider.LargeChange = Math.Min(10, ts.Seconds / 2);

                currentPositionLabel.Content = String.Format("00:00:00");
                currentPositionLabel.Content = String.Format("{0:00}:{1:00}:{2:00}",

                    ts.Hours, ts.Minutes, ts.Seconds);
            }
            timer.Start();
        }
       
        private void MediaPropertiesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // display the music and video elements properties
            String location = dlg.FileName.ToString();
            MusicID3Tag musicTag = new MusicID3Tag(location);
            FileInfo fileInfo = new FileInfo(location);

            long f = fileInfo.Length;

            TimeSpan ts = videoMediaElement.NaturalDuration.TimeSpan;
            // Title, Location, FileSize, Length, 
          
            // if mp3 file is currently playing show properties of file
            if(dlg.FileName.Substring(dlg.FileName.Length - 3) == "mp3")
            {
                MessageBox.Show(musicTag.GetMP3Info());
            }

            // if video is present show properties of video
            else
            {
                MessageBox.Show("Title: " + fileInfo.Name + "\n" + "Location: " + location + "\n" + "File Size: " + f + "\n" + "Length:" + String.Format("{0:00}:{1:00}:{2:00}",

                ts.Hours, ts.Minutes, ts.Seconds)+System.Environment.NewLine);
            }
        }

            private void WolfWallpaper_Click(object sender, RoutedEventArgs e)
            {
                ImageSource image = new BitmapImage(new Uri("wolf.jpg", UriKind.Relative));
                System.Windows.Media.ImageBrush dk=new ImageBrush(image);

                Grid wolfGrid = new Grid();
                myGrid.Background =new ImageBrush(image);
               
            }

            private void SamuriWallpaper_Click(object sender, RoutedEventArgs e)
            {
                ImageSource image = new BitmapImage(new Uri("Samurai.jpg", UriKind.Relative));
                System.Windows.Media.ImageBrush dk = new ImageBrush(image);

                Grid wolfGrid = new Grid();
                myGrid.Background = new ImageBrush(image);
            }

            private void GlassWallpaper_Click(object sender, RoutedEventArgs e)
            {
                ImageSource image = new BitmapImage(new Uri("glass-rose.jpg", UriKind.Relative));
                System.Windows.Media.ImageBrush dk = new ImageBrush(image);

                Grid wolfGrid = new Grid();
                myGrid.Background = new ImageBrush(image);
            }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Close the media player
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
