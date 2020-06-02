
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using NUnit.Framework;

using ocasAssignment.Data;
using ocasAssignment.Models.Communication;
using ocasAssignment.Models.Database;
using ocasAssignment.Services;


namespace ocasAssignmentTest.ServiceTests
{
    /// <summary>
    /// This Fixture tests the Employee service
    /// </summary>
    [TestFixture]
    public class EmployeeServiceTest
    {

        private Mock<AppDbContext> _dbContextMock;

        private Mock<IMapper> _autoMapperMock;

        private IEmployeeService _employeeService;

        /// <summary>
        /// This is the test SetUp method.
        /// It will setsup the services required for the following Tests. 
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            //Mock => AppDbContext
            var options = new DbContextOptionsBuilder<AppDbContext>().Options;
            _dbContextMock = new Mock<AppDbContext>(options);

            //Mock => AutoMapper
            _autoMapperMock = new Mock<IMapper>();

            //Get Services Collection
            var services = new ServiceCollection();

            //Add the neccessary services to instatiate and test the SignUpService
            services.AddSingleton(_dbContextMock.Object);
            services.AddSingleton(_autoMapperMock.Object);

            //Add SignUpService
            services.AddTransient<IEmployeeService, EmployeeService>();

            //Build the Service Provider
            var serviceProvider = services.BuildServiceProvider();
            //Set the _signUpService
            _employeeService = serviceProvider.GetRequiredService<IEmployeeService>();
        }

        /// <summary>
        /// This method tests the GetEmployee() method of Employee Service.
        /// This test will pass valid input to GetEmployee()
        /// </summary>
        [Test]
        public void TestGetEmployee_ValidInput()
        {
            //Setup the required properties
            var rand = new Random();
            var employeeId = rand.Next(1000);

            var firstName = "Test E";
            var lastName = "Test E2";
            var emailAddress = "testE@testE2.com";

            var employeeDTO = Mock.Of<EmployeeDTO>(e => e.EmailAddress == emailAddress 
                    && e.FirstName == firstName 
                    && e.LastName == lastName 
                    && e.Id == employeeId);

            var employees = new List<Employee>
            {
                new Employee { FirstName = "Test A", LastName = "Test A2", EmailAddress = "testA@testA2.com", Id = rand.Next(1000) },
                new Employee { FirstName = "Test B", LastName = "Test B2", EmailAddress = "testB@testB2.com", Id = rand.Next(1000) },
                new Employee { FirstName = "Test C", LastName = "Test C2", EmailAddress = "testC@testC2.com", Id = rand.Next(1000) },
                new Employee { FirstName = "Test D", LastName = "Test D2", EmailAddress = "testD@testD2.com", Id = rand.Next(1000) },
                new Employee { FirstName = firstName, LastName = lastName, EmailAddress = emailAddress, Id = employeeId }
            }.AsQueryable();

            //Setup the Mock DbSet and its properties
            var employeeDbSetMock = new Mock<DbSet<Employee>>();

            employeeDbSetMock.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employees.Provider);
            employeeDbSetMock.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employees.Expression);
            employeeDbSetMock.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employees.ElementType);
            employeeDbSetMock.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employees.GetEnumerator());

            //Setup the Employees field for the _dbContextMock
            _dbContextMock.Setup(m => m.Employees).Returns(employeeDbSetMock.Object);

            //Setup the Mock for the 
            _autoMapperMock.Setup(m => m.Map<EmployeeDTO>(It.IsNotNull<Employee>())).Returns(employeeDTO);

            //Call the method to test => GetEmployee()
            var employee = _employeeService.GetEmployee(employeeId);

            //Verify _dbContextMock method calls
            _dbContextMock.Verify(x => x.Employees, Times.Once);

            //Verify _autoMapperMock method calls
            _autoMapperMock.Verify(x => x.Map<EmployeeDTO>(It.IsAny<Employee>()), Times.Once);

            //Test the Returned Object
            Assert.IsNotNull(employee);
            Assert.AreEqual(employeeId, employee.Id);
            Assert.AreEqual(firstName, employee.FirstName);
            Assert.AreEqual(lastName, employee.LastName);
            Assert.AreEqual(emailAddress, employee.EmailAddress);
        }

        /// <summary>
        /// This method tests the GetEmployee() method of Employee Service.
        /// This test will pass an invalid input to GetEmployee()
        /// </summary>
        [Test]
        public void TestGetEmployee_InvalidEmployeeId()
        {
            //Setup the required properties
            var rand = new Random();
            var employeeId = -2;

            var firstName = "Test E";
            var lastName = "Test E2";
            var emailAddress = "testE@testE2.com";

            var employeeDTO = Mock.Of<EmployeeDTO>(e => e.EmailAddress == emailAddress
                    && e.FirstName == firstName
                    && e.LastName == lastName
                    && e.Id == 5);

            var employees = new List<Employee>
            {
                new Employee { FirstName = "Test A", LastName = "Test A2", EmailAddress = "testA@testA2.com", Id = rand.Next(1000) },
                new Employee { FirstName = "Test B", LastName = "Test B2", EmailAddress = "testB@testB2.com", Id = rand.Next(1000) },
                new Employee { FirstName = "Test C", LastName = "Test C2", EmailAddress = "testC@testC2.com", Id = rand.Next(1000) },
                new Employee { FirstName = "Test D", LastName = "Test D2", EmailAddress = "testD@testD2.com", Id = rand.Next(1000) },
                new Employee { FirstName = firstName, LastName = lastName, EmailAddress = emailAddress, Id = rand.Next(1000) }
            }.AsQueryable();

            //Setup the Mock DbSet and its properties
            var employeeDbSetMock = new Mock<DbSet<Employee>>();

            employeeDbSetMock.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employees.Provider);
            employeeDbSetMock.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employees.Expression);
            employeeDbSetMock.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employees.ElementType);
            employeeDbSetMock.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employees.GetEnumerator());

            //Setup the Employees field for the _dbContextMock
            _dbContextMock.Setup(m => m.Employees).Returns(employeeDbSetMock.Object);

            //Setup the Mock for the 
            _autoMapperMock.Setup(m => m.Map<EmployeeDTO>(It.IsNotNull<Employee>())).Returns(employeeDTO);

            //Call the method to test => GetEmployee()
            var employee = _employeeService.GetEmployee(employeeId);

            //Verify _dbContextMock method calls
            _dbContextMock.Verify(x => x.Employees, Times.Never);

            //Verify _autoMapperMock method calls
            _autoMapperMock.Verify(x => x.Map<EmployeeDTO>(It.IsAny<Employee>()), Times.Never);

            //Test the Returned Object
            Assert.IsNull(employee);
        }
    }
}
