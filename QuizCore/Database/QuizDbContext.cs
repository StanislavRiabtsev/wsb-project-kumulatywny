using Microsoft.EntityFrameworkCore;
using QuizCore.Database.Entities;

namespace QuizCore.Database
{
    public class QuizDbContext : DbContext
    {
        public DbSet<QuizEntity> Quizzes { get; set; }
        public DbSet<QuestionEntity> Questions { get; set; }
        public DbSet<AnswerEntity> Answers { get; set; }
        public QuizDbContext(DbContextOptions<QuizDbContext> options)
            : base(options)
        {
        }
        public QuizDbContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WsbQuizDb_Project;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}