using RequestPipelines.Handlers;
using Runner.Example.Models;

namespace Runner.Example.Handlers;

public class Handler3 : PipelineHandler<Model3, Model3>
{
    public override Model3 Handle(Model3 request)
    {
        return new Model3
        {
            SomeNumber = request.SomeNumber * 2
        };
    }
}