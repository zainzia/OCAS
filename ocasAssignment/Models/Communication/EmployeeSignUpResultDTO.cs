namespace ocasAssignment.Models.Communication
{
    /// <summary>
    /// This is a communication model that describes the result of an Employee SignUp.
    /// </summary>
    public class EmployeeSignUpResultDTO
    {

        public bool Result { get; set; }

        public string ErrorMessage { get; set; }

        public EmployeeSignUpDTO EmployeeSignUp { get; set; }

    }
}
