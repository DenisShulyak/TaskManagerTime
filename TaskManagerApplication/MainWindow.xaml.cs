using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.IO;

namespace TaskManagerApplication
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate bool AsyncDelegate();

        public MainWindow()
        {
            InitializeComponent();

            

        }
        private bool SendEmail()
        {

            try{
                MailAddress from = new MailAddress("emailformyapp@mail.ru", "Task");
            // кому отправляем

            string whereTextBoxText = null;
            Dispatcher.Invoke(() => whereTextBoxText = whereTextBox.Text);
            MailAddress to = new MailAddress(whereTextBoxText);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Очень важное письмо";
            // текст письма
            m.Body = "Здесь должна быть ваша реклама";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("emailformyapp@mail.ru", "ppaymrofliame");
            smtp.EnableSsl = true;
            smtp.Send(m);
            }
            catch
            {
                MessageBox.Show("Неверно введенный адресс");
            }
            return true;
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datePicker.SelectedDate.Value >= DateTime.Now.Date)
                {
                    for (int i = 0; i < 24; i++)
                    {
                        if (hoursComboBox.Text == i.ToString())
                        {
                            for (int j = 0; j < 60; j++)
                            {
                                if (minutesComboBox.Text == j.ToString())
                                {
                                    if (reiterationComboBox.Text == oneMonthTextBlock.Text || reiterationComboBox.Text == oneWeekTextBlock.Text || reiterationComboBox.Text == oneYearTextBlock.Text)
                                    {
                                        if (operationComboBox.Text == downloadTextBlock.Text || operationComboBox.Text == emailTextBlock.Text || operationComboBox.Text == movingDirectoryTextBlock.Text)
                                        {
                                            if(fromTextBox.Text != " " && whereTextBox.Text != " ")
                                            {
                                                //Запись в базу данных
                                                var executer = new AsyncDelegate(LoadToDatabase); // AsyncDelegate  executer = Sum;
                                                var result = executer.BeginInvoke(LoadToDatabaseCallBack, null);
                                                if(!result.IsCompleted)
                                                {
                                                    //Выполнение выбранной операции
                                                    if (operationComboBox.Text == "Email")
                                                    {
                                                                PerformingAnOperation();
                                                                executer = new AsyncDelegate(SendEmail); // AsyncDelegate  executer = Sum;
                                                                result = executer.BeginInvoke(SendEmailCallBack, null);
                                                    }
                                                    else if(operationComboBox.Text == "Скачка файла")
                                                    {
                                                        try
                                                        {
                                                             PerformingAnOperation();
                                                            WebClient webClient = new WebClient();  //https://vscode.ru/filesForArticles/test.docx тестовая ссылка на скачивание
                                                            webClient.DownloadFile(fromTextBox.Text, whereTextBox.Text);
                                                        }
                                                        catch
                                                        {
                                                            MessageBox.Show("Путь к файлу или ссылка указаны не верно!(на конце пути к файлу обязательно напишите название файла и .<формат>)");
                                                        }
                                                    }
                                                    else if(operationComboBox.Text == "Перемещение каталога")
                                                    {
                                                       PerformingAnOperation();
                                                        File.Copy(fromTextBox.Text, whereTextBox.Text);
                                                        File.Delete(fromTextBox.Text);
                                                        
                                                        
                                                    }

                                                }
                                                
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Заполните поле *Тип операции*");
                                        }

                                    }
                                    else
                                    {
                                        MessageBox.Show("Заполните поле *Повторение*");

                                    }
                                }
                            }
                        }
                    }
                }

                else
                {
                    MessageBox.Show("Дата должна быть не меньше текущей!");
                }
            }
            catch
            {
                MessageBox.Show("Некоректные данные");
            }
        }
        
        public static void LoadToDatabaseCallBack(IAsyncResult result)
        {
            var executor = (result as AsyncResult).AsyncDelegate as AsyncDelegate;
            var dataResult = executor.EndInvoke(result);
        }public static void SendEmailCallBack(IAsyncResult result)
        {
            var executor = (result as AsyncResult).AsyncDelegate as AsyncDelegate;
            var dataResult = executor.EndInvoke(result);
        }

        private void PerformingAnOperation()
        {
            using (var context = new TaskManagerContext())
            {
                int year = 0;
                int month = 0;
                int day = 0;
                Dispatcher.Invoke(() => {
                    year = datePicker.SelectedDate.Value.Date.Year;
                    month = datePicker.SelectedDate.Value.Date.Month;
                    day = datePicker.SelectedDate.Value.Date.Day;
                });
                string hoursComboBoxText = null;
                string minutesComboBoxText = null;
                Dispatcher.Invoke(() => hoursComboBoxText = hoursComboBox.Text);
                Dispatcher.Invoke(() => minutesComboBoxText = minutesComboBox.Text);
                DateTime dateTime = new DateTime(year,month,day, int.Parse(hoursComboBoxText), int.Parse(minutesComboBoxText), 0);
                var interval = dateTime - DateTime.Now;
                MessageBox.Show("Операция будет выполнена в заданное время!");
                Thread.Sleep(interval.Seconds*1000);
                    MessageBox.Show("Готово");
            }

        }

        private bool LoadToDatabase()
        {
            using (var context = new TaskManagerContext())
            {
               
                string reiterationComboBoxText = null;
                Dispatcher.Invoke(() => reiterationComboBoxText = reiterationComboBox.Text);
                Reiteration reiteration = new Reiteration
                {
                    Name = reiterationComboBoxText
                };
                context.Reiterations.Add(reiteration);

                string operationComboBoxText = null;
                Dispatcher.Invoke(() => operationComboBoxText = operationComboBox.Text);
                TypeOperation operation = new TypeOperation
                {
                    Name = operationComboBoxText
                };
                context.TypeOperations.Add(operation);
                DateTime dateTime = new DateTime();
                Dispatcher.Invoke(() => dateTime = datePicker.SelectedDate.Value.Date);
                string hoursComboBoxText = null;
                string minutesComboBoxText = null;
                Dispatcher.Invoke(() => hoursComboBoxText = hoursComboBox.Text);
                Dispatcher.Invoke(() => minutesComboBoxText = minutesComboBox.Text);
                TimeSpan timeSpan = new TimeSpan(int.Parse(hoursComboBoxText), int.Parse(minutesComboBoxText), 0);
                dateTime = dateTime.Date + timeSpan;
                Task task = new Task { DateTime = dateTime, ReiterationId = reiteration.Id, TypeOperationId = operation.Id };
                context.Tasks.Add(task);

                context.SaveChanges();

            }
            return true;
        }
    }
}
