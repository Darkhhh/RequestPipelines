namespace RequestPipelines.Handlers;

/// <summary>
/// Асинхронный конвейерный обработчик запроса.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
/// <typeparam name="TResult">Тип результата.</typeparam>
public interface IAsyncPipelineHandler<TRequest, TResult>
{
    /// <summary>
    /// Обработка запроса.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат обработки.</returns>
    Task<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken);
}