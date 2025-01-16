namespace RequestPipelines.Resolvers;

public class DefaultResolver : IHandlerResolver
{
    public object Resolve(Type handler)
    {
        return Activator.CreateInstance(handler) ?? throw new InvalidOperationException();
    }
}