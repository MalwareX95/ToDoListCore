using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public abstract class ApplicationDbContextBase : DbContext
    {
        public ApplicationDbContextBase(){}
        public virtual DbSet<ToDoItem> ToDoItems { get; set; }
        public ApplicationDbContextBase(DbContextOptions options) : base(options){}
    }
}
