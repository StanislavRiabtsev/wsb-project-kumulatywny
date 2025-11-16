using QuizCore.Interfaces;
using System.Collections.Generic;

namespace QuizCore.Interfaces 
{
    public interface IQuestion<TAnswer> where TAnswer : IAnswer
    {
        string Tresc { get; set; }
        List<TAnswer> Odpowiedzi { get; }

        void DodajOdpowiedz(TAnswer odp);
        bool SprawdzOdpowiedz(int indeks);
        void Wyswietl();
    }
}
