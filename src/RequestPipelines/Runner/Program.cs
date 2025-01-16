// See https://aka.ms/new-console-template for more information

using RequestPipelines.Handlers;
using RequestPipelines.PipelineBuilder;

Console.WriteLine("Hello, World!");

var pipeline = Pipeline
    .Create()
    .Process<Model1>(new Model1())
    .Add<Handler1, Model2>()
    .Add<Handler2, Model3>()
    .Add<Handler3, Model3>()
    .Add<Handler4, Model4>()
    .Seal();

Console.WriteLine("Hello, World!");

struct Model1;
struct Model2;
struct Model3;
struct Model4;

class Handler1 : IPipelineHandler<Model1, Model2>
{
    public Model2 Handle(Model1 request)
    {
        throw new NotImplementedException();
    }
}

class Handler2 : IPipelineHandler<Model2, Model3>
{
    public Model3 Handle(Model2 request)
    {
        throw new NotImplementedException();
    }
}

class Handler3 : IPipelineHandler<Model3, Model3>
{
    public Model3 Handle(Model3 request)
    {
        throw new NotImplementedException();
    }
}

class Handler4 : IPipelineHandler<Model3, Model4>
{
    public Model4 Handle(Model3 request)
    {
        throw new NotImplementedException();
    }
}
