using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotConstructor
{
    public class BotConfig
    {
        public string bot_name { get; set; }
        public string token_env_var { get; set; }
        public long admin_chat_id { get; set; }
        public string start_message { get; set; }
        public FAQBlock faq { get; set; }
        public bool save_data { get; set; } = false; // Для базы клиентов
        public bool form_notification { get; set; } = false; // Для формы заказов
        public string bot_path { get; set; } = ""; // Путь к папке бота
    }
}
