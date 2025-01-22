using RequestPipelines.Handlers;
using Runner.Example.Models;

namespace Runner.Example.Handlers;

public class Handler1 : PipelineHandler<Model1, Model2>
{
    public override Model2 Handle(Model1 request)
    {
        var model = new Model2
        {
            SomeString = request.SomeInteger.ToString()
        };

        return model;
    }
}