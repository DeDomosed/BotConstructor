using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotConstructor
{
    public class FAQBlock
    {
        public string IntroMessage { get; set; } = "Выберите интересующий вопрос из списка:";
        public string NoAnswerQuestion { get; set; } = "Не нашёл ответ на вопрос";
        public string UserQuestionPrompt { get; set; } = "Пожалуйста, введите свой вопрос.";
        public string UserQuestionFinal { get; set; } = "Ваш вопрос был направлен администратору. Среднее время ожидания — 15 минут.";

        public List<FAQItem> Items { get; set; } = new List<FAQItem>();
    }
}
