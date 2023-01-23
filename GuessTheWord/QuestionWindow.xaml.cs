using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GuessTheWord
{
    /// <summary>
    /// Логика взаимодействия для QuestionWindow.xaml
    /// </summary>
    public partial class QuestionWindow : Window
    {
        public QuestionWindow()
        {
            InitializeComponent();
        }

        private void addQuestion_Click(object sender, RoutedEventArgs e)
        {
            bool one = CheckText(tbAnswer.Text);
            bool two = CheckText(tbQuestion.Text);
            if (tbQuestion.Text.Length > 0 && tbAnswer.Text.Length > 0 && one && two)
            {
                Question newQuestion = new Question(tbQuestion.Text, tbAnswer.Text);
                Mongo.AddToDataBase(newQuestion);
                MessageBox.Show("Вопрос добавлен");
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка ввода!");
            }
        }

        private bool CheckText(string textCheck)
        {
            int num = 0;
            string text = textCheck.ToLower();
            for(int i = 0; i < text.Length; i ++)
            {
                num = Convert.ToInt32(text[i]);
                if((num > 63 && num < 1072) || (num > 32 && num < 63) || num > 1105 || num < 32)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
