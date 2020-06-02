using System.Collections.Generic;

namespace ocasAssignment.Models.Communication
{
    /// <summary>
    /// This is a communication model that defines a response to an Event Details request.
    /// </summary>
    public class EventDetailsResponseDTO
    {

        public bool Result { get; set; }

        public string ErrorMessage { get; set; }

        public List<EmployeeDTO> Employees { get; set; }

    }
}
