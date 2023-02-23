using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CodeChallenge.Models;

using CodeChallenge.Tests.Integration.Extensions;
using CodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;
        private readonly static JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
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

        [DataTestMethod]
        [DataRow("16a596ae-edd3-4847-99fe-c4518e82c86f", 4)]
        [DataRow("03aa1462-ffa9-4978-901b-7c001562cf6f", 2)]
        [DataRow("62c1084e-6e34-4630-93fd-9153afb65309", 0)]
        public async Task GetReportingById_Expected(string employeeId, int expectedReports)
        {
            // Execute
            var response = await _httpClient.GetAsync($"api/reporting/{employeeId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reporting = JsonSerializer.Deserialize<ReportingStructure>(await response.Content.ReadAsStringAsync(), jsonOptions);
            Assert.AreEqual(expectedReports, reporting.NumberOfReports);
        }
    }
}
