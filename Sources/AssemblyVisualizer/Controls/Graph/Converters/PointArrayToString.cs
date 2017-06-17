﻿// Adopted, originally created as part of GraphSharp project
// This code is distributed under Microsoft Public License 
// (for details please see \docs\Ms-PL)

using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace AssemblyVisualizer.Controls.Graph.Converters
{
	public class PointArrayToString : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var points = value as Point[];

			if (points == null)
				return string.Empty;
			var sb = new StringBuilder();
			foreach (var point in points)
				sb.AppendLine(point.ToString());
			return sb;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}