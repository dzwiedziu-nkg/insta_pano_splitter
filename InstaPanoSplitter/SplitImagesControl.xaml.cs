using System;
using System.Collections.Generic;
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

namespace InstaPanoSplitter
{
    /// <summary>
    /// Interaction logic for SplitImagesControl.xaml
    /// </summary>
    public partial class SplitImagesControl : UserControl
    {
        public SplitImagesControl()
        {
            InitializeComponent();
        }

        public void SetSplitImages(ImageProcessing ip)
        {
            StackPanel.Children.Clear();
            for (var i = 0; i < ip.SplitCount(); i++)
            {
                StackPanel.Children.Add(new Image { Source = ip.SplitImage[i], Margin = new Thickness(i == 0 ? 0 : 8, 0, 0, 0)});
            }
        }
    }
}
