using AspNetExample.Domain.Models;
using AspNetExample.Domain.Repositories;
using Moq;

namespace AspNetExample.Domain.Test;

[TestFixture]
public class WrapperTest
{
    private Mock<IRepository> _repositoryMock;

    private const string CreateModelMethodName = nameof(IRepository.CreateModel) + ". ";
    private readonly Guid _id = Guid.Parse("75D50016-466F-4030-9EAE-4D8D690C9957");

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new(MockBehavior.Strict);
    }

    [TestCase("data", Description = "Create model with data", TestName = CreateModelMethodName + "With data")]
    [TestCase(null, Description = "Create model without data", TestName = CreateModelMethodName + "Without data")]
    public void CreateModel(string data)
    {
        // Arrange
        SetupRepository(data);

        // Act
        var actual = new Wrapper(_repositoryMock.Object).CreateModel(data);

        // Assert
        MyModel expected = data is null ? null : new(data, _id);
        Assert.That(actual, Is.EqualTo(expected));
    }

    private void SetupRepository(string data) => _repositoryMock
        .Setup(x => x.CreateModel(It.Is<string>(d => d == data)))
        .Returns(data is null ? null :new MyModel(data, _id));
}

internal class Wrapper
{
    private readonly IRepository _repository;

    public Wrapper(IRepository repository)
    {
        _repository = repository;
    }

    public MyModel CreateModel(string data) => _repository.CreateModel(data);

    // public MyModel CreateModel(string data) => _repository.CreateModel(data + 1);
}