
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CodeChallenge.Models;

using CodeChallenge.Tests.Integration.Extensions;
using CodeChallenge.Tests.Integration.Helpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeChallenge.Tests.Integration
{
    [TestClass]
    public class EmployeeControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public async Task CreateEmployee_Returns_Created()
        {
            // Arrange
            var employee = new Employee()
            {
                Department = "Complaints",
                FirstName = "Debbie",
                LastName = "Downer",
                Position = "Receiver",
            };

            var requestContent = JsonSerializer.Serialize(employee);

            // Execute
            var response = await _httpClient.PostAsync("api/employee",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var newEmployee = await response.DeserializeContent<Employee>();
            newEmployee.EmployeeId.Should().NotBeNull();
            newEmployee.Should().BeEquivalentTo(employee, o => o.Excluding(e => e.EmployeeId));
        }

        [TestMethod]
        public async Task GetEmployeeById_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";

            // Execute
            var response = await _httpClient.GetAsync($"api/employee/{employeeId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var employee = await response.DeserializeContent<Employee>();
            employee.FirstName.Should().Be(expectedFirstName);
            employee.LastName.Should().Be(expectedLastName);
        }

        [TestMethod]
        public async Task UpdateEmployee_Returns_Ok()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                Department = "Engineering",
                FirstName = "Pete",
                LastName = "Best",
                Position = "Developer VI",
            };
            var requestContent = JsonSerializer.Serialize(employee);

            // Execute
            var response = await _httpClient.PutAsync($"api/employee/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var newEmployee = await response.DeserializeContent<Employee>();
            newEmployee.Should().BeEquivalentTo(employee);
        }

        [TestMethod]
        public async Task UpdateEmployee_Returns_NotFound()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "Invalid_Id",
                Department = "Music",
                FirstName = "Sonny",
                LastName = "Bono",
                Position = "Singer/Song Writer",
            };
            var requestContent = JsonSerializer.Serialize(employee);

            // Execute
            var response = await _httpClient.PutAsync($"api/employee/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
