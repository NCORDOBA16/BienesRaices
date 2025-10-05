using Application.Mappings.Profiles;
using AutoMapper;

namespace Application.Tests.Mappings.Profiles
{
    [TestFixture]
    public class AppProfileTests
    {
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ApplicationProfileHierarchy>();
            });
            _mapper = config.CreateMapper();
        }

        [Test]
        public void Should_Map_CargaGenerada_To_CargaGeneradaDto()
        {
            // Arrange
            var entity = new ExampleEntity { };

            // Act
            var dto = _mapper.Map<ExampleEntityDto>(entity);

            // Assert
            Assert.That(dto, Is.Not.Null);
        }

    }

    public class ExampleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class ExampleEntityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class ApplicationProfileHierarchy : ApplicationProfile
    {
        public ApplicationProfileHierarchy()
        {
            CreateMap<ExampleEntity, ExampleEntityDto>().ReverseMap();
        }
    }
}