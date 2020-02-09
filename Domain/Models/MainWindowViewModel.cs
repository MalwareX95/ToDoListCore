using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace Domain.Models
{
    public class MainWindowViewModel : IModel, INotifyPropertyChanged
    {
        public MainWindowViewModel(IApplicationDbContext dbContext, INotificationService notificationService)
        {

            ToDoItems = new ObservableCollection<ToDoItem>();
            AddToDoItemCommand = new RelayCommand(AddToDoItemCommand_Execute);
            RemoveToDoItemCommand = new RelayCommand(RemoveToDoItemCommand_CanExecute, RemoveToDoItemCommand_Execute);
            DbContext = dbContext;
            NotificationService = notificationService;
        }

        public DateTime SelectedDate { get; set; } = DateTime.Today;

        private ObservableCollection<ToDoItem> _ToDoItems;
        public ObservableCollection<ToDoItem> ToDoItems
        {
            get => _ToDoItems;
            set
            {
                _ToDoItems = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string property = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public ICommand AddToDoItemCommand { get; protected set; }
        public ICommand RemoveToDoItemCommand { get; protected set; }
        public IApplicationDbContext DbContext { get; }
        public INotificationService NotificationService { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void AddToDoItemCommand_Execute(object parameter)
        {
            var todoItem = new ToDoItem { EventDay = SelectedDate };
            ToDoItems.Add(todoItem);
            DbContext.ToDoItems.Add(todoItem);
        }

        private bool RemoveToDoItemCommand_CanExecute(object parameter)
        {
            var collection = parameter as IEnumerable ?? Enumerable.Empty<ToDoItem>();
            return collection.Cast<ToDoItem>().Any();
        }

        private void RemoveToDoItemCommand_Execute(object parameter)
        {
            var collection = parameter as IEnumerable ?? Enumerable.Empty<ToDoItem>();
            collection
                .Cast<ToDoItem>()
                .ToList()
                .ForEach(x =>
                {
                    ToDoItems.Remove(x);
                    if (x.Id != default) DbContext.ToDoItems.Remove(x);
                    NotificationService.CancelNotification(x);
                });
        }
    }
}
