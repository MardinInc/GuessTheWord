using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GuessTheWord
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public int activeLetter;
        public static List<Question> allQuestions = Mongo.GetCollection();
        public static Random rnd = new Random();
        public static int indexQuestion;
        public static string answer;
        public string[] arrayAnswer;
        public GameInfo newGame = new GameInfo();
        public Question question;
        public int healPoint = 3;
        public GamePage()
        {
            InitializeComponent();
            allQuestions = Mongo.GetCollection();
            Start();
        }
        public void Start()
        {
            try
            {
                wpOne.Children.Clear();
                wpAnswer.Children.Clear();
                activeLetter = 0;
                indexQuestion = rnd.Next(0, allQuestions.Count);
                question = allQuestions[indexQuestion];
                answer = question.answer.ToUpper();
                arrayAnswer = new string[answer.Length];
                tbTextQuestion.Text = question.textQuestion;
                string[] letters = new string[40];
                AddToArrary(answer, letters);
                letters = RandomizeArray(letters);
                CreateKeyboard(letters, answer);
            }
            catch
            {
                MessageBox.Show("Вопросы закончились!");
                Mongo.AddToDataBaseGame(newGame);
                NavigationService.Navigate(new StartPage());
            }
        }
        public void AddToArrary(string textQuestion, string[] letters)
        {
            for (int i = 0; i < textQuestion.Length; i++)
            {
                letters[i] = textQuestion[i].ToString().ToUpper();
            }
            for(int i = textQuestion.Length; i < letters.Length; i++)
            {
                letters[i] = ((char)(rnd.Next(1040, 1104))).ToString().ToUpper();
            }
        }

        private void CreateKeyboard(string[] letters, string answer)
        {
            lbHp.Content = healPoint;
            lRightAnswer.Content = newGame.rightAnswer;
            for (int i = 0; i < 40; i++)
            {
                Button btt = new Button();
                btt.Content = letters[i];
                btt.Width = 48;
                btt.Height = 48;
                btt.FontSize = 30;
                btt.Margin = new Thickness(0, 0, 0, 0);
                btt.Click += new RoutedEventHandler(this.AddLetter);
                btt.BorderBrush = Brushes.Blue;
                btt.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/IconSkin/orangebutton.png")) };
                wpOne.Children.Add(btt);
            }
            for(int i = 0; i < answer.Length; i++)
            {
                Button btt = new Button();
                btt.Content = "";
                btt.Width = 48;
                btt.Height = 48;
                btt.FontSize = 30;
                btt.Margin = new Thickness(0, 0, 0, 0);
                btt.BorderBrush = Brushes.Blue;
                btt.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/IconSkin/orangebutton.png")) };
                wpAnswer.Children.Add(btt);
            }
            for(int i = 0; i < arrayAnswer.Length; i++)
            {
                arrayAnswer[i] = "0";
            }
        }
        public void AddLetter(Object sender,EventArgs e)
        {
            if(activeLetter < answer.Length)
            {
                Button clickedButton = (Button)sender;
                Button btt = new Button();
                clickedButton.IsEnabled = false;
                btt.Content = clickedButton.Content;
                btt.Width = 48;
                btt.Height = 48;
                btt.FontSize = 30;
                btt.Margin = new Thickness(0, 0, 0, 0);
                btt.Click += new RoutedEventHandler(this.AddLetter);
                btt.HorizontalAlignment = HorizontalAlignment.Center;
                btt.VerticalAlignment = VerticalAlignment.Center;
                btt.Click -= new RoutedEventHandler(this.AddLetter);
                btt.Click += new RoutedEventHandler(this.DeleteLetter);
                wpOne.Children.Remove(clickedButton);
                int indexLetter = FirstLetter(arrayAnswer);
                wpAnswer.Children.RemoveAt(indexLetter);
                arrayAnswer[indexLetter] = btt.Content.ToString();
                btt.BorderBrush = Brushes.Blue;
                btt.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/IconSkin/orangebutton.png")) };
                wpAnswer.Children.Insert(indexLetter, btt);
                activeLetter += 1;
                CheckWin();
            }
        }

        public int FirstLetter(string[] array)
        {
            int index = -1;
            for(int i = 0; i < array.Length; i ++)
            {
                if (array[i] == "0")
                {
                    index = i;
                    i = array.Length;
                }
            }
            return index;
        }
        private void DeleteLetter(object sender, RoutedEventArgs e)
        {
            var btt = (Button)sender;
            Button newBtt = btt;
            int indBtn = wpAnswer.Children.IndexOf(btt);
            wpAnswer.Children.Remove(btt);
            newBtt.Click -= new RoutedEventHandler(this.DeleteLetter);
            newBtt.Click += new RoutedEventHandler(this.AddLetter);
            newBtt.BorderBrush = Brushes.Blue;
            newBtt.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/IconSkin/orangebutton.png")) };
            wpOne.Children.Add(newBtt);
            activeLetter -= 1;
            Button bttTwo = new Button();
            bttTwo.Content = "";
            bttTwo.Width = 48;
            bttTwo.Height = 48;
            bttTwo.FontSize = 30;
            bttTwo.Margin = new Thickness(0, 0, 0, 0);
            arrayAnswer[indBtn] = "0";
            bttTwo.BorderBrush = Brushes.Blue;
            bttTwo.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/IconSkin/orangebutton.png")) };
            wpAnswer.Children.Insert(indBtn, bttTwo);
        }
        public void CheckWin()
        {
            if(activeLetter == arrayAnswer.Length)
            {
                string checkAnswer = "";
                for(int i = 0; i < arrayAnswer.Length; i++) 
                {
                    checkAnswer += arrayAnswer[i];
                }
                if(checkAnswer == answer)
                {
                    MessageBox.Show("Правильный ответ!");
                    lRightAnswer.Content = newGame.rightAnswer;
                    allQuestions.Remove(question);
                    Start();
                    newGame.rightAnswer += 1;
                }
                else if(healPoint == 0)
                { 
                    MessageBox.Show("Игра окончена! Ваш счет: " + newGame.rightAnswer);
                    Mongo.AddToDataBaseGame(newGame);
                    NavigationService.Navigate(new StartPage());
                }
                else
                {
                    MessageBox.Show("Упс... Вы потеряли очко здоровья!");
                    healPoint -= 1;
                    lbHp.Content = healPoint;
                }
            }
        }
        public string[] RandomizeArray(string[] letters)
        {
            string a;
            string b;
            int c;
            for(int i = 0; i < 38; i++)
            {
                c = rnd.Next(39);
                a = letters[i];
                b = letters[c];
                letters[i] = b;
                letters[c] = a;
            }
            return letters;
        }
        private void btnBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }
    }
}
