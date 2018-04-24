using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Web.Models;

namespace TestProject.Test
{
    public class TestFixture<TStartup> : IDisposable
    {
        private readonly TestServer _server;

        public IServiceProvider ServerService { get; }
        public HttpClient Client { get; }

        public TestFixture()
            : this(Path.Combine(""))
        {
        }

        protected TestFixture(string relativeTargetProjectParentDir)
        {
            var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(relativeTargetProjectParentDir, startupAssembly);

            var builder = new WebHostBuilder()
                .UseContentRoot(contentRoot)
                .ConfigureServices(services =>
                {
                    services.AddDbContext<UserInfoContext>(option => { option.UseInMemoryDatabase("UserInfoContext"); });
                })
                .UseEnvironment("Development")
                .UseStartup(typeof(TStartup));

            _server = new TestServer(builder);
            ServerService = _server.Host.Services;
            Client = _server.CreateClient();
            SeedData(ServerService.GetService<UserInfoContext>());
        }

        private void SeedData(UserInfoContext dbContext)
        {
            dbContext.AddRange(new List<UserInfoDto>() {
                new UserInfoDto(){Name="1",CreateTime=DateTime.Now},
                new UserInfoDto(){ Name="2",CreateTime=DateTime.Now},
                new UserInfoDto(){ Name="3",CreateTime=DateTime.Now}
            });

            dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }

        /// <summary>
        /// Gets the full path to the target project that we wish to test
        /// </summary>
        /// <param name="projectRelativePath">
        /// The parent directory of the target project.
        /// e.g. src, samples, test, or test/Websites
        /// </param>
        /// <param name="startupAssembly">The target project's assembly.</param>
        /// <returns>The full path to the target project.</returns>
        private static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = System.AppContext.BaseDirectory;

            // Find the path to the target project
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));
                if (projectDirectoryInfo.Exists)
                {
                    var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj"));
                    if (projectFileInfo.Exists)
                    {
                        return Path.Combine(projectDirectoryInfo.FullName, projectName);
                    }
                }
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }
    }
}
