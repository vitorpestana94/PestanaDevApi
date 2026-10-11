using System.Text.Json;
using PestanaDevApi.Interfaces.Services;
using Locale = PestanaDevApi.Constants.LocaleConstants;

namespace PestanaDevApi.Services
{
    /// <summary>
    /// i18next-like translation service. Loads every Locales/{locale}/{namespace}.json once
    /// and keeps them in memory, so it must be registered as Singleton.
    /// </summary>
    public class TranslationService: ITranslationService
    {
        // locale -> ("namespace:path.to.key" -> text)
        private readonly Dictionary<string, Dictionary<string, string>> _translations = new(StringComparer.OrdinalIgnoreCase);

        public TranslationService()
        {
            string localesDirectory = Path.Combine(Directory.GetCurrentDirectory(), Locale.LocalesDirectory);

            if (!Directory.Exists(localesDirectory))
                return;

            foreach (string localeDirectory in Directory.GetDirectories(localesDirectory))
            {
                Dictionary<string, string> localeTranslations = [];

                foreach (string file in Directory.GetFiles(localeDirectory, "*.json"))
                {
                    using JsonDocument json = JsonDocument.Parse(File.ReadAllText(file));

                    LoadKeys(json.RootElement, prefix: $"{Path.GetFileNameWithoutExtension(file)}:", localeTranslations);
                }

                _translations[Path.GetFileName(localeDirectory)] = localeTranslations;
            }
        }

        public string Translate(string locale, string key, Dictionary<string, string>? values = null)
        {
            string text = GetText(locale, key) ?? GetText(Locale.Default, key) ?? key;

            if (values == null)
                return text;

            foreach (KeyValuePair<string, string> value in values)
            {
                text = text.Replace($"{{{{{value.Key}}}}}", value.Value);
            }

            return text;
        }

        #region Private Methods
        private string? GetText(string locale, string key)
        {
            if (!_translations.TryGetValue(locale ?? string.Empty, out Dictionary<string, string>? localeTranslations))
                return null;

            return localeTranslations.TryGetValue(key, out string? text) ? text : null;
        }

        /// <summary>
        /// Flattens the json into "namespace:path.to.key" entries.
        /// </summary>
        private static void LoadKeys(JsonElement element, string prefix, Dictionary<string, string> translations)
        {
            if (element.ValueKind != JsonValueKind.Object)
            {
                translations[prefix] = element.GetString() ?? string.Empty;
                return;
            }

            foreach (JsonProperty property in element.EnumerateObject())
            {
                string key = prefix.EndsWith(':') ? $"{prefix}{property.Name}" : $"{prefix}.{property.Name}";

                LoadKeys(property.Value, key, translations);
            }
        }
        #endregion
    }
}
