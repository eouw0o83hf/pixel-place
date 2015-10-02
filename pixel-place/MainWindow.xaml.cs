using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Filters.Photo;
using pixel_place.Filters;

namespace pixel_place
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICollection<IImageFilter> _filters;

        public MainWindow(ICollection<IImageFilter> filters)
        {
            _filters = filters;

            InitializeComponent();
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();

            if (!(dialog.ShowDialog() ?? false))
            {
                return;
            }

            var imageBytes = File.ReadAllBytes(dialog.FileName);
            Image image;
            using (var factory = new ImageFactory())
            {
                using (var instream = new MemoryStream(imageBytes))
                {
                    factory.Load(instream);
                }

                ApplyFilter<BlurFilter>(factory);

                using (var outstream = new MemoryStream())
                {
                    factory.Save(outstream);
                    image = Image.FromStream(outstream);
                }
            }

            //var image = Image.FromFile(dialog.FileName);
            var bmp = new Bitmap(image);
            MainImage.Width = image.Width;
            MainImage.Height = image.Height;

            var bmpSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bmp.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            MainImage.Source = bmpSource;
        }

        private ImageFactory ApplyFilter<T>(ImageFactory factory)
            where T : IImageFilter
        {
            var target = _filters.OfType<T>().Single();
            using (var bmp = new FastBitmap(factory.Image))
            {
                target.ApplyFilter(bmp);
            }
            return factory;
        }
    }
}
