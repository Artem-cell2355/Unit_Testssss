using AspNetExample.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AspNetExample.Domain.Test
{
    internal class DateTimeProviderTest
    {
    }
}

[TestFixture]
public class DateTimeProviderTest
{
    [Test]
    public void GetDateTime_ShouldReturnUtcTime()
    {
        IDateTimeProvider provider = new DateTimeProvider();
        var actual = provider.GetDateTime();
        Assert.That(actual.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(actual, Is.InRange(DateTime.UtcNow.AddSeconds(-5), DateTime.UtcNow.AddSeconds(5)));
    }
}