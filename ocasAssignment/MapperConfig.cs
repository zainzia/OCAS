using AutoMapper;

using ocasAssignment.Models.Communication;

using ocasAssignment.Models.Database;

namespace ocasAssignment
{
    /// <summary>
    /// This file defines the Mapping Profile between the Database Entities and the Communication Models.
    /// </summary>
    public class MapperConfig : Profile
    {

        public MapperConfig()
        {
            CreateMap<Event, EventDTO>();

            CreateMap<Employee, EmployeeDTO>();

            CreateMap<EmployeeSignUps, EmployeeSignUpDTO>();

            CreateMap<EmployeeSignUps, EmployeeDTO>();
        }

    }
}
