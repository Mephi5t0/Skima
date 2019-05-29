using System;
using System.Threading.Tasks;
using Models;
using MongoDB.Driver;

namespace EventGenerator.Repository
{
    public class SettingsRepository
    {
        private readonly IMongoCollection<SettingsEventOfRegistration> settingsEventOfRegistration;

        private readonly IMongoCollection<SettingsEventOfSubscribeToActivity> settigsEventOfSubscribeToActivity;

        private readonly IMongoCollection<SettingsEventOfActivity> settingsEventOfActivity;

        private readonly IMongoCollection<SettingsEventOfStartSprint> settingsEventOfStartSprint;


        public SettingsRepository(Configuration config)
        {
            var client = new MongoClient(config.GetConnectionString("SkimaDb"));
            var database = client.GetDatabase("SkimaDb");
            settingsEventOfRegistration = database.GetCollection<SettingsEventOfRegistration>("SettingsEventOfRegistration");
            settigsEventOfSubscribeToActivity =
                database.GetCollection<SettingsEventOfSubscribeToActivity>("SettigsEventOfSubscribeToActivity");
            settingsEventOfActivity = database.GetCollection<SettingsEventOfActivity>("SettingsEventOfActivity");
            settingsEventOfStartSprint = database.GetCollection<SettingsEventOfStartSprint>("SettingsEventOfStartSprint");
        }

        public Task<SettingsEventOfRegistration> GetLastRegistrationSettings()
        {
            var lastUpdate = settingsEventOfRegistration.Find(setting => true).SortByDescending(x => x.DateOfLastNotificationUser).FirstOrDefault();
            return Task.FromResult(lastUpdate);
        }

        public Task<SettingsEventOfRegistration> CreateRegistrationEventSettingsAsync(SettingsEventOfRegistration settingEvent)
        {
            if (settingEvent == null)
            {
                throw new ArgumentNullException();
            }
            
            settingsEventOfRegistration.InsertOne(settingEvent);
            return Task.FromResult(settingEvent);
        }

        public Task<SettingsEventOfSubscribeToActivity> GetLastSubscribeSettings()
        {
            var lastUpdate = settigsEventOfSubscribeToActivity.Find(settings => true).SortByDescending(x => x.CreatedAt)
                .FirstOrDefault();
            return Task.FromResult(lastUpdate);
        }

        public Task<SettingsEventOfSubscribeToActivity> CreateSubscribeToActivitySettings(SettingsEventOfSubscribeToActivity settingEvent)
        {
            if (settingEvent == null)
            {
                throw new ArgumentNullException();
            }
            
            settigsEventOfSubscribeToActivity.InsertOne(settingEvent);
            return Task.FromResult<SettingsEventOfSubscribeToActivity>(settingEvent);
        }

        public Task<SettingsEventOfActivity> GetLastActivitySettings()
        {
            var lastUpdate = settingsEventOfActivity.Find(setting => true)
                .SortByDescending(x => x.DateOfLastCheckedActivity).FirstOrDefault();
            return Task.FromResult(lastUpdate);
        }

        public Task<SettingsEventOfActivity> CreateLastActivitySettings(SettingsEventOfActivity setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException();
            }

            settingsEventOfActivity.InsertOne(setting);
            return Task.FromResult(setting);
        }

        public Task<SettingsEventOfStartSprint> GetLastStartSprintSettings()
        {
            var lastUpdate = settingsEventOfStartSprint.Find(setting => true).SortByDescending(x=>x.DateOfLastCheckedSprint).FirstOrDefault();
            return Task.FromResult(lastUpdate);
        }

        public Task<SettingsEventOfStartSprint> CreateLastStartSprintSettings(SettingsEventOfStartSprint setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException();
            }
            
            settingsEventOfStartSprint.InsertOne(setting);
            return Task.FromResult(setting);
        }


    }
}