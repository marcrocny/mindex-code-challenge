using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CodeChallenge.Tests.Integration.Helpers
{
    public class TestServer : IDisposable, IAsyncDisposable
    {
        private readonly WebApplicationFactory<Program> applicationFactory;

        public TestServer()
        {
            applicationFactory = new WebApplicationFactory<Program>();
        }

        public HttpClient NewClient()
        {
            return applicationFactory.CreateClient();
        }


        public ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);  
            return ((IAsyncDisposable)applicationFactory).DisposeAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            ((IDisposable)applicationFactory).Dispose();
        }
    }
}
