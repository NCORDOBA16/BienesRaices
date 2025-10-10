using Application.Features.Properties.Commands.UploadPropertyImage;
using AutoMapper;
using Domain.Entities;
using Moq;
using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.Contracts.Persistence.Common.BaseRepository;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using System;
using Application.Tests.Shared;
using Application.Contracts.Services.FileServices;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Application.Tests.Features.Properties.Commands.UploadPropertyImage
{
    public class UploadPropertyImageCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock = null!;
        private IMapper _mapper = null!;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = TestFixtures.CreateUnitOfWorkMock();
            _mapper = TestFixtures.CreateMapper();
        }

        private IFormFile CreateDummyFormFile()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("dummy");
            writer.Flush();
            stream.Position = 0;
            return new FormFile(stream, 0, stream.Length, "name", "file.jpg");
        }

        [Test]
        public async Task Handler_Should_Add_PropertyImage_When_None_Exists()
        {
            var property = new Property { IdProperty = Guid.NewGuid() };

            var propRepoMock = TestFixtures.CreateRepoMock<Property>();
            propRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(property);

            var imageRepoMock = TestFixtures.CreateRepoMock<PropertyImage>();
            imageRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Ardalis.Specification.ISpecification<PropertyImage>>(), It.IsAny<CancellationToken>())).ReturnsAsync((PropertyImage?)null);
            imageRepoMock.Setup(r => r.AddAsync(It.IsAny<PropertyImage>(), It.IsAny<CancellationToken>())).ReturnsAsync((PropertyImage p, CancellationToken _) => p).Verifiable();

            var imageServiceMock = new Mock<IImageUploadService>();
            imageServiceMock.Setup(s => s.UploadImageAsync(It.IsAny<IFormFile>())).ReturnsAsync("https://img.test/file.jpg");

            _unitOfWorkMock.Setup(u => u.Repository<Property>()).Returns(propRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<PropertyImage>()).Returns(imageRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(1);

            var handler = new UploadPropertyImageCommandHandler(_unitOfWorkMock.Object, imageServiceMock.Object);

            var command = new UploadPropertyImageCommand { PropertyId = property.IdProperty, PropertyImage = CreateDummyFormFile() };

            var result = await handler.Handle(command, default);

            imageRepoMock.Verify(r => r.AddAsync(It.IsAny<PropertyImage>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.That(result.Data, Is.EqualTo("https://img.test/file.jpg"));
        }

        [Test]
        public async Task Handler_Should_Update_PropertyImage_When_Exists()
        {
            var property = new Property { IdProperty = Guid.NewGuid() };

            var propRepoMock = TestFixtures.CreateRepoMock<Property>();
            propRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(property);

            var existing = new PropertyImage { IdPropertyImage = Guid.NewGuid(), IdProperty = property.IdProperty, File = "old.jpg" };
            var imageRepoMock = TestFixtures.CreateRepoMock<PropertyImage>();
            imageRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Ardalis.Specification.ISpecification<PropertyImage>>(), It.IsAny<CancellationToken>())).ReturnsAsync(existing);
            imageRepoMock.Setup(r => r.UpdateAsync(It.IsAny<PropertyImage>(), It.IsAny<CancellationToken>())).ReturnsAsync(0).Verifiable();

            var imageServiceMock = new Mock<IImageUploadService>();
            imageServiceMock.Setup(s => s.UploadImageAsync(It.IsAny<IFormFile>())).ReturnsAsync("https://img.test/new.jpg");

            _unitOfWorkMock.Setup(u => u.Repository<Property>()).Returns(propRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<PropertyImage>()).Returns(imageRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(1);

            var handler = new UploadPropertyImageCommandHandler(_unitOfWorkMock.Object, imageServiceMock.Object);

            var command = new UploadPropertyImageCommand { PropertyId = property.IdProperty, PropertyImage = CreateDummyFormFile() };

            var result = await handler.Handle(command, default);

            imageRepoMock.Verify(r => r.UpdateAsync(It.IsAny<PropertyImage>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.That(result.Data, Is.EqualTo("https://img.test/new.jpg"));
        }

        [Test]
        public void Handler_Should_Throw_KeyNotFound_When_Property_NotFound()
        {
            var propRepoMock = TestFixtures.CreateRepoMock<Property>();
            propRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Property?)null);

            _unitOfWorkMock.Setup(u => u.Repository<Property>()).Returns(propRepoMock.Object);

            var imageServiceMock = new Mock<IImageUploadService>();

            var handler = new UploadPropertyImageCommandHandler(_unitOfWorkMock.Object, imageServiceMock.Object);
            var command = new UploadPropertyImageCommand { PropertyId = Guid.NewGuid(), PropertyImage = CreateDummyFormFile() };

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await handler.Handle(command, default));
        }

        [Test]
        public void Handler_Should_Propagate_Exception_When_ImageService_Fails()
        {
            var property = new Property { IdProperty = Guid.NewGuid() };

            var propRepoMock = TestFixtures.CreateRepoMock<Property>();
            propRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(property);

            var imageRepoMock = TestFixtures.CreateRepoMock<PropertyImage>();
            imageRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Ardalis.Specification.ISpecification<PropertyImage>>(), It.IsAny<CancellationToken>())).ReturnsAsync((PropertyImage?)null);

            var imageServiceMock = new Mock<IImageUploadService>();
            imageServiceMock.Setup(s => s.UploadImageAsync(It.IsAny<IFormFile>())).ThrowsAsync(new InvalidOperationException("upload failed"));

            _unitOfWorkMock.Setup(u => u.Repository<Property>()).Returns(propRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<PropertyImage>()).Returns(imageRepoMock.Object);

            var handler = new UploadPropertyImageCommandHandler(_unitOfWorkMock.Object, imageServiceMock.Object);
            var command = new UploadPropertyImageCommand { PropertyId = property.IdProperty, PropertyImage = CreateDummyFormFile() };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, default));
        }
    }
}
