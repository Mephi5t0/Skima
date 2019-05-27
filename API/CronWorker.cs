using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SimpleMailSender;

namespace API
{
    internal class CronWorker : IHostedService, IDisposable
    {
        private const long PeriodInSeconds = 30;
        private MailSender mailSender;
        private EventGenerator.EventGenerator eventGenerator;
        private Timer timer;

        public CronWorker(MailSender mailSender, EventGenerator.EventGenerator eventGenerator)
        {
            this.eventGenerator = eventGenerator;
            this.mailSender = mailSender;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(PeriodInSeconds));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            eventGenerator.GenerateEventForUnRegistrationUser();
            eventGenerator.GenerateEventOfSubscription();
            eventGenerator.GenerateEventOfEndActivityAsync();
            eventGenerator.GenerateEventOfStartSprintAsync();
            mailSender.NotifyOnRegistration();
            mailSender.NotifyOnSubscribeOnEvent();
            mailSender.NotifyOnStartSprintEvent();
            mailSender.NotifyOnActivityFinished();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}