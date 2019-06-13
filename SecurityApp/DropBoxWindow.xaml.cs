using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace SecurityApp
{
    /// <summary>
    /// Логика взаимодействия для DropBoxWindow.xaml
    /// </summary>
         
    public partial class DropBoxWindow : Window
    {
        private string _token = "nBfQeK-E5gAAAAAAAAAACi9jxY30l4AURAIhSMxSNgWxmb12LJ8BFNbwpOuu2GQ2";

        public DropBoxWindow()
        {
            InitializeComponent();
        }

        private async void LoadFileButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(filePathTextBox.Text))
            {
                MessageBox.Show("Введите название файла");
                return;
            }
            if (!File.Exists(filePathTextBox.Text))
            {
                MessageBox.Show("Файл не существует");
                return;
            }
            using (var dbx = new DropboxClient(_token))
            {
                await Upload(filePathTextBox.Text);
            }
        }

        private async Task Upload(string file)
        {
            string fileName = file.Remove(0, file.LastIndexOf("\\") + 1);
            string folder = "";

            using (DropboxClient dbx = new DropboxClient(_token))
            using (var memoryStream = new MemoryStream(File.ReadAllBytes(file)))
            {
                var updated = await dbx.Files.UploadAsync(
                    folder + "/" + fileName,
                    WriteMode.Overwrite.Instance,
                    body: memoryStream);
            }
        }

        private void ChooseFileButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            filePathTextBox.Text = fileDialog.FileName;
        }
    }
}