using System.Linq.Expressions;
using RequestPipelines.Handlers;
using RequestPipelines.PipelineExecution;
using RequestPipelines.Resolvers;

namespace RequestPipelines.PipelineBuilder;

/// <summary>
/// Соединитель конвейера.
/// </summary>
/// <typeparam name="TInput">Входящий тип.</typeparam>
public readonly struct PipeConnector<TInput>
{
    private readonly IPipelineHelper _pipelineHelper;
    private readonly IHandlerResolver _resolver;

    /// <summary>
    /// Инициализация <see cref="PipeConnector{T}"/>.
    /// </summary>
    /// <param name="pipelineHelper">Сборщик конвейера.</param>
    /// <param name="resolver">Производитель обработчиков.</param>
    internal PipeConnector(IPipelineHelper pipelineHelper, IHandlerResolver resolver)
    {
        _pipelineHelper = pipelineHelper;
        _resolver = resolver;
    }
    
    /// <summary>
    /// Добавить обработчик.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика.</typeparam>
    /// <typeparam name="TOutput">Тип результата, возвращаемого обработчиком.</typeparam>
    /// <returns>Соединитель конвейера.</returns>
    public PipeConnector<TOutput> Add<THandler, TOutput>() where THandler : PipelineHandler<TInput, TOutput>
    {
        var handler = _resolver.Resolve(typeof(THandler)) as PipelineHandler<TInput, TOutput>;
        if (handler is null) throw new NullReferenceException("Handler is null");

        var expression = handler.BuildExpression(_pipelineHelper.GetCurrentInputParameter());
        var variable = handler.GetOutputVariableExpression();
        
        _pipelineHelper.AddExpression(expression, variable);
        return new PipeConnector<TOutput>(_pipelineHelper, _resolver);
    }

    /// <summary>
    /// Завершить последовательность обработчиков.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика.</typeparam>
    /// <typeparam name="TOutput">Тип результата, возвращаемого обработчиком.</typeparam>
    /// <returns>Конвейер.</returns>
    public IPipelineExecutor<TOutput> SealWith<THandler, TOutput>() where THandler : PipelineHandler<TInput, TOutput>
    {
        var handler = _resolver.Resolve(typeof(THandler)) as PipelineHandler<TInput, TOutput>;
        if (handler is null) throw new NullReferenceException("Handler is null");

        var expression = handler.BuildExpression(_pipelineHelper.GetCurrentInputParameter());
        
        return _pipelineHelper.AddFinalExpression<TOutput>(expression);
    }

    /// <summary>
    /// Добавить промежуточное действие.
    /// </summary>
    /// <param name="action">Действие.</param>
    /// <returns>Соединитель конвейера.</returns>
    public PipeConnector<TInput> AddAction(Action<TInput> action)
    {
        Expression<Action<TInput>> expression = x => action(x);
        var invoke = Expression.Invoke(expression, _pipelineHelper.GetCurrentInputParameter());
        _pipelineHelper.AddExpression(invoke);
        
        return new PipeConnector<TInput>(_pipelineHelper, _resolver);
    }

    /// <summary>
    /// Добавить промежуточное действие.
    /// </summary>
    /// <param name="action">Действие.</param>
    /// <returns>Соединитель конвейера.</returns>
    public PipeConnector<TInput> AddAction(Action action)
    {
        Expression<Action> expression = () => action();
        var invoke = Expression.Invoke(expression);
        _pipelineHelper.AddExpression(invoke);
        
        return new PipeConnector<TInput>(_pipelineHelper, _resolver);
    }
}