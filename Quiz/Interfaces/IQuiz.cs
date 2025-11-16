using SystemQuiz.Interfaces;
using System.Collections.Generic;

namespace SystemQuiz.Interfaces
{
    public interface IQuiz<TQuestion, TAnswer>
        where TQuestion : IQuestion<TAnswer>
        where TAnswer : IAnswer
    {
        string Tytul { get; set; }
        List<TQuestion> Pytania { get; }

        void DodajPytanie(TQuestion pytanie);
        void PrzeprowadzQuiz();
    }
}
