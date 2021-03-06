﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.TodoList.Models
{
    public partial class TodoItem
    {
        public Guid Id { get; set; }

        public string Task { get; set; }

        public bool Done { get; set; }

        public TodoItem(Guid id, string task)
        {
            Id = id;
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
