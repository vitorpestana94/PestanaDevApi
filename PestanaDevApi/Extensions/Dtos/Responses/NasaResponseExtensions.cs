using PestanaDevApi.Dtos.Responses;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;

namespace PestanaDevApi.Extensions.Dtos.Responses
{
    public static class NasaResponseExtensions
    {
        public static NasaResponse ParseNasaResponse(this NasaResponse response)
        {
            response.Explanation = RemoveUndesiredExplanationContent(ParseHtmlToText(response.Explanation));
            response.Copyright = ParseHtmlToText(response.Copyright);
            response.HdUrl = IsVideoThatNeedsParse(response) ? GetVideoFromBasicHtml(response.BasicHtml) : response.HdUrl;
            response.BasicHtml = string.Empty;

            return response;
        }


        public static IEnumerable<NasaResponse> ParseNasaResponse(this IEnumerable<NasaResponse> response)
        {
            foreach (NasaResponse json in response)
            {
                json.ParseNasaResponse();
            }

            return response.OrderByDescending(response => response.Date);
        }

        private static bool IsVideoThatNeedsParse(NasaResponse response) => response.MediaType == "video" && response.HdUrl == "";

        private static string GetVideoFromBasicHtml(string basicHtml)
        {
            Match match = Regex.Match(basicHtml, @"<source[^>]+src=""([^""]+)""");

            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        private static string ParseHtmlToText(string htmlText)
        {
            HtmlDocument doc = new();
            doc.LoadHtml(htmlText);

           return doc.DocumentNode.InnerText;
        }

        private static string RemoveUndesiredExplanationContent(string explanation)
        {
            return Regex.Replace(explanation, @"^Explanation:\s*|Tomorrow's picture:[\s\S]*$","",RegexOptions.IgnoreCase);
        }
    }
}
