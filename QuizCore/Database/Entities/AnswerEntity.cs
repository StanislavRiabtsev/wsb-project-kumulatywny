using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizCore.Database.Entities
{
    public class AnswerEntity
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public QuestionEntity Question { get; set; }
    }
}