using Microsoft.EntityFrameworkCore;
using QuizCore.Database.Entities;
using QuizCore.Serialization;
using System.Collections.Generic;
using System.Linq;

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

        public QuizData GetQuizById(int quizId)
        {
            using var context = new QuizDbContext();

            var quizEntity = context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefault(q => q.Id == quizId);

            if (quizEntity == null) return null;

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

        public void UpdateQuizTitle(int quizId, string newTitle)
        {
            using var context = new QuizDbContext();
            var quiz = context.Quizzes.Find(quizId);
            if (quiz != null)
            {
                quiz.Title = newTitle;
                context.SaveChanges();
            }
        }
        public void DeleteQuiz(int quizId)
        {
            using var context = new QuizDbContext();
            var quiz = context.Quizzes.Find(quizId);
            if (quiz != null)
            {
                context.Quizzes.Remove(quiz);
                context.SaveChanges();
            }
        }
    }
}