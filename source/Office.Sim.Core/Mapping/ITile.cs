namespace Office.Sim.Core.Mapping
{
    public interface ITile
    {
        ICorner C1 { get; }
        ICorner C2 { get; }
        ICorner C3 { get; }
        ICorner C4 { get; }
    }
}