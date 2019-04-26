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
        private Timer timer;

        public CronWorker(MailSender mailSender)
        {
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
            mailSender.NotifyOnRegistration();
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