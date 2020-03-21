using BlazorApp.TodoList.Api.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(BlazorApp.TodoList.Api.Startup))]

namespace BlazorApp.TodoList.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<ITodoDb>(x =>
            {
                var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                var tableName = Environment.GetEnvironmentVariable("TableName");

                return new TodoTableStorage(connectionString, tableName, x.GetRequiredService<ILogger<TodoTableStorage>>());
            });
        }
    }
}
