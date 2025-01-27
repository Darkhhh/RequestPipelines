using System.Linq.Expressions;
using RequestPipelines.PipelineExecution;

namespace RequestPipelines.PipelineBuilder;

/// <summary>
/// Интерфейс сборщика конвейера.
/// </summary>
internal interface IPipelineHelper
{
    /// <summary>
    /// Получить текущий параметр.
    /// </summary>
    /// <returns>Текущий параметр.</returns>
    Expression GetCurrentInputParameter();

    /// <summary>
    /// Установить параметр.
    /// </summary>
    /// <param name="parameter">Объект.</param>
    /// <param name="objectType">Тип объекта.</param>
    void SetCurrentInputParameter(object? parameter, Type objectType);
    
    /// <summary>
    /// Добавить обработчик цепи.
    /// </summary>
    /// <param name="expression">Выражение вызова метода.</param>
    /// <param name="variable">Выражение получаемой переменной.</param>
    void AddExpression(Expression expression, ParameterExpression variable);

    /// <summary>
    /// Добавить выражение в блок.
    /// </summary>
    /// <param name="expression">Выражение.</param>
    void AddExpression(Expression expression);

    /// <summary>
    /// Закончить построение конвейера. 
    /// </summary>
    /// <returns>Исполнитель конвейера.</returns>
    IPipelineExecutor<TResult> AddFinalExpression<TResult>(Expression expression);
}