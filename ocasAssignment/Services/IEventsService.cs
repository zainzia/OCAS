using System.Collections.Generic;

using ocasAssignment.Models.Communication;
using ocasAssignment.Models.Database;

namespace ocasAssignment.Services
{
    public interface IEventsService
    {

        IList<EventDTO> GetAll();

        EventDTO Get(int eventId);

        IList<EmployeeSignUps> GetSignUpsForEvent(int eventId);

        EmployeeSignUpDTO AddEmployeeToEvent(int eventId, int employeeId, string comments);

        EventDetailsResponseDTO GetDetailsIfEmployeeSignedUp(EventDetailsDTO eventDetailsDTO);

    }
}
