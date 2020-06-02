using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Localization;
using ocasAssignment.Data;
using ocasAssignment.Models.Communication;
using ocasAssignment.Models.Database;

namespace ocasAssignment.Services
{
    /// <summary>
    /// This is the Events Service.
    /// This service is used by other services and the API to perform operations on the Event Entities.
    /// </summary>
    public class EventsService : IEventsService
    {

        private AppDbContext _appDbContext;

        private IEmployeeService _employeeService;

        private readonly IMapper _mapper;

        private readonly IStringLocalizer<IEventsService> _localizer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appDbContext">Application Database Context object</param>
        /// <param name="mapper">AutoMapper object</param>
        /// <param name="employeeService">Employee Service Injection</param>
        public EventsService(AppDbContext appDbContext, IMapper mapper, IEmployeeService employeeService, IStringLocalizer<IEventsService> localizer) 
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
            _employeeService = employeeService;
            _localizer = localizer;
        }

        /// <summary>
        /// This method is used by the GetDetailsIfEmployeeSignedUp() method to create the EventDetailsResponse object
        /// </summary>
        /// <param name="result">Result of the operation</param>
        /// <param name="errorMessage">Error message if any ocurred during the operation</param>
        /// <param name="employees">A list of employees that have signed up for an event</param>
        /// <returns>EventDetailsResponse object</returns>
        private EventDetailsResponseDTO GetEventDetailsResponseObject(bool result, string errorMessage, List<EmployeeDTO> employees = null)
        {
            return new EventDetailsResponseDTO
            {
                Result = result,
                ErrorMessage = errorMessage,
                Employees = employees
            };
        }

        /// <summary>
        /// This method is used to obtain all Events.
        /// </summary>
        /// <returns>A list of all the Events.</returns>
        public IList<EventDTO> GetAll()
        {
            var events = _appDbContext.Events.ToList();
            return _mapper.Map<List<EventDTO>>(events);
        }

        /// <summary>
        /// This method is used to obtain details about a single Event.
        /// </summary>
        /// <param name="eventId">The Id of the Event for which to obtain the details.</param>
        /// <returns>An Event object</returns>
        public EventDTO Get(int eventId)
        {
            if (eventId == 0)
            {
                return null;
            }

            var events = _appDbContext.Events.FirstOrDefault(x => x.Id == eventId);
            return _mapper.Map<EventDTO>(events);
        }

        /// <summary>
        /// This method provides a list of all the employee sign ups for an event.
        /// </summary>
        /// <param name="eventId">Id of the Event for which to obtain the details</param>
        /// <returns>A list of Employee Sign Ups</returns>
        public IList<EmployeeSignUps> GetSignUpsForEvent(int eventId)
        {

            if(eventId == 0)
            {
                return null;
            }

            return _appDbContext.EmployeeSignUps
                                .Include(x => x.Employee)
                                .Include(x => x.Event)
                                .Where(x => x.Event.Id == eventId).ToList();
        }

        /// <summary>
        /// This method adds an employee to an event.
        /// </summary>
        /// <param name="eventId">Id of the event for which the employee is signing up</param>
        /// <param name="employeeId">Id of the employee who is signing up</param>
        /// <param name="comments">any comments from the employee</param>
        /// <returns>An Employee Sign Up object</returns>
        public EmployeeSignUpDTO AddEmployeeToEvent(int eventId, int employeeId, string comments)
        {
            if (eventId == 0 || employeeId == 0)
            {
                return null;
            }

            var newSignUp = new EmployeeSignUps
            {
                Employee = _appDbContext.Employees.FirstOrDefault(x => x.Id == employeeId),
                Event = _appDbContext.Events.FirstOrDefault(x => x.Id == eventId),
                Comments = comments
            };

            _appDbContext.EmployeeSignUps.Add(newSignUp);
            _appDbContext.SaveChanges();

            return _mapper.Map<EmployeeSignUpDTO>(newSignUp);
        }

        /// <summary>
        /// This method signs up a new employee to an event.
        /// </summary>
        /// <param name="eventDetailsDTO">The details about the event and the employee that is signing up</param>
        /// <returns>The result of the Sign Up operation as well as an error message</returns>
        public EventDetailsResponseDTO GetDetailsIfEmployeeSignedUp(EventDetailsDTO eventDetailsDTO)
        {
            if(eventDetailsDTO == null || eventDetailsDTO.eventId == 0)
            {
                return null;
            }

            if(string.IsNullOrEmpty(eventDetailsDTO.emailAddress))
            {
                return GetEventDetailsResponseObject(false, _localizer["NotAuthorized"].Value);
            }

            //Is Employee registered in system
            var employee = _employeeService.GetEmployee(eventDetailsDTO.emailAddress);

            if(employee == null)
            {
                return GetEventDetailsResponseObject(false, _localizer["NotAuthorized"].Value);
            }

            //Is Employee registered for the Event
            var signUpsForEvent = GetSignUpsForEvent(eventDetailsDTO.eventId);
            if(signUpsForEvent.Any(x => x.Employee.Id == employee.Id))
            {
                return GetEventDetailsResponseObject(true, string.Empty, _mapper.Map<List<EmployeeDTO>>(signUpsForEvent.Select(x => x.Employee)));
            }

            return GetEventDetailsResponseObject(false, _localizer["NotAuthorized"].Value);

        }

    }
}
