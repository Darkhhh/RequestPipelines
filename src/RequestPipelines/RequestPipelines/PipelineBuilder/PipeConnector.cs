using RequestPipelines.Handlers;

namespace RequestPipelines.PipelineBuilder;

/// <summary>
/// Соединитель конвейера.
/// </summary>
/// <typeparam name="TInput">Входящий тип.</typeparam>
public readonly struct PipeConnector<TInput>
{
    /// <summary>
    /// Добавить обработчик.
    /// </summary>
    /// <param name="handler">Обработчик.</param>
    /// <typeparam name="TOutput">Тип результата, возвращаемого обработчиком.</typeparam>
    /// <returns>Соединитель конвейера.</returns>
    public PipeConnector<TOutput> Add<TOutput>(IPipelineHandler<TInput, TOutput> handler)
    {
        return new PipeConnector<TOutput>();
    }
    
    /// <summary>
    /// Добавить обработчик.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика.</typeparam>
    /// <typeparam name="TOutput">Тип результата, возвращаемого обработчиком.</typeparam>
    /// <returns>Соединитель конвейера.</returns>
    public PipeConnector<TOutput> Add<THandler, TOutput>() where THandler : IPipelineHandler<TInput, TOutput>
    {
        return new PipeConnector<TOutput>();
    }
}