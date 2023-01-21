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
            Question newQuestion = new Question(tbQuestion.Text, tbAnswer.Text);
            Mongo.AddToDataBase(newQuestion);
            this.Close();
        }
    }
}
