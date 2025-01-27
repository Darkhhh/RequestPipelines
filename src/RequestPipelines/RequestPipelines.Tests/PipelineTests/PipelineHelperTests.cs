using System.Linq.Expressions;
using RequestPipelines.PipelineBuilder;

namespace RequestPipelines.Tests.PipelineTests;

[TestClass]
public class PipelineHelperTests
{
    [TestMethod]
    public void GetCurrentParameter_NotSet_ReturnsNull()
    {
        #region Arrange

        var helper = new PipelineHelper();

        #endregion
        
        #region Act

        var p = helper.GetCurrentInputParameter();

        #endregion

        #region Assert

        Assert.IsNull(p);

        #endregion
    }

    [TestMethod]
    public void GetCurrentInputParameter_AfterSet_Success()
    {
        #region Arrange

        var helper = new PipelineHelper();
        helper.SetCurrentInputParameter(10, typeof(int));

        #endregion
        
        #region Act

        var p = helper.GetCurrentInputParameter();

        #endregion

        #region Assert

        Assert.IsNotNull(p);
        Assert.IsInstanceOfType<ConstantExpression>(p);

        #endregion
    }

    [TestMethod]
    public void AddExpression_WithVariable_ChangeCurrentInputParameter()
    {
        #region Arrange

        var helper = new PipelineHelper();
        helper.AddExpression(Expression.Constant(10), Expression.Variable(typeof(int)));

        #endregion
        
        #region Act
        
        var p = helper.GetCurrentInputParameter();

        #endregion

        #region Assert

        Assert.IsNotNull(p);
        Assert.IsInstanceOfType<ParameterExpression>(p);

        #endregion
    }
    
    [TestMethod]
    public void AddExpression_WithoutVariable_NotChangedCurrentInputParameter()
    {
        #region Arrange

        var helper = new PipelineHelper();
        helper.AddExpression(Expression.Constant(10));
        
        #endregion
        
        #region Act
        
        var p = helper.GetCurrentInputParameter();

        #endregion

        #region Assert

        Assert.IsNull(p);

        #endregion
    }
    
    [TestMethod]
    public void AddFinalExpression_ReturnsExecutor_CorrectExecutionResult()
    {
        #region Arrange

        var helper = new PipelineHelper();
        const int expected = 10;
        Expression<Func<int>> expression = () => expected;

        #endregion
        
        #region Act

        var executor = helper.AddFinalExpression<int>(Expression.Invoke(expression));

        #endregion

        #region Assert

        Assert.IsNotNull(executor);
        Assert.AreEqual(expected, executor.Execute());

        #endregion
    }
}