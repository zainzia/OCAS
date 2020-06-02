using System.Linq;

using Microsoft.Extensions.Localization;

using ocasAssignment.Models.Communication;


namespace ocasAssignment.Services
{
    /// <summary>
    /// This service is used by the API to sign up a new employee to an event.
    /// </summary>
    public class SignUpService : ISignUpService
    {

        private readonly IEventsService _eventsService;

        private readonly IEmployeeService _employeeService;
        
        private readonly IStringLocalizer<ISignUpService> _localizer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventsService">Events Service Injection</param>
        /// <param name="employeeService">Employees Service Injection</param>
        public SignUpService(IEventsService eventsService, IEmployeeService employeeService, IStringLocalizer<ISignUpService> localizer)
        {
            _eventsService = eventsService;
            _employeeService = employeeService;
            _localizer = localizer;
        }

        /// <summary>
        /// This method gets an employee with the provided email address
        /// </summary>
        /// <param name="emailAddress">The email Address of the employee</param>
        /// <returns>An Employee object</returns>
        private EmployeeDTO GetEmployee(string emailAddress)
        {
            return _employeeService.GetEmployee(emailAddress);
        }

        /// <summary>
        /// This method adds a new employee to the system
        /// </summary>
        /// <param name="employeeSignUpDTO">The EmployeeSignUp object</param>
        /// <returns>The Employee object</returns>
        private EmployeeDTO AddNewEmployee(EmployeeSignUpDTO employeeSignUpDTO)
        {
            return _employeeService.AddNewEmployee(employeeSignUpDTO);
        }

        /// <summary>
        /// This method checks if the Employee is already registered for the event.
        /// </summary>
        /// <param name="eventId">The Id of the event</param>
        /// <param name="employeeId">The Id of the employee</param>
        /// <returns>A boolean indicating whether the employee is registered for the event.</returns>
        private bool IsEmployeeRegisteredForEvent(int eventId, int employeeId)
        {
            var signUps = _eventsService.GetSignUpsForEvent(eventId);

            return signUps.Any(x => x.Employee.Id == employeeId);
        }

        /// <summary>
        /// This method Signs Up a new Employee for an event.
        /// </summary>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="employeeId">Id of the Employee</param>
        /// <param name="comments">Any Comments the Employee added in the Sign Up process.</param>
        /// <returns></returns>
        private EmployeeSignUpDTO SignUpNewEmployee(int eventId, int employeeId, string comments)
        {
            return _eventsService.AddEmployeeToEvent(eventId, employeeId, comments);
        }

        /// <summary>
        /// This method is used by NewSignUp() method to create an object that specifies the result of the Sign Up operation.
        /// </summary>
        /// <param name="result">Result of the operation</param>
        /// <param name="error">Eror Message</param>
        /// <param name="employeeSignUpDTO">Employee Sign Up object</param>
        /// <returns></returns>
        private EmployeeSignUpResultDTO GetEmployeeSignUpResult(bool result, string error, EmployeeSignUpDTO employeeSignUpDTO = null)
        {
            return new EmployeeSignUpResultDTO
            {
                Result = result,
                ErrorMessage = error,
                EmployeeSignUp = employeeSignUpDTO
            };
        }

        /// <summary>
        /// This method is used to Sign Up a new Employee for an Event.
        /// The employee will only be signed up if the employee is not already signed up for the event.
        /// </summary>
        /// <param name="employeeSignUpDTO">EmployeeSignUp object</param>
        /// <returns>The result of the operation of an Employee's Sign Up</returns>
        public EmployeeSignUpResultDTO NewSignUp(EmployeeSignUpDTO employeeSignUpDTO)
        {
            if(employeeSignUpDTO == null || 
                (string.IsNullOrEmpty(employeeSignUpDTO.FirstName)) ||
                (string.IsNullOrEmpty(employeeSignUpDTO.LastName)) ||
                (string.IsNullOrEmpty(employeeSignUpDTO.EmailAddress)) ||
                (employeeSignUpDTO.EventId == 0))
            {
                return GetEmployeeSignUpResult(false, _localizer["InvalidData"].Value);
            }

            //Get Employee
            var employee = GetEmployee(employeeSignUpDTO.EmailAddress);
            
            //Add Employee If not exists
            if(employee == null)
            {
                employee = AddNewEmployee(employeeSignUpDTO);
            }

            if(employee == null)
            {
                return GetEmployeeSignUpResult(false, _localizer["InvalidData"].Value);
            }

            //Check If Employee is already registered
            if(IsEmployeeRegisteredForEvent(employeeSignUpDTO.EventId, employee.Id))
            {
                return GetEmployeeSignUpResult(false, _localizer["AlreadyRegistered"].Value);
            }

            //Sign Up Employee for the Event
            var employeeSignUp = SignUpNewEmployee(employeeSignUpDTO.EventId, employee.Id, employeeSignUpDTO.Comments);

            return GetEmployeeSignUpResult(true, string.Empty, employeeSignUp);
        }
    }
}
