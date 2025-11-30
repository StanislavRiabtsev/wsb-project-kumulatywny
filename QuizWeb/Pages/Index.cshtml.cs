using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizCore.Database;
using QuizCore.Database.Entities;
using System.Collections.Generic;

namespace QuizWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly QuizRepository _repository;
        public List<QuizEntity> Quizzes { get; set; } = new List<QuizEntity>();

        public IndexModel(QuizRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            Quizzes = _repository.GetAllQuizzes();
        }
    }
}