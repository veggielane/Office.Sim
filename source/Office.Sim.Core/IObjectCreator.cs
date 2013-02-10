namespace Office.Sim.Core
{
    public interface IObjectCreator
    {
        T Create<T>();
    }
}
