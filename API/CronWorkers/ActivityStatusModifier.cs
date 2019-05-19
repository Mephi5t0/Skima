using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Models.Activity.Repository;

namespace API.CronWorkers
{
    internal class ActivityStatusModifier : IHostedService, IDisposable
    {
        private const long PeriodInMinutes = 10;
        private readonly ActivityRepository activityRepository;
        private Timer timer;

        public ActivityStatusModifier(ActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromMinutes(PeriodInMinutes));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            activityRepository.ChangeActivitiesStatus();
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