using SystemQuiz.Serialization;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace SystemQuiz.Serialization
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

        public static void SaveToXml(string path, QuizData data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(QuizData));
            using var writer = new StreamWriter(path);
            serializer.Serialize(writer, data);
        }

        public static QuizData LoadFromXml(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(QuizData));
            using var reader = new StreamReader(path);
            return (QuizData)serializer.Deserialize(reader);
        }
    }
}
