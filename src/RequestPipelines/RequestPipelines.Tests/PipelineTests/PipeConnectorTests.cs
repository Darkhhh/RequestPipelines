using RequestPipelines.Handlers;
using RequestPipelines.PipelineBuilder;

namespace RequestPipelines.Tests.PipelineTests;

[TestClass]
public class PipeConnectorTests
{
    [TestMethod]
    public void Add_WithInstances_Success()
    {
        _ = new PipeConnector<Model1>()
            .Add(new Handler1())
            .Add(new Handler2());
    }

    [TestMethod]
    public void Add_WithoutInstances_Success()
    {
        _ = new PipeConnector<Model1>()
            .Add<Handler1, Model2>()
            .Add<Handler2, Model3>();
    }

    private struct Model1;
    private struct Model2;
    private struct Model3;
    
    private class Handler1 : IPipelineHandler<Model1, Model2>
    {
        public Model2 Handle(Model1 request)
        {
            return new Model2();
        }
    }
    
    private class Handler2 : IPipelineHandler<Model2, Model3>
    {
        public Model3 Handle(Model2 request)
        {
            return new Model3();
        }
    }
}