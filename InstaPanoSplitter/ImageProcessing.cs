using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace InstaPanoSplitter
{
    public class ImageProcessing
    {
        private string _origFileName;

        public Bitmap Bitmap { get; }
        public Bitmap[] Split { get; }
        public BitmapImage[] SplitImage { get; }

        public ImageProcessing(string file)
        {
            _origFileName = file;
            Bitmap = LoadBitmap(file);
            Split = new Bitmap[SplitCount()];
            SplitImage = new BitmapImage[SplitCount()];
            SplitImages();
        }

        public BitmapImage SourceBitmapImage()
        {
            return new BitmapImage(new Uri(_origFileName));
        }

        public int SplitCount()
        {
            return (int)Math.Ceiling((decimal)Bitmap.Width / Bitmap.Height);
        }

        public string GetDestFileName(string path, int no)
        {
            return $"{path}{Path.DirectorySeparatorChar}{Path.GetFileNameWithoutExtension(_origFileName)}_{no+1:D2}.jpg";
        }

        public void SaveSplit(string destDirText)
        {
            for (var i = 0; i < SplitCount(); i++)
            {
                SaveBitmap(Split[i], GetDestFileName(destDirText, i));
            }
        }

        private void SplitImages()
        {
            for (var i = 0; i < SplitCount(); i++)
            {
                Split[i] = new Bitmap(Bitmap.Height, Bitmap.Height, Bitmap.PixelFormat);
                using (var grD = Graphics.FromImage(Split[i]))
                {
                    grD.DrawImage(Bitmap, 0, 0, new Rectangle(i * Bitmap.Height, 0, (i+1) * Bitmap.Height, Bitmap.Height), GraphicsUnit.Pixel);
                }
                SplitImage[i] = ExportBitmapImage(Split[i]);
            }
        }

        private static Bitmap LoadBitmap(string file)
        {
            using Stream bitmapStream = File.Open(file, FileMode.Open);
            var img = Image.FromStream(bitmapStream);
            return new Bitmap(img);
        }

        private static void SaveBitmap(Bitmap bitmap, string file)
        {
            using Stream bitmapStream = File.Open(file, FileMode.Create);
            bitmap.Save(bitmapStream, ImageFormat.Jpeg);
        }

        private static BitmapImage ExportBitmapImage(Image bitmap)
        {
            using var memory = new MemoryStream();
            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }

        
    }
}
