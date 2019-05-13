using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventGenerator.Models;
using Models;
using MongoDB.Driver;

namespace EventGenerator.Repository
{
    public class SubscribeEventInfoRepository
    {
        private IMongoCollection<SubscribeEventInfo> subscribeEventInfo;


        public SubscribeEventInfoRepository(Configuration config)
        {
            var client = new MongoClient(config.GetConnectionString("SkimaDb"));
            var database = client.GetDatabase("SkimaDb");
            subscribeEventInfo = database.GetCollection<SubscribeEventInfo>("SubscribeEventInfo");
        }

        public Task<List<SubscribeEventInfo>> GetAllSubscribeEventInfo()
        {
            var search = subscribeEventInfo.Find(info => true);
            var result = search.ToList();
            return Task.FromResult(result);
        }


        public Task<SubscribeEventInfo> CreateSubscribeEventInfoAsync(SubscribeEventInfo newSubscribeEventInfoEventInfo)
        {
            if (newSubscribeEventInfoEventInfo == null)
            {
                throw new ArgumentNullException();
            }

            subscribeEventInfo.InsertOne(newSubscribeEventInfoEventInfo);
            return Task.FromResult(newSubscribeEventInfoEventInfo);
        }
        
        public Task UpdateAsync(string id, SubscribeEventInfo subscribeEventInfo)
        {
            this.subscribeEventInfo.ReplaceOne(info => info.Id == id, subscribeEventInfo);

            return Task.CompletedTask;
        }
    }
}