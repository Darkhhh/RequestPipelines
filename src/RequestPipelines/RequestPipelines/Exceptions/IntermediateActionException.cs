namespace RequestPipelines.Exceptions;

/// <summary>
/// Исключение для промежуточного действия в конвейере.
/// </summary>
/// <param name="message">Описание исключения.</param>
public class IntermediateActionException(string message) : Exception(message);