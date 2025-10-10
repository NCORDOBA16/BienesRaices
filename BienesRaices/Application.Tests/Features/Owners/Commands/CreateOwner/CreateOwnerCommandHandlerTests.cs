using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.Contracts.Services.FileServices;
using Application.DTOs.Owners;
using Application.Features.Owners.Commands.CreateOwner;
using Application.Tests.Shared;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace Application.Tests.Features.Owners.Commands.CreateOwner
{
    public class CreateOwnerCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock = null!;
        private IMapper _mapper = null!;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = TestFixtures.CreateUnitOfWorkMock();
            _mapper = TestFixtures.CreateMapper();
        }

        [Test]
        public async Task Handler_Should_Create_Owner_When_Valid()
        {
            // Arrange
            var ownerRepoMock = TestFixtures.CreateRepoMock<Owner>();
            ownerRepoMock.Setup(r => r.AddAsync(It.IsAny<Owner>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Owner o, CancellationToken _) => o)
                .Verifiable();

            _unitOfWorkMock.Setup(u => u.Repository<Owner>()).Returns(ownerRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(1);

            // 🔹 Mock del servicio de imágenes
            var imageServiceMock = new Mock<IImageUploadService>();
            imageServiceMock
                .Setup(s => s.UploadImageAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync("https://fakeurl.com/foto.jpg");

            // 🔹 Crear el handler con los tres parámetros
            var handler = new CreateOwnerCommandHandler(_unitOfWorkMock.Object, _mapper, imageServiceMock.Object);

            // 🔹 Simular un archivo (imagen)
            var fileMock = new Mock<IFormFile>();
            var content = "FakeImageContent";
            var fileName = "test.jpg";
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(ms.Length);
            fileMock.Setup(f => f.ContentType).Returns("image/jpeg");

            // 🔹 Crear el comando
            var command = new CreateOwnerCommand
            {
                Owner = new CreateOwnerDto
                {
                    Name = "John Doe",
                    Address = "Calle 123",
                    Birthday = new DateTime(1990, 1, 1),
                    Photo = fileMock.Object // 👈 Aquí va la imagen simulada
                }
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            ownerRepoMock.Verify(r => r.AddAsync(It.IsAny<Owner>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Name, Is.EqualTo("John Doe"));
        }
    }

}
