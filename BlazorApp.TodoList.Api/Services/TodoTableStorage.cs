using BlazorApp.TodoList.Api.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorApp.TodoList.Api.Services
{
    public class TodoTableStorage : ITodoDb
    {
        private readonly string _connectionString;

        private readonly string _tableName;

        private readonly ILogger<TodoTableStorage> _logger;

        public TodoTableStorage(string connectionString, string tableName, ILogger<TodoTableStorage> logger)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CloudTable> CreateTableAsync(string tableName, string connectionString)
        {
            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(connectionString);

            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            // Create a table client for interacting with the table service 
            CloudTable table = tableClient.GetTableReference(tableName);
            if (await table.CreateIfNotExistsAsync())
            {
                _logger.LogInformation("Created Table named: {tableName}", tableName);
            }
            else
            {
                _logger.LogInformation("Table {tableName} already exists", tableName);
            }

            return table;
        }

        public CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                throw;
            }

            return storageAccount;
        }


        public async Task<TodoItem> Add(TodoItem todoItem, CancellationToken cancellationToken)
        {
            if (todoItem == null)
                throw new ArgumentNullException(nameof(todoItem));

            try
            {
                TableOperation insert = TableOperation.InsertOrReplace(todoItem);

                var table = await CreateTableAsync(_tableName, _connectionString);

                var result = await table.ExecuteAsync(insert, cancellationToken);

                TodoItem itemInserted = result.Result as TodoItem;

                return itemInserted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when trying to insert item");
                throw;
            }
        }

        public async Task Delete(TodoItem todoItem, CancellationToken cancellationToken)
        {
            if (todoItem == null)
                throw new ArgumentNullException(nameof(todoItem));

            try
            {
                TableOperation insert = TableOperation.Delete(todoItem);

                var table = await CreateTableAsync(_tableName, _connectionString);

                _ = await table.ExecuteAsync(insert, cancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when trying to insert item");
                throw;
            }
        }

        public Task<TodoItem> FindById(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<TodoItem>> GetAll(CancellationToken cancellationToken)
        {
            var table = await CreateTableAsync(_tableName, _connectionString);

            var operation = await table.CreateQuery<TodoItem>().ExecuteSegmentedAsync(new TableContinuationToken(), cancellationToken);

            return operation.Results;
        }

        public Task<TodoItem> Update(TodoItem todoItem, CancellationToken cancellationToken)
        {
            return Add(todoItem, cancellationToken);
        }
    }
}
