using QuizCore;
using QuizCore.Serialization;
using QuizCore.Database;
using QuizCore.Database.Entities; // Нужно для работы со списком
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

        // 1. Загрузка из файла
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

                    StartQuiz();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd pliku: " + ex.Message);
                }
            }
        }

        // 2. Поиск в БД (LINQ)
        private void SearchDb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var repo = new QuizRepository();
                string searchText = SearchBox.Text;

                // Вызов метода с LINQ запросом
                var results = repo.SearchQuizzes(searchText);

                DbQuizList.ItemsSource = results; // Привязка данных к списку

                if (results.Count == 0)
                    MessageBox.Show("Nie znaleziono quizów.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd bazy danych: " + ex.Message);
            }
        }

        // 3. Выбор квиза из списка
        private void DbQuizList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DbQuizList.SelectedItem is QuizEntity selectedQuizEntity)
            {
                try
                {
                    var repo = new QuizRepository();
                    // Загружаем полный квиз (с вопросами) по ID
                    _quizData = repo.GetQuizById(selectedQuizEntity.Id);

                    StartQuiz();

                    // Сброс выбора (чтобы можно было выбрать снова тот же)
                    DbQuizList.SelectedItem = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd ładowania quizu: " + ex.Message);
                }
            }
        }

        // Общий метод запуска
        private void StartQuiz()
        {
            _currentQuestionIndex = 0;
            _score = 0;
            StartPanel.Visibility = Visibility.Collapsed;
            ResultPanel.Visibility = Visibility.Collapsed;
            QuizPanel.Visibility = Visibility.Visible;
            ShowQuestion();
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
                QuizPanel.Visibility = Visibility.Collapsed;
                ResultPanel.Visibility = Visibility.Visible;
                ResultText.Text = $"Twój wynik: {_score}/{_quizData.questions.Count}";
            }
            else
            {
                ShowQuestion();
            }
        }

        private void SaveToDb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var repo = new QuizRepository();
                repo.AddQuiz(_quizData);
                MessageBox.Show("Zapisano!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex.Message);
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ResultPanel.Visibility = Visibility.Collapsed;
            StartPanel.Visibility = Visibility.Visible;
            SearchBox.Text = "";
            DbQuizList.ItemsSource = null;
        }
    }
}