using QuizCore;
using QuizCore.Serialization;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace WPFQuiz
{
    public partial class MainWindow : Window
    {
        private QuizData _quizData;
        private int _currentQuestionIndex = 0;
        private int _score = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadQuiz_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Quiz files|*.json;*.xml";

            if (dialog.ShowDialog() == true)
            {
                string file = dialog.FileName;

                if (file.EndsWith(".json"))
                    _quizData = QuizSerializer.LoadFromJson(file);
                else
                    _quizData = QuizSerializer.LoadFromXml(file);

                StartPanel.Visibility = Visibility.Collapsed;
                QuizPanel.Visibility = Visibility.Visible;

                ShowQuestion();
            }
        }

        private void ShowQuestion()
        {
            var q = _quizData.questions[_currentQuestionIndex];

            QuestionText.Text = q.tresc;

            AnswersPanel.Children.Clear();

            for (int i = 0; i < q.odpowiedzi.Count; i++)
            {
                var rb = new RadioButton
                {
                    Content = q.odpowiedzi[i].tresc,
                    Tag = i,
                    FontSize = 16,
                    Margin = new Thickness(0, 5, 0, 5)
                };

                AnswersPanel.Children.Add(rb);
            }
        }

        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            int selected = -1;

            foreach (RadioButton rb in AnswersPanel.Children)
            {
                if (rb.IsChecked == true)
                {
                    selected = (int)rb.Tag;
                    break;
                }
            }

            if (selected == -1)
            {
                MessageBox.Show("Wybierz odpowiedź!");
                return;
            }

            if (_quizData.questions[_currentQuestionIndex].odpowiedzi[selected].czyPoprawna)
                _score++;

            _currentQuestionIndex++;

            if (_currentQuestionIndex >= _quizData.questions.Count)
            {
                QuizPanel.Visibility = Visibility.Collapsed;
                ResultPanel.Visibility = Visibility.Visible;

                ResultText.Text = $"Twój wynik: {_score}/{_quizData.questions.Count}";
            }
            else
            {
                ShowQuestion();
            }
        }
    }
}
