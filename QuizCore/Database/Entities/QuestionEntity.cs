using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizCore.Database.Entities
{
    public class QuestionEntity
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }

        public int QuizId { get; set; }

        [ForeignKey("QuizId")]
        public QuizEntity Quiz { get; set; }
        public List<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();
    }
}