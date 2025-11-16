using SystemQuiz.Interfaces;
using System;
using System.Collections.Generic;

namespace SystemQuiz.Models
{
    public class Quiz<TQuestion, TAnswer> : IQuiz<TQuestion, TAnswer>
        where TQuestion : IQuestion<TAnswer>
        where TAnswer : IAnswer
    {
        public string Tytul { get; set; }
        public List<TQuestion> Pytania { get; private set; }

        public Quiz()
        {
            Pytania = new List<TQuestion>();
        }

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
                        Console.WriteLine("✓ Dobrze!\n");
                        wynik++;
                    }
                    else
                    {
                        Console.WriteLine("✗ Źle!\n");
                    }
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy wybór.\n");
                }
            }

            Console.WriteLine($"Twój wynik: {wynik}/{Pytania.Count}");
            Console.WriteLine("Dziękujemy za udział w quizie!");
        }
    }
}
