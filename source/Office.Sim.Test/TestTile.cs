using Office.Sim.Core;
using Office.Sim.Core.Mapping;

namespace Office.Sim.Test
{
    public class TestTile : ITile
    {

        public ICorner C1 { get; private set; }
        public ICorner C2 { get; private set; }
        public ICorner C3 { get; private set; }
        public ICorner C4 { get; private set; }

        public TestTile(ICorner c1, ICorner c2, ICorner c3, ICorner c4)
        {
            C1 = c1;
            C2 = c2;
            C3 = c3;
            C4 = c4;
        }

        public override string ToString()
        {
            return "{0},{1},{2},{3}".Fmt(C1, C2, C3, C4);
        }
    }
}