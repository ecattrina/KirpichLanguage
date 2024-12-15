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
        private static readonly string BotToken = "7558519571:AAEyZwai6i8vyGnos6KfJzKrnuul42vNLTs";

        static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient(BotToken);

            using var cts = new CancellationTokenSource();

            // Запускаем получение обновлений от Telegram
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                cancellationToken: cts.Token
            );

            Console.ReadLine();
            cts.Cancel();
        }

        // Обработка входящих сообщений
        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Проверяем, что сообщение не пустое
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message && update.Message?.Text != null)
            {
                var chatId = update.Message.Chat.Id;
                var userMessage = update.Message.Text;

                // Если пользователь отправил команду /start
                if (userMessage == "/start")
                {
                    await botClient.SendTextMessageAsync(chatId, "Привет! Помнишь в детстве играли в Кирпичный язык. Введи фразу, и я переведу её на тот язык.", cancellationToken: cancellationToken);
                }
                else
                {
                    // Переводим текст на кирпичный язык
                    string translatedMessage = Translate(userMessage);

                    // Отправляем обратно результат перевода
                    await botClient.SendTextMessageAsync(chatId, $"🔹 Перевод: {translatedMessage}", cancellationToken: cancellationToken);
                }
            }
        }

        private static string Translate(string input)
        {
            string vowels = "aeiouаеёиоуыэюя"; // Гласные буквы
            string result = "";

            foreach (char c in input)
            {
                // Если это согласная буква
                if (char.IsLetter(c) && !vowels.Contains(char.ToLower(c)))
                {
                    result += $"{c}о{char.ToLower(c)}"; // Добавляем слог
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
