using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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


        public Task UpdateAsync(string id, StartSprintEventInfo startSprintEventInfo)
        {
            this.startSprintEventInfo.ReplaceOne(info => info.Id == id, startSprintEventInfo);

            return Task.CompletedTask;
        }
    }
}