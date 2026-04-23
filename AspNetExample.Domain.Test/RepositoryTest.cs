using AspNetExample.Domain.Models;
using AspNetExample.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using AspNetExample.Service;

namespace AspNetExample.Domain.Test;

[TestFixture]
public class RepositoryTest
{
    private IRepository _repository;
    private Mock<IGuidProvider> _guidProviderMock;
    private Mock<IStorage> _storageMock;

    private const string CreateModelMethodName = nameof(IRepository.CreateModel) + ". ";
    private readonly Guid _id = Guid.Parse("75D50016-466F-4030-9EAE-4D8D690C9957");

    [SetUp]
    public void Setup()
    {
        ServiceCollection services = new();
        services.RegisterDomain();

        _guidProviderMock = new(MockBehavior.Strict);
        services.AddSingleton(_guidProviderMock.Object);

        _storageMock = new(MockBehavior.Strict);
        services.AddSingleton(_storageMock.Object);

        var serviceProvider = services.BuildServiceProvider();

        _repository = serviceProvider.GetRequiredService<IRepository>();
    }

    [TestCase("data", Description = "Create model with data", TestName = CreateModelMethodName + "With data")]
    [TestCase(null, Description = "Create model without data", TestName = CreateModelMethodName + "Without data")]
    public void CreateModel(string data)
    {
        // Arrange
        SetupStorage();
        SetupGuidProvider();

        // Act
        var actual = _repository.CreateModel(data);

        // Assert
        MyModel expected = data is null ? null : new(data, _id);
        Assert.That(actual, Is.EqualTo(expected));
        _guidProviderMock.Verify(x => x.GetGuid(), data is null ? Times.Never : Times.Once);
        _storageMock.Verify(x => x.Models, data is null ? Times.Never : Times.Once);
    }

    private void SetupStorage() => _storageMock.Setup(x => x.Models).Returns(new Dictionary<Guid, MyModel>());

    private void SetupGuidProvider() => _guidProviderMock.Setup(x => x.GetGuid()).Returns(_id);
}