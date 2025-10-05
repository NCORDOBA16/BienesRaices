using System.Linq.Expressions;
using Application.Tools.Expressions;

namespace Application.Tests.Tools.Expressions
{
    [TestFixture]
    public class PredicateBuilderTests
    {
        [Test]
        public void And_CombinesPredicatesWithAnd()
        {
            // Arrange
            Expression<Func<int, bool>> predicate1 = x => x > 1;
            Expression<Func<int, bool>> predicate2 = x => x < 3;

            // Act
            var combined = predicate1.And(predicate2);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(combined.Compile()(2));
                Assert.That(combined.Compile()(1), Is.False);
                Assert.That(combined.Compile()(3), Is.False);
            });
        }

        [Test]
        public void Or_CombinesPredicatesWithOr()
        {
            // Arrange
            Expression<Func<int, bool>> predicate1 = x => x < 2;
            Expression<Func<int, bool>> predicate2 = x => x > 3;

            // Act
            var combined = predicate1.Or(predicate2);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(combined.Compile()(1));
                Assert.That(combined.Compile()(4));
                Assert.That(combined.Compile()(2), Is.False);
            });
        }

        [Test]
        public void CombineWithAnd_CombinesMultiplePredicatesWithAnd()
        {
            // Arrange
            var predicates = new List<Expression<Func<int, bool>>>
            {
                x => x > 1,
                x => x < 3,
                x => x != 2
            };

            // Act
            var combined = predicates.CombineWithAnd();

            // Assert
            Assert.That(combined.Compile()(2), Is.False);
        }

        [Test]
        public void CombineWithOr_CombinesMultiplePredicatesWithOr()
        {
            // Arrange
            var predicates = new List<Expression<Func<int, bool>>>
            {
                x => x < 2,
                x => x > 3,
                x => x == 2
            };

            // Act
            var combined = predicates.CombineWithOr();

            // Assert
            Assert.That(combined.Compile()(2));
        }
    }
}
