using SystemQuiz.Interfaces;
using SystemQuiz.Models;
using SystemQuiz.Serialization;
using System.Linq;

namespace SystemQuiz.Serialization
{
    public static class QuizConverter
    {
        public static QuizData ConvertToData(Quiz<IQuestion<IAnswer>, IAnswer> quiz)
        {
            return new QuizData
            {
                quizTitle = quiz.Tytul,
                questions = quiz.Pytania.Select(q => new QuestionData
                {
                    tresc = q.Tresc,
                    odpowiedzi = q.Odpowiedzi.Select(o => new AnswerData
                    {
                        tresc = o.Tresc,
                        czyPoprawna = o.CzyPoprawna
                    }).ToList()
                }).ToList()
            };
        }

        public static Quiz<IQuestion<IAnswer>, IAnswer> ConvertFromData(QuizData data)
        {
            var quiz = new Quiz<IQuestion<IAnswer>, IAnswer>(data.quizTitle);

            foreach (var q in data.questions)
            {
                var pytanie = new Pytanie<IAnswer>(q.tresc);
                foreach (var a in q.odpowiedzi)
                {
                    pytanie.DodajOdpowiedz(new Odpowiedz(a.tresc, a.czyPoprawna));
                }
                quiz.DodajPytanie(pytanie);
            }

            return quiz;
        }
    }
}
