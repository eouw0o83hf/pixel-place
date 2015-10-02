using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor.Imaging;

namespace pixel_place.Filters
{
    public class BlurFilter : IImageFilter
    {
        public void ApplyFilter(FastBitmap image)
        {
            const int blurRadius = 5;
            var map = new List<Color>[image.Width, image.Height];
            for (var i = 0; i < image.Width; ++i)
            {
                for (var j = 0; j < image.Height; ++j)
                {
                    for (var iInner = -blurRadius; iInner < blurRadius; ++iInner)
                    {
                        var x = i + iInner;
                        if (x < 0)
                        {
                            continue;
                        }
                        if (x >= image.Width)
                        {
                            break;
                        }

                        for (var jInner = -blurRadius; jInner < blurRadius; ++jInner)
                        {
                            var y = j + jInner;
                            if (y < 0)
                            {
                                continue;
                            }
                            if (y >= image.Height)
                            {
                                break;
                            }

                            var distance = Math.Sqrt((iInner*iInner) + (jInner*jInner));
                            if (distance > blurRadius)
                            {
                                continue;
                            }

                            if (map[i, j] == null)
                            {
                                map[i, j] = new List<Color>();
                            }
                            map[i, j].Add(image.GetPixel(x, y));
                        }
                    }
                }
            }

            for (var i = 0; i < image.Width; ++i)
            {
                for (var j = 0; j < image.Height; ++j)
                {
                    var r = (int)map[i, j].Average(a => a.R);
                    var g = (int)map[i, j].Average(a => a.G);
                    var b = (int)map[i, j].Average(a => a.B);

                    image.SetPixel(i, j, r, g, b);
                }
            }
        }
    }
}
