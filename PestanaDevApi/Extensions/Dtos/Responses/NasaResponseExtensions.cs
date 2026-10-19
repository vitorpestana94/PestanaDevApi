using PestanaDevApi.Dtos.Responses;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using static Google.Apis.Requests.BatchRequest;

namespace PestanaDevApi.Extensions.Dtos.Responses
{
    public static class NasaResponseExtensions
    {
        public static NasaResponse ParseNasaResponse(this NasaResponse response)
        {
            response.Explanation = RemoveUndesiredExplanationContent(ParseHtmlToText(response.Explanation)); // This was done because due the currently nasa's response intability, they are adding HTML tags to response. And I dont wanna that to be received by the frontend.
            response.Copyright = ParseHtmlToText(response.Copyright); // Same here, nasa is returning unnecessary HTML tags.
            response.HdUrl = IsVideoThatNeedsParse(response) ? GetVideoFromBasicHtml(response.BasicHtml) : response.HdUrl; // Nasa is not currently returning the video directly on Hdurl, it's returning an empty string instead. So, until deal with it, i'm diging the video inside basichtml response.
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

        private static bool IsVideoThatNeedsParse(NasaResponse response) => 
            response.MediaType == "video" && 
            (response.HdUrl == "" || 
            !response.HdUrl.Contains("mp4") || 
            !response.HdUrl.Contains("youtube")); // Sadly, NASA'S response is unstable. So, sometimes the response does not contain a video to be shown.

        private static string GetVideoFromBasicHtml(string basicHtml)
        {
            Match match = Regex.Match(basicHtml, @"<(?:source|iframe)\b[^>]*\bsrc=""([^""]+)""");

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
            return Regex.Replace(explanation, @"^Explanation:\s*|Tomorrow's picture:[\s\S]*$","",RegexOptions.IgnoreCase); // I dont want this in my respose
        }
    }
}
