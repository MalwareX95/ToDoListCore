using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
        }

        public MainWindowViewModel Model => DataContext as MainWindowViewModel;

        private async void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var date = e.AddedItems[0] as DateTime?;
            await Model.DbContext.SaveChangesAsync();
            Model.ToDoItems = new ObservableCollection<ToDoItem>(await Model.DbContext.ToDoItems.Where(x => x.EventDay.Date == date).ToListAsync());
            Model.NotificationService.Run();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Model.DbContext.SaveChangesAsync();
            App.Current.Windows
                       .Cast<Window>()
                       .Except(new[] { App.Current.MainWindow })
                       .ToList()
                       .ForEach(x => x.Close());
        }
    }
}
