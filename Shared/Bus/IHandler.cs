namespace MfePoc.Shared.Bus
{
    public interface IHandler<TMessage>
    {
        void Handle(TMessage message);
    }
}
