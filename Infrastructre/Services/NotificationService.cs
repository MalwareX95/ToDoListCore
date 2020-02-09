using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Infrastructre.Services
{
    public class NotificationService : INotificationService
    {
        public int MaxNotificationCount { get; } = 5;
        public TimeSpan NotificationPeriod { get; } = TimeSpan.FromMinutes(1);

        private Timer IncomingNotificationChecker;

        private Dictionary<ToDoItem, Timer> Timers;

        private readonly IApplicationDbContext dbContext;

        public NotificationService(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            Timers = new Dictionary<ToDoItem, Timer>(MaxNotificationCount);
        }

        private void Notify(object state)
        {
            var toDoItem = state as ToDoItem;
            Timer timer;
            if (!Timers.TryGetValue(toDoItem, out timer)) return;
            DateTime dateTime;
            
            var notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = $"Zadanie: {toDoItem.EventDay}",
                Message = toDoItem.Descrption,
                Type = NotificationType.Information
            }, expirationTime: TimeSpan.FromSeconds(10));
            if (toDoItem.EventDay <= DateTime.Now)
            {
                Timers.Remove(toDoItem);
            }
            else if ((dateTime = DateTime.Now.Add(NotificationPeriod)) > toDoItem.EventDay)
            {
                timer.Change(NotificationPeriod - (dateTime - toDoItem.EventDay), TimeSpan.FromMilliseconds(-1));
            }
        }

        private Timer CreateTimer(ToDoItem toDoItem) => new Timer(Notify, toDoItem, TimeSpan.Zero, NotificationPeriod);
        private void IncomingNotificationCheckerHandler(object state)
        {
            dbContext.SaveChanges();
            if (Timers.Count < MaxNotificationCount)
            {
                dbContext.ToDoItems
                     .OrderBy(x => x.EventDay)
                     .Where(x => x.EventDay > DateTime.Now)
                     .Take(MaxNotificationCount)
                     .ToListAsync()
                     .ContinueWith(task =>
                     {
                         task.Result
                             .Except(Timers.Keys)
                             .Select(toDoItem => (timer: CreateTimer(toDoItem), toDoItem))
                             .ToList()
                             .ForEach(tuple => Timers.Add(tuple.toDoItem, tuple.timer));
                     });
            }
        }

        public void Run()
            => IncomingNotificationChecker = new Timer(IncomingNotificationCheckerHandler, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

        public void CancelNotification(ToDoItem key)
        {
            if (Timers.TryGetValue(key, out var timer))
            {
                timer.Dispose();
                GC.SuppressFinalize(timer);
                Timers.Remove(key);
            }
        }
    }
}
