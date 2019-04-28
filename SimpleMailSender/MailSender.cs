using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Models.Users.Repository;

namespace SimpleMailSender
{
    public class MailSender
    {
        private static Dictionary<string, bool> notifiedUsers = new Dictionary<string, bool>();
        private UserRepository userRepository;

        public MailSender(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async void NotifyOnRegistration()
        {
            var users = await userRepository.GetAllAsync();
            foreach (var user in users)
            {
                if (!notifiedUsers.ContainsKey(user.Id))
                {
                    notifiedUsers.Add(user.Id, false);
                }

                if (!notifiedUsers[user.Id])
                {
                    SendEmailAsync(user.Email).GetAwaiter();
                    notifiedUsers[user.Id] = true;
                    Console.WriteLine("Сообщение отправлено");
                }
            }
        }
        
        private static async Task SendEmailAsync(string receiverAddress)
        {
            var from = new MailAddress("skima.mail4@gmail.com", "Skima");
            var to = new MailAddress(receiverAddress);
            var m = new MailMessage(from, to);
            m.Subject = "Уведомление";
            m.Body = "Пользователь успешно прошёл регистрацию";
            var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("skima.mail4@gmail.com", "backendskima");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }
    }
}