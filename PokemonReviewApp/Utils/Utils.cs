using System.Text.RegularExpressions;

namespace PokemonReviewApp.Utils
{
    public static class Utils
    {

        public static string RemoveAccents(string str)
        {
            return Regex.Replace(str, @"[áàãâä]", "a")
                        .Replace("é", "e")
                        .Replace("è", "e")
                        .Replace("ê", "e")
                        .Replace("ë", "e")
                        .Replace("í", "i")
                        .Replace("ì", "i")
                        .Replace("î", "i")
                        .Replace("ï", "i")
                        .Replace("ó", "o")
                        .Replace("ò", "o")
                        .Replace("ô", "o")
                        .Replace("õ", "o")
                        .Replace("ö", "o")
                        .Replace("ú", "u")
                        .Replace("ù", "u")
                        .Replace("û", "u")
                        .Replace("ü", "u")
                        .Replace("ç", "c")
                        .Replace("ñ", "n");
        }
    }
}
