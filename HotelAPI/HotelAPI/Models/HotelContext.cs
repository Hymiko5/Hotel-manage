using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Elasticsearch.Net;

namespace HotelAPI.Models
{
    public class HotelContext:DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Service> services { get; set; }
        public DbSet<RoomType> roomTypes { get; set; }

        private const string connectionString = "Data Source=DESKTOP-2HDJBB3;Initial Catalog=HotelDB;Integrated Security=True;TrustServerCertificate=True";

        public HotelContext(DbContextOptions<HotelContext> options)
        : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);            optionsBuilder.UseSqlServer(connectionString);
        }

        //public UserDTO userToDTO(User user) => new UserDTO
        //{
        //    Id = user.Id,
        //    Name = user.Name,
        //    Phone = user.Phone,
        //    IdentificationCard = user.IdentificationCard,
        //    Gmail = user.Gmail
        //};

        //public ServiceDTO serviceToDTO(Service service) => new ServiceDTO
        //{
        //    Name= service.Name,
        //    Description= service.Description,
        //    Price = service.Price,
        //    UserId = service.UserId
        //};

        //public RoomTypeDTO roomTypeToDTO(RoomType roomType) => new RoomTypeDTO
        //{
        //    Id= roomType.Id,
        //    Description= roomType.Description,
        //    Totals= roomType.Totals,
        //    TypeName = roomType.TypeName
        //};

        public Mapper userToDTO()
        {
            

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
            });

            var mapper = new Mapper(config);
            return mapper;
        }


        public Mapper serviceToDTO()
        {

            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Service, ServiceDTO>();
            });

            var mapper = new Mapper(config);
            return mapper;
        }
        public Mapper roomTypeToDTO()
        {


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoomType, RoomTypeDTO>();
            });

            var mapper = new Mapper(config);
            return mapper;
        }

        public Mapper DTOToUser()
        {


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, User>();
            });

            var mapper = new Mapper(config);
            return mapper;
        }


        public Mapper DTOToService()
        {


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ServiceDTO, Service>();
            });

            var mapper = new Mapper(config);
            return mapper;
        }
        public Mapper DTOToRoomType()
        {


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoomTypeDTO, RoomType>();
            });

            var mapper = new Mapper(config);
            return mapper;
        }

        


        public void SetLogging()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
        }
    }
}
