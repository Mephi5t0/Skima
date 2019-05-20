using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventGenerator.Models;
using Models;
using MongoDB.Driver;

namespace EventGenerator.Repository
{
    public class StartSprintEventRepository
    {
        private IMongoCollection<StartSprintEventInfo> startSprintEventInfo;

        public StartSprintEventRepository(Configuration config)
        {
            var client = new MongoClient(config.GetConnectionString("SkimaDb"));
            var database = client.GetDatabase("SkimaDb");
            startSprintEventInfo = database.GetCollection<StartSprintEventInfo>("StartSprintEventInfo");
        }

        public Task<List<StartSprintEventInfo>> GetAllSubscribeEventInfo()
        {
            var search = startSprintEventInfo.Find(info => true);
            var result = search.ToList();
            return Task.FromResult(result);
        }

        public Task<StartSprintEventInfo> CreateStartSprintEventInfo(StartSprintEventInfo sprintEventInfo)
        {
            if (sprintEventInfo == null)
            {
                throw new ArgumentNullException();
            }

            startSprintEventInfo.InsertOne(sprintEventInfo);
            return Task.FromResult(sprintEventInfo);
        }


        public Task UpdateAsync(StartSprintEventInfo sprint)
        {
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


            this.startSprintEventInfo.ReplaceOne(info => info.Id == sprint.Id, newSprintInfo);

            return Task.CompletedTask;
        }
    }
}