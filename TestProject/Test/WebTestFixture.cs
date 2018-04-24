using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Web;
using Web.Models;

namespace TestProject
{
    public class WebTestFixture
    {
        public ServiceProvider _clientService { get; private set; }

        public TestServer _testServer { get; }
        public HttpClient _client { get; }

        public IServiceProvider _serverServices { get; }

        public WebTestFixture()
        {
            IWebHostBuilder builder = WebHost.CreateDefaultBuilder()
                                         .ConfigureServices(services =>
                                         {
                                             services.AddDbContext<UserInfoContext>(option => { option.UseInMemoryDatabase("UserInfoContext"); });
                                         }).UseStartup<Startup>();
            _testServer = new TestServer(builder);
            _client = _testServer.CreateClient();
            _serverServices = _testServer.Host.Services;
            SeedData(_serverServices.GetService<UserInfoContext>());
            ConfigClientService(_serverServices);
        }

        private void ConfigClientService(IServiceProvider serverServices)
        {
            var services = new ServiceCollection();
            //add client Need service
            _clientService = services.BuildServiceProvider();
        }

        public void SeedData(UserInfoContext dbContext)
        {
            dbContext.AddRange(new List<UserInfoDto>() {
                new UserInfoDto(){Name="1",CreateTime=DateTime.Now},
                new UserInfoDto(){ Name="2",CreateTime=DateTime.Now},
                new UserInfoDto(){ Name="3",CreateTime=DateTime.Now}
            });

            dbContext.SaveChanges();
        }
    }
}
