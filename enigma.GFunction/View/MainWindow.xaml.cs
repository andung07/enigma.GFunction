using System.Windows;
using System.Windows.Controls;
using enigma.GFunction.ViewModel;
using Microsoft.Win32;
using enigma.GFunction.Model;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;

namespace enigma.GFunction.View
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            Messenger.Default.Register<NotificationMessage>(this, message =>
            {
                MessageBox.Show(message.Notification);
            });
        }

        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            openFileDialog.DefaultExt = ".csv";

            if (openFileDialog.ShowDialog(this) == true)
            {
                var filePath = openFileDialog.InitialDirectory + openFileDialog.FileName;
                Messenger.Default.Send(new MessageObject
                {
                    Id = "FilePath",
                    Content = filePath.ToString()
                }); 
             
            }
        }
        private void FileOpen_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var newSize = new WindowSize { Width = Convert.ToInt16(e.NewSize.Width), Height = Convert.ToInt16(e.NewSize.Height) };
            Messenger.Default.Send(newSize);
        }    

    }
}