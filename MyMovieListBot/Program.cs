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
using System.Diagnostics;
using System.Text;

namespace MyMovieListBot
{
    public class Program
    {
        static ITelegramBotClient botClient;
        public static int Count = 0;
        public static int t = 0;
        public static MovieList movieList = new MovieList();
        public static MovieListService service = new MovieListService();
        public static UnwatchedList unwatchedList = new UnwatchedList();
        public static UnwatchedListService unwatchedListService = new UnwatchedListService(); 


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
            Thread.Sleep(int.MaxValue);;
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
                            new KeyboardButton("Найти фильм"),
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

            if(e.Message.Text == "Записать фильм в список буду смотреть")
            {
                unwatchedList = new UnwatchedList();
                Count = 0;
                unwatchedList.SenderId = e.Message.From.Id;
                Count = Count + 10;

                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Название фильма?",
                    replyMarkup: new ReplyKeyboardRemove() { });
                return;
            }

            if(Count == 10)
            {
                unwatchedList.Movie = e.Message.Text;
                Count = 0;

                unwatchedListService.Save(unwatchedList);
                await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: "Сохранён"); 
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
                            new KeyboardButton("Найти фильм"),
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

            if (e.Message.Text == "Посмотреть мои просмотренные фильмы")
            {
                ListService listService = new ListService();
                await botClient.SendTextMessageAsync(
                         chatId: e.Message.Chat,
                         text: listService.WatchedMovieListService(service.OpenList(e.Message.From.Id)));

                Thread.Sleep(500);

                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Ваш список фильмов с оценками");

                Thread.Sleep(500);

                var rkm = new ReplyKeyboardMarkup
                {
                    Keyboard = new KeyboardButton[][]
                    {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("Найти фильм"),
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

                //Thread.Sleep(500);

                //await botClient.SendTextMessageAsync(
                //    chatId: e.Message.Chat,
                //    text: "Вы можете посмотеть фильмы с определенными оценками либо выйти в главное меню");

                //Thread.Sleep(500);

                //var rkm = new ReplyKeyboardMarkup
                //{
                //    Keyboard = new KeyboardButton[][]
                //    {
                //        new KeyboardButton[]
                //        {
                //            new KeyboardButton("Посмотреть список фильмов с определенными оценками"),
                //            new KeyboardButton("Выйти в главное меню"),
                //        }
                //    }
                //};

                //await botClient.SendTextMessageAsync(
                //   chatId: e.Message.Chat,
                //   text: "Так что же вы хотите сделать?",
                //   replyMarkup: rkm);
                //return;
            }

            //if (e.Message.Text == "Посмотреть список фильмов с определенными оценками")
            //{
            //    var t = 0;
            //    var rkm = new ReplyKeyboardMarkup
            //    {
            //        Keyboard = new KeyboardButton[][]
            //        {
            //            new KeyboardButton[]
            //            {
            //                new KeyboardButton("1"),
            //                new KeyboardButton("2"),
            //                new KeyboardButton("3"),
            //                new KeyboardButton("4"),
            //                new KeyboardButton("5"),
            //            },
            //            new KeyboardButton[]
            //            {
            //                new KeyboardButton("6"),
            //                new KeyboardButton("7"),
            //                new KeyboardButton("8"),
            //                new KeyboardButton("9"),
            //                new KeyboardButton("10"),
            //            }
            //        }
            //    };

            //    t++;

            //    await botClient.SendTextMessageAsync(
            //        chatId: e.Message.Chat,
            //        text: "С какой оценкой список фильмов тебе нужен?",
            //        replyMarkup: rkm);
            //    return;   
            //}

            //if (t == 1)
            //{
            //    ListService listService = new ListService();
            //    await botClient.SendTextMessageAsync(
            //             chatId: e.Message.Chat,
            //             text: listService.WatchedMovieListService(service.ListWithRating(e.Message.From.Id, e.Message.Text)));
            //}

            if (e.Message.Text == "Посмотреть список буду смотреть")
            {
                var t = 0;
                //unwatchedList = new UnwatchedList();
                ListService listService = new ListService();
                await botClient.SendTextMessageAsync(
                         chatId: e.Message.Chat,
                         text: listService.UnwatchedMovieListService(unwatchedListService.OpenUnwatchedList(e.Message.From.Id)));
            
                Thread.Sleep(500);

                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Ваш список буду смотреть");

                

                var rkm = new ReplyKeyboardMarkup
                {
                    Keyboard = new KeyboardButton[][]
                    {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("Я посмотрел фильм и хочу поставить ему оценку"),
                            new KeyboardButton("Выйти в главное меню"),
                        }
                    }
                };
                

                await botClient.SendTextMessageAsync(
                   chatId: e.Message.Chat,
                   text: "Что вы хотите сделать?",
                   replyMarkup: rkm);
                return;
            }
            //ТУТ СДЕЛАТЬ ФАКТ ЧЕКИНГ И ПОФИКСИТЬ 
            if (e.Message.Text == "Я посмотрел фильм и хочу поставить ему оценку")
            {
               t++;
                await botClient.SendTextMessageAsync(
                   chatId: e.Message.Chat,
                   text: "Какой фильм вы посмотрели?");
                return;
            }

            if (t == 1)
            {
                movieList = new MovieList();
                service = new MovieListService();

                unwatchedList = new UnwatchedList();
                unwatchedListService = new UnwatchedListService();

                // if()

                unwatchedListService.Delete(e.Message.Text);

                movieList.SenderId = e.Message.From.Id;
                movieList.Movie = e.Message.Text;

                t++;

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

            if(t==2)
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
                            new KeyboardButton("Найти фильм"),
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

            if (e.Message.Text == "Выйти в главное меню")
            {
                var rkm = new ReplyKeyboardMarkup
                {
                    Keyboard = new KeyboardButton[][]
                   {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("Найти фильм"),
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