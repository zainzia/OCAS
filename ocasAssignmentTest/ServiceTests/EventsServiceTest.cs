
using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

using Moq;

using NUnit.Framework;

using ocasAssignment.Data;
using ocasAssignment.Models.Communication;
using ocasAssignment.Models.Database;
using ocasAssignment.Services;

namespace ocasAssignmentTest.ServiceTests
{
    /// <summary>
    /// This fixture tests Events Service
    /// </summary>
    [TestFixture]
    public class EventsServiceTest
    {
        private Mock<AppDbContext> _dbContextMock;

        private Mock<IMapper> _autoMapperMock;

        private Mock<IEmployeeService> _employeeService;

        private IEventsService _eventsService;

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

            //Mock => Localizer
            var stringLocalizerMock = new Mock<IStringLocalizer<IEventsService>>();
            stringLocalizerMock.Setup(p => p[It.IsAny<string>()]).Returns(new LocalizedString("Test", "Test String"));

            //Mock => EmployeeService
            _employeeService = new Mock<IEmployeeService>();

            //Get Services Collection
            var services = new ServiceCollection();

            //Add the neccessary services to instatiate and test the SignUpService
            services.AddSingleton(_dbContextMock.Object);
            services.AddSingleton(_autoMapperMock.Object);
            services.AddSingleton(stringLocalizerMock.Object);
            services.AddSingleton(_employeeService.Object);

            //Add SignUpService
            services.AddTransient<IEventsService, EventsService>();

            //Build the Service Provider
            var serviceProvider = services.BuildServiceProvider();
            //Set the _signUpService
            _eventsService = serviceProvider.GetRequiredService<IEventsService>();
        }

        /// <summary>
        /// This method is used to setup all the required properties and services to test the
        /// GetDtailsIfEmployeeSignedUp() method
        /// </summary>
        /// <param name="eventId">Id of the event</param>
        /// <param name="emailAddress">Email Address of the employee</param>
        /// <returns>EventDetailsDTO object</returns>
        private EventDetailsDTO SetupParametersForTestGetDetailsIfEmployeeSignedUp(int eventId, string emailAddress)
        {
            //Setup the required properties

            var rand = new Random();
            var employee1Id = rand.Next(1000);
            var employee2Id = rand.Next(1000);
            var employee3Id = rand.Next(1000);

            var employee = Mock.Of<EmployeeDTO>(x => x.Id == employee1Id && x.EmailAddress == emailAddress);

            var eventDetails = Mock.Of<EventDetailsDTO>(x => x.emailAddress == emailAddress && x.eventId == eventId);

            var employees = new List<Employee>
            {
                new Employee { FirstName = "Test A", LastName = "Test A2", EmailAddress = "testA@testA2.com", Id = rand.Next(1000) },
                new Employee { FirstName = "Test B", LastName = "Test B2", EmailAddress = "testB@testB2.com", Id = rand.Next(1000) },
                new Employee { FirstName = "Test C", LastName = "Test C2", EmailAddress = "testC@testC2.com", Id = employee3Id },
                new Employee { FirstName = "Test D", LastName = "Test D2", EmailAddress = "testD@testD2.com", Id = employee2Id },
                new Employee { FirstName = "Test E", LastName = "Test E2", EmailAddress = emailAddress, Id = employee1Id }
            };
            var employeesQuerable = employees.AsQueryable();

            var events = new List<Event>
            {
                new Event
                {
                    Id = rand.Next(1000),
                    Name = "Event A",
                    StartDate = new DateTime(2020, 6, 3, 18, 0, 0),
                    EndDate = new DateTime(2020, 6, 3, 17, 0, 0)
                },
                new Event
                {
                    Id = rand.Next(1000),
                    Name = "Event B",
                    StartDate = new DateTime(2020, 6, 4, 18, 0, 0),
                    EndDate = new DateTime(2020, 6, 4, 17, 0, 0)
                },
                new Event
                {
                    Id = rand.Next(1000),
                    Name = "Event C",
                    StartDate = new DateTime(2020, 6, 5, 18, 0, 0),
                    EndDate = new DateTime(2020, 6, 5, 17, 0, 0)
                },
                new Event
                {
                    Id = eventId,
                    Name = "Event D",
                    StartDate = new DateTime(2020, 6, 6, 18, 0, 0),
                    EndDate = new DateTime(2020, 6, 6, 17, 0, 0)
                },
                new Event
                {
                    Id = rand.Next(1000),
                    Name = "Event E",
                    StartDate = new DateTime(2020, 6, 7, 18, 0, 0),
                    EndDate = new DateTime(2020, 6, 7, 17, 0, 0)
                },
            };
            var eventsQuerable = events.AsQueryable();

            var employeeSignUps = new List<EmployeeSignUps>
            {
                new EmployeeSignUps { Id = rand.Next(1000), Employee = employees[0], Event = events[0], Comments = "Test Comments A" },
                new EmployeeSignUps { Id = rand.Next(1000), Employee = employees[1], Event = events[1], Comments = "Test Comments B" },
                new EmployeeSignUps { Id = rand.Next(1000), Employee = employees[2], Event = events[3], Comments = "Test Comments C" },
                new EmployeeSignUps { Id = rand.Next(1000), Employee = employees[3], Event = events[3], Comments = "Test Comments D" },
                new EmployeeSignUps { Id = rand.Next(1000), Employee = employees[4], Event = events[3], Comments = "Test Comments E" },
            }.AsQueryable();

            var employeesDTO = new List<EmployeeDTO>
            {
                new EmployeeDTO
                {
                    Id = employees[2].Id, FirstName = employees[2].FirstName, LastName = employees[2].LastName, EmailAddress = employees[2].EmailAddress
                },
                new EmployeeDTO
                {
                    Id = employees[3].Id, FirstName = employees[3].FirstName, LastName = employees[3].LastName, EmailAddress = employees[3].EmailAddress
                },
                new EmployeeDTO
                {
                    Id = employees[4].Id, FirstName = employees[4].FirstName, LastName = employees[4].LastName, EmailAddress = employees[4].EmailAddress
                }
            };

            //Setup the Mock DbSet and its properties
            var employeeSignUpsMock = new Mock<DbSet<EmployeeSignUps>>();

            employeeSignUpsMock.As<IQueryable<EmployeeSignUps>>().Setup(m => m.Provider).Returns(employeeSignUps.Provider);
            employeeSignUpsMock.As<IQueryable<EmployeeSignUps>>().Setup(m => m.Expression).Returns(employeeSignUps.Expression);
            employeeSignUpsMock.As<IQueryable<EmployeeSignUps>>().Setup(m => m.ElementType).Returns(employeeSignUps.ElementType);
            employeeSignUpsMock.As<IQueryable<EmployeeSignUps>>().Setup(m => m.GetEnumerator()).Returns(employeeSignUps.GetEnumerator());

            var employeeDbSetMock = new Mock<DbSet<Employee>>();

            employeeDbSetMock.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employeesQuerable.Provider);
            employeeDbSetMock.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employeesQuerable.Expression);
            employeeDbSetMock.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employeesQuerable.ElementType);
            employeeDbSetMock.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employeesQuerable.GetEnumerator());

            var eventDbSetMock = new Mock<DbSet<Event>>();

            eventDbSetMock.As<IQueryable<Event>>().Setup(m => m.Provider).Returns(eventsQuerable.Provider);
            eventDbSetMock.As<IQueryable<Event>>().Setup(m => m.Expression).Returns(eventsQuerable.Expression);
            eventDbSetMock.As<IQueryable<Event>>().Setup(m => m.ElementType).Returns(eventsQuerable.ElementType);
            eventDbSetMock.As<IQueryable<Event>>().Setup(m => m.GetEnumerator()).Returns(eventsQuerable.GetEnumerator());

            //Setup the Employees field for the _dbContextMock
            _dbContextMock.Setup(m => m.EmployeeSignUps).Returns(employeeSignUpsMock.Object);

            //Setup the Mock for AutoMapper
            _autoMapperMock.Setup(m => m.Map<List<EmployeeDTO>>(It.IsNotNull<IEnumerable<Employee>>())).Returns(employeesDTO);

            //Setup the _employeeService methods
            _employeeService.Setup(x => x.GetEmployee(It.IsAny<string>())).Returns(employee);

            return eventDetails;
        }


        /// <summary>
        /// This method tests the GetDetailsIfEmployeeSignedUp() method of EventsService with valid input.
        /// </summary>
        [Test]
        public void TestGetDetailsIfEmployeeSignedUp_ValidInput()
        {
            var eventDetails = SetupParametersForTestGetDetailsIfEmployeeSignedUp(30, "randomA@randomB.com");

            var employeeSignUpList = _eventsService.GetDetailsIfEmployeeSignedUp(eventDetails);

            //Verify the Employees field for the _dbContextMock
            _dbContextMock.Verify(m => m.EmployeeSignUps, Times.Once);

            //Setup the Mock for AutoMapper
            _autoMapperMock.Verify(m => m.Map<List<EmployeeDTO>>(It.IsNotNull<IEnumerable<Employee>>()), Times.Once);

            //Setup the _employeeService methods
            _employeeService.Verify(x => x.GetEmployee(It.IsAny<string>()), Times.Once);

            //Test the returned object
            Assert.IsTrue(employeeSignUpList.Result);
            Assert.IsEmpty(employeeSignUpList.ErrorMessage);
            Assert.AreEqual(3, employeeSignUpList.Employees.Count());
        }


        /// <summary>
        /// This method tests the GetDetailsIfEmployeeSignedUp() method of EventsService with invalid input.
        /// The eventId = 0 in this test.
        /// </summary>
        [Test]
        public void TestGetDetailsIfEmployeeSignedUp_InvalidEventId()
        {
            var eventDetails = SetupParametersForTestGetDetailsIfEmployeeSignedUp(0, "randomA@randomB.com");
            
            var employeeSignUpList = _eventsService.GetDetailsIfEmployeeSignedUp(eventDetails);

            //Verify the Employees field for the _dbContextMock
            _dbContextMock.Verify(m => m.EmployeeSignUps, Times.Never);

            //Setup the Mock for AutoMapper
            _autoMapperMock.Verify(m => m.Map<List<EmployeeDTO>>(It.IsNotNull<IEnumerable<Employee>>()), Times.Never);

            //Setup the _employeeService methods
            _employeeService.Verify(x => x.GetEmployee(It.IsAny<string>()), Times.Never);

            //Test the returned object
            Assert.IsNull(employeeSignUpList);
        }

        /// <summary>
        /// This method tests the GetDetailsIfEmployeeSignedUp() method of EventsService with invalid input.
        /// The emailAddress is null in this test.
        /// </summary>
        [Test]
        public void TestGetDetailsIfEmployeeSignedUp_InvalidEmailAddress()
        {
            var eventDetails = SetupParametersForTestGetDetailsIfEmployeeSignedUp(30, null);

            var employeeSignUpList = _eventsService.GetDetailsIfEmployeeSignedUp(eventDetails);

            //Verify the Employees field for the _dbContextMock
            _dbContextMock.Verify(m => m.EmployeeSignUps, Times.Never);

            //Setup the Mock for AutoMapper
            _autoMapperMock.Verify(m => m.Map<List<EmployeeDTO>>(It.IsNotNull<IEnumerable<Employee>>()), Times.Never);

            //Setup the _employeeService methods
            _employeeService.Verify(x => x.GetEmployee(It.IsAny<string>()), Times.Never);

            //Test the returned object
            Assert.IsFalse(employeeSignUpList.Result);
            Assert.IsNotEmpty(employeeSignUpList.ErrorMessage);
            Assert.IsNull(employeeSignUpList.Employees);
        }


    }
}
