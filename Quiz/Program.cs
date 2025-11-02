using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SystemQuiz
{
    public interface IAnswer
    {
        string Tresc { get; set; }
        bool CzyPoprawna { get; set; }
    }

    public interface IQuestion<TAnswer> where TAnswer : IAnswer
    {
        string Tresc { get; set; }
        List<TAnswer> Odpowiedzi { get; }

        void DodajOdpowiedz(TAnswer odp);
        bool SprawdzOdpowiedz(int indeks);
        void Wyswietl();
    }

    public interface IQuiz<TQuestion, TAnswer>
        where TQuestion : IQuestion<TAnswer>
        where TAnswer : IAnswer
    {
        string Tytul { get; set; }
        List<TQuestion> Pytania { get; }

        void DodajPytanie(TQuestion pytanie);
        void PrzeprowadzQuiz();
    }

    public class Odpowiedz : IAnswer
    {
        public string Tresc { get; set; }
        public bool CzyPoprawna { get; set; }

        public Odpowiedz(string tresc, bool czyPoprawna = false)
        {
            Tresc = tresc;
            CzyPoprawna = czyPoprawna;
        }

        public override string ToString() => Tresc;
    }

    public class Pytanie<TAnswer> : IQuestion<TAnswer> where TAnswer : IAnswer
    {
        public string Tresc { get; set; }
        public List<TAnswer> Odpowiedzi { get; private set; }

        public Pytanie(string tresc)
        {
            Tresc = tresc;
            Odpowiedzi = new List<TAnswer>();
        }

        public void DodajOdpowiedz(TAnswer odp)
        {
            Odpowiedzi.Add(odp);
        }

        public bool SprawdzOdpowiedz(int indeks)
        {
            return indeks >= 0 && indeks < Odpowiedzi.Count && Odpowiedzi[indeks].CzyPoprawna;
        }

        public void Wyswietl()
        {
            Console.WriteLine(Tresc);
            for (int i = 0; i < Odpowiedzi.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Odpowiedzi[i]}");
            }
        }
    }

    public class Quiz<TQuestion, TAnswer> : IQuiz<TQuestion, TAnswer>
        where TQuestion : IQuestion<TAnswer>
        where TAnswer : IAnswer
    {
        public string Tytul { get; set; }
        public List<TQuestion> Pytania { get; private set; }

        public Quiz(string tytul)
        {
            Tytul = tytul;
            Pytania = new List<TQuestion>();
        }

        public void DodajPytanie(TQuestion pytanie)
        {
            Pytania.Add(pytanie);
        }

        public void PrzeprowadzQuiz()
        {
            Console.WriteLine($"\n=== {Tytul} ===\n");
            int wynik = 0;

            for (int i = 0; i < Pytania.Count; i++)
            {
                Console.WriteLine($"Pytanie {i + 1}/{Pytania.Count}:");
                Pytania[i].Wyswietl();

                Console.Write("Wybierz numer odpowiedzi: ");
                string input = Console.ReadLine()!;

                if (int.TryParse(input, out int wybor))
                {
                    if (Pytania[i].SprawdzOdpowiedz(wybor - 1))
                    {
                        Console.WriteLine("✅ Dobrze!\n");
                        wynik++;
                    }
                    else
                    {
                        Console.WriteLine("❌ Źle!\n");
                    }
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy wybór.\n");
                }
            }

            Console.WriteLine($"Twój wynik: {wynik}/{Pytania.Count}");
            Console.WriteLine("\nDziękujemy za udział w quizie!");
        }
    }

    public class QuizData
    {
        public string QuizTitle { get; set; }
        public List<QuestionData> Questions { get; set; }
    }

    public class QuestionData
    {
        public string Tresc { get; set; }
        public List<AnswerData> Odpowiedzi { get; set; }
    }

    public class AnswerData
    {
        public string Tresc { get; set; }
        public bool CzyPoprawna { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string jsonPath = Path.Combine(AppContext.BaseDirectory, "pytania.json");

            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($"❌ Nie znaleziono pliku: {jsonPath}");
                return;
            }

            string json = File.ReadAllText(jsonPath);

            QuizData data;
            try
            {
                data = JsonSerializer.Deserialize<QuizData>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Błąd podczas deserializacji JSON:");
                Console.WriteLine(ex.Message);
                return;
            }

            if (data == null || data.Questions == null)
            {
                Console.WriteLine("❌ Błąd podczas wczytywania danych z pliku JSON (pusty wynik).");
                return;
            }

            var quiz = new Quiz<IQuestion<IAnswer>, IAnswer>(data.QuizTitle);

            foreach (var q in data.Questions)
            {
                var pytanie = new Pytanie<IAnswer>(q.Tresc);
                foreach (var a in q.Odpowiedzi)
                {
                    pytanie.DodajOdpowiedz(new Odpowiedz(a.Tresc, a.CzyPoprawna));
                }
                quiz.DodajPytanie(pytanie);
            }

            quiz.PrzeprowadzQuiz();
        }
    }
}
