using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InstaPanoSplitter
{
    /// <summary>
    /// Interaction logic for SourceImageControl.xaml
    /// </summary>
    public partial class SourceImageControl : UserControl
    {
        

        public SourceImageControl()
        {
            InitializeComponent();
            DataContext = null;
        }

        public void SetImage(ImageProcessing ip)
        {
            DataContext = new DataContextObj(ip);
        }
    }
    public class DataContextObj
    {
        private readonly ImageProcessing _ip;

        public Bitmap SourceImage => _ip.Bitmap;
        public BitmapImage Image => _ip.SourceBitmapImage();
        public string Size => $"Size: {SourceImage.Width}x{SourceImage.Height}";
        public string Ratio => $"Ratio: 1:{((decimal)SourceImage.Width / SourceImage.Height):0.000}";

        public string Conclusion => $"Will be divided into: {_ip.SplitCount()} images {SourceImage.Height}x{SourceImage.Height}px";

        public DataContextObj(ImageProcessing ip)
        {
            _ip = ip;
        }
    }
}
