namespace RequestPipelines.Resolvers;

/// <summary>
/// Интерфейс производителя обработчиков.
/// </summary>
public interface IHandlerResolver
{
    /// <summary>
    /// Создать обработчик.
    /// </summary>
    /// <param name="handler">Тип обработчика.</param>
    /// <returns>Обработчик.</returns>
    public object Resolve(Type handler);
}