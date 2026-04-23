using AspNetExample.Service;
using AspNetExample.Shared;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AspNetExample.Domain.Test
{
    internal class ModelDescriptionProviderTest
    {
    }
}

[TestFixture]
public class ModelDescriptionProviderTest
{
    private IModelDescriptionProvider _provider;
    private Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly Guid _testId = Guid.Parse("75D50016-466F-4030-9EAE-4D8D690C9957");
    private readonly DateTime _testDateTime = new(2025, 12, 8, 10, 30, 0, DateTimeKind.Utc);

    [SetUp]
    public void Setup()
    {
        _dateTimeProviderMock = new Mock<IDateTimeProvider>(MockBehavior.Strict);
        _provider = new ModelDescriptionProvider(_dateTimeProviderMock.Object);
    }

    [Test]
    public void GetDescription_ShouldReturnFormattedStringWithDateTime()
    {
        _dateTimeProviderMock.Setup(x => x.GetDateTime()).Returns(_testDateTime);
        var expected = $"Some data for model {_testId}: {_testDateTime}";

        var actual = _provider.GetDescription(_testId);

        Assert.That(actual, Is.EqualTo(expected));
        _dateTimeProviderMock.Verify(x => x.GetDateTime(), Times.Once);
    }
}