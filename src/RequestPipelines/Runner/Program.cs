using RequestPipelines.PipelineBuilder;
using Runner.Example.Handlers;
using Runner.Example.Models;

var initModel = new Model1
{
    SomeInteger = 15
};

var pipeline = Pipeline
    .Create()
    .Process<Model1>(initModel)
    .Add<Handler1, Model2>()
    .Add<Handler2, Model3>()
    .Add<Handler3, Model3>()
    .SealWith<Handler4, Model4>();

var result = pipeline.Execute();

Console.WriteLine(result.SomeDouble);
