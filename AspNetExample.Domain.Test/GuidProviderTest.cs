using AspNetExample.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AspNetExample.Domain.Test
{

    [TestFixture]
    public class GuidProviderTest
    {
        [Test]
        public void GetGuid_ShouldReturnNonEmptyGuid()
        {
            // Arrange
            IGuidProvider provider = new GuidProvider();

            // Act
            var actual = provider.GetGuid();

            // Assert
            Assert.That(actual, Is.Not.EqualTo(Guid.Empty));
        }
    }
}