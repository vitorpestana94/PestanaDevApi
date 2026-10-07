using PestanaDevApi.Dtos.Responses;
using HtmlAgilityPack;

namespace PestanaDevApi.Extensions.Dtos.Responses
{
    public static class NasaResponseExtensions
    {
        public static NasaResponse ParseNasaResponse(this NasaResponse response)
        {
            response.Explanation = ParseHtmlToText(response.Explanation);
            response.Copyright = ParseHtmlToText(response.Copyright);

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

        private static string ParseHtmlToText(string htmlText)
        {
            HtmlDocument doc = new();
            doc.LoadHtml(htmlText);

           return doc.DocumentNode.InnerText;
        }
    }
}
