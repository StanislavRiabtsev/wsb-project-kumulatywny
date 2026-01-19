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
            Console.WriteLine("Podaj nazwę pliku do wczytania (JSON):");
            string path = Console.ReadLine();

            if (!File.Exists(path))
            {
                Console.WriteLine("Plik nie istnieje!");
                return;
            }

            QuizData data;
            if (path.EndsWith(".json"))
            {
                data = QuizSerializer.LoadFromJson(path);
            }
            else
            {
                Console.WriteLine("Obsługiwany format: .json");
                return;
            }

            var quiz = QuizConverter.ConvertFromData(data);

            quiz.PrzeprowadzQuiz();

            Console.WriteLine("\nCzy zapisać quiz do JSON? (tak/nie)");
            if (Console.ReadLine().ToLower() == "tak")
            {
                var saveData = QuizConverter.ConvertToData(quiz);

                QuizSerializer.SaveToJson("zapisany_quiz.json", saveData);
                Console.WriteLine("Zapisano plik: zapisany_quiz.json");
            }
        }
    }
}