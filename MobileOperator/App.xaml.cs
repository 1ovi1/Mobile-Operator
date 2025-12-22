using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using MobileOperator.views;

namespace MobileOperator
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static DbContextOptions<Infrastructure.MobileOperator> DbOptions { get; private set; }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            string connectionString = "Host=localhost;Port=5432;Database=mobile-operator-final;Username=postgres;Password=";
            
            var optionsBuilder = new DbContextOptionsBuilder<MobileOperator.Infrastructure.MobileOperator>();
            
            optionsBuilder.UseNpgsql(connectionString); 
            
            DbOptions = optionsBuilder.Options;
            
            // try
            // {
            //     using (var context = new MobileOperator.Infrastructure.MobileOperator(optionsBuilder.Options))
            //     {
            //         if (context.Database.CanConnect())
            //         {
            //             MessageBox.Show("Успешное подключение к БД!", "Проверка", MessageBoxButton.OK, MessageBoxImage.Information);
            //         }
            //         else
            //         {
            //             MessageBox.Show("Не удалось подключиться к БД.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //         }
            //     }
            // }
            // catch (System.Exception ex)
            // {
            //     MessageBox.Show($"Ошибка подключения:\n{ex.Message}", "Исключение", MessageBoxButton.OK, MessageBoxImage.Error);
            // }
            
            var _context = new Infrastructure.MobileOperator(App.DbOptions);
            
            var dialog = new Login(_context);
            dialog.ShowDialog();
        }

    }
}