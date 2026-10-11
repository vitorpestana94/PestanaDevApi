namespace PestanaDevApi.Interfaces.Services
{
    public interface ITranslationService
    {
        /// <summary>
        /// Translates a key to the requested locale, just like i18next's t() function.
        /// The translations are read from Locales/{locale}/{namespace}.json and the key
        /// must be written as "namespace:path.to.key" (e.g. "email:contact.client.title").
        /// If the locale or the key doesn't exist, the default locale (english) is used;
        /// if the key doesn't exist on the default locale either, the key itself is returned.
        /// </summary>
        /// <param name="locale">
        /// The desired locale (e.g. "en" or "pt").
        /// </param>
        /// <param name="key">
        /// The translation key, in the format "namespace:path.to.key".
        /// </param>
        /// <param name="values">
        /// Optional values that replace the {{variables}} of the translation (e.g. { "name": "Vitor" } for "Hello, {{name}}!").
        /// </param>
        /// <returns>
        /// The translated text with its variables replaced.
        /// </returns>
        string Translate(string locale, string key, Dictionary<string, string>? values = null);
    }
}
