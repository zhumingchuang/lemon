namespace LemonFramework.AssetBundles
{
    public interface ICommandHandler<in T>
    {
        void Handle(T cmd);
    }
}