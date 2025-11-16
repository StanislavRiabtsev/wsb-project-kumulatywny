using System.Collections.Generic;

namespace SystemQuiz.Serialization
{
    public class QuizData
    {
        public string quizTitle { get; set; }
        public List<QuestionData> questions { get; set; }
    }

    public class QuestionData
    {
        public string tresc { get; set; }
        public List<AnswerData> odpowiedzi { get; set; }
    }

    public class AnswerData
    {
        public string tresc { get; set; }
        public bool czyPoprawna { get; set; }
    }
}
