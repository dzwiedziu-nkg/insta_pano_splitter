using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
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
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Win32;
using Path = System.IO.Path;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace InstaPanoSplitter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageProcessing _ip;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var src = SourceFile.Text;
                var ip = new ImageProcessing(src);
                ImgSource.SetImage(ip);
                ImgDest.SetSplitImages(ip);

                var path = Path.GetDirectoryName(src);
                DestDir.Text = path;
                SaveButton.IsEnabled = true;
                _ip = ip;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error loading file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var w = new CommonOpenFileDialog();
            if (w.ShowDialog(this) != CommonFileDialogResult.Ok) return;

            SourceFile.Text = w.FileName;
            Load_Click(sender, e);
        }

        private void BrowseDest_Click(object sender, RoutedEventArgs e)
        {
            var w = new CommonOpenFileDialog();
            w.IsFolderPicker = true;
            if (w.ShowDialog(this) != CommonFileDialogResult.Ok) return;
            DestDir.Text = w.FileName;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var exists = new List<string>();
                var all = new List<string>();

                for (var i = 0; i < _ip.SplitCount(); i++)
                {
                    var f = _ip.GetDestFileName(DestDir.Text, i);
                    if (File.Exists(f))
                    {
                        exists.Add(f);
                    }
                    all.Add(f);
                }

                if (exists.Count > 0)
                {
                    var ret = MessageBox.Show(this, $"Files exists:\n{string.Join('\n', exists)}\n\nOverride?",
                        "Destination file exists, override?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (ret == MessageBoxResult.No) return;
                }

                _ip.SaveSplit(DestDir.Text);
                MessageBox.Show(this, $"Images saved to:\n{string.Join('\n', all)}", "Done", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error loading file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
