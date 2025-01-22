namespace RequestPipelines.PipelineExecution;

/// <summary>
/// Интерфейс исполнителя конвейера.
/// </summary>
/// <typeparam name="TResult">Тип получаемого результата.</typeparam>
public interface IPipelineExecutor<out TResult>
{
    /// <summary>
    /// Выполнить конвейер.
    /// </summary>
    /// <returns>Результат выполнения конвейера.</returns>
    TResult Execute();
}