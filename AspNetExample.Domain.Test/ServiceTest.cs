using AspNetExample.Domain.Repositories;
using AspNetExample.Service.Services;
using AspNetExample.Service;
using Moq;
using NUnit.Framework;
using System;
using MyDomainModel = AspNetExample.Domain.Models.MyModel;
using MyServiceModel = AspNetExample.Service.Models.MyModel;

namespace AspNetExample.Domain.Test
{
    [TestFixture]
    public class ServiceTest
    {
        private IService _service;
        private Mock<IRepository> _repositoryMock;
        private Mock<IModelDescriptionProvider> _modelDescriptionProviderMock;

        private readonly Guid _id = Guid.Parse("75D50016-466F-4030-9EAE-4D8D690C9957");
        private readonly MyDomainModel _domainModel = new("testData", Guid.Parse("75D50016-466F-4030-9EAE-4D8D690C9957"));
        private readonly MyServiceModel _serviceModel = new("testData", Guid.Parse("75D50016-466F-4030-9EAE-4D8D690C9957"));
        private const string TestDescription = "Test description";

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
            _modelDescriptionProviderMock = new Mock<IModelDescriptionProvider>(MockBehavior.Strict);

            _service = new AspNetExample.Service.Services.Service(_repositoryMock.Object, _modelDescriptionProviderMock.Object);
        }

        [Test]
        public void CreateModel_ShouldCallRepositoryAndConvertModel()
        {
            const string data = "testData";
            _repositoryMock.Setup(x => x.CreateModel(data)).Returns(_domainModel);
            var actual = _service.CreateModel(data);
            Assert.That(actual, Is.EqualTo(_serviceModel));
            _repositoryMock.Verify(x => x.CreateModel(data), Times.Once);
        }

        [Test]
        public void GetModel_ShouldCallRepositoryAndProviderAndAddDescription()
        {
            _repositoryMock.Setup(x => x.GetModel(_id)).Returns(_domainModel);
            _modelDescriptionProviderMock.Setup(x => x.GetDescription(_id)).Returns(TestDescription);
            var expected = _serviceModel with { Description = TestDescription };

            var actual = _service.GetModel(_id);

            Assert.That(actual, Is.EqualTo(expected));
            _repositoryMock.Verify(x => x.GetModel(_id), Times.Once);
            _modelDescriptionProviderMock.Verify(x => x.GetDescription(_id), Times.Once);
        }

        [Test]
        public void UpdateModel_ShouldConvertCallRepositoryAndConvertBack()
        {
            _repositoryMock.Setup(x => x.UpdateModel(It.IsAny<MyDomainModel>())).Returns(_domainModel);
            var actual = _service.UpdateModel(_serviceModel);
            Assert.That(actual, Is.EqualTo(_serviceModel));
            _repositoryMock.Verify(x => x.UpdateModel(It.Is<MyDomainModel>(d => d.Id == _id)), Times.Once);
        }

        [Test]
        public void DeleteModel_ShouldCallRepository()
        {
            _repositoryMock.Setup(x => x.DeleteModel(_id)).Returns(true);
            var actual = _service.DeleteModel(_id);
            Assert.That(actual, Is.True);
            _repositoryMock.Verify(x => x.DeleteModel(_id), Times.Once);
        }
    }
}