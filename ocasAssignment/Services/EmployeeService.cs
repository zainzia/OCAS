using System.Linq;

using AutoMapper;

using ocasAssignment.Data;
using ocasAssignment.Models.Communication;
using ocasAssignment.Models.Database;

namespace ocasAssignment.Services
{
    /// <summary>
    /// This is the Employee Service.
    /// This service is used by the API to perform operations on Entities and fulfill request made by the front-end.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {

        private readonly AppDbContext _appDbContext;

        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appDbContext">Application Database Context object</param>
        /// <param name="mapper">AutoMapper object</param>
        public EmployeeService(AppDbContext appDbContext, IMapper mapper)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// This method is used to obtain an Employee.
        /// </summary>
        /// <param name="employeeId">The Id of the Employee for which to obtain the details.</param>
        /// <returns>An employee object</returns>
        public EmployeeDTO GetEmployee(int employeeId)
        {
            if(employeeId < 1)
            {
                return null;
            }

            var employee = _appDbContext.Employees.FirstOrDefault(x => x.Id == employeeId);
            return _mapper.Map<EmployeeDTO>(employee);
        }

        /// <summary>
        /// An overload of the GetEmployee(int employeeId) moethod.
        /// This method is used to obtain the details of an Employee using their emailAddress.
        /// </summary>
        /// <param name="emailAddress">The email Address of the employee for which to obtain the details</param>
        /// <returns>An Employee object</returns>
        public EmployeeDTO GetEmployee(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                return null;
            }

            var employee = _appDbContext.Employees.FirstOrDefault(x => x.EmailAddress == emailAddress);
            return _mapper.Map<EmployeeDTO>(employee);
        }

        /// <summary>
        /// This method adds a new Employee in the system.
        /// </summary>
        /// <param name="employeeSignUpDTO">Object that provides the employee sign up details</param>
        /// <returns>The Employee details that have been added to the system</returns>
        public EmployeeDTO AddNewEmployee(EmployeeSignUpDTO employeeSignUpDTO)
        {
            if(employeeSignUpDTO == null || string.IsNullOrEmpty(employeeSignUpDTO.EmailAddress))
            {
                return null;
            }

            var employee = new Employee
            {
                FirstName = employeeSignUpDTO.FirstName,
                LastName = employeeSignUpDTO.LastName,
                EmailAddress = employeeSignUpDTO.EmailAddress
            };

            _appDbContext.Employees.Add(employee);
            _appDbContext.SaveChanges();

            return _mapper.Map<EmployeeDTO>(employee);
        }

    }
}
