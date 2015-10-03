using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Drawing.Point;

namespace pixel_place
{
    public class CurvesHandler
    {
        private readonly Canvas _canvas;

        public CurvesHandler(Canvas canvas)
        {
            _canvas = canvas;
        }

        private IList<Point> _points = new List<Point>
        {
            new Point(0, 0),
            new Point(200, 200)
        };

        private readonly ICollection<Line> _gridLines = new List<Line>();
        private const int GridSections = 4;

        private readonly ICollection<UIElement> _curveComponents = new List<UIElement>();

        public void Init()
        {
            for (var i = 1; i < 4; ++i)
            {
                var latitude = new Line
                {
                    Stroke = Brushes.DarkGray,
                    StrokeThickness = 1,
                    X1 = (_canvas.Width / GridSections) * i,
                    X2 = (_canvas.Width / GridSections) * i,
                    Y1 = 0,
                    Y2 = _canvas.Height
                };
                _gridLines.Add(latitude);
                _canvas.Children.Add(latitude);
            }
            for (var i = 1; i < 4; ++i)
            {
                var longitude = new Line
                {
                    Stroke = Brushes.DarkGray,
                    StrokeThickness = 1,
                    X1 = 0,
                    X2 = _canvas.Width,
                    Y1 = (_canvas.Height / GridSections) * i,
                    Y2 = (_canvas.Height / GridSections) * i
                };
                _gridLines.Add(longitude);
                _canvas.Children.Add(longitude);
            }

            RenderCurves();

            _canvas.MouseDown += _canvas_MouseDown;
        }

        private void RenderCurves()
        {
            foreach (var c in _curveComponents)
            {
                _canvas.Children.Remove(c);
            }
            _curveComponents.Clear();

            for (var i = 1; i < _points.Count; ++i)
            {
                var component = new Line
                {
                    Stroke = Brushes.SteelBlue,
                    StrokeThickness = 2,
                    X1 = _points[i - 1].X,
                    Y1 = _canvas.Height - _points[i - 1].Y,
                    X2 = _points[i].X,
                    Y2 = _canvas.Height - _points[i].Y
                };

                _curveComponents.Add(component);
                _canvas.Children.Add(component);

                var pointSize = 3;

                var point = new Rectangle
                {
                    Width = pointSize,
                    Height = pointSize,
                    Fill = Brushes.DarkGray,
                    Stroke = null
                };

                var pointOffset = pointSize / 2;

                _curveComponents.Add(point);
                _canvas.Children.Add(point);
                Canvas.SetLeft(point, _points[i].X - pointOffset);
                Canvas.SetBottom(point, _points[i].Y - pointOffset);
            }
        }

        void _canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var coords = e.GetPosition(_canvas);
            var position = 0;
            for (var i = 0; i < _points.Count; ++i)
            {
                if (coords.X > _points[i].X)
                {
                    position++;
                }
                else
                {
                    break;
                }
            }
            _points.Insert(position, new Point((int)coords.X, (int)_canvas.Height - (int)coords.Y));
            RenderCurves();
        }
    }
}
