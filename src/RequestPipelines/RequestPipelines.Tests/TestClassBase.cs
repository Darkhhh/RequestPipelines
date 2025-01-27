using Moq;
using Moq.AutoMock;

namespace RequestPipelines.Tests;

/// <summary>
/// Базовый класс для тестов.
/// </summary>
public abstract class TestClassBase
{
    /// <summary>
    /// Генератор экземпляров классов.
    /// </summary>
    private readonly AutoMocker _mocker = new();
    
    /// <summary>
    /// Получить мок класса.
    /// </summary>
    /// <typeparam name="T">Тип класса.</typeparam>
    /// <returns>Мок.</returns>
    protected Mock<T> GetMock<T>() where T : class => _mocker.GetMock<T>();
    
    /// <summary>
    /// Получить экземпляр класса.
    /// </summary>
    /// <typeparam name="T">Тип класса.</typeparam>
    /// <returns>Экземпляр.</returns>
    protected T Get<T>() where T : class => _mocker.Get<T>();
    
    /// <summary>
    /// Использовать заданный экземпляр класса.
    /// </summary>
    /// <param name="instance">Экземпляр.</param>
    /// <typeparam name="T">Тип класса.</typeparam>
    protected void Use<T>(T instance) where T : class => _mocker.Use(instance);
}