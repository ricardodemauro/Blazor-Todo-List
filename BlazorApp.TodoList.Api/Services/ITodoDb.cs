using BlazorApp.TodoList.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorApp.TodoList.Api.Services
{
    public interface ITodoDb
    {
        Task<TodoItem> FindById(Guid id, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<TodoItem>> GetAll(CancellationToken cancellationToken);

        Task<TodoItem> Add(TodoItem todoItem, CancellationToken cancellationToken);

        Task<TodoItem> Update(TodoItem todoItem, CancellationToken cancellationToken);

        Task Delete(TodoItem todoItem, CancellationToken cancellationToken);
    }
}
