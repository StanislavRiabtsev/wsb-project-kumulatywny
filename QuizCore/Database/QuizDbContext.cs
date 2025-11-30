using Microsoft.EntityFrameworkCore;
using QuizCore.Database.Entities;

namespace QuizCore.Database
{
    public class QuizDbContext : DbContext
    {
        public DbSet<QuizEntity> Quizzes { get; set; }
        public DbSet<QuestionEntity> Questions { get; set; }
        public DbSet<AnswerEntity> Answers { get; set; }

        // 1. Конструктор для ASP.NET Core (Веб-сайт)
        // Он позволяет сайту передавать настройки из appsettings.json
        public QuizDbContext(DbContextOptions<QuizDbContext> options)
            : base(options)
        {
        }

        // 2. Пустой конструктор для WPF и Консоли
        // Он нужен, чтобы старый код (new QuizDbContext()) не сломался
        public QuizDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Эта проверка важна!
            // Если настройки НЕ были переданы через конструктор (значит, это Консоль или WPF),
            // то используем жестко прописанную строку.
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WsbQuizDb_Project;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}