using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor.Imaging;

namespace pixel_place.Filters
{
    public interface IImageFilter
    {
        void ApplyFilter(FastBitmap image);
    }
}
