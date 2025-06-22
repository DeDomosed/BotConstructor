using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace BotConstructor
{
    /// <summary>
    /// Логика взаимодействия для FAQEditorPage.xaml
    /// </summary>
    public partial class FAQEditorPage : Page
    {

        private int questionCounter = 1;

        public FAQEditorPage()
        {
            InitializeComponent();
        }

        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            AddQuestionBlock($"Вопрос {questionCounter++}");
        }

        private void AddQuestionBlock(string title = null)
        {
            var expander = new Expander
            {
                Margin = new Thickness(0, 0, 0, 10),
                IsExpanded = false
            };

            var headerPanel = new DockPanel();

            var headerText = new TextBlock
            {
                Text = title ?? $"Вопрос {questionCounter++}",
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.Bold
            };
            DockPanel.SetDock(headerText, Dock.Left);

            var deleteButton = new Button
            {
                Content = "🗑",
                Width = 25,
                Height = 25,
                Background = Brushes.Transparent,
                BorderBrush = null,
                Padding = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            DockPanel.SetDock(deleteButton, Dock.Right);

            deleteButton.Click += (s, e) =>
            {
                QuestionListPanel.Children.Remove(expander);
                RenumberQuestions();
            };

            headerPanel.Children.Add(headerText);
            headerPanel.Children.Add(deleteButton);
            expander.Header = headerPanel;

            var grid = new Grid
            {
                Margin = new Thickness(10)
            };
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            var questionGrid = new Grid
            {
                Height = 30,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var questionBox = new TextBox
            {
                Background = Brushes.Transparent,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            var questionPlaceholder = new TextBlock
            {
                Text = "Вопрос:",
                Margin = new Thickness(5, 0, 0, 0),
                Foreground = Brushes.Gray,
                VerticalAlignment = VerticalAlignment.Center,
                IsHitTestVisible = false
            };

            questionBox.TextChanged += (s, e) =>
            {
                questionPlaceholder.Visibility = string.IsNullOrEmpty(questionBox.Text)
                    ? Visibility.Visible
                    : Visibility.Hidden;
            };

            questionGrid.Children.Add(questionPlaceholder);
            questionGrid.Children.Add(questionBox);
            Grid.SetRow(questionGrid, 0);

            var answerGrid = new Grid
            {
                Height = 40,
                Margin = new Thickness(0)
            };

            var answerPlaceholder = new TextBlock
            {
                Text = "Ответ:",
                Margin = new Thickness(5, 5, 0, 0),
                Foreground = Brushes.Gray,
                VerticalAlignment = VerticalAlignment.Top,
                IsHitTestVisible = false
            };
            Panel.SetZIndex(answerPlaceholder, 1);

            var answerBox = new TextBox
            {
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                VerticalContentAlignment = VerticalAlignment.Top
            };
            Panel.SetZIndex(answerBox, 0);

            answerBox.TextChanged += (s, e) =>
            {
                answerPlaceholder.Visibility = string.IsNullOrWhiteSpace(answerBox.Text)
                    ? Visibility.Visible
                    : Visibility.Hidden;
            };

            answerGrid.Children.Add(answerBox);
            answerGrid.Children.Add(answerPlaceholder);

            Grid.SetRow(answerGrid, 1);

            grid.Children.Add(questionGrid);
            grid.Children.Add(answerGrid);

            expander.Content = grid;
            QuestionListPanel.Children.Add(expander);
        }

        private void RenumberQuestions()
        {
            int index = 1;
            foreach (var child in QuestionListPanel.Children)
            {
                if (child is Expander expander &&
                    expander.Header is DockPanel header &&
                    header.Children[0] is TextBlock headerText)
                {
                    headerText.Text = $"Вопрос {index++}";
                }
            }

            questionCounter = index;
        }

        public class FAQItem
        {
            public string Question { get; set; }
            public string Answer { get; set; }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var faqItems = new List<FAQItem>();

            foreach (Expander expander in QuestionListPanel.Children)
            {
                if (expander.Content is Grid grid)
                {
                    var questionGrid = grid.Children.OfType<Grid>().FirstOrDefault();
                    var answerGrid = grid.Children.OfType<Grid>().Skip(1).FirstOrDefault();

                    if (questionGrid != null && answerGrid != null)
                    {
                        var questionBox = questionGrid.Children.OfType<TextBox>().FirstOrDefault();
                        var answerBox = answerGrid.Children.OfType<TextBox>().FirstOrDefault();

                        if (questionBox != null && answerBox != null)
                        {
                            string questionText = questionBox.Text.Trim();
                            string answerText = answerBox.Text.Trim();

                            if (!string.IsNullOrEmpty(questionText) && !string.IsNullOrEmpty(answerText))
                            {
                                faqItems.Add(new FAQItem
                                {
                                    Question = questionText,
                                    Answer = answerText
                                });
                            }
                        }
                    }
                }
            }

            if (faqItems.Count == 0)
            {
                MessageBox.Show("Пожалуйста, добавьте хотя бы один вопрос и ответ.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
            };

            string json = JsonSerializer.Serialize(faqItems, options);

            string filePath = "faq.json";
            File.WriteAllText(filePath, json);

            MessageBox.Show("FAQ успешно сохранён.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentFrame.GoBack();
        }
    }
}
