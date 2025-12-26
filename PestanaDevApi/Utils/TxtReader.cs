namespace PestanaDevApi.Utils
{
    public class TxtReader
    {
        /// <summary>
        /// Reads a property file (.txt) and loads its key-value pairs into the provided dictionary.
        /// Lines starting with '#' or empty lines are ignored.
        /// </summary>
        /// <param name="data">
        /// Dictionary to populate with key-value pairs from the file. Existing keys will be overwritten.
        /// </param>
        /// <param name="subfolder">
        /// Subfolder inside the 'Config' directory where the file is located.
        /// </param>
        /// <param name="fileName">
        /// Name of the file to read.
        /// </param>
        public static void ReadConfigFile(ref Dictionary<string, string> data, string subfolder, string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Config", subfolder, fileName);

            if (!File.Exists(filePath))
                return;

            string[] rows = File.ReadAllLines(filePath);
            string[] rowSplitted;

            foreach (string row in rows)
            {
                if (string.IsNullOrWhiteSpace(row) || row.StartsWith("#")) continue;
                rowSplitted = row.Split('=');
                data[rowSplitted.First()] = string.Join("=", rowSplitted.Skip(1));
            }
        }
    }
}
