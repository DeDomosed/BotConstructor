using System;
using System.IO;
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
            MessageBox.Show("Открыть редактор формы (в разработке)");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Открыть редактор базы клиентов (в разработке)");
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
                var welcomeMessage = WelcomeMessageTextBox.Text.Trim();
                var token = _isTokenVisible ? TokenTextBox.Text.Trim() : TokenPasswordBox.Password.Trim();

                // Проверка на placeholder'ы
                if (botName == BotNamePlaceholder || string.IsNullOrWhiteSpace(botName))
                {
                    MessageBox.Show("Пожалуйста, введите имя бота.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (welcomeMessage == WelcomeMessagePlaceholder || string.IsNullOrWhiteSpace(welcomeMessage))
                {
                    MessageBox.Show("Пожалуйста, введите приветственное сообщение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!long.TryParse(AdminChatIdTextBox.Text.Trim(), out var adminChatId) || adminChatId == 0)
                {
                    MessageBox.Show("Пожалуйста, введите корректный Chat ID администратора.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(token))
                {
                    MessageBox.Show("Пожалуйста, введите токен бота.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var config = new BotConfig
                {
                    bot_name = botName,
                    token_env_var = token,
                    admin_chat_id = adminChatId,
                    start_message = welcomeMessage,
                    faq = FAQCheckBox.IsChecked == true ? faqBlock : null
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                };

                string json = JsonSerializer.Serialize(config, options);
                string fileName = $"{config.bot_name}_config.json";

                File.WriteAllText(fileName, json);

                MessageBox.Show($"Конфигурация бота '{config.bot_name}' успешно сохранена в файл {fileName}!",
                    "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении конфигурации: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}