using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using MongoDB.Driver;

namespace EventGenerator.Models.Repository
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
        
        public Task UpdateAsync(SubscribeEventInfo entry)
        {
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
            
            this.subscribeEventInfo.ReplaceOne(info => info.Id ==entry.Id, subscribeEventInfo);

            return Task.CompletedTask;
        }
    }
}