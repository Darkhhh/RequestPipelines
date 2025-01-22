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
    .SealWith<Handler4, Model4>()
    .Execute();

Console.WriteLine("Hello, World!");

struct Model1;
struct Model2;
struct Model3;
struct Model4;

class Handler1 : PipelineHandler<Model1, Model2>
{
    public override Model2 Handle(Model1 request)
    {
        return new Model2();
    }
}

class Handler2 : PipelineHandler<Model2, Model3>
{
    public override Model3 Handle(Model2 request)
    {
        return new Model3();
    }
}

class Handler3 : PipelineHandler<Model3, Model3>
{
    public override Model3 Handle(Model3 request)
    {
        return new Model3();
    }
}

class Handler4 : PipelineHandler<Model3, Model4>
{
    public override Model4 Handle(Model3 request)
    {
        return new Model4();
    }
}
