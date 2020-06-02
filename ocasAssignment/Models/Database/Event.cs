using System;

namespace ocasAssignment.Models.Database
{
    /// <summary>
    /// This is a Database Entity Model that defines an Event
    /// </summary>
    public class Event
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
