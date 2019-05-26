using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using EventGenerator;
using EventGenerator.Models;
using EventGenerator.Repository;
using Models.Entries.Repository;
using Models.Maraphone.Task.Repository;
using Models.Users;
using Models.Users.Repository;

namespace SimpleMailSender
{
    public class MailSender
    {
        private RegistrationEventInfoRepository registrationEventInfoRepository;
        private SubscribeEventInfoRepository subscribeEventInfoRepository;
        private ActivityFinishedRepository activityFinishedInfoRepository;
        private StartSprintEventRepository startSprintEventRepository;
        private UserRepository userRepository;
        private EntryRepository entryRepository;
        private ContentRepository contentRepository;

        public MailSender(UserRepository userRepository,
            RegistrationEventInfoRepository registrationEventInfoRepository,
            SubscribeEventInfoRepository subscribeEventInfoRepository,
            ActivityFinishedRepository activityFinishedInfoRepository,
            StartSprintEventRepository startSprintEventRepository, ContentRepository contentRepository, EntryRepository entryRepository)
        {
            this.userRepository = userRepository;
            this.registrationEventInfoRepository = registrationEventInfoRepository;
            this.subscribeEventInfoRepository = subscribeEventInfoRepository;
            this.activityFinishedInfoRepository = activityFinishedInfoRepository;
            this.startSprintEventRepository = startSprintEventRepository;
            this.contentRepository = contentRepository;
            this.entryRepository = entryRepository;
        }

        private static async Task SendEmailAsync(string receiverAddress, Attachment attachment, string subject,
            string body)
        {
            var sourceMail = new MailAddress("skima.mail4@gmail.com", "Skima");
            var destinyMail = new MailAddress(receiverAddress);
            var message = new MailMessage(sourceMail, destinyMail);
            if (attachment != null)
            {
                message.Attachments.Add(attachment);
            }

            message.Subject = subject;


            message.Body = body;
            message.IsBodyHtml = true;

            var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("skima.mail4@gmail.com", "backendskima");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(message);
        }

        public async void NotifyOnRegistration()
        {

            var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            string messageBody;
            using (var streamReader =
                new StreamReader($"{directoryInfo.Parent.FullName}/SimpleMailSender/html/skima_registration.html"))
            {
                messageBody = streamReader.ReadToEnd();
            }

            var users = await registrationEventInfoRepository.GetAllRegistrationEventInfo();
            foreach (var user in users)
            {
                if (!user.IsChecked)

                {
                    SendEmailAsync(user.Email, null, "Регистрация", messageBody)
                        .GetAwaiter();
                    await registrationEventInfoRepository.UpdateAsync(user);
                }
            }
        }


        public async void NotifyOnSubscribeOnEvent()
        {
            var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            string sourceHtml;
            using (var streamReader =
                new StreamReader($"{directoryInfo.Parent.FullName}/SimpleMailSender/html/subscribe.html"))
            {
                sourceHtml = streamReader.ReadToEnd();
            }

            var entries = await subscribeEventInfoRepository.GetAllSubscribeEventInfo();
            foreach (var entry in entries)
            {
                if (!entry.IsChecked)
                {
                    var bodyMessage = sourceHtml.Replace("НАЗВАНИЕ СПРИНТА", entry.Title);

                    SendEmailAsync(entry.Email, null, "Подписка", bodyMessage).GetAwaiter();
                    await subscribeEventInfoRepository.UpdateAsync(entry);
                }
            }
        }


        public async void NotifyOnActivityFinished()
        {
            var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            string sourceHtml;
            using (var streamReader =
                new StreamReader($"{directoryInfo.Parent.FullName}/SimpleMailSender/html/skima_finish.html"))
            {
                sourceHtml = streamReader.ReadToEnd();
            }

            var activityInfo = await activityFinishedInfoRepository.GetAllActivityEventInfo();
            foreach (var info in activityInfo)
            {
                if (!info.IsChecked)
                {
                    var allEntry = await entryRepository.GetAllAsync();
                    foreach (var entry in allEntry)
                    {
                        if (entry.ActivityId == info.ActivityId)
                        {
                            var bodyMessage = sourceHtml.Replace("НАЗВАНИЕ МАРАФОНА", info.Title);
                            SendEmailAsync(userRepository.GetByIdAsync(entry.UserId).Result.Email, null,
                                "Завершение активности", bodyMessage).GetAwaiter();
                        }
                    }
                    await activityFinishedInfoRepository.UpdateAsync(info);
                }
            }
        }


        public async void NotifyOnStartSprintEvent()
        {
            var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            string sourceHtml;
            using (var streamReader =
                new StreamReader($"{directoryInfo.Parent.FullName}/SimpleMailSender/html/skima_task.html"))
            {
                sourceHtml = streamReader.ReadToEnd();
            }

            var sprints = await startSprintEventRepository.GetAllSubscribeEventInfo();
            foreach (var sprint in sprints)
            {
                if (!sprint.IsChecked)
                {
                    var allEntry = await entryRepository.GetAllAsync();
                    foreach (var entry in allEntry)
                    {
                        if (entry.ActivityId == sprint.ActivityId)
                        {
                            var tasks = sprint.Tasks;
                            foreach (var task in tasks)
                            {

                                Attachment attachment = null;
                                var content = contentRepository.GetAsync(task.ContentId).Result;
                                var messageBody = sourceHtml.Replace("Заголовок задания", task.Title);
                                if (content.Type != "text")
                                {
                                    messageBody = messageBody.Replace("ОПИСАНИЕ", "");
                                    var memoryStream = new MemoryStream();
                                    await memoryStream.WriteAsync(content.Data, 0, content.Data.Length);
                                    attachment = new Attachment(memoryStream, MediaTypeNames.Application.Octet);

                                    SendEmailAsync(userRepository.GetByIdAsync(entry.UserId).Result.Email, attachment,
                                        "Начало Спринта",
                                        messageBody
                                    ).GetAwaiter();
                                }
                                else
                                {
                                    messageBody = messageBody.Replace("ОПИСАНИЕ",
                                        Encoding.UTF8.GetString(content.Data));
                                    SendEmailAsync(userRepository.GetByIdAsync(entry.UserId).Result.Email, attachment,
                                        "Начало Спринта",
                                        messageBody
                                    ).GetAwaiter();
                                }
                            }
                        }
                    }
                    await startSprintEventRepository.UpdateAsync(sprint);
                }
            }
        }
    }
}