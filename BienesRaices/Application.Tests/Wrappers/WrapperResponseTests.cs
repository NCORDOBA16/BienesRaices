using Application.Wrappers;

namespace Application.Tests.Wrappers
{
    [TestFixture]
    public class WrapperResponseTests
    {
        [Test]
        public void Constructor_WithEmptyBody_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var ctor = new WrapperResponse<List<string>>();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(ctor.Data, Is.Null);
                Assert.That(ctor.Succeeded, Is.False);
            });
        }

        [Test]
        public void Constructor_WithMessageBody_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var ctor = new WrapperResponse<List<string>>("test_message");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(ctor.Data, Is.Null);
                Assert.That(ctor.Succeeded, Is.False);
                Assert.That(ctor.Message, Is.EqualTo("test_message"));
            });
        }

        [Test]
        public void Constructor_WithDataAndMessageBody_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var sData = new List<string> { "Test1", "Test2" };
            var ctor = new WrapperResponse<List<string>>(sData, "test_message");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(ctor.Data, Is.Not.Null);
                Assert.That(ctor.Succeeded, Is.True);
                Assert.That(ctor.Message, Is.EqualTo("test_message"));
            });
        }
    }
}
