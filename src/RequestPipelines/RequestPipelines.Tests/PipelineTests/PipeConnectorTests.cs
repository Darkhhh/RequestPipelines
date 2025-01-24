using System.Linq.Expressions;
using Moq;
using RequestPipelines.Handlers;
using RequestPipelines.PipelineBuilder;
using RequestPipelines.Resolvers;

namespace RequestPipelines.Tests.PipelineTests;

[TestClass]
public class PipeConnectorTests : TestClassBase
{
    private readonly Mock<Pipeline> _pipelineMock;
    private readonly Mock<IHandlerResolver> _resolverMock;

    public PipeConnectorTests()
    {
        _resolverMock = GetMock<IHandlerResolver>();
        _pipelineMock = new Mock<Pipeline>(_resolverMock.Object);
        _pipelineMock.Object.Process(new Model1());
    }
    
    [TestMethod]
    public void Add_ReturnNextConnector_Success()
    {
        #region Arrange

        var connector = Pipeline.Create(_resolverMock.Object).Process(new Model1());
        _resolverMock.Setup(x => x.Resolve(typeof(Handler1))).Returns(new Handler1());

        #endregion

        #region Act

        var nextConnector = connector.Add<Handler1, Model2>();

        #endregion

        #region Assert

        Assert.IsInstanceOfType<PipeConnector<Model2>>(nextConnector);
        
        _resolverMock.Verify(
            x => x.Resolve(It.IsAny<Type>()), 
            Times.Once);

        #endregion
    }

    [TestMethod]
    public void Add_ChainCalls_Success()
    {
        #region Arrange

        var handlers = 2;
        var connector = Pipeline.Create(_resolverMock.Object).Process(new Model1());
        _resolverMock.Setup(x => x.Resolve(typeof(Handler1))).Returns(new Handler1());
        _resolverMock.Setup(x => x.Resolve(typeof(Handler2))).Returns(new Handler2());

        #endregion

        #region Act

        _ = connector
            .Add<Handler1, Model2>()
            .Add<Handler2, Model3>();

        #endregion
        
        #region Assert
        
        _resolverMock.Verify(
            x => x.Resolve(It.IsAny<Type>()), 
            Times.Exactly(handlers));
        
        #endregion
    }
    
    private struct Model1;
    private struct Model2;
    private struct Model3;
    
    private class Handler1 : PipelineHandler<Model1, Model2>
    {
        public override Model2 Handle(Model1 request)
        {
            return new Model2();
        }
    }
    
    private class Handler2 : PipelineHandler<Model2, Model3>
    {
        public override Model3 Handle(Model2 request)
        {
            return new Model3();
        }
    }
}