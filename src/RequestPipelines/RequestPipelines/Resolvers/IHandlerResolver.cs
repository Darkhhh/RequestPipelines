namespace RequestPipelines.Resolvers;

public interface IHandlerResolver
{
    public object Resolve(Type handler);
}