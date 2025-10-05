using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Moq;
using BienesRaicesAPI.Extensions;

namespace BienesRaicesAPI.Tests.Extensions
{
    [TestFixture]
    public class AppExtensionTests
    {

        [Test]
        public void UseErrorHandlingMiddleware_ShouldCallUseMiddleware()
        {
            var appBuilderMock = new Mock<IApplicationBuilder>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var requestDelegate = new RequestDelegate(context => Task.CompletedTask);

            appBuilderMock.Setup(app => app.ApplicationServices).Returns(serviceProviderMock.Object);
            appBuilderMock.Setup(app => app.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()))
                          .Callback<Func<RequestDelegate, RequestDelegate>>(middleware => middleware(requestDelegate));

            // Act
            AppExtension.UseErrorHandlingMiddleware(appBuilderMock.Object);

            // Assert
            appBuilderMock.Verify(app => app.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce);
        }
    }
}
