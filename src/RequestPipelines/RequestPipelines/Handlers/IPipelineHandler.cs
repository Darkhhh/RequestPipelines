namespace RequestPipelines.Handlers;

/// <summary>
/// Конвейерный обработчик запроса.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
/// <typeparam name="TResult">Тип результата.</typeparam>
public interface IPipelineHandler<TRequest, TResult>
{
    /// <summary>
    /// Обработка запроса.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <returns>Результат обработки.</returns>
    TResult Handle(TRequest request);
}