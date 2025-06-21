using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            MessageBox.Show("Окно создания бота появится здесь");
        }
    }

    public class Bot
    {
        public string Name { get; set; }
    }
}