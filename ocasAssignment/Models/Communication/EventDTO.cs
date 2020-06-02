using System;

namespace ocasAssignment.Models.Communication
{
    /// <summary>
    /// This is a communication model that defines an Event.
    /// </summary>
    public class EventDTO
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
