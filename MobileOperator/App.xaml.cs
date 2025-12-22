using System.Windows;
using Microsoft.EntityFrameworkCore;
using MobileOperator.views;
using MobileOperator.Domain.Entities;

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

            var optionsBuilder = new DbContextOptionsBuilder<Infrastructure.MobileOperator>();
            optionsBuilder.UseNpgsql(connectionString);
            DbOptions = optionsBuilder.Options; 
            
            var context = new Infrastructure.MobileOperator(DbOptions);

            try
            {
                {
                    bool created = context.Database.EnsureCreated();

                    if (created)
                    {
                        SeedInitialData(context);
                        MessageBox.Show("База данных успешно создана и заполнена начальными данными!", "Инициализация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подключении или создании БД:\n{ex.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var dialog = new Login(context);
            dialog.ShowDialog();
        }

        /// <summary>
        /// Метод для инициализации базы данных несколькими начальными записями, чтобы просто можно было провереть функционал приложения
        /// Не думаю, что этот коммент нейронка писала, они почти не юзают xml комменты
        /// </summary>
        private void SeedInitialData(Infrastructure.MobileOperator context)
        {
            var cityType = new CallType { Name = "По городу" };
            var intercityType = new CallType { Name = "Междугородний" };
            var internationalType = new CallType { Name = "Международный" };
            
            context.CallType.AddRange(cityType, intercityType, internationalType);
            context.SaveChanges();
            
            var rate1 = new Rate
            {
                Name = "Безлимитище",
                CityCost = 1.5m,
                IntercityCost = 3.0m,
                InternationalCost = 15.0m,
                GB = 50,
                SMS = 500,
                Minutes = 1000,
                ConnectionCost = 300,
                GBCost = 5,
                SMSCost = 2,
                Cost = 550,
                Corporate = false
            };

            var rate2 = new Rate
            {
                Name = "Супер МТС",
                CityCost = 2.0m,
                IntercityCost = 4.0m,
                InternationalCost = 20.0m,
                GB = 30,
                SMS = 300,
                Minutes = 800,
                ConnectionCost = 0,
                GBCost = 7,
                SMSCost = 3,
                Cost = 450,
                Corporate = false
            };

            context.Rate.AddRange(rate1, rate2);
            context.SaveChanges();
            
            var service1 = new Service 
            { 
                Name = "Определитель номера", 
                Cost = 50, 
                ConnectionCost = 0, 
                Conditions = "Показывает номер входящего вызова" 
            };
            
            var service2 = new Service 
            { 
                Name = "Мобильный интернет 100 ГБ", 
                Cost = 500, 
                ConnectionCost = 100, 
                Conditions = "Дополнительные 100 ГБ интернета" 
            };

            context.Service.AddRange(service1, service2);
            context.SaveChanges();
            
            var adminUser = new User { Password = "admin123" };
            context.User.Add(adminUser);
            context.SaveChanges();

            context.Admin.Add(new Admin { UserId = adminUser.Id, Login = "admin_sidorov" });
            context.SaveChanges();
            
            var user1 = new User { Password = "pass123" };
            context.User.Add(user1);
            context.SaveChanges();

            var client1 = new Client
            {
                UserId = user1.Id,
                Number = "79161234567",
                Balance = 850.50m,
                RateId = rate1.Id,
                Minutes = 500,
                GB = 25,
                SMS = 200
            };
            context.Client.Add(client1);
            context.SaveChanges();

            context.FL.Add(new FL
            {
                UserId = client1.UserId,
                FIO = "Сидоров Сидор Сидорыч",
                PassportDetails = "4512345678"
            });
            
            var user2 = new User { Password = "qwerty456" };
            context.User.Add(user2);
            context.SaveChanges();

            var client2 = new Client
            {
                UserId = user2.Id,
                Number = "79257654321",
                Balance = 1200.00m,
                RateId = rate2.Id,
                Minutes = 300,
                GB = 15,
                SMS = 150
            };
            context.Client.Add(client2);
            context.SaveChanges();

            context.FL.Add(new FL
            {
                UserId = client2.UserId,
                FIO = "Петрова Мария Сергеевна",
                PassportDetails = "4598765432"
            });

            context.SaveChanges();
        }
    }
}