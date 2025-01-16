using RequestPipelines.PipelineExecution;
using RequestPipelines.Resolvers;

namespace RequestPipelines.PipelineBuilder;

/// <summary>
/// Конвейер.
/// </summary>
/// <param name="resolver">Класс, создающий экземпляры обработчиков.</param>
public class Pipeline(IHandlerResolver resolver)
{
    /// <summary>
    /// Конвейер.
    /// </summary>
    private readonly LinkedList<PipelineElement> _pipeline = new();
    
    /// <summary>
    /// Запрос.
    /// </summary>
    private object? _request;
    
    /// <summary>
    /// Тип запроса.
    /// </summary>
    private Type? _requestType;

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
        _request = request;
        _requestType = typeof(TRequest);
        return new PipeConnector<TRequest>(this);
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
    
    /// <summary>
    /// Добавить обработчик.
    /// </summary>
    /// <param name="input">Тип входящего значения.</param>
    /// <param name="output">Тип результирующего значения.</param>
    /// <param name="handlerType">Тип обработчика.</param>
    /// <param name="protoHandler">Экземпляр обработчика.</param>
    internal void Add(Type input, Type output, Type handlerType, object? protoHandler = null)
    {
        var handler = protoHandler ?? resolver.Resolve(handlerType);
        _pipeline.AddLast(new PipelineElement(input, output, handler));
    }

    /// <summary>
    /// Закончить построение конвейера. 
    /// </summary>
    /// <returns>Исполнитель конвейера.</returns>
    internal PipelineExecutor Seal()
    {
        if (!Validate() || _requestType is null || _request is null) throw new InvalidOperationException();

        return new PipelineExecutor(_requestType, _request, _pipeline);
    }

    /// <summary>
    /// Проверка корректности заданного конвейера.
    /// </summary>
    /// <returns>Корректность конвейера.</returns>
    private bool Validate()
    {
        var head = _pipeline.First;
        if (head is null) return true;

        while (head.Next is not null)
        {
            if (head.Value.OutputType != head.Next.Value.InputType) return false;
            
            head = head.Next;
        }
        
        return true;
    }
}