using System.Windows;
using System.Windows.Controls;

namespace BotConstructor
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();

            // Тестовый список ботов
            var bots = new List<Bot>
            {
                new Bot { Name = "Бот заказов" },
                new Bot { Name = "FAQ-бот" }
            };

            BotList.ItemsSource = bots;
        }

        private void AddBotButton_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentFrame.Navigate(new CreateBotPage());
        }
    }
}
