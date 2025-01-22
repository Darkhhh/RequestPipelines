using RequestPipelines.Resolvers;

namespace RequestPipelines.PipelineBuilder.Factories;

/// <summary>
/// Фабрика конвейеров.
/// </summary>
/// <param name="resolver">Производитель обработчиков.</param>
public class PipelineFactory(IHandlerResolver resolver) : IPipelineFactory
{
    /// <inheritdoc />
    public Pipeline CreatePipeline()
    {
        var pipeline = Pipeline.Create(resolver);
        
        return pipeline;
    }
}