using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ocasAssignment.Models.Communication;
using ocasAssignment.Models.Database;
using ocasAssignment.Services;

namespace ocasAssignment.Controllers
{
    /// <summary>
    /// This is the API for Employee Sign Ups
    /// </summary>
    [Route("API/SignUp")]
    [ApiController]
    public class SignUpController : Controller
    {

        private ISignUpService _signUpService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="signUpService"></param>
        public SignUpController(ISignUpService signUpService)
        {

            _signUpService = signUpService;

        }

        /// <summary>
        /// This method is used to SignUp a new employee for an event
        /// </summary>
        /// <param name="employeeSignUpDTO">EmployeeSignUp object that specifies the employee and event details</param>
        /// <returns>Returns whether the operation was successful and an error message</returns>
        [HttpPost]
        public IActionResult GetAll([FromBody] EmployeeSignUpDTO employeeSignUpDTO)
        {

            if(employeeSignUpDTO == null)
            {
                return new BadRequestResult();
            }

            var result = _signUpService.NewSignUp(employeeSignUpDTO);

            if (result == null)
            {
                //Internal Server Error
                return StatusCode(500);
            }

            return new JsonResult(result);
        }
    }
}