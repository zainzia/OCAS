using Microsoft.AspNetCore.Mvc;
using ocasAssignment.Models.Communication;
using ocasAssignment.Services;

namespace ocasAssignment.Controllers
{
    /// <summary>
    /// This is the API for the Events Entity
    /// </summary>
    [Route("API/Events")]
    [ApiController]
    public class EventsController : Controller
    {

        private IEventsService _eventsService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventsService"></param>
        public EventsController(IEventsService eventsService) 
        {

            _eventsService = eventsService;

        }

        /// <summary>
        /// This API method is used to obtain all events.
        /// </summary>
        /// <returns>A list of all events</returns>
        [HttpGet("All")]
        public IActionResult GetAll()
        {

            var events = _eventsService.GetAll();

            if (events == null)
            {
                return new EmptyResult();
            }

            return new JsonResult(events);
        }

        /// <summary>
        /// This API method is used to obtain a specific event.
        /// </summary>
        /// <param name="id">id of the event for which to obtain the details</param>
        /// <returns>The event details</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            var _event = _eventsService.Get(id);

            if (_event == null)
            {
                return new EmptyResult();
            }

            return new JsonResult(_event);
        }

        /// <summary>
        /// This API method is used to obtain all the employees signed up for an event
        /// </summary>
        /// <param name="eventDetails">EventDetails parameter that specifies the event and the employee</param>
        /// <returns>A list of all the signed up employees for the event</returns>
        [HttpPost("Employees")]
        public IActionResult GetEmployeesForEvent(EventDetailsDTO eventDetails)
        {

            if(eventDetails == null)
            {
                return new BadRequestResult();
            }

            var signUpDetails = _eventsService.GetDetailsIfEmployeeSignedUp(eventDetails);

            if (signUpDetails == null)
            {
                return new EmptyResult();
            }

            return new JsonResult(signUpDetails);
        }

    }
}
