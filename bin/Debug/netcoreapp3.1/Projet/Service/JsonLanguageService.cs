using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

namespace Projet.Service
{
    public class JsonLanguageService : ILanguageService
    {
        private readonly Dictionary<string, string> _dict;

        public JsonLanguageService(string jsonPath)
        {
            _dict = File.Exists(jsonPath)
                ? JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(jsonPath))
                    ?? new Dictionary<string, string>()
                : new Dictionary<string, string>();
        }

        public string Translate(string key) =>
            _dict.TryGetValue(key, out string value) ? value : key;
    }
}
