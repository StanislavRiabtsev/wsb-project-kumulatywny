using QuizCore.Serialization;
using QuizCore.Models; 
using QuizCore.Interfaces;
using System;
using System.IO;

namespace Quiz
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Podaj nazwę pliku do wczytania (JSON lub XML):");
            string path = Console.ReadLine();

            if (!File.Exists(path))
            {
                Console.WriteLine("❌ Plik nie istnieje!");
                return;
            }

            QuizData data;

            if (path.EndsWith(".json"))
                data = QuizSerializer.LoadFromJson(path);
            else if (path.EndsWith(".xml"))
                data = QuizSerializer.LoadFromXml(path);
            else
            {
                Console.WriteLine("❌ Obsługiwane formaty: .json, .xml");
                return;
            }

            var quiz = QuizConverter.ConvertFromData(data);

            quiz.PrzeprowadzQuiz();

            Console.WriteLine("\nCzy zapisać quiz do JSON i XML? (t/n)");
            if (Console.ReadLine().ToLower() == "t")
            {
                var saveData = QuizConverter.ConvertToData(quiz);

                QuizSerializer.SaveToJson("zapisany_quiz.json", saveData);
                QuizSerializer.SaveToXml("zapisany_quiz.xml", saveData);

                Console.WriteLine("✓ Zapisano pliki: zapisany_quiz.json oraz zapisany_quiz.xml");
            }
        }
    }
}
