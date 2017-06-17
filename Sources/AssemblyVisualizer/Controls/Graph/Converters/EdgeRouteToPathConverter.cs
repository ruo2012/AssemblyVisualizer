﻿// Adopted, originally created as part of GraphSharp project
// This code is distributed under Microsoft Public License 
// (for details please see \docs\Ms-PL)

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AssemblyVisualizer.Controls.Graph.Converters
{
	/// <summary>
	///     Converts the position and sizes of the source and target points, and the route informations
	///     of an edge to a path.
	///     The edge can bend, or it can be straight line.
	/// </summary>
	public class EdgeRouteToPathConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Debug.Assert(values != null && values.Length == 9,
				"EdgeRouteToPathConverter should have 9 parameters: pos (1,2), size (3,4) of source; pos (5,6), size (7,8) of target; routeInformation (9).");

			#region Get the inputs

			//get the position of the source
			var sourcePos = new Point
			{
				X = values[0] != DependencyProperty.UnsetValue ? (double) values[0] : 0.0,
				Y = values[1] != DependencyProperty.UnsetValue ? (double) values[1] : 0.0
			};
			//get the size of the source
			var sourceSize = new Size
			{
				Width = values[2] != DependencyProperty.UnsetValue ? (double) values[2] : 0.0,
				Height = values[3] != DependencyProperty.UnsetValue ? (double) values[3] : 0.0
			};
			//get the position of the target
			var targetPos = new Point
			{
				X = values[4] != DependencyProperty.UnsetValue ? (double) values[4] : 0.0,
				Y = values[5] != DependencyProperty.UnsetValue ? (double) values[5] : 0.0
			};
			//get the size of the target
			var targetSize = new Size
			{
				Width = values[6] != DependencyProperty.UnsetValue ? (double) values[6] : 0.0,
				Height = values[7] != DependencyProperty.UnsetValue ? (double) values[7] : 0.0
			};

			//get the route informations
			var routeInformation = values[8] != DependencyProperty.UnsetValue ? (Point[]) values[8] : null;

			#endregion

			var hasRouteInfo = routeInformation != null && routeInformation.Length > 0;

			//
			// Create the path
			//
			var p1 = GraphConverterHelper.CalculateAttachPoint(sourcePos, sourceSize,
				hasRouteInfo ? routeInformation[0] : targetPos);
			var p2 = GraphConverterHelper.CalculateAttachPoint(targetPos, targetSize,
				hasRouteInfo ? routeInformation[routeInformation.Length - 1] : sourcePos);


			var segments = new PathSegment[1 + (hasRouteInfo ? routeInformation.Length : 0)];
			if (hasRouteInfo)
				//append route points
				for (var i = 0; i < routeInformation.Length; i++)
					segments[i] = new LineSegment(routeInformation[i], true);

			var pLast = hasRouteInfo ? routeInformation[routeInformation.Length - 1] : p1;
			var v = pLast - p2;
			v = v / v.Length * 5;
			var n = new Vector(-v.Y, v.X) * 0.3;

			segments[segments.Length - 1] = new LineSegment(p2 + v, true);

			var pfc = new PathFigureCollection(2);
			pfc.Add(new PathFigure(p1, segments, false));
			pfc.Add(new PathFigure(p2,
				new PathSegment[]
				{
					new LineSegment(p2 + v - n, true),
					new LineSegment(p2 + v + n, true)
				}, true));

			return pfc;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}