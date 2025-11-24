using Microsoft.EntityFrameworkCore;
using QuizCore.Database.Entities;
using System.Collections.Generic;

namespace QuizCore.Database
{
    public class QuizDbContext : DbContext
    {
        public DbSet<QuizEntity> Quizzes { get; set; }
        public DbSet<QuestionEntity> Questions { get; set; }
        public DbSet<AnswerEntity> Answers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WsbQuizDb_Project;Trusted_Connection=True;");
        }
    }
}