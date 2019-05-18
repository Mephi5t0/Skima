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
            StartSprintEventRepository startSprintEventRepository, ContentRepository contentRepository)
        {
            this.userRepository = userRepository;
            this.registrationEventInfoRepository = registrationEventInfoRepository;
            this.subscribeEventInfoRepository = subscribeEventInfoRepository;
            this.activityFinishedInfoRepository = activityFinishedInfoRepository;
            this.startSprintEventRepository = startSprintEventRepository;
            this.contentRepository = contentRepository;
        }

        private static async Task SendEmailAsync(string receiverAddress, Attachment attachment, string subject,
            string fileName)
        {
            var sourceMail = new MailAddress("skima.mail4@gmail.com", "Skima");
            var destinyMail = new MailAddress(receiverAddress);
            var message = new MailMessage(sourceMail, destinyMail);
            if (attachment != null)
            {
                message.Attachments.Add(attachment);
            }

            var bodyContent = File.ReadAllText($"html/{fileName}");
            message.Subject = subject;
            message.Body = bodyContent;
            message.IsBodyHtml = true;
            
            var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("skima.mail4@gmail.com", "backendskima");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(message);
        }

        public async void NotifyOnRegistration()
        {
            var users = await registrationEventInfoRepository.GetAllRegistrationEventInfo();
            foreach (var user in users)
            {
                if (!user.IsChecked)
                {
                    SendEmailAsync(user.Email, null, "Регистрация", "skima_registration.html")
                        .GetAwaiter();
                    var regestrationEventInfo = new RegistrationEventInfo()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        RegisteredAt = user.RegisteredAt,
                        IsChecked = true
                    };
                    await registrationEventInfoRepository.UpdateAsync(user.Id, regestrationEventInfo);
                }
            }
        }


        public async void NotifyOnSubscribeOnEvent()
        {
            var entries = await subscribeEventInfoRepository.GetAllSubscribeEventInfo();
            foreach (var entry in entries)
            {
                if (!entry.IsChecked)
                {
                    SendEmailAsync(entry.Email, null, "Подписка", "subscribe.html").GetAwaiter();
                    var subscribeEventInfo = new SubscribeEventInfo()
                    {
                        FirstName = entry.FirstName,
                        LastName = entry.LastName,
                        Email = entry.Email,
                        Title = entry.Title,
                        Description = entry.Description,
                        CreatedAt = entry.CreatedAt,
                        IsChecked = false
                    };
                    await subscribeEventInfoRepository.UpdateAsync(entry.Id, subscribeEventInfo);
                }
            }
        }

        public async void NotifyOnActivityFinished()
        {
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
                            SendEmailAsync(userRepository.GetByIdAsync(entry.UserId).Result.Email, null,
                                "Завершение активности", "skima_finish.html").GetAwaiter();
                        }
                    }

                    var newActivityFinishedInfo = new ActivityFinishedInfo()
                    {
                        ActivityId = info.ActivityId,
                        Title = info.Title,
                        Description = info.Description,
                        IsChecked = true
                    };
                    await activityFinishedInfoRepository.UpdateAsync(info.Id, newActivityFinishedInfo);
                }
            }
        }

        public async void NotifyOnStartSprintEvent()
        {
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
                                if (content.Type != "text")
                                {
                                    var memoryStream = new MemoryStream();
                                    await memoryStream.WriteAsync(content.Data, 0, content.Data.Length);
                                    attachment = new Attachment(memoryStream, MediaTypeNames.Application.Octet);

                                    SendEmailAsync(userRepository.GetByIdAsync(entry.UserId).Result.Email, attachment,
                                        "Начало Спринта",
                                        "skima_start.html"
                                    ).GetAwaiter();
                                }
                                else
                                {
                                    SendEmailAsync(userRepository.GetByIdAsync(entry.UserId).Result.Email, attachment,
                                        "Начало Спринта",
                                        Encoding.UTF8.GetString(content.Data)
                                    ).GetAwaiter();
                                }
                            }
                        }
                    }

                    var newSprintInfo = new StartSprintEventInfo()
                    {
                        Number = sprint.Number,
                        ActivityId = sprint.ActivityId,
                        StartAt = sprint.StartAt,
                        Description = sprint.Description,
                        Tasks = sprint.Tasks,
                        CreatedAt = sprint.CreatedAt,
                        Duration = sprint.Duration,
                        IsChecked = true
                    };
                    await startSprintEventRepository.UpdateAsync(sprint.Id, newSprintInfo);
                }
            }
        }
    }
}