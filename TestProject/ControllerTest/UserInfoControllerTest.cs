using System;
using System.Collections.Generic;
using System.Text;
using Web.Models;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Web.Controllers;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using System.Net.Http;
using TestProject.Test;
using Web;

namespace TestProject.ControllerTest
{
    public class UserInfoControllerTest : IClassFixture<TestFixture<Startup>>
    {
        private UserInfoContext _dbContext;
        private UserInfoController _controller { get; }

        private HttpClient _client;

        public UserInfoControllerTest(TestFixture<Startup> testFixture)
        {
            _dbContext = testFixture.ServerService.GetService<UserInfoContext>();
            _controller = new UserInfoController(_dbContext);
            _client = testFixture.Client;
        }
        [Fact]
        public async Task Index()
        {
            var users = _dbContext.UserInfoDto.ToList();
            users.ShouldNotBeEmpty();
            var rst = await _controller.Index();
            rst.ShouldNotBeNull();
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Create()
        {
            var ViewResult = _controller.Create();
            ViewResult.ShouldNotBeNull();
            var response = await _client.GetAsync("/UserInfo/Create");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.ShouldNotBeNull();
        }
    }
}
