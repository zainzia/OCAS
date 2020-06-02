
namespace ocasAssignment.Models.Database
{
    /// <summary>
    /// This is a Database Entity Model that defines an Employee Sign Up
    /// </summary>
    public class EmployeeSignUps
    {

        public int Id { get; set; }

        public Event Event { get; set; }

        public Employee Employee { get; set; }

        public string Comments { get; set; }

    }
}
