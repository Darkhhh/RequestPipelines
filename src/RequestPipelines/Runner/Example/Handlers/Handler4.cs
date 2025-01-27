using RequestPipelines.Handlers;
using Runner.Example.Models;

namespace Runner.Example.Handlers;

public class Handler4 : PipelineHandler<Model3, Model4>
{
    public override Model4 Handle(Model3 request)
    {
        return new Model4
        {
            SomeDouble = request.SomeNumber * 3.14159,
        };
    }
}