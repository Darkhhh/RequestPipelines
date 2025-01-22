namespace RequestPipelines.PipelineExecution;

public class PipelineExecutor<TResult>
{
    private readonly Func<TResult> _pipeline;

    public TResult Execute() => _pipeline();

    internal PipelineExecutor(Func<TResult> pipeline)
    {
        _pipeline = pipeline;
    }
}