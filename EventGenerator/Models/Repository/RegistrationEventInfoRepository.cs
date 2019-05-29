using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using MongoDB.Driver;

namespace EventGenerator.Models.Repository
{
    public class RegistrationEventInfoRepository
    {
        private readonly IMongoCollection<RegistrationEventInfo> registrationEventInfo;

        public RegistrationEventInfoRepository(Configuration config)
        {
            var client = new MongoClient(config.GetConnectionString("SkimaDb"));
            var database = client.GetDatabase("SkimaDb");
            registrationEventInfo = database.GetCollection<RegistrationEventInfo>("RegistrationEventInfo");
        }

        public Task<List<RegistrationEventInfo>> GetAllRegistrationEventInfo()
        {
            var search = registrationEventInfo.Find(info => true);
            var result = search.ToList();
            return Task.FromResult(result);
        }

        public Task<RegistrationEventInfo> CreateRegistrationEventInfoAsync(
            RegistrationEventInfo newRegistrationEventInfo)
        {
            if (newRegistrationEventInfo == null)
            {
                throw new ArgumentNullException();
            }

            registrationEventInfo.InsertOne(newRegistrationEventInfo);
            return Task.FromResult(newRegistrationEventInfo);
        }

        public Task UpdateAsync(RegistrationEventInfo user)
        {
            var newRegestrationEventInfo = new RegistrationEventInfo()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RegisteredAt = user.RegisteredAt,
                IsChecked = true
            };
            this.registrationEventInfo.ReplaceOne(info => info.Id == user.Id, newRegestrationEventInfo);

            return Task.CompletedTask;
        }
    }
}