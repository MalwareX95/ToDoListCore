using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Domain.Tests
{
    public class MainWindowViewModelTests
    {

        [Fact]
        public void AddToDoItemCommand_WhenCall_CollectionShouldntBeEmpty()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "testingDb")
                    .Options;
            using (var db = new ApplicationDbContext(options))
            {
                var model = new MainWindowViewModel(db, new Mock<INotificationService>().Object);
                model.AddToDoItemCommand.Execute(null);
                db.SaveChanges();
            };

            Assert.Equal(expected: 1, actual: new ApplicationDbContext(options).ToDoItems.Count());
        }

        [Fact]
        public void RemoveToDoItemCommand_WhenCalled_ShouldInvokeCancelNotification()
        {
            var dbContextMock = new Mock<ApplicationDbContext>();
            var toDoItem = new ToDoItem {Id = 1};
            var toDoItems = new List<ToDoItem> {toDoItem};
            dbContextMock
                .Setup(x => x.ToDoItems)
                .ReturnsDbSet(toDoItems)
                .Verifiable();

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock
                .Setup(x => x.CancelNotification(toDoItem))
                .Verifiable();
            var model = new MainWindowViewModel(dbContextMock.Object, notificationServiceMock.Object);
            model.RemoveToDoItemCommand.Execute(new[] { toDoItem });
            notificationServiceMock.Verify(x => x.CancelNotification(toDoItem));

        }
    }
}
