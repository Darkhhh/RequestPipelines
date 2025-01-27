using RequestPipelines.Resolvers;

namespace RequestPipelines.PipelineBuilder;

/// <summary>
/// Конвейер.
/// </summary>
/// <param name="resolver">Класс, создающий экземпляры обработчиков.</param>
public class Pipeline(IHandlerResolver resolver)
{
    /// <summary>
    /// Сборщик конвейера.
    /// </summary>
    private readonly IPipelineHelper _pipelineHelper = new PipelineHelper();

    /// <summary>
    /// Инициализация экземпляра класса <see cref="Pipeline"/>.
    /// </summary>
    private Pipeline() : this(new DefaultResolver()) { }
    
    /// <summary>
    /// Обрабатывать запрос заданного типа.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <returns>Соединитель конвейера.</returns>
    public PipeConnector<TRequest> Process<TRequest>(TRequest request)
    {
        _pipelineHelper.SetCurrentInputParameter(request, typeof(TRequest));
        return new PipeConnector<TRequest>(_pipelineHelper, resolver);
    }

    /// <summary>
    /// Создать конвейер.
    /// </summary>
    /// <param name="resolver">Класс, создающий экземпляры обработчиков.</param>
    /// <returns>Конвейер.</returns>
    public static Pipeline Create(IHandlerResolver? resolver = null)
    {
        return resolver is null ? new Pipeline() : new Pipeline(resolver);
    }
}