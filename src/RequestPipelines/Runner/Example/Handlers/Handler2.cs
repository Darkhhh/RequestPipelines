using RequestPipelines.Handlers;
using Runner.Example.Models;

namespace Runner.Example.Handlers;

public class Handler2 : PipelineHandler<Model2, Model3>
{
    public override Model3 Handle(Model2 request)
    {
        return new Model3
        {
            SomeNumber = request.SomeString.Length
        };
    }
}