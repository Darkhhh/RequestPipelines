using RequestPipelines.Handlers;
using RequestPipelines.PipelineExecution;

namespace RequestPipelines.PipelineBuilder;

/// <summary>
/// Соединитель конвейера.
/// </summary>
/// <typeparam name="TInput">Входящий тип.</typeparam>
public readonly struct PipeConnector<TInput>(Pipeline pipeline)
{
    /// <summary>
    /// Добавить обработчик.
    /// </summary>
    /// <param name="handler">Обработчик.</param>
    /// <typeparam name="TOutput">Тип результата, возвращаемого обработчиком.</typeparam>
    /// <returns>Соединитель конвейера.</returns>
    public PipeConnector<TOutput> Add<TOutput>(IPipelineHandler<TInput, TOutput> handler)
    {
        pipeline.Add(typeof(TInput), typeof(TOutput), handler.GetType(), handler);
        return new PipeConnector<TOutput>(pipeline);
    }
    
    /// <summary>
    /// Добавить обработчик.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика.</typeparam>
    /// <typeparam name="TOutput">Тип результата, возвращаемого обработчиком.</typeparam>
    /// <returns>Соединитель конвейера.</returns>
    public PipeConnector<TOutput> Add<THandler, TOutput>() where THandler : IPipelineHandler<TInput, TOutput>
    {
        pipeline.Add(typeof(TInput), typeof(TOutput), typeof(THandler));
        return new PipeConnector<TOutput>(pipeline);
    }

    /// <summary>
    /// Завершить последовательность обработчиков.
    /// </summary>
    /// <returns>Конвейер.</returns>
    public PipelineExecutor Seal()
    {
        return pipeline.Seal();
    }
}