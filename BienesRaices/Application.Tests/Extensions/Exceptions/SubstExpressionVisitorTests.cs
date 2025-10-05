using System.Linq.Expressions;
using Application.Tools.Expressions;


namespace Application.Tests.Extensions.Exceptions
{
    [TestFixture]
    public class SubstExpressionVisitorTests
    {
        [Test]
        public void VisitParameter_WhenParameterExistsInSubstitutions_ShouldReturnSubstitutedExpression()
        {
            // Arrange
            var visitor = new SubstExpressionVisitor();
            var originalParameter = Expression.Parameter(typeof(int), "a");
            var substitutedParameter = Expression.Parameter(typeof(int), "b");

            // Añadimos la regla de sustitución al diccionario del visitor.
            visitor.Subst[originalParameter] = substitutedParameter;

            // Act
            // El método público 'Visit' de ExpressionVisitor iniciará el proceso de visita.
            var result = visitor.Visit(originalParameter);

            // Assert
            // Verificamos que el resultado es el parámetro de sustitución.
            Assert.That(result, Is.EqualTo(substitutedParameter));
        }

        [Test]
        public void VisitParameter_WhenParameterDoesNotExistInSubstitutions_ShouldReturnOriginalExpression()
        {
            // Arrange
            var visitor = new SubstExpressionVisitor();
            var originalParameter = Expression.Parameter(typeof(string), "param");

            // El diccionario de sustituciones está vacío.

            // Act
            var result = visitor.Visit(originalParameter);

            // Assert
            // Verificamos que el resultado es el mismo parámetro original.
            Assert.That(result, Is.EqualTo(originalParameter));
        }
    }
}