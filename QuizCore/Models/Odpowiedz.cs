using QuizCore.Interfaces;

namespace QuizCore.Models
{
    public class Odpowiedz : IAnswer
    {
        public string Tresc { get; set; }
        public bool CzyPoprawna { get; set; }

        public Odpowiedz() { }

        public Odpowiedz(string tresc, bool czyPoprawna = false)
        {
            Tresc = tresc;
            CzyPoprawna = czyPoprawna;
        }

        public override string ToString() => Tresc;
    }
}
