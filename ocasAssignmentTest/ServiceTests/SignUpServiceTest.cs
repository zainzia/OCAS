
using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

using Moq;

using NUnit.Framework;

using ocasAssignment.Models.Communication;
using ocasAssignment.Models.Database;
using ocasAssignment.Services;


namespace ocasAssignmentTest.ServiceTests
{
    /// <summary>
    /// This Fixture tests the SignUp service
    /// </summary>
    [TestFixture]
    public class SignUpServiceTest
    {
        private ISignUpService _signUpService;

        private Mock<IEmployeeService> _employeeService;

        private Mock<IEventsService> _eventsService;

        /// <summary>
        /// This is the test SetUp method.
        /// It will setsup the services required for the following Tests. 
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            //Mock => Localizer
            var stringLocalizerMock = new Mock<IStringLocalizer<ISignUpService>>();
            stringLocalizerMock.Setup(p => p[It.IsAny<string>()]).Returns(new LocalizedString("Test", "Test String"));

            //Mock => EmployeeService
            _employeeService = new Mock<IEmployeeService>();

            //Mock => EventsService
            _eventsService = new Mock<IEventsService>();

            //Get Services Collection
            var services = new ServiceCollection();

            //Add the neccessary services to instatiate and test the SignUpService
            services.AddSingleton(stringLocalizerMock.Object);
            services.AddSingleton(_employeeService.Object);
            services.AddSingleton(_eventsService.Object);

            //Add SignUpService
            services.AddTransient<ISignUpService, SignUpService>();

            //Build the Service Provider
            var serviceProvider = services.BuildServiceProvider();
            //Set the _signUpService
            _signUpService = serviceProvider.GetRequiredService<ISignUpService>();
        }

        /// <summary>
        /// This method will test the NewSignUp() method in SignUp service with valid input.
        /// </summary>
        [Test]
        public void TestNewSignUp_ValidInput()
        {

            //Setup the required properties
            var random = new Random();
            var eventId = random.Next(2000);
            var employeeId = random.Next(2000);

            var emailAddress = "test2@test2.com";
            var comments = "Test 2 Comments";
            var firstName = "Test 2 First Name";
            var lastName = "Test 2 Last Name";

            var employeeDTO = Mock.Of<EmployeeDTO>(e => e.EmailAddress == emailAddress
                && e.FirstName == firstName
                && e.LastName == lastName
                && e.Id == employeeId);

            var employeeSignUps = Mock.Of<List<EmployeeSignUps>>();

            //Setup the test object
            var employeeSignUpDTO = Mock.Of<EmployeeSignUpDTO>(x => x.Comments == comments
                && x.EmailAddress == emailAddress
                && x.EventId == eventId
                && x.FirstName == firstName
                && x.LastName == lastName);

            //Setup the _employeeService methods
            _employeeService.Setup(x => x.GetEmployee(It.IsAny<string>())).Returns((EmployeeDTO)null);
            _employeeService.Setup(x => x.AddNewEmployee(It.IsAny<EmployeeSignUpDTO>())).Returns(employeeDTO);

            //Setup the _eventsService methods
            _eventsService.Setup(x => x.GetSignUpsForEvent(It.IsAny<int>())).Returns(employeeSignUps);
            _eventsService.Setup(x => x.AddEmployeeToEvent(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(employeeSignUpDTO);

            //Call the method to test => NewSignUp()
            var signUpResult = _signUpService.NewSignUp(employeeSignUpDTO);

            //Verify Employee Service method calls
            _employeeService.Verify(m => m.GetEmployee(It.IsAny<string>()), Times.Once);
            _employeeService.Verify(m =>
                m.AddNewEmployee(It.Is<EmployeeSignUpDTO>(s => s.Comments == comments
                    && s.EmailAddress == emailAddress
                    && s.EventId == eventId
                    && s.FirstName == firstName
                    && s.LastName == lastName)
                ), Times.Once);

            //Verify Event Service method calls
            _eventsService.Verify(m => m.GetSignUpsForEvent(It.Is<int>(x => x == eventId)), Times.Once);
            _eventsService.Verify(m => m.AddEmployeeToEvent(It.Is<int>(x => x == eventId),
                It.Is<int>(x => x == employeeId), It.Is<string>(x => x == comments)), Times.Once);

            //Test the Returned Object
            Assert.True(signUpResult.Result);
            Assert.IsEmpty(signUpResult.ErrorMessage);
            Assert.IsNotNull(signUpResult.EmployeeSignUp);

            Assert.AreEqual(signUpResult.EmployeeSignUp.Comments, comments);
            Assert.AreEqual(signUpResult.EmployeeSignUp.EmailAddress, emailAddress);
            Assert.AreEqual(signUpResult.EmployeeSignUp.EventId, eventId);
            Assert.AreEqual(signUpResult.EmployeeSignUp.FirstName, firstName);
            Assert.AreEqual(signUpResult.EmployeeSignUp.LastName, lastName);
        }

        /// <summary>
        /// This method will test the NewSignUp() method in SignUp service with an Employee
        /// that has already signed up for the event.
        /// </summary>
        [Test]
        public void TestNewSignUp_EmployeeAlreadyRegistered()
        {

            //Setup the required properties
            var random = new Random();
            var eventId = random.Next(1000);
            var employeeId = random.Next(1000);

            var emailAddress = "test@test.com";
            var comments = "Test Comments";
            var firstName = "Test First Name";
            var lastName = "Test Last Name";

            var employeeDTO = Mock.Of<EmployeeDTO>(e => e.EmailAddress == emailAddress
                && e.FirstName == firstName
                && e.LastName == lastName
                && e.Id == employeeId);

            var employeeSignUp = Mock.Of<EmployeeSignUps>(y => y.Comments == comments
                    && y.Employee == Mock.Of<Employee>(z => z.EmailAddress == emailAddress
                        && z.FirstName == firstName
                        && z.LastName == lastName
                        && z.Id == employeeId)
                    && y.Event == Mock.Of<Event>(z => z.Id == eventId));

            //Randomly chosen 9 EmployeeSignUps and 1 employeeSignUp object
            var employeeSignUps = new List<EmployeeSignUps>
            {
                employeeSignUp,
                Mock.Of<EmployeeSignUps>(),
                Mock.Of<EmployeeSignUps>(),
                Mock.Of<EmployeeSignUps>(),
                Mock.Of<EmployeeSignUps>(),
                Mock.Of<EmployeeSignUps>(),
                Mock.Of<EmployeeSignUps>(),
                Mock.Of<EmployeeSignUps>(),
                Mock.Of<EmployeeSignUps>(),
                Mock.Of<EmployeeSignUps>()
            };

            //Setup the test object
            var employeeSignUpDTO = Mock.Of<EmployeeSignUpDTO>(x => x.Comments == comments
                && x.EmailAddress == emailAddress
                && x.EventId == eventId
                && x.FirstName == firstName
                && x.LastName == lastName);

            //Setup the _employeeService methods
            _employeeService.Setup(x => x.GetEmployee(It.IsAny<string>())).Returns(employeeDTO);
            _employeeService.Setup(x => x.AddNewEmployee(It.IsAny<EmployeeSignUpDTO>())).Returns(employeeDTO);

            //Setup the _eventsService methods
            _eventsService.Setup(x => x.GetSignUpsForEvent(It.IsAny<int>())).Returns(employeeSignUps);
            _eventsService.Setup(x => x.AddEmployeeToEvent(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(employeeSignUpDTO);

            //Call the test method
            var signUpResult = _signUpService.NewSignUp(employeeSignUpDTO);

            //Verify Employee Service method calls
            _employeeService.Verify(m => m.GetEmployee(It.IsAny<string>()), Times.Once);
            _employeeService.Verify(m =>
                m.AddNewEmployee(It.Is<EmployeeSignUpDTO>(s => s.Comments == comments
                    && s.EmailAddress == emailAddress
                    && s.EventId == eventId
                    && s.FirstName == firstName
                    && s.LastName == lastName)
                ), Times.Never);

            //Verify Event Service method calls
            _eventsService.Verify(m => m.GetSignUpsForEvent(It.Is<int>(x => x == eventId)), Times.Once);
            _eventsService.Verify(m => m.AddEmployeeToEvent(It.Is<int>(x => x == eventId),
                It.Is<int>(x => x == 0), It.Is<string>(x => x == comments)), Times.Never);

            //Test the Returned Object
            Assert.False(signUpResult.Result);
            Assert.IsNotEmpty(signUpResult.ErrorMessage);
            Assert.IsNull(signUpResult.EmployeeSignUp);
        }

        /// <summary>
        /// This method will test the NewSignUp() method in SignUp service with invalid input.
        /// In this case the First Name of the Employee is null.
        /// </summary>
        [Test]
        public void TestNewSignUp_InvalidInput()
        {

            //Setup the required properties
            var random = new Random();
            var eventId = random.Next(1000);
            var employeeId = random.Next(1000);

            var emailAddress = "test@test.com";
            var comments = "Test Comments";
            var lastName = "Test Last Name";
            
            string firstName = null;

            var employeeDTO = Mock.Of<EmployeeDTO>(e => e.EmailAddress == emailAddress
                && e.FirstName == firstName
                && e.LastName == lastName
                && e.Id == employeeId);

            //Setup the test object
            var employeeSignUpDTO = Mock.Of<EmployeeSignUpDTO>(x => x.Comments == comments
                && x.EmailAddress == emailAddress
                && x.EventId == eventId
                && x.FirstName == firstName
                && x.LastName == lastName);

            //Setup the _employeeService methods
            _employeeService.Setup(x => x.GetEmployee(It.IsAny<string>())).Returns(Mock.Of<EmployeeDTO>());
            _employeeService.Setup(x => x.AddNewEmployee(It.IsAny<EmployeeSignUpDTO>())).Returns(Mock.Of<EmployeeDTO>());

            //Setup the _eventsService methods
            _eventsService.Setup(x => x.GetSignUpsForEvent(It.IsAny<int>())).Returns(Mock.Of<IList<EmployeeSignUps>>());
            _eventsService.Setup(x => x.AddEmployeeToEvent(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(Mock.Of<EmployeeSignUpDTO>());

            //Call the test method
            var signUpResult = _signUpService.NewSignUp(employeeSignUpDTO);

            //Verify Employee Service method calls
            _employeeService.Verify(m => m.GetEmployee(It.IsAny<string>()), Times.Never);
            _employeeService.Verify(m =>
                m.AddNewEmployee(It.Is<EmployeeSignUpDTO>(s => s.Comments == comments
                    && s.EmailAddress == emailAddress
                    && s.EventId == eventId
                    && s.FirstName == firstName
                    && s.LastName == lastName)
                ), Times.Never);

            //Verify Event Service method calls
            _eventsService.Verify(m => m.GetSignUpsForEvent(It.Is<int>(x => x == eventId)), Times.Never);
            _eventsService.Verify(m => m.AddEmployeeToEvent(It.Is<int>(x => x == eventId),
                It.Is<int>(x => x == 0), It.Is<string>(x => x == comments)), Times.Never);

            //Test the Returned Object
            Assert.False(signUpResult.Result);
            Assert.IsNotEmpty(signUpResult.ErrorMessage);
            Assert.IsNull(signUpResult.EmployeeSignUp);
        }
    }
}
