using System.Linq.Expressions;

namespace RequestPipelines.Handlers;

/// <summary>
/// Базовый класс конвейерного обработчика запроса.
/// </summary>
/// <typeparam name="TIn">Тип обрабатываемого значения.</typeparam>
/// <typeparam name="TOut">Тип результата.</typeparam>
public abstract class PipelineHandler<TIn, TOut> : IPipelineHandler<TIn, TOut>
{
    /// <summary>
    /// Тип конвейерного обработчика.
    /// </summary>
    private static Type HandlerType => typeof(IPipelineHandler<TIn, TOut>);

    /// <summary>
    /// Тип обрабатываемого значения.
    /// </summary>
    private static Type InputType => typeof(TIn);

    /// <summary>
    /// Тип результата.
    /// </summary>
    private static Type OutputType => typeof(TOut);

    /// <summary>
    /// Получить выражение, отображающее переменную, в которую записывается результат обработки.
    /// </summary>
    /// <returns>Выражение.</returns>
    internal ParameterExpression GetOutputVariableExpression() => Expression.Variable(OutputType);

    /// <summary>
    /// Получить выражение, отображающее получение результата обработчиком.
    /// </summary>
    /// <param name="inputParameter">Входящий параметр.</param>
    /// <returns>Выражение.</returns>
    internal Expression BuildExpression(Expression inputParameter)
    {
        Expression<Func<IPipelineHandler<TIn, TOut>, TIn, TOut>> expression = (handler, r) => handler.Handle(r);

        var handler = Expression.Constant(this, HandlerType);

        return Expression.Invoke(expression, handler, inputParameter);
    }
    
    /// <inheritdoc />
    public abstract TOut Handle(TIn request);
}