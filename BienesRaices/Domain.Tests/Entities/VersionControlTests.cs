using Domain.Entities;
using Domain.Entities.Common;

namespace Domain.Tests.Entities
{
    [TestFixture]
    public class VersionControlTests
    {
        [Test]
        public void Entity_Should_Inherit_From_BaseEntity()
        {
            // Arrange & Act
            var versionControl = new VersionControl();
            // Assert

            Assert.That(versionControl, Is.InstanceOf<BaseEntity>());
        }
        [Test]
        public void VersionControl_CurrentVersion_Should_Be_Initialized()
        {
            // Arrange
            var version = "1.0.0";

            // Act
            var versionControl = new VersionControl
            {
                CurrentVersion = version
            };

            // Assert
            Assert.That(versionControl.CurrentVersion, Is.Not.Null);
            Assert.That(versionControl.CurrentVersion, Is.TypeOf<string>());
            Assert.That(versionControl.CurrentVersion, Is.EqualTo(version));
        }
    }
}
