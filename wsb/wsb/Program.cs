using System;
using System.Collections.Generic;

namespace SystemQuiz
{
    public interface IAnswer
    {
        string Tresc { get; set; }
        bool CzyPoprawna { get; set; }
    }

    public interface IQuestion
    {
        string Tresc { get; set; }
        List<IAnswer> Odpowiedzi { get; }

        void DodajOdpowiedz(IAnswer odp);
        bool SprawdzOdpowiedz(int indeks);
        void Wyswietl();
    }

    public interface IQuiz
    {
        string Tytul { get; set; }
        List<IQuestion> Pytania { get; }

        void DodajPytanie(IQuestion pytanie);
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

        public override string ToString()
        {
            return Tresc;
        }
    }

    public class Pytanie : IQuestion
    {
        public string Tresc { get; set; }
        public List<IAnswer> Odpowiedzi { get; private set; }

        public Pytanie(string tresc)
        {
            Tresc = tresc;
            Odpowiedzi = new List<IAnswer>();
        }

        public void DodajOdpowiedz(IAnswer odp)
        {
            Odpowiedzi.Add(odp);
        }

        public bool SprawdzOdpowiedz(int indeks)
        {
            if (indeks < 0 || indeks >= Odpowiedzi.Count)
                return false;
            return Odpowiedzi[indeks].CzyPoprawna;
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

    public class Quiz : IQuiz
    {
        public string Tytul { get; set; }
        public List<IQuestion> Pytania { get; private set; }

        public Quiz(string tytul)
        {
            Tytul = tytul;
            Pytania = new List<IQuestion>();
        }

        public void DodajPytanie(IQuestion pytanie)
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
                string input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int wybor))
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
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            IQuiz quiz = new Quiz("Quiz z programowania");

            IQuestion p1 = new Pytanie("Które słowo kluczowe w C# służy do dziedziczenia klasy?");
            p1.DodajOdpowiedz(new Odpowiedz("inherits"));
            p1.DodajOdpowiedz(new Odpowiedz("extends"));
            p1.DodajOdpowiedz(new Odpowiedz("base"));
            p1.DodajOdpowiedz(new Odpowiedz(":", true));

            IQuestion p2 = new Pytanie("Które z poniższych to typy wartościowe w C#?");
            p2.DodajOdpowiedz(new Odpowiedz("int", true));
            p2.DodajOdpowiedz(new Odpowiedz("string"));
            p2.DodajOdpowiedz(new Odpowiedz("class"));
            p2.DodajOdpowiedz(new Odpowiedz("interface"));

            IQuestion p3 = new Pytanie("Co oznacza OOP?");
            p3.DodajOdpowiedz(new Odpowiedz("Object-Oriented Programming", true));
            p3.DodajOdpowiedz(new Odpowiedz("Overpowered Operator Pattern"));
            p3.DodajOdpowiedz(new Odpowiedz("Only One Process"));
            p3.DodajOdpowiedz(new Odpowiedz("Operation On Platform"));

            quiz.DodajPytanie(p1);
            quiz.DodajPytanie(p2);
            quiz.DodajPytanie(p3);

            quiz.PrzeprowadzQuiz();

            Console.WriteLine("\nDziękujemy za udział w quizie!");
        }
    }
}
