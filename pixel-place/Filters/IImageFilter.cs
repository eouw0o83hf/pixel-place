using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Imaging;
using System.Drawing;

namespace pixel_place.Filters
{
    public interface IImageFilter
    {
        void ApplyFilter(FastBitmap image);
    }

    public static class FilterExtensions
    {
        public static void SetPixel(this FastBitmap bmp, int x, int y, int r, int g, int b)
        {
            bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
        }
    }
}
