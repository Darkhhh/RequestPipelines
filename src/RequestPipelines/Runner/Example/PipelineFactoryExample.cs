using RequestPipelines.PipelineBuilder.Factories;
using Runner.Example.Handlers;
using Runner.Example.Models;

namespace Runner.Example;

public class PipelineFactoryExample
{
    public static void Example(IPipelineFactory factory)
    {
        var initModel = new Model1
        {
            SomeInteger = 15
        };

        var pipeline = factory
            .CreatePipeline()
            .Process<Model1>(initModel)
            .Add<Handler1, Model2>()
            .Add<Handler2, Model3>()
            .Add<Handler3, Model3>()
            .SealWith<Handler4, Model4>();

        var result = pipeline.Execute();
    }
}