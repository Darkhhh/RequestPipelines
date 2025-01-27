using System.Linq.Expressions;
using RequestPipelines.PipelineExecution;

namespace RequestPipelines.PipelineBuilder;

/// <summary>
/// Сборщик конвейера.
/// </summary>
internal class PipelineHelper : IPipelineHelper
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
    private Expression _currentInputParameter = null!;

    /// <inheritdoc />
    public Expression GetCurrentInputParameter() => _currentInputParameter;

    /// <inheritdoc />
    public void SetCurrentInputParameter(object? parameter, Type objectType)
    {
        _currentInputParameter = Expression.Constant(parameter, objectType);
    }

    /// <inheritdoc />
    public void AddExpression(Expression expression, ParameterExpression variable)
    {
        _variables.Add(variable);
        _expressions.Add(Expression.Assign(variable, expression));
        _currentInputParameter = variable;
    }

    /// <inheritdoc />
    public void AddExpression(Expression expression) => _expressions.Add(expression);

    /// <inheritdoc />
    public IPipelineExecutor<TResult> AddFinalExpression<TResult>(Expression expression)
    {
        _expressions.Add(expression);
        
        var block = Expression.Block(_variables, _expressions);
        
        var lambda = Expression.Lambda<Func<TResult>>(block).Compile();

        return new PipelineExecutor<TResult>(lambda);
    }
}