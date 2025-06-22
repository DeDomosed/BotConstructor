using System;
using System.Collections.Generic;
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

namespace BotConstructor
{
    /// <summary>
    /// Логика взаимодействия для CreateBotPage.xaml
    /// </summary>
    public partial class CreateBotPage : Page
    {
        public CreateBotPage()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentFrame.GoBack();
        }

        private void BotNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BotNameTextBox.Text == "Введите имя бота")
            {
                BotNameTextBox.Text = "";
                BotNameTextBox.Foreground = Brushes.Black;
            }
        }
        private void BotNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BotNameTextBox.Text))
            {
                BotNameTextBox.Text = "Введите имя бота";
                BotNameTextBox.Foreground = Brushes.Gray;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            App.CurrentFrame.Navigate(new FAQEditorPage());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Открыть редактор формы (в разработке)");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Открыть редактор базы клиентов (в разработке)");
        }
    }
}
