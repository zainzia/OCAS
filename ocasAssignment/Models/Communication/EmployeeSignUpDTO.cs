
namespace ocasAssignment.Models.Communication
{
    /// <summary>
    /// This is a communication model that defines a new Employee Sign up for an event.
    /// </summary>
    public class EmployeeSignUpDTO
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public int EventId { get; set; }

        public string Comments { get; set; }

    }
}
