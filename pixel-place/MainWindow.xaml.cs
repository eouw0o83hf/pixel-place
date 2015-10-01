﻿using System;
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

namespace pixel_place
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
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
            using (var instream = new MemoryStream(imageBytes))
            {
                using (var outstream = new MemoryStream())
                {
                    using (var factory = new ImageFactory())
                    {
                        factory.Load(instream)
                            .Rasterize()
                            .Save(outstream);
                    }
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
    }

    public static class ImageFactoryExtensions
    {
        public static ImageFactory Rasterize(this ImageFactory factory)
        {
            using(var bmp = new FastBitmap(factory.Image))
            for (var i = 0; i < factory.Image.Height; ++i)
            {
                for (var j = 0; j < factory.Image.Width; ++j)
                {
                    var pixel = bmp.GetPixel(j, i);
                    if (pixel.R + pixel.G + pixel.B > 350)
                    {
                        bmp.SetPixel(j, i, System.Drawing.Color.FromArgb(200, 255, 255));
                    }
                    else
                    {
                        bmp.SetPixel(j, i, System.Drawing.Color.FromArgb(255, 0, 0, 30));
                    }
                }
            }
            return factory;
        }
    }
}
