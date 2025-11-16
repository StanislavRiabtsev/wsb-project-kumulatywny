using SystemQuiz.Interfaces;
using System;
using System.Collections.Generic;

namespace SystemQuiz.Models
{
    public class Pytanie<TAnswer> : IQuestion<TAnswer> where TAnswer : IAnswer
    {
        public string Tresc { get; set; }
        public List<TAnswer> Odpowiedzi { get; private set; }

        public Pytanie()
        {
            Odpowiedzi = new List<TAnswer>();
        }

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
}
