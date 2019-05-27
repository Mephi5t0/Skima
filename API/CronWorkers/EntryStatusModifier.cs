using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Models.Activity.Repository;
using Models.Entries;
using Models.Entries.Repository;

namespace API.CronWorkers
{
    internal class EntryStatusModifier : IHostedService, IDisposable
    {
        private const long PeriodInMinutes = 10;
        private readonly EntryRepository entryRepository;
        private readonly ActivityRepository activityRepository;
        private Timer timer;

        public EntryStatusModifier(EntryRepository entryRepository, ActivityRepository activityRepository)
        {
            this.entryRepository = entryRepository;
            this.activityRepository = activityRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromMinutes(PeriodInMinutes));

            return Task.CompletedTask;
        }

        private  void DoWork(object state)
        {
            var allEntries = entryRepository.GetAllAsync().Result;

            foreach (var entry in allEntries)
            {
                var activity = activityRepository.GetByIdAsync(entry.Id).Result;

                if (activity != null && activity.EndAt <= DateTime.Now)
                {
                    entry.Status = Status.Finished;
                    entryRepository.ReplaceOne(entry.Id, entry);
                }
            }
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