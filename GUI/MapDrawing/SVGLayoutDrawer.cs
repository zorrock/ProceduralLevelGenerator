﻿namespace GUI.MapDrawing
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using GeneralAlgorithms.Algorithms.Polygons;
	using GeneralAlgorithms.DataStructures.Common;
	using GeneralAlgorithms.DataStructures.Polygons;
	using MapGeneration.Core.Interfaces;

	/// <inheritdoc />
	/// <summary>
	/// Class to draw a layout as an SVG.
	/// </summary>
	/// <typeparam name="TNode"></typeparam>
	public class SVGLayoutDrawer<TNode> : AbstractLayoutDrawer<TNode>
	{
		private readonly CachedPolygonPartitioner polygonPartitioner = new CachedPolygonPartitioner();
		private readonly StringBuilder data = new StringBuilder();

		/// <summary>
		/// Draws a given layout and returns a string with SVG data.
		/// </summary>
		/// <param name="layout"></param>
		/// <param name="width">Result will have this width and height will be computed to match the layout.</param>
		/// <returns></returns>
		public string DrawLayout(IMapLayout<TNode> layout, int width)
		{
			if (width <= 0)
				throw new ArgumentException("Width must be greater than zero.", nameof(width));

			var ratio = GetWidthHeightRatio(layout);
			var height = (int) (width / ratio);

			data.AppendLine($"<svg width=\"{width}\" height=\"{height}\" xmlns=\"http://www.w3.org/2000/svg\">");

			DrawLayout(layout, width, height, true);

			data.AppendLine("</svg>");

			var svgData = data.ToString();
			data.Clear();

			return svgData;
		}


		private float GetWidthHeightRatio(IMapLayout<TNode> layout)
		{
			var polygons = layout.GetRooms().Select(x => x.Shape + x.Position).ToList();
			var points = polygons.SelectMany(x => x.GetPoints()).ToList();

			var minx = points.Min(x => x.X);
			var miny = points.Min(x => x.Y);
			var maxx = points.Max(x => x.X);
			var maxy = points.Max(x => x.Y);

			var ratio = (maxx - minx) / (float) (maxy - miny);

			return ratio;
		}

		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="polygon"></param>
		/// <param name="outline"></param>
		/// <param name="penWidth"></param>
		protected override void DrawRoom(GridPolygon polygon, List<Tuple<IntVector2, bool>> outline, float penWidth)
		{
			// Draw polygon
			data.Append($"    <polygon points=\"");

			foreach (var point in polygon.GetPoints())
			{
				data.Append($"{point.X},{point.Y} ");
			}

			data.Append($"\" style=\"fill:rgb(211,211,211);stroke:rgb(211,211,211)\" />");
			data.AppendLine();

			// Draw path
			data.Append($"    <path d=\"");

			var lastPoint = outline[outline.Count - 1].Item1;
			data.Append($"M {lastPoint.X} {lastPoint.Y} ");

			foreach (var pair in outline)
			{
				var point = pair.Item1;

				data.Append(pair.Item2 ? $"L {point.X} {point.Y} " : $"M {point.X} {point.Y} ");
			}

			data.Append($"\" fill=\"none\" stroke=\"black\" stroke-linecap=\"square\" stroke-width=\"{penWidth}\" />");
			data.AppendLine();
		}

		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="polygon"></param>
		/// <param name="text"></param>
		/// <param name="penWidth"></param>
		protected override void DrawTextOntoPolygon(GridPolygon polygon, string text, float penWidth)
		{
			var partitions = polygonPartitioner.GetPartitions(polygon);
			var biggestRectangle = partitions.OrderByDescending(x => x.Width).First();

			data.AppendLine(
				$"    <text x=\"{biggestRectangle.Center.X}\" y=\"{biggestRectangle.Center.Y}\" alignment-baseline=\"middle\" text-anchor=\"middle\" font-size=\"{(int) (1.2 * penWidth)}\">{text}</text>");
		}
	}
}