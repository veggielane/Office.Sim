using System;
using System.Text;
using Office.Sim.Core.Mapping;
using Veg.Maths;

namespace Office.Sim.Test
{
    public class TestMap : IMap
    {
        public double TileSize { get; private set; }
        public double HeightOffset { get; private set; }
        public ITile[,] Tiles { get; private set; }
        public TestMap(int size)
        {
            TileSize = 5f;

            Tiles = new ITile[size, size];

            var r = new Random(3);


            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    var a = r.Next(-1, 2);
                    var b = r.Next(-1, 2);
                    var c = r.Next(-1, 2);
                    var d = r.Next(-1, 2);


                    var c1 = new TestCorner(new Vect3(i * TileSize - (TileSize / 2f), j * TileSize - (TileSize / 2f), 0));
                    var c2 = new TestCorner(new Vect3(i * TileSize - (TileSize / 2f), j * TileSize + (TileSize / 2f), 0));
                    var c3 = new TestCorner(new Vect3(i * TileSize + (TileSize / 2f), j * TileSize + (TileSize / 2f), 0));
                    var c4 = new TestCorner(new Vect3(i * TileSize + (TileSize / 2f), j * TileSize - (TileSize / 2f), 0));

                    Tiles[i, j] = new TestTile(c1, c2, c3, c4);
                }
            }


            // Tiles[0, 0].C1.Height = -1;
            Tiles[0, 0].C2.Position += new Vect3(0, 0, 2);
            Tiles[0, 1].C1.Position += new Vect3(0, 0, 2);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < Tiles.GetLength(0); i++)
            {
                for (var j = 0; j < Tiles.GetLength(1); j++)
                {
                    sb.Append("#");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}