using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyDartsManager.Helper
{
    /// <summary>
    /// Helper class used to calculate points in 2D space and create Paths of certain shape.
    /// Class is used to create dartboard.
    /// </summary>
    public static class GUIComponentHelper
    {
        /// <summary>
        /// Used to calculate points in 2D space based on given radius and angle.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Point PolarToCartesianPoint(int radius, int angle)
        {
            double x = Math.Cos(Math.PI / 180 * angle) * radius;
            double y = Math.Sin(Math.PI / 180 * angle) * radius;

            return new Point((int)x, (int)y);
        }

        /// <summary>
        /// Used to calculate points in 2D space based on given radius and angle and an offset point.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Point PolarToCartesianPoint(int radius, int angle, Point offset)
        {
            double x = Math.Cos(Math.PI / 180 * angle) * radius;
            double y = Math.Sin(Math.PI / 180 * angle) * radius;

            return new Point((int)x + offset.X, (int)y + offset.Y);
        }

        /// <summary>
        /// Method used to create rectangular shapes with curved sides.
        /// This method is used to create curved rectangles on the dartboard representing double/triple multiplier throws (f.e. 2*20 or 3*20).
        /// </summary>
        /// <param name="angle1">angle of first pair of points on the 2 circles</param>
        /// <param name="angle2">angle of second pair of points on the 2 circles</param>
        /// <param name="radius1">radius of first circle</param>
        /// <param name="radius2">radius of second circle</param>
        /// <param name="offset">center point of circles (of dartboard)</param>
        /// <returns></returns>
        public static Path createCurve(int angle1, int angle2, int radius1, int radius2, Point offset)
        {
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = PolarToCartesianPoint(radius1, angle1, offset);

            pathFigure.Segments.Add(
                new BezierSegment(
                    PolarToCartesianPoint(radius1 + 1, ((angle2 - angle1) / 3 + angle1), offset),
                    PolarToCartesianPoint(radius1 + 1, ((angle2 - angle1) / 3 * 2 + angle1), offset),
                    PolarToCartesianPoint(radius1, angle2, offset),
                    true));

            pathFigure.Segments.Add(
                new LineSegment(PolarToCartesianPoint(radius2, angle2, offset), true));

            pathFigure.Segments.Add(
                new BezierSegment(
                    PolarToCartesianPoint(radius2 + 1, ((angle2 - angle1) / 3 + angle1), offset),
                    PolarToCartesianPoint(radius2 + 1, ((angle2 - angle1) / 3 * 2 + angle1), offset),
                    PolarToCartesianPoint(radius2, angle1, offset),
                    true));

            pathFigure.Segments.Add(
                new LineSegment(PolarToCartesianPoint(radius1, angle1, offset), true));

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            Path path = new Path();
            path.Stroke = Brushes.Silver;
            path.StrokeThickness = 2;
            path.Data = pathGeometry;
            path.Fill = Brushes.White;

            return path;
        }

        /// <summary>
        /// Method used to create triangular shape with 1 curved side.
        /// This method is used to create slices of the dartboard representing single multiplier throws (f.e. 1 * 20).
        /// </summary>
        /// <param name="angle1">angle of the first point on the circle</param>
        /// <param name="angle2">angle of the second point on the circle</param>
        /// <param name="radius">radius of the circle</param>
        /// <param name="offset">offset point (center of dartboard)</param>
        /// <returns></returns>
        public static Path createSlice(int angle1, int angle2, int radius, Point offset)
        {
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = PolarToCartesianPoint(radius, angle1, offset);

            pathFigure.Segments.Add(
                new BezierSegment(
                    PolarToCartesianPoint(radius + 1, ((angle2 - angle1) / 3 + angle1), offset),
                    PolarToCartesianPoint(radius + 1, ((angle2 - angle1) / 3 * 2 + angle1), offset),
                    PolarToCartesianPoint(radius, angle2, offset),
                    true));

            pathFigure.Segments.Add(
                new LineSegment(PolarToCartesianPoint(0, 0, offset), true));

            pathFigure.Segments.Add(
                new LineSegment(PolarToCartesianPoint(radius, angle1, offset), true));

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            Path path = new Path();
            path.Stroke = Brushes.Silver;
            path.StrokeThickness = 2;
            path.Data = pathGeometry;
            path.Fill = Brushes.White;

            return path;
        }

        /// <summary>
        /// Method used to create circle of certain radius around a point.
        /// Used to create centers of dartboard and the outer 0 point circle.
        /// </summary>
        /// <param name="center">center point of the circle</param>
        /// <param name="radius">radius of the circle</param>
        /// <returns></returns>
        public static Path createCircle(Point center, int radius)
        {
            EllipseGeometry geometry = new EllipseGeometry();
            geometry.RadiusX = radius;
            geometry.RadiusY = radius;
            geometry.Center = center;

            Path path = new Path();
            path.Data = geometry;
            path.Stroke = Brushes.Silver;
            path.StrokeThickness = 2;
            path.Fill = Brushes.White;

            return path;
        }
    }
}
