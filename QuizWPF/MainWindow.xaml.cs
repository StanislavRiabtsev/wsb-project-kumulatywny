using QuizCore;
using QuizCore.Serialization;
using QuizCore.Database; // Важно: подключили базу данных
using Microsoft.Win32;
using System;
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

                try
                {
                    if (file.EndsWith(".json"))
                        _quizData = QuizSerializer.LoadFromJson(file);
                    else
                        _quizData = QuizSerializer.LoadFromXml(file);

                    // Сброс состояния
                    _currentQuestionIndex = 0;
                    _score = 0;

                    StartPanel.Visibility = Visibility.Collapsed;
                    QuizPanel.Visibility = Visibility.Visible;
                    ResultPanel.Visibility = Visibility.Collapsed;

                    ShowQuestion();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd wczytywania pliku: " + ex.Message);
                }
            }
        }

        private void ShowQuestion()
        {
            if (_quizData == null || _quizData.questions.Count == 0) return;

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
                // Конец квиза
                QuizPanel.Visibility = Visibility.Collapsed;
                ResultPanel.Visibility = Visibility.Visible;
                ResultText.Text = $"Twój wynik: {_score}/{_quizData.questions.Count}";
            }
            else
            {
                ShowQuestion();
            }
        }

        // Логика сохранения в базу данных
        private void SaveToDb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_quizData != null)
                {
                    var repo = new QuizRepository();
                    repo.AddQuiz(_quizData);
                    MessageBox.Show("Sukces! Quiz został zapisany do bazy danych SQL.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd zapisu do bazy: {ex.Message}\n{ex.InnerException?.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}