using System.Windows;

namespace BotConstructor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AddBotButton.Click += AddBotButton_Click;

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
            var createWindow = new CreateBot();
            createWindow.Show();
            this.Close();
        }
    }

    public class Bot
    {
        public string Name { get; set; }
    }
}