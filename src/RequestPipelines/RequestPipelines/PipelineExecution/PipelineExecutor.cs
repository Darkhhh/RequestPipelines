namespace RequestPipelines.PipelineExecution;

/// <summary>
/// Исполнитель конвейера.
/// </summary>
/// <typeparam name="TResult">Тип получаемого результата.</typeparam>
public class PipelineExecutor<TResult> : IPipelineExecutor<TResult>
{
    /// <summary>
    /// Конвейер.
    /// </summary>
    private readonly Func<TResult> _pipeline;

    /// <inheritdoc />
    public TResult Execute() => _pipeline();

    /// <summary>
    /// Инициализация экземпляра класса <see cref="PipelineExecutor{T}"/>.
    /// </summary>
    /// <param name="pipeline">Конвейер.</param>
    internal PipelineExecutor(Func<TResult> pipeline) => _pipeline = pipeline;
}