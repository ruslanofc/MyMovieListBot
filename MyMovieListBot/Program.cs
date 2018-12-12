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
using MyMovieListBot;
 

namespace MyMovieListBot
{
    public class Program
    {
        static ITelegramBotClient botClient;
        public static int Count = 0;
        public static MovieList movieList = new MovieList();
        public static MovieListService service = new MovieListService();

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
                            new KeyboardButton("Угадать фильм по картинке"),
                            new KeyboardButton("Записать фильм в список буду смотреть"),
                            new KeyboardButton("Посмотреть список буду смотреть"),
                        },
                        new KeyboardButton[]
                        {
                            new KeyboardButton("Поставить оценку просмотренному фильму"),
                            new KeyboardButton("Посмотреть мои просмотренные фильмы"),
                        }
                    }
                };
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Что вы хотите сделать?",
                    replyMarkup: rkm);
                return;
            }

            if (e.Message.Text == "Поставить оценку просмотренному фильму")
            {
                movieList = new MovieList();
                Count = 0;
                movieList.SenderId = e.Message.From.Id;
                Count++;

                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Название фильма?",
                    replyMarkup: new ReplyKeyboardRemove() { });
                return;
            }

            if(Count == 1)
            {
                movieList.Movie = e.Message.Text;
                Count++;

                var rkm = new ReplyKeyboardMarkup
                {
                    Keyboard = new KeyboardButton[][]
                    {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("1"),
                            new KeyboardButton("2"),
                            new KeyboardButton("3"),
                            new KeyboardButton("4"),
                            new KeyboardButton("5"),
                        },
                        new KeyboardButton[]
                        {
                            new KeyboardButton("6"),
                            new KeyboardButton("7"),
                            new KeyboardButton("8"),
                            new KeyboardButton("9"),
                            new KeyboardButton("10"),
                        }
                    }
                };

                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Какую оценку ты ему ставишь?",
                    replyMarkup: rkm);
                return;
            }

            if(Count == 2)
            {
                movieList.Rating = e.Message.Text;
                Count = 0;

                service.Save(movieList);
                await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: "Сохранён");

                var rkm = new ReplyKeyboardMarkup
                {
                    Keyboard = new KeyboardButton[][]
                    {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("Угадать фильм по картинке"),
                            new KeyboardButton("Записать фильм в список буду смотреть"),
                            new KeyboardButton("Посмотреть список буду смотреть"),
                        },
                        new KeyboardButton[]
                        {
                            new KeyboardButton("Поставить оценку просмотренному фильму"),
                            new KeyboardButton("Посмотреть мои просмотренные фильмы"),
                        }
                    }
                };

                await botClient.SendTextMessageAsync(
                   chatId: e.Message.Chat,
                   text: "Что вы хотите сделать?",
                   replyMarkup: rkm);
                return;
            }

            if(e.Message.Text == "Посмотреть мои просмотренные фильмы")
            {
                service.Open(movieList);
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