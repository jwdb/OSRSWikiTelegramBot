﻿using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using System.Reactive.Linq;
using Telegram.Bot.Args;
using System.Net.Http;
using Newtonsoft.Json;
using Telegram.Bot.Types.InlineQueryResults;
using System.Linq;
using Telegram.Bot.Types.Enums;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace TGBotOSRSWiki
{
    public class Bottu
    {
        private const string queryFormat = @"api.php?action=query&format=json&list=search&utf8=1&srsearch={0}";
        private const string urlsQueryFormat = @"api.php?action=query&format=json&prop=info&continue=&pageids={0}&inprop=url";
        private const string urlsQueryFormatFallbackTitle = @"api.php?action=query&format=json&prop=info&continue=&titles={0}&inprop=url";

        private readonly HttpClient MediaWikiApi;
        private readonly Telegram.Bot.TelegramBotClient client;

        public Bottu(string Token, string baseApiAddress)
        {
            AutoResetEvent are = new AutoResetEvent(false);
            client = new Telegram.Bot.TelegramBotClient(Token);
            MediaWikiApi = new HttpClient
            {
                BaseAddress = new Uri(baseApiAddress)
            };

            var InlineObservable = Observable.FromEventPattern<InlineQueryEventArgs>(c => client.OnInlineQuery += c, c => client.OnInlineQuery -= c);

            InlineObservable
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Where(c => c?.EventArgs?.InlineQuery != null && !string.IsNullOrWhiteSpace(c.EventArgs.InlineQuery.Query))
                .Select(c => c.EventArgs.InlineQuery)
                .Select(c => Observable.FromAsync(async () => await ProcessInline(c)))
                .Concat()
                .Subscribe();

            client.StartReceiving(new[] { UpdateType.InlineQuery });

            Console.WriteLine("Started.");

            are.WaitOne();

            client.StopReceiving();
            Console.WriteLine("Stopped.");
        }


        private async Task ProcessInline(InlineQuery query)
        {
            Console.WriteLine($"Got query: {query.Query}");
            List<InlineQueryResultArticle> returnResults = new List<InlineQueryResultArticle>();
            try
            {
                var MediaApiQuery = await MediaWikiApi.GetStringAsync(string.Format(queryFormat, query.Query));

                var result = JsonConvert.DeserializeObject<JsonResultFormat.Root>(MediaApiQuery);

                if (result.Query?.Search?.Any() == true)

                {
                    Dictionary<string, JsonResultFormat.Page> URLresult = null;
                    
                    try
                    {
                        var MediaApiQueryUrls = await MediaWikiApi.GetStringAsync(string.Format(urlsQueryFormat, string.Join('|', result.Query.Search.Select(c => c.Pageid).ToArray())));

                        URLresult = JsonConvert.DeserializeObject<JsonResultFormat.UrlRoot>(MediaApiQueryUrls).Query.Pages;

                    }
                    catch (Exception) // Fallback to titles
                    {
                        var MediaApiQueryUrls = await MediaWikiApi.GetStringAsync(string.Format(urlsQueryFormatFallbackTitle, string.Join('|', result.Query.Search.Select(c => c.Title).ToArray())));

                        URLresult = JsonConvert.DeserializeObject<JsonResultFormat.UrlRoot>(MediaApiQueryUrls).Query.Pages;

                        foreach (var item in result.Query.Search)
                        {
                            item.Pageid = URLresult.FirstOrDefault(c => c.Value.Title == item.Title).Value.Pageid;
                        }
                    }
                    returnResults = result
                        .Query
                        .Search
                        .Join(URLresult, c => c.Pageid, c => c.Value.Pageid, (c, d) => new { c, d })
                        .Take(10) // limit
                        .Select(c =>
                        new InlineQueryResultArticle(c.c.Pageid.ToString(), c.c.Title, new InputTextMessageContent(c.d.Value.Canonicalurl.ToString()))
                        {
                            Description = StripHTML(System.Net.WebUtility.HtmlDecode(c.c.Snippet))
                        }).ToList();
                }
                await client.AnswerInlineQueryAsync(query.Id, returnResults);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", " ");
        }
    }
}
