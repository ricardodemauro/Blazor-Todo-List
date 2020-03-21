using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.TodoList.Api.Models
{
    public partial class TodoItem : TableEntity
    {
        public string Task { get; set; }

        public bool Done { get; set; }

        public TodoItem(Guid id, string task)
        {
            this.PartitionKey = "rick";
            this.RowKey = id.ToString();
            Task = task;
        }

        public TodoItem(string task)
            : this(Guid.NewGuid(), task)
        {
        }

        public TodoItem()
            : this(task: string.Empty)
        {

        }
    }
}
