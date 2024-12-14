using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;

namespace Telegram_Bot
{
    class Program
    {
       

        private static string Translate(string input)
        {
            string vowels = "aeiouаеёиоуыэюя"; // Гласные буквы
            string result = "";

            foreach (char c in input)
            {
                // Если это согласная буква
                if (char.IsLetter(c) && !vowels.Contains(char.ToLower(c)))
                {
                    result += $"{c}о{char.ToLower(c)}"; // Добавляем "кирпичный" слог
                }
                else
                {
                    result += c; // Оставляем остальные символы без изменений
                }
            }

            return result;
        }

        // Обработка ошибок
        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Произошла ошибка: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
