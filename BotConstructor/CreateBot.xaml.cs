using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BotConstructor
{
    /// <summary>
    /// Логика взаимодействия для CreateBot.xaml
    /// </summary>
    public partial class CreateBot : Window
    {
        public CreateBot()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
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

        private void FunctionItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item)
            {
                var checkBox = FindVisualChild<CheckBox>(item);
                if (checkBox == null) return;

                if (checkBox.IsChecked == true)
                {
                    if (checkBox.Name == "FAQCheckBox")
                    {
                        var faqEditor = new FAQEditor();
                        faqEditor.Show();
                        this.Close();
                    }

                    else if (checkBox.Name == "FormCheckBox")
                        MessageBox.Show("Открыть редактор формы (в разработке)");

                    else if (checkBox.Name == "ClientsCheckBox")
                        MessageBox.Show("Открыть редактор базы клиентов (в разработке)");
                }
                else
                {

                    checkBox.IsChecked = true;
                }
            }
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T correctlyTyped)
                    return correctlyTyped;

                var result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == FAQCheckBox) FAQText.Foreground = Brushes.Black;
            if (sender == FormCheckBox) FormText.Foreground = Brushes.Black;
            if (sender == ClientsCheckBox) ClientsText.Foreground = Brushes.Black;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender == FAQCheckBox) FAQText.Foreground = Brushes.Gray;
            if (sender == FormCheckBox) FormText.Foreground = Brushes.Gray;
            if (sender == ClientsCheckBox) ClientsText.Foreground = Brushes.Gray;
        }
    }
}
