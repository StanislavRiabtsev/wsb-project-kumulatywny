using Microsoft.EntityFrameworkCore;
using QuizCore.Database.Entities;
using QuizCore.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizCore.Database
{
    public class QuizRepository
    {
        public void AddQuiz(QuizData quizData)
        {
            using var context = new QuizDbContext();

            var quizEntity = new QuizEntity
            {
                Title = quizData.quizTitle,
                Questions = quizData.questions.Select(q => new QuestionEntity
                {
                    Content = q.tresc,
                    Answers = q.odpowiedzi.Select(a => new AnswerEntity
                    {
                        Content = a.tresc,
                        IsCorrect = a.czyPoprawna
                    }).ToList()
                }).ToList()
            };

            context.Quizzes.Add(quizEntity);
            context.SaveChanges();
        }

        public List<QuizEntity> GetAllQuizzes()
        {
            using var context = new QuizDbContext();
            return context.Quizzes.ToList();
        }
        public async Task<List<QuizEntity>> GetAllQuizzesAsync()
        {
            using var context = new QuizDbContext();
            return await context.Quizzes.ToListAsync();
        }

        public List<QuizEntity> SearchQuizzes(string searchText)
        {
            using var context = new QuizDbContext();

            return context.Quizzes
                .Where(q => q.Title.Contains(searchText))
                .OrderBy(q => q.Title)
                .ToList();
        }
        public QuizData GetQuizById(int quizId)
        {
            using var context = new QuizDbContext();

            var quizEntity = context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefault(q => q.Id == quizId);

            if (quizEntity == null) return null;

            return MapEntityToData(quizEntity);
        }
        public async Task<QuizData> GetQuizByIdAsync(int quizId)
        {
            using var context = new QuizDbContext();

            var quizEntity = await context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quizEntity == null) return null;

            return MapEntityToData(quizEntity);
        }

        private QuizData MapEntityToData(QuizEntity quizEntity)
        {
            return new QuizData
            {
                quizTitle = quizEntity.Title,
                questions = quizEntity.Questions.Select(q => new QuestionData
                {
                    tresc = q.Content,
                    odpowiedzi = q.Answers.Select(a => new AnswerData
                    {
                        tresc = a.Content,
                        czyPoprawna = a.IsCorrect
                    }).ToList()
                }).ToList()
            };
        }
    }
}