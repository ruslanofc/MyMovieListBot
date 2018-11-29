using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using com.LandonKey.SocksWebProxy;
using com.LandonKey.SocksWebProxy.Proxy;
using System.Net;
using System.Net.Sockets;
using Npgsql;
using telegramBot;

namespace telegramBot
{
    public class Program
    {
        static ITelegramBotClient botClient;
        public static int Count = 0;

        static void Main()
        {
            botClient = new TelegramBotClient(
                "728991596:AAGn5lcWrOVxVWDAVHy97ca_BhqXpocYRHQ",
                new SocksWebProxy(
                        new ProxyConfig(
                            IPAddress.Parse("127.0.0.1"),
                            GetNextFreePort(),
                            IPAddress.Parse("185.20.184.217"),
                            3693,
                            ProxyConfig.SocksVersion.Five,
                            "userid66n9",
                            "pSnEA7M"),
                        false)
            );

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
                $"Привет, юзер! Я {me.Id} и меня зовут {me.FirstName}. Напиши мне что-нибудь:)"
            );
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == "/start")
            {
                var rkm = new ReplyKeyboardMarkup
                {
                    Keyboard = new KeyboardButton[][]
                    {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("Зарегистрироваться"),
                            new KeyboardButton("Войти"),
                        }
                    }
                };
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Что вы хотите сделать?",
                    replyMarkup: rkm);
                return;
            }
        }

        private static int GetNextFreePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();

            return port;
        }
    }
}