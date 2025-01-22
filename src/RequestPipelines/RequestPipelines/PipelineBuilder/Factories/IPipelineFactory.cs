namespace RequestPipelines.PipelineBuilder.Factories;

/// <summary>
/// Интерфейс фабрики конвейеров.
/// </summary>
public interface IPipelineFactory
{
    /// <summary>
    /// Создать конвейер.
    /// </summary>
    /// <returns>Конвейер.</returns>
    Pipeline CreatePipeline();
}