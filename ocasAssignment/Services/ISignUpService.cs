using ocasAssignment.Models.Communication;

namespace ocasAssignment.Services
{
    public interface ISignUpService
    {
        EmployeeSignUpResultDTO NewSignUp(EmployeeSignUpDTO employeeSignUpDTO);

    }
}
