using System.Linq.Expressions;
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
    /// <typeparam name="THandler">Тип обработчика.</typeparam>
    /// <typeparam name="TOutput">Тип результата, возвращаемого обработчиком.</typeparam>
    /// <returns>Соединитель конвейера.</returns>
    public PipeConnector<TOutput> Add<THandler, TOutput>() where THandler : PipelineHandler<TInput, TOutput>
    {
        var handler = pipeline.Resolver.Resolve(typeof(THandler)) as PipelineHandler<TInput, TOutput>;
        if (handler is null) throw new NullReferenceException("Handler is null");

        var expression = handler.BuildExpression(pipeline.CurrentInputParameter);
        var variable = handler.GetOutputVariableExpression();
        
        pipeline.AddExpression(expression, variable);
        return new PipeConnector<TOutput>(pipeline);
    }

    /// <summary>
    /// Завершить последовательность обработчиков.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика.</typeparam>
    /// <typeparam name="TOutput">Тип результата, возвращаемого обработчиком.</typeparam>
    /// <returns>Конвейер.</returns>
    public IPipelineExecutor<TOutput> SealWith<THandler, TOutput>() where THandler : PipelineHandler<TInput, TOutput>
    {
        var handler = pipeline.Resolver.Resolve(typeof(THandler)) as PipelineHandler<TInput, TOutput>;
        if (handler is null) throw new NullReferenceException("Handler is null");

        var expression = handler.BuildExpression(pipeline.CurrentInputParameter);
        
        return pipeline.AddFinalExpression<TOutput>(expression);
    }

    /// <summary>
    /// Добавить промежуточное действие.
    /// </summary>
    /// <param name="action">Действие.</param>
    /// <returns>Соединитель конвейера.</returns>
    public PipeConnector<TInput> AddAction(Action<TInput> action)
    {
        Expression<Action<TInput>> expression = x => action(x);
        var invoke = Expression.Invoke(expression, pipeline.CurrentInputParameter);
        pipeline.AddExpression(invoke);
        
        return new PipeConnector<TInput>(pipeline);
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
        pipeline.AddExpression(invoke);
        
        return new PipeConnector<TInput>(pipeline);
    }
}