using Office.Sim.Core;
using Office.Sim.Core.Mapping;
using Veg.Maths;

namespace Office.Sim.Test
{
    public class TestCorner : ICorner
    {
        public Vect3 Position { get; set; }

        public TestCorner(Vect3 position)
        {
            Position = position;
        }

        public override string ToString()
        {
            return "Corner:{0}".Fmt(Position);
        }
    }
}