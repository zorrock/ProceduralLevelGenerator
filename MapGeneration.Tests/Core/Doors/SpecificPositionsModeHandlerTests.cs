﻿namespace MapGeneration.Tests.Core.Doors
{
	using System.Collections.Generic;
	using GeneralAlgorithms.Algorithms.Common;
	using GeneralAlgorithms.DataStructures.Common;
	using GeneralAlgorithms.DataStructures.Polygons;
	using Interfaces.Core.Doors;
	using MapGeneration.Core.Doors;
	using MapGeneration.Core.Doors.DoorHandlers;
	using MapGeneration.Core.Doors.DoorModes;
	using NUnit.Framework;

	[TestFixture]
	public class SpecificPositionsModeHandlerTests
	{
		private SpecificPositionsModeHandler overlapModeHandler;

		[SetUp]
		public void SetUp()
		{
			overlapModeHandler = new SpecificPositionsModeHandler();
		}

		[Test]
		public void Rectangle_LengthZeroCorners()
		{
			var polygon = GridPolygon.GetRectangle(3, 5);
			var mode = new SpecificPositionsMode(new List<OrthogonalLine>()
			{
				new OrthogonalLine(new IntVector2(0, 0), new IntVector2(0, 0)),
				new OrthogonalLine(new IntVector2(0, 5), new IntVector2(0, 5)),
				new OrthogonalLine(new IntVector2(3, 5), new IntVector2(3, 5)),
				new OrthogonalLine(new IntVector2(3, 0), new IntVector2(3, 0)),
			});
			var doorPositions = overlapModeHandler.GetDoorPositions(polygon, mode);

			var expectedPositions = new List<IDoorLine>()
			{
				new DoorLine(new OrthogonalLine(new IntVector2(0, 0), new IntVector2(0, 0), OrthogonalLine.Direction.Left), 0),
				new DoorLine(new OrthogonalLine(new IntVector2(0, 0), new IntVector2(0, 0), OrthogonalLine.Direction.Top), 0),
				new DoorLine(new OrthogonalLine(new IntVector2(0, 5), new IntVector2(0, 5), OrthogonalLine.Direction.Top), 0),
				new DoorLine(new OrthogonalLine(new IntVector2(0, 5), new IntVector2(0, 5), OrthogonalLine.Direction.Right), 0),
				new DoorLine(new OrthogonalLine(new IntVector2(3, 5), new IntVector2(3, 5), OrthogonalLine.Direction.Right), 0),
				new DoorLine(new OrthogonalLine(new IntVector2(3, 5), new IntVector2(3, 5), OrthogonalLine.Direction.Bottom), 0),
				new DoorLine(new OrthogonalLine(new IntVector2(3, 0), new IntVector2(3, 0), OrthogonalLine.Direction.Bottom), 0),
				new DoorLine(new OrthogonalLine(new IntVector2(3, 0), new IntVector2(3, 0), OrthogonalLine.Direction.Left), 0),
			};

			Assert.IsTrue(doorPositions.SequenceEqualWithoutOrder(expectedPositions));
		}

		[Test]
		public void Rectangle_LengthZeroInside()
		{
			var polygon = GridPolygon.GetRectangle(3, 5);
			var mode = new SpecificPositionsMode(new List<OrthogonalLine>()
			{
				new OrthogonalLine(new IntVector2(0, 1), new IntVector2(0, 1)),
				new OrthogonalLine(new IntVector2(1, 5), new IntVector2(1, 5)),
				new OrthogonalLine(new IntVector2(3, 4), new IntVector2(3, 4)),
				new OrthogonalLine(new IntVector2(2, 0), new IntVector2(2, 0)),
			});
			var doorPositions = overlapModeHandler.GetDoorPositions(polygon, mode);

			var expectedPositions = new List<IDoorLine>()
			{
				new DoorLine(new OrthogonalLine(new IntVector2(0, 1), new IntVector2(0, 1), OrthogonalLine.Direction.Top), 0),
				new DoorLine(new OrthogonalLine(new IntVector2(1, 5), new IntVector2(1, 5), OrthogonalLine.Direction.Right), 0),
				new DoorLine(new OrthogonalLine(new IntVector2(3, 4), new IntVector2(3, 4), OrthogonalLine.Direction.Bottom), 0),
				new DoorLine(new OrthogonalLine(new IntVector2(2, 0), new IntVector2(2, 0), OrthogonalLine.Direction.Left), 0),
			};

			Assert.IsTrue(doorPositions.SequenceEqualWithoutOrder(expectedPositions));
		}
	}
}