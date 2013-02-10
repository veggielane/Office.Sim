using Office.Sim.Core.Mapping;

namespace Office.Sim.Test
{
    public class TestLevel : ILevel
    {
        public IMap Map { get; private set; }
        public TestLevel()
        {
            Map = new TestMap(12);
        }
    }
}