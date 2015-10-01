using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor.Imaging;

namespace pixel_place.Filters
{
    public class PosterizationFilter : IImageFilter
    {
        public void ApplyFilter(FastBitmap image)
        {
            for (var i = 0; i < image.Height; ++i)
            {
                for (var j = 0; j < image.Width; ++j)
                {
                    var pixel = image.GetPixel(j, i);
                    if (pixel.R + pixel.G + pixel.B > 350)
                    {
                        image.SetPixel(j, i, System.Drawing.Color.FromArgb(200, 255, 255));
                    }
                    else
                    {
                        image.SetPixel(j, i, System.Drawing.Color.FromArgb(255, 0, 0, 30));
                    }
                }
            }
        }
    }
}
