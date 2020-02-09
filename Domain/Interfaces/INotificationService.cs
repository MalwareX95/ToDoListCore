using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface INotificationService
    {
        void Run();
        void CancelNotification(ToDoItem key);
    }
}
