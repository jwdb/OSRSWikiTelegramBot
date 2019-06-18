using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TGBotOSRSWiki
{
    public class JsonResultFormat
    {
        public partial class Root
        {
            [JsonProperty("batchcomplete")]
            public string Batchcomplete { get; set; }

            [JsonProperty("continue")]
            public Continue Continue { get; set; }

            [JsonProperty("query")]
            public Query Query { get; set; }
        }

        public partial class Continue
        {
            [JsonProperty("sroffset")]
            public long Sroffset { get; set; }

            [JsonProperty("continue")]
            public string ContinueContinue { get; set; }
        }

        public partial class Query
        {
            [JsonProperty("searchinfo")]
            public Searchinfo Searchinfo { get; set; }

            [JsonProperty("search")]
            public Search[] Search { get; set; }
        }

        public partial class Search
        {
            [JsonProperty("ns")]
            public long Ns { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("pageid")]
            public long Pageid { get; set; }

            [JsonProperty("size")]
            public long Size { get; set; }

            [JsonProperty("wordcount")]
            public long Wordcount { get; set; }

            [JsonProperty("snippet")]
            public string Snippet { get; set; }

            [JsonProperty("timestamp")]
            public DateTimeOffset Timestamp { get; set; }
        }

        public partial class Searchinfo
        {
            [JsonProperty("totalhits")]
            public long Totalhits { get; set; }
        }

        public partial class UrlRoot
        {
            [JsonProperty("batchcomplete")]
            public string Batchcomplete { get; set; }

            [JsonProperty("query")]
            public URLQuery Query { get; set; }
        }

        public partial class URLQuery
        {
            [JsonProperty("pages")]
            public Dictionary<string, Page> Pages { get; set; }
        }

        public partial class Page
        {
            [JsonProperty("pageid")]
            public long Pageid { get; set; }

            [JsonProperty("ns")]
            public long Ns { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("contentmodel")]
            public string Contentmodel { get; set; }

            [JsonProperty("pagelanguage")]
            public string Pagelanguage { get; set; }

            [JsonProperty("pagelanguagehtmlcode")]
            public string Pagelanguagehtmlcode { get; set; }

            [JsonProperty("pagelanguagedir")]
            public string Pagelanguagedir { get; set; }

            [JsonProperty("touched")]
            public DateTimeOffset Touched { get; set; }

            [JsonProperty("lastrevid")]
            public long Lastrevid { get; set; }

            [JsonProperty("length")]
            public long Length { get; set; }

            [JsonProperty("fullurl")]
            public Uri Fullurl { get; set; }

            [JsonProperty("editurl")]
            public Uri Editurl { get; set; }

            [JsonProperty("canonicalurl")]
            public Uri Canonicalurl { get; set; }
        }

    }
}