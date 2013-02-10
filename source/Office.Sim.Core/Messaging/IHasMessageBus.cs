namespace Office.Sim.Core.Messaging
{
    public interface IHasMessageBus
    {
        IMessageBus Bus { get; }
    }
}