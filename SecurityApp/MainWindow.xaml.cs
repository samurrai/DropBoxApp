using SecurityApp.DataAccess;
using SecurityApp.Models;
using SecurityApp.Services;
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

namespace SecurityApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SignInButtonClick(object sender, RoutedEventArgs e)
        {
            var login = loginTextBox.Text;
            var password = passwordBox.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            signInButton.IsEnabled = false;
            using (var context = new SecurityContext())
            {
                var user = await GetUser(context, login);
                if (user == null || !SecurityHasher.VerifyPassword(password, user.Password))
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
            OpenDropBoxWindow();
        }

        private Task<User> GetUser(SecurityContext context, string login)
        {
            return Task.Run(() => {
                return context.Users.SingleOrDefault(searchingUser => searchingUser.Login == login);
            });
        }

        private void RegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            signInButton.Click -= SignInButtonClick;
            signInButton.Click += SignUpButtonClick;
            signInButton.Content = "Зарегистрироваться";
            registrationButton.Visibility = Visibility.Collapsed;
            registrationButton.IsEnabled = false;
            loginTextBox.Text = "";
            passwordBox.Password = "";
            Title = "Регистрация";
        }

        private async void SignUpButtonClick(object sender, RoutedEventArgs e)
        {
            var login = loginTextBox.Text;
            var password = passwordBox.Password;

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Введите логин");
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            signInButton.IsEnabled = false;
            using (var context = new SecurityContext())
            {
                var user = await GetUser(context, login);
                if (user == null)
                {
                    context.Users.Add(new User
                    {
                        Login = login,
                        Password = SecurityHasher.HashPassword(password)
                    });
                    await context.SaveChangesAsync();
                }
                else
                {
                    MessageBox.Show("Аккаунт с таким логином уже зарегистрирован");
                    signInButton.IsEnabled = true;
                    return;
                }
            }
            OpenDropBoxWindow();
        }
        private void OpenDropBoxWindow()
        {
            new DropBoxWindow().Show();
            Close();
        }
    }
}