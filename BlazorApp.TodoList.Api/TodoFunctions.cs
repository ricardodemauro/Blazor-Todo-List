using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BlazorApp.TodoList.Api.Services;
using System.Threading;
using BlazorApp.TodoList.Api.Models;

namespace BlazorApp.TodoList.Api
{
    public class TodoFunctions
    {
        private readonly ILogger<TodoFunctions> _logger;

        private readonly ITodoDb _db;

        public TodoFunctions(ILogger<TodoFunctions> logger, ITodoDb db)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("FunctionGetAll")]
        public async Task<IActionResult> RunGetAll([HttpTrigger(AuthorizationLevel.Function, "get", Route = "api/todos")] HttpRequest req, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Triggered function {FunctionName} processed a request.", nameof(RunGetAll));

            var result = await _db.GetAll(cancellationToken);

            return new OkObjectResult(result);
        }

        [FunctionName("FunctionCreate")]
        public async Task<IActionResult> RunCreate([HttpTrigger(AuthorizationLevel.Function, "post", Route = "api/todos")] TodoItem req, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Triggered function {FunctionName} processed a request.", nameof(RunCreate));

            var result = await _db.Add(req, cancellationToken);

            return new OkObjectResult(result);
        }

        [FunctionName("FunctionUpdate")]
        public async Task<IActionResult> RunUpdate([HttpTrigger(AuthorizationLevel.Function, "put", Route = "api/todos")] TodoItem req, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Triggered function {FunctionName} processed a request.", nameof(RunUpdate));

            var result = await _db.Update(req, cancellationToken);

            return new OkObjectResult(result);
        }
    }
}
