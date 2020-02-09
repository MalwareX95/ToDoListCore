using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Descrption { get; set; }
        public DateTime EventDay { get; set; }
    }
}
