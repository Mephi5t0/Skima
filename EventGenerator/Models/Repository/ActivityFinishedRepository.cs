using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using MongoDB.Driver;

namespace EventGenerator.Models.Repository
{
    public class ActivityFinishedRepository
    {
        private readonly IMongoCollection<ActivityFinishedInfo> activityFinishedInfo;

        public ActivityFinishedRepository(Configuration config)
        {
            var client = new MongoClient(config.GetConnectionString("SkimaDb"));
            var database = client.GetDatabase("SkimaDb");
            activityFinishedInfo = database.GetCollection<ActivityFinishedInfo>("ActivityFinishedInfo");
        }
        
        
        public Task<List<ActivityFinishedInfo>> GetAllActivityEventInfo()
        {
            var search = activityFinishedInfo.Find(info => true);
            var result = search.ToList();
            return Task.FromResult(result);
        }
        
        public Task<ActivityFinishedInfo> CreateActivityFinishedInfoAsync(ActivityFinishedInfo newActivityFinishedInfo)
        {
            if (newActivityFinishedInfo == null)
            {
                throw new ArgumentNullException();
            }
            
            activityFinishedInfo.InsertOne(newActivityFinishedInfo);
            return Task.FromResult(newActivityFinishedInfo);
        }
        
        public Task UpdateAsync(ActivityFinishedInfo info)
        {
            var newActivityFinishedInfo = new ActivityFinishedInfo()
            {
                Id = info.Id,
                ActivityId = info.ActivityId,
                Title = info.Title,
                Description = info.Description,
                IsChecked = true
            };
            
            
            activityFinishedInfo.ReplaceOne(user => user.Id == info.Id, newActivityFinishedInfo);

            return Task.CompletedTask;
        }
    }
}