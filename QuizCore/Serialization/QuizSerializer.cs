using QuizCore.Serialization;
using System.IO;
using System.Text.Json;

namespace QuizCore.Serialization
{
    public static class QuizSerializer
    {
        public static void SaveToJson(string path, QuizData data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(path, json);
        }

        public static QuizData LoadFromJson(string path)
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<QuizData>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}