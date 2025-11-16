using QuizCore.Interfaces;
using System.Collections.Generic;

namespace QuizCore.Interfaces
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
