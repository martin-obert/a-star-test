using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PathFinding;
using Runtime.Grid;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using Runtime.Grid.Services;
using Runtime.Terrains;
using UnityEngine;

namespace Tests
{
    public class AStarTests
    {
        private static IGridCellViewModel[] Create(IReadOnlyList<TerrainType[]> source)
        {
            var result = new IGridCellViewModel[source.Sum(y => y.Length)];
            for (var rowIndex = 0; rowIndex < source.Count; rowIndex++)
            {
                var testData = source[rowIndex];
                for (var colIndex = 0; colIndex < testData.Length; colIndex++)
                {
                    var data = testData[colIndex];
                    result[rowIndex * source.Count + colIndex] = new GridCellViewModel(new GridCellSave
                    {
                        ColIndex = colIndex,
                        RowIndex = rowIndex,
                        TerrainType = data
                    }, new TerrainVariantMock(data));
                }
            }

            GridGenerator.PopulateNeighbours(result);

            return result;
        }


        private sealed class TerrainVariantMock : ITerrainVariant
        {
            public TerrainVariantMock(TerrainType type)
            {
                Type = type;
            }

            public int DaysTravelCost => Type.GetDaysToTravelCost();
            public bool IsWalkable => Type.IsWalkable();
            public TerrainType Type { get; }
            public Texture TextureOverride => throw new NotImplementedException();
        }

        [Test]
        public void Path_NoWater_SmallGrid()
        {
            var grid = Create(new[]
            {
                new[] { TerrainType.Forest, TerrainType.Forest, },
                /**/new[] { TerrainType.Grass, TerrainType.Mountain, },
            });

            var actualPath = AStar.GetPath(grid[0], grid[3]);
            AssertPath(new[]
            {
                grid[0], grid[2], grid[3]
            }, actualPath.OfType<IGridCellViewModel>().ToArray());
        }

        [Test]
        public void Path_NoWater_MediumGrid()
        {
            var grid = Create(new[]
            {
                new[] { TerrainType.Forest, TerrainType.Forest, TerrainType.Desert },
                /**/new[] { TerrainType.Grass, TerrainType.Mountain, TerrainType.Desert },
                new[] { TerrainType.Grass, TerrainType.Grass, TerrainType.Desert },
            });

            var actualPath = AStar.GetPath(grid[0], grid[4]);
            AssertPath(new[]
            {
                grid[0], grid[3], grid[4]
            }, actualPath.OfType<IGridCellViewModel>().ToArray());
        }


        [Test]
        public void Path_WithWater_MediumGrid()
        {
            var grid = Create(new[]
            {
                new[] { TerrainType.Forest, TerrainType.Forest, TerrainType.Desert },
                /**/new[] { TerrainType.Water, TerrainType.Mountain, TerrainType.Desert },
                new[] { TerrainType.Grass, TerrainType.Grass, TerrainType.Desert },
            });

            var actualPath = AStar.GetPath(grid[0], grid[8]);
            AssertPath(new[]
            {
                grid[0], grid[1], grid[4], grid[8]
            }, actualPath.OfType<IGridCellViewModel>().ToArray());
        }

        private static void AssertPath(IReadOnlyList<IGridCellViewModel> expected,
            IReadOnlyList<IGridCellViewModel> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count,
                $"Expected path len {expected.Count} doesn't match actual path len {actual.Count}");
            for (var index = 0; index < actual.Count; index++)
            {
                var a = actual[index];
                var e = expected[index];

                Assert.AreEqual(e.RowIndex, a.RowIndex,
                    $"Row Indexes doesn't match: expected - r:{e.RowIndex}, c:{e.ColIndex}, actual - r:{a.RowIndex}, c:{a.ColIndex}");

                Assert.AreEqual(e.ColIndex, a.ColIndex,
                    $"Row Indexes doesn't match: expected - r:{e.RowIndex}, c:{e.ColIndex}, actual - r:{a.RowIndex}, c:{a.ColIndex}");
            }
        }
    }
}