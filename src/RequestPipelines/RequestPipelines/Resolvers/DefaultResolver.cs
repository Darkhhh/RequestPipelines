namespace RequestPipelines.Resolvers;

/// <summary>
/// Производитель обработчиков.
/// </summary>
public class DefaultResolver : IHandlerResolver
{
    /// <inheritdoc />
    public object Resolve(Type handler)
    {
        return Activator.CreateInstance(handler) ?? throw new InvalidOperationException();
    }
}