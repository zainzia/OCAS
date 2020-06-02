using ocasAssignment.Models.Communication;

namespace ocasAssignment.Services
{
    public interface IEmployeeService
    {

        EmployeeDTO GetEmployee(int employeeId);

        EmployeeDTO GetEmployee(string emailAddress);

        EmployeeDTO AddNewEmployee(EmployeeSignUpDTO employeeSignUpDTO);

    }
}
