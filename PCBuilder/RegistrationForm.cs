﻿using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PCBuilder
{
    public partial class RegistrationForm : Form
    {
        // Константы для адреса электронной почты отправителя и пароля
        private const string EmailFrom = "PCBuilder58@yandex.ru";
        private const string Password = "ofgljslytncnjmbj";

        public RegistrationForm()
        {
            InitializeComponent();
        }

        

        // Метод для валидации данных перед регистрацией
        private bool ValidateRegistration()
        {
            string login = LoginTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;
            string confirmPassword = ConfirmPasswordTextBox.Text;

            // Проверка заполнения всех полей
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return false;
            }

            // Проверка соответствия пароля требованиям
            if (!IsPasswordValid(password))
            {
                MessageBox.Show("Пароль должен содержать минимум 8 символов, " +
                                "включая заглавные и строчные буквы, цифры и специальные символы.");
                return false;
            }

            // Проверка совпадения паролей
            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.");
                return false;
            }

            // Проверка корректности введенного email
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Пожалуйста, введите корректный адрес электронной почты.");
                EmailTextBox.Clear(); // Очищаем поле email в случае некорректного ввода
                return false;
            }

            return true;
        }

        // Метод для валидации email
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
        }

        // Метод для валидации пароля
        private bool IsPasswordValid(string password)
        {
            var passwordPattern = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
            return passwordPattern.IsMatch(password);
        }

        private bool IsLoginExists(string login)
        {
            // Подключение к базе данных
            using (var connection = new SqlConnection("Server=DESKTOP-RNGC4SV; Database= PCUsersAndTemplates;Trusted_Connection=True;"))
            {
                // Открытие соединения
                connection.Open();

                // Выполнение SQL-запроса
                using (var command = new SqlCommand($"SELECT COUNT(*) FROM Users WHERE Login = '{login}'", connection))
                {
                    // Получение результата выполнения запроса
                    int count = (int)command.ExecuteScalar();

                    // Если пользователь с таким логином существует, возвращаем true
                    return count > 0;
                }
            }
        }


        // Метод для генерации случайного кода
        private string GenerateRandomCode()
        {
            Random rnd = new Random();
            int digitsCount = rnd.Next(3, 5);
            int lettersCount = 6 - digitsCount;
            string digits = new string(Enumerable.Range(0, digitsCount)
                .Select(_ => (char)('0' + rnd.Next(10))).ToArray());
            string letters = new string(Enumerable.Range(0, lettersCount)
                .Select(_ => (char)('A' + rnd.Next(26))).ToArray());
            string code = new string((digits + letters).OrderBy(c => rnd.Next()).ToArray());
            return code;
        }

        private async void SendCodeButton_Click(object sender, EventArgs e)
        {
            // Проверяем корректность введенного email перед отправкой
            if (!ValidateEmail() || !ValidateRegistration())
                return;

            string login = LoginTextBox.Text;

            // Проверка существования пользователя с таким же логином в базе данных
            if (IsLoginExists(login))
            {
                MessageBox.Show("Пользователь с таким логином уже существует. Пожалуйста, выберите другой логин.");
                return;
            }

            // Генерируем случайный код
            string code = GenerateRandomCode();

            // Формируем письмо с кодом подтверждения
            MailAddress fromAddress = new MailAddress(EmailFrom, "Personal computer builder");
            MailAddress toAddress = new MailAddress(EmailTextBox.Text);
            MailMessage message = new MailMessage(fromAddress, toAddress)
            {
                Body = $"Здравствуйте, {LoginTextBox.Text}! Спасибо, что выбрали нас!\nВаш код для регистрации: {code}",
                Subject = "Спасибо за регистрацию!"
            };

            // Настройка SMTP клиента для отправки письма
            SmtpClient smtpClient = new SmtpClient("smtp.yandex.ru")
            {
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(EmailFrom, Password)
            };

            try
            {
                // Отправка письма с кодом подтверждения
                await smtpClient.SendMailAsync(message);
                MessageBox.Show("Письмо с кодом для регистрации отправлено на вашу почту.");
                CodeForm cf = new CodeForm(code);
                cf.ShowDialog();
            }
            catch (Exception ex)
            {
                // Вывод сообщения об ошибке в случае неудачной отправки письма
                MessageBox.Show($"Ошибка при отправке письма: {ex.Message}");
            }
        }



        // Метод для валидации введенного email перед отправкой
        private bool ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните поле 'Email'.");
                return false;
            }

            if (!IsValidEmail(EmailTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите корректный адрес электронной почты.");
                EmailTextBox.Clear();
                return false;
            }

            return true;
        }

        private void LoginTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateRegistration();
        }

        private void EmailTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateRegistration();
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateRegistration();
        }

        private void ConfirmPasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateRegistration();
        }



        private void RegistrationForm_Load(object sender, EventArgs e)
        {

        }

        private void backButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            AuthorizationForm af = new AuthorizationForm();
            af.Show();
            
        }
    }
}
