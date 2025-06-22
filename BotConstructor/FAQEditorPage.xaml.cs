using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BotConstructor
{
    public partial class FAQEditorPage : Page
    {
        private int questionCounter = 1;
        private FAQBlock faqBlock;
        private Action<FAQBlock> onSaveCallback;

        public FAQEditorPage(FAQBlock existingFaq, Action<FAQBlock> onSave)
        {
            InitializeComponent();
            faqBlock = existingFaq;
            onSaveCallback = onSave;

            foreach (var item in faqBlock.Items)
            {
                AddQuestionBlock(item.Question, item.Answer);
            }
        }

        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            AddQuestionBlock();
        }

        private void AddQuestionBlock(string question = "", string answer = "")
        {
            var expander = new Expander
            {
                Margin = new Thickness(0, 0, 0, 10),
                IsExpanded = false
            };

            var headerPanel = new DockPanel();
            var headerText = new TextBlock
            {
                Text = $"Вопрос {questionCounter++}",
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

            var grid = new Grid { Margin = new Thickness(10) };
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            // Поле вопроса
            var questionBox = new TextBox
            {
                Background = Brushes.Transparent,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            if (string.IsNullOrEmpty(question))
            {
                questionBox.Text = "Вопрос";
                questionBox.Foreground = Brushes.Gray;
            }
            else
            {
                questionBox.Text = question;
                questionBox.Foreground = Brushes.Black;
            }

            questionBox.GotFocus += (s, e) =>
            {
                if (questionBox.Text == "Вопрос")
                {
                    questionBox.Text = "";
                    questionBox.Foreground = Brushes.Black;
                }
            };

            questionBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(questionBox.Text))
                {
                    questionBox.Text = "Вопрос";
                    questionBox.Foreground = Brushes.Gray;
                }
            };

            Grid.SetRow(questionBox, 0);
            grid.Children.Add(questionBox);

            // Поле ответа
            var answerBox = new TextBox
            {
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                VerticalContentAlignment = VerticalAlignment.Top
            };

            if (string.IsNullOrEmpty(answer))
            {
                answerBox.Text = "Ответ";
                answerBox.Foreground = Brushes.Gray;
            }
            else
            {
                answerBox.Text = answer;
                answerBox.Foreground = Brushes.Black;
            }

            answerBox.GotFocus += (s, e) =>
            {
                if (answerBox.Text == "Ответ")
                {
                    answerBox.Text = "";
                    answerBox.Foreground = Brushes.Black;
                }
            };

            answerBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(answerBox.Text))
                {
                    answerBox.Text = "Ответ";
                    answerBox.Foreground = Brushes.Gray;
                }
            };

            Grid.SetRow(answerBox, 1);
            grid.Children.Add(answerBox);

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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var newItems = new List<FAQItem>();

            foreach (Expander expander in QuestionListPanel.Children)
            {
                if (expander.Content is Grid grid && grid.Children.Count == 2)
                {
                    var questionBox = grid.Children[0] as TextBox;
                    var answerBox = grid.Children[1] as TextBox;

                    if (questionBox != null && answerBox != null)
                    {
                        string questionText = questionBox.Text.Trim();
                        string answerText = answerBox.Text.Trim();

                        if (questionText != "Вопрос" && answerText != "Ответ" &&
                            !string.IsNullOrEmpty(questionText) && !string.IsNullOrEmpty(answerText))
                        {
                            newItems.Add(new FAQItem
                            {
                                Question = questionText,
                                Answer = answerText
                            });
                        }
                    }
                }
            }

            if (newItems.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы один вопрос и ответ.");
                return;
            }

            faqBlock.Items = newItems;
            onSaveCallback?.Invoke(faqBlock);
            App.CurrentFrame.GoBack();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentFrame.GoBack();
        }
    }
}