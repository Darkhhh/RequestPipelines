using System.Linq.Expressions;
using Moq;
using RequestPipelines.Handlers;
using RequestPipelines.PipelineBuilder;
using RequestPipelines.PipelineExecution;
using RequestPipelines.Resolvers;

namespace RequestPipelines.Tests.PipelineTests;

[TestClass]
public class PipeConnectorTests : TestClassBase
{
    private readonly Mock<IHandlerResolver> _resolver;
    private readonly Mock<IPipelineHelper> _helper;

    public PipeConnectorTests()
    {
        _resolver = GetMock<IHandlerResolver>();
        _helper = GetMock<IPipelineHelper>();
    }

    [TestMethod]
    public void Add_OneHandler_Success()
    {
        #region Arrange

        var connector = new PipeConnector<Model1>(_helper.Object, _resolver.Object);
        _resolver
            .Setup(x => x.Resolve(It.IsAny<Type>()))
            .Returns(new Handler1());
        _helper
            .Setup(x => x.GetCurrentInputParameter())
            .Returns(Expression.Constant(new Model1()));

        #endregion
        
        #region Act

        var nextConnector = connector.Add<Handler1, Model2>();

        #endregion
        
        #region Assert
        
        Assert.IsInstanceOfType<PipeConnector<Model2>>(nextConnector);
        
        _resolver.Verify(x => x.Resolve(It.IsAny<Type>()), 
            Times.Once);
        _helper.Verify(x => x.AddExpression(It.IsAny<Expression>(), It.IsAny<ParameterExpression>()), 
            Times.Once);
        
        #endregion
    }
    
    [TestMethod]
    public void SealWith_Success()
    {
        #region Arrange

        var connector = new PipeConnector<Model1>(_helper.Object, _resolver.Object);
        _resolver
            .Setup(x => x.Resolve(It.IsAny<Type>()))
            .Returns(new Handler1());
        _helper
            .Setup(x => x.GetCurrentInputParameter())
            .Returns(Expression.Constant(new Model1()));
        _helper
            .Setup(x => x.AddFinalExpression<Model2>(It.IsAny<Expression>()))
            .Returns(new PipelineExecutor<Model2>(() => new Model2()));

        #endregion
        
        #region Act

        var executor = connector.SealWith<Handler1, Model2>();

        #endregion
        
        #region Assert
        
        Assert.IsTrue(executor.GetType().IsAssignableTo(typeof(IPipelineExecutor<Model2>)));
        
        _resolver.Verify(x => x.Resolve(It.IsAny<Type>()), 
            Times.Once);
        _helper.Verify(x => x.AddFinalExpression<Model2>(It.IsAny<Expression>()), 
            Times.Once);
        
        #endregion
    }

    [TestMethod]
    public void AddActionT_Success()
    {
        #region Arrange

        var connector = new PipeConnector<Model1>(_helper.Object, _resolver.Object);
        _helper
            .Setup(x => x.GetCurrentInputParameter())
            .Returns(Expression.Constant(new Model1()));

        #endregion
        
        #region Act

        var nextConnector = connector.AddAction(x => _ = x.GetType());

        #endregion
        
        #region Assert
        
        Assert.IsInstanceOfType<PipeConnector<Model1>>(nextConnector);
        
        _resolver.Verify(x => x.Resolve(It.IsAny<Type>()), 
            Times.Never);
        _helper.Verify(x => x.GetCurrentInputParameter(), 
            Times.Once);
        _helper.Verify(x => x.AddExpression(It.IsAny<Expression>()), 
            Times.Once);
        
        #endregion
    }
    
    [TestMethod]
    public void AddAction_Success()
    {
        #region Arrange

        var connector = new PipeConnector<Model1>(_helper.Object, _resolver.Object);

        #endregion
        
        #region Act

        var nextConnector = connector.AddAction(() => Mock.Verify());

        #endregion
        
        #region Assert
        
        Assert.IsInstanceOfType<PipeConnector<Model1>>(nextConnector);
        
        _resolver.Verify(x => x.Resolve(It.IsAny<Type>()), 
            Times.Never);
        _helper.Verify(x => x.GetCurrentInputParameter(), 
            Times.Never);
        _helper.Verify(x => x.AddExpression(It.IsAny<Expression>()), 
            Times.Once);
        
        #endregion
    }
    
    private struct Model1;
    private struct Model2;
    private struct Model3;
    
    private class Handler1 : PipelineHandler<Model1, Model2>
    {
        public override Model2 Handle(Model1 request) => new();
    }
    
    private class Handler2 : PipelineHandler<Model2, Model3>
    {
        public override Model3 Handle(Model2 request) => new();
    }
}