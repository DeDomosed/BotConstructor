using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BotConstructor
{
    public partial class CreateBotPage : Page
    {
        private FAQBlock faqBlock = new FAQBlock();
        private bool _isTokenVisible = false;
        private const string BotNamePlaceholder = "Введите имя бота";
        private const string WelcomeMessagePlaceholder = "Введите приветствие бота";

        public CreateBotPage()
        {
            InitializeComponent();
            TokenPasswordBox.PasswordChanged += TokenPasswordBox_PasswordChanged;
        }

        private void TokenPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_isTokenVisible)
            {
                TokenTextBox.Text = TokenPasswordBox.Password;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentFrame.GoBack();
        }

        private void BotNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BotNameTextBox.Text == BotNamePlaceholder)
            {
                BotNameTextBox.Text = "";
                BotNameTextBox.Foreground = Brushes.Black;
            }
        }

        private void BotNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BotNameTextBox.Text))
            {
                BotNameTextBox.Text = BotNamePlaceholder;
                BotNameTextBox.Foreground = Brushes.Gray;
            }
        }

        private void WelcomeMessageTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (WelcomeMessageTextBox.Text == WelcomeMessagePlaceholder)
            {
                WelcomeMessageTextBox.Text = "";
                WelcomeMessageTextBox.Foreground = Brushes.Black;
            }
        }

        private void WelcomeMessageTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(WelcomeMessageTextBox.Text))
            {
                WelcomeMessageTextBox.Text = WelcomeMessagePlaceholder;
                WelcomeMessageTextBox.Foreground = Brushes.Gray;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var faqEditorPage = new FAQEditorPage(
                faqBlock,
                updatedFaq =>
                {
                    faqBlock = updatedFaq;
                    FAQCheckBox.IsChecked = true;
                });

            App.CurrentFrame.Navigate(faqEditorPage);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FormCheckBox.IsChecked = true;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ClientsCheckBox.IsChecked = true;
        }

        private void ToggleTokenVisibilityButton_Click(object sender, RoutedEventArgs e)
        {
            _isTokenVisible = !_isTokenVisible;

            if (_isTokenVisible)
            {
                TokenTextBox.Text = TokenPasswordBox.Password;
                TokenPasswordBox.Visibility = Visibility.Collapsed;
                TokenTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                TokenPasswordBox.Password = TokenTextBox.Text;
                TokenTextBox.Visibility = Visibility.Collapsed;
                TokenPasswordBox.Visibility = Visibility.Visible;
            }
        }

        private void SaveBotConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var botName = BotNameTextBox.Text.Trim();
                var token = _isTokenVisible ? TokenTextBox.Text.Trim() : TokenPasswordBox.Password.Trim();
                var welcomeMessage = WelcomeMessageTextBox.Text.Trim();
                var adminChatId = long.TryParse(AdminChatIdTextBox.Text.Trim(), out var id) ? id : 0;
                var botPath = BotPathTextBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(botName))
                {
                    MessageBox.Show("Введите имя бота");
                    return;
                }

                if (string.IsNullOrWhiteSpace(token))
                {
                    MessageBox.Show("Введите токен бота");
                    return;
                }

                if (adminChatId == 0)
                {
                    MessageBox.Show("Введите корректный Chat ID администратора");
                    return;
                }

                if (string.IsNullOrWhiteSpace(botPath))
                {
                    MessageBox.Show("Выберите путь для сохранения бота");
                    return;
                }

                var config = new BotConfig
                {
                    bot_name = botName,
                    token_env_var = token,
                    admin_chat_id = adminChatId,
                    start_message = welcomeMessage,
                    faq = FAQCheckBox.IsChecked == true ? faqBlock : null,
                    save_data = ClientsCheckBox.IsChecked == true,
                    form_notification = FormCheckBox.IsChecked == true,
                    bot_path = botPath
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                Directory.CreateDirectory(botPath);

                string fileName = Path.Combine(botPath, $"{botName}_config.json");
                string json = JsonSerializer.Serialize(config, options);
                File.WriteAllText(fileName, json, Encoding.UTF8);

                MessageBox.Show($"Бот '{botName}' успешно сохранен по пути:\n{fileName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void SelectPathButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Выберите папку для сохранения бота",
                Filter = "Папки|*.thisisnotafile",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "selectthisfolder"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                BotPathTextBox.Text = selectedPath;
            }
        }
    }
}