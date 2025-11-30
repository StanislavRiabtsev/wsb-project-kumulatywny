using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizCore.Database;
using QuizCore.Serialization;

namespace QuizWeb.Pages
{
    public class SolveModel : PageModel
    {
        private readonly QuizRepository _repository;

        public SolveModel(QuizRepository repository)
        {
            _repository = repository;
        }

        public QuizData CurrentQuiz { get; set; }
        public int? Score { get; set; }
        public int TotalQuestions { get; set; }
        public IActionResult OnGet(int id)
        {
            CurrentQuiz = _repository.GetQuizById(id);
            if (CurrentQuiz == null)
            {
                return RedirectToPage("Index");
            }
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            CurrentQuiz = _repository.GetQuizById(id);
            if (CurrentQuiz == null) return RedirectToPage("Index");

            int correctCount = 0;
            TotalQuestions = CurrentQuiz.questions.Count;

            for (int i = 0; i < TotalQuestions; i++)
            {
                string selectedVal = Request.Form[$"q_{i}"];

                if (int.TryParse(selectedVal, out int answerIndex))
                {
                    if (CurrentQuiz.questions[i].odpowiedzi[answerIndex].czyPoprawna)
                    {
                        correctCount++;
                    }
                }
            }

            Score = correctCount;
            return Page();
        }
    }
}