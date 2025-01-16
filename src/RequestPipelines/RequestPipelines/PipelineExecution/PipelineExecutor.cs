using RequestPipelines.PipelineBuilder;

namespace RequestPipelines.PipelineExecution;

public class PipelineExecutor
{
    private readonly Type _requestType;
    private readonly object _request;
    private readonly LinkedList<PipelineElement> _pipeline;

    internal PipelineExecutor(Type requestType, object request, LinkedList<PipelineElement> pipeline)
    {
        _requestType = requestType;
        _request = request;
        _pipeline = pipeline;
    }
}