using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizCore.Database.Entities
{
    public class QuizEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public List<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();
    }
}