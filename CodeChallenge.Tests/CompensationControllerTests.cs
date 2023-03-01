
using System;
using System.Linq;
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
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        private const string _pathTemplate = "api/employee/{0}/compensation";
        private const string _employee_1 = "16a596ae-edd3-4847-99fe-c4518e82c86f";
        private const string _employee_2 = "b7839309-3348-463b-a7e3-5de1c168beb3";
        private readonly static JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true, 
        };

        private Task<HttpResponseMessage> CompensationPut(string employeeId, string json)
            => _httpClient.PutAsync(string.Format(_pathTemplate, employeeId), new StringContent(json, Encoding.UTF8, "application/json"));

        private Task<HttpResponseMessage> CompensationGet(string employeeId)
            => _httpClient.GetAsync(string.Format(_pathTemplate, employeeId));

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
        public async Task Compensation_Create_Happy()
        {
            var model = new CompensationModel
            {
                EffectiveDate = DateTime.Today.ToString("yyyy-MM-dd"),
                Salary = 1m,
            };

            var requestJson = JsonSerializer.Serialize(model);

            // act
            var response = await CompensationPut(_employee_1, requestJson);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returned = JsonSerializer.Deserialize<CompensationModel>(await response.Content.ReadAsStringAsync(), jsonOptions);
            returned.Should().BeEquivalentTo(model);
        }

        [TestMethod]
        public async Task Compensation_Create_FlattenDate()
        {
            var now = DateTime.Now.AddDays(1);
            var model = new CompensationModel
            {
                EffectiveDate = now.ToString(),
                Salary = 2m,
            };

            var requestJson = JsonSerializer.Serialize(model);

            // act
            var response = await CompensationPut(_employee_1, requestJson);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returned = JsonSerializer.Deserialize<CompensationModel>(await response.Content.ReadAsStringAsync(), jsonOptions);
            returned.EffectiveDate.Should().Be(now.Date.ToString("yyyy-MM-dd"));
        }

        [TestMethod]
        public async Task Compensation_Create_InvalidInputs()
        {
            var model = new CompensationModel
            {
                EffectiveDate = "invalid",
                Salary = 0m,
            };

            var requestJson = JsonSerializer.Serialize(model);

            // act
            var response = await CompensationPut(_employee_1, requestJson);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var returned = JsonSerializer.Deserialize<string[]>(await response.Content.ReadAsStringAsync(), jsonOptions);
            returned.Should().HaveCount(2);
            returned.Should().ContainSingle(s => s.Contains("effectiveDate"));
            returned.Should().ContainSingle(s => s.StartsWith("salary must"));
        }

        [TestMethod]
        public async Task Compensation_Retrieve_Happy()
        {
            var model = new CompensationModel
            {
                EffectiveDate = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd"),
                Salary = 3m,
            };

            var requestJson = JsonSerializer.Serialize(model);

            // act
            await CompensationPut(_employee_1, requestJson);
            var response = await CompensationGet(_employee_1);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returned = JsonSerializer.Deserialize<CompensationModel[]>(await response.Content.ReadAsStringAsync(), jsonOptions);
            var fromResponse = returned.Single(c => c.EffectiveDate == model.EffectiveDate);
            fromResponse.Should().BeEquivalentTo(model);
        }

        [TestMethod]
        public async Task Compensation_ExistingEmployee_Retrieve_Empty()
        {
            // act
            var response = await CompensationGet(_employee_2);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returned = await response.Content.ReadAsStringAsync();
            returned.Should().Be("[]"); // empty JSON array
        }

        [TestMethod]
        public async Task Compensation_Update_Expected()
        {
            var model = new CompensationModel
            {
                EffectiveDate = DateTime.Today.AddDays(5).ToString("yyyy-MM-dd"),
                Salary = 1.618m,
            };

            var requestJson = JsonSerializer.Serialize(model);

            // act
            await CompensationPut(_employee_1, requestJson);

            model.Salary = 3.1415m;
            requestJson = JsonSerializer.Serialize(model);
            var putResponse = await CompensationPut(_employee_1, requestJson);
            var getResponse = await CompensationGet(_employee_1);

            // assert
            putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var returned = JsonSerializer.Deserialize<CompensationModel[]>(await getResponse.Content.ReadAsStringAsync(), jsonOptions);
            // still only one
            var fromResponse = returned.Single(c => c.EffectiveDate == model.EffectiveDate);
            // matches new version (updatd salary)
            fromResponse.Should().BeEquivalentTo(model);

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            returned = JsonSerializer.Deserialize<CompensationModel[]>(await getResponse.Content.ReadAsStringAsync(), jsonOptions);
            fromResponse = returned.Single(c => c.EffectiveDate == model.EffectiveDate);
            fromResponse.Should().BeEquivalentTo(model);
        }

        [TestMethod]
        public async Task Compensation_UnknownEmployee_Create_NotFound()
        {
            var model = new CompensationModel
            {
                EffectiveDate = DateTime.Today.ToString("yyyy-MM-dd"),
                Salary = 1m,
            };

            var requestJson = JsonSerializer.Serialize(model);

            // act
            var response = await CompensationPut("who?", requestJson);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task Compensation_UnknownEmployee_Retrieve_NotFound()
        {
            // act
            var response = await CompensationGet("never heard a ya");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
