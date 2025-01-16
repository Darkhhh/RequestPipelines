namespace RequestPipelines.PipelineBuilder;

/// <summary>
/// Элемент конвейера.
/// </summary>
/// <param name="input">Тип входящего значения.</param>
/// <param name="output">Тип результирующего значения.</param>
/// <param name="handler">Обработчик.</param>
public readonly struct PipelineElement(Type input, Type output, object handler)
{
    /// <summary>
    /// Тип входящего значения.
    /// </summary>
    public Type InputType => input;
    
    /// <summary>
    /// Тип результирующего значения.
    /// </summary>
    public Type OutputType => output;
    
    /// <summary>
    /// Обработчик.
    /// </summary>
    public object Handler => handler;
    
    /// <summary>
    /// Тип обработчика.
    /// </summary>
    public Type HandlerType => handler.GetType();
}