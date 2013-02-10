namespace Office.Sim.Core.Mapping
{
    public interface IMap
    {
        double TileSize { get; }
        double HeightOffset { get; }
        ITile[,] Tiles { get; }
    }
}