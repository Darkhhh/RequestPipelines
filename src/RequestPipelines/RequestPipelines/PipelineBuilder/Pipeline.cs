using System.Linq.Expressions;
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
    /// Переменные выражения.
    /// </summary>
    private readonly List<ParameterExpression> _variables = new();
    
    /// <summary>
    /// Выражения.
    /// </summary>
    private readonly List<Expression> _expressions = new();

    /// <summary>
    /// Текущий параметр.
    /// </summary>
    internal Expression CurrentInputParameter { get; private set; } = null!;
    
    /// <summary>
    /// Фабрика обработчиков.
    /// </summary>
    internal IHandlerResolver Resolver => resolver;
    
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
        CurrentInputParameter = Expression.Constant(request, typeof(TRequest));
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
    /// Добавить обработчик цепи.
    /// </summary>
    /// <param name="expression">Выражение вызова метода.</param>
    /// <param name="variable">Выражение получаемой переменной.</param>
    internal void AddExpression(Expression expression, ParameterExpression variable)
    {
        _variables.Add(variable);
        _expressions.Add(Expression.Assign(variable, expression));
        CurrentInputParameter = variable;
    }

    /// <summary>
    /// Закончить построение конвейера. 
    /// </summary>
    /// <returns>Исполнитель конвейера.</returns>
    internal IPipelineExecutor<TResult> AddFinalExpression<TResult>(Expression expression)
    {
        _expressions.Add(expression);
        
        var block = Expression.Block(_variables, _expressions);
        
        var lambda = Expression.Lambda<Func<TResult>>(block).Compile();

        return new PipelineExecutor<TResult>(lambda);
    }

    /// <summary>
    /// Добавить выражение в блок.
    /// </summary>
    /// <param name="expression">Выражение.</param>
    internal void AddExpression(Expression expression)
    {
        _expressions.Add(expression);
    }
}