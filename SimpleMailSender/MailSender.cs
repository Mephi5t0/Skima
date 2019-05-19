using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
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

        private static async Task SendEmailAsync(string receiverAddress, string subject, string description)
        {
            var from = new MailAddress("skima.mail4@gmail.com", "Skima");
            var to = new MailAddress(receiverAddress);
            var m = new MailMessage(from, to);
            m.Subject = subject;
            m.Body = description;
            var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("skima.mail4@gmail.com", "backendskima");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }

        public async void NotifyOnRegistration()
        {
            var users = await registrationEventInfoRepository.GetAllRegistrationEventInfo();
            foreach (var user in users)
            {
                if (!user.IsChecked)
                {
                    SendEmailAsync(user.Email, "Регистрация", "Пользователь успешно прошёл регистрацию").GetAwaiter();
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
                    SendEmailAsync(entry.Email, "Подписка", entry.Description).GetAwaiter();
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
                            SendEmailAsync(userRepository.GetByIdAsync(entry.UserId).Result.Email,
                                "Завершение активности", info.Description).GetAwaiter();
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
                                var content = contentRepository.GetAsync(task.ContentId).Result.Data;
                                SendEmailAsync(userRepository.GetByIdAsync(entry.UserId).Result.Email, "Начало Спринта",
                                    Encoding.UTF8.GetString(content)
                                ).GetAwaiter();
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