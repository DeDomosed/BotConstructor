namespace BotConstructor
{
    public class BotConfig
    {
        public string BotName { get; set; }
        public string TokenEnvVar { get; set; }
        public long AdminChatId { get; set; }
        public string StartMessage { get; set; }

        public FaqBlock FAQ { get; set; } = new FaqBlock();

        public class FaqBlock
        {
            public string IntroMessage { get; set; } = "Выберите интересующий вопрос из списка:";
            public string NoAnswerQuestion { get; set; } = "Не нашёл ответ на вопрос";
            public string UserQuestionPrompt { get; set; } = "Пожалуйста, введите свой вопрос.";
            public string UserQuestionFinal { get; set; } = "Ваш вопрос был направлен администратору. Среднее время ожидания — 15 минут.";
            public List<FAQItem> Items { get; set; } = new List<FAQItem>();
        }

        public class FAQItem
        {
            public string Question { get; set; }
            public string Answer { get; set; }
        }
    }
}
