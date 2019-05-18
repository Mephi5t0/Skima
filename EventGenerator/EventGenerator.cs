using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Models.Maraphone;
using EventGenerator.Models;
using EventGenerator.Repository;
using Models.Activity;
using Models.Activity.Repository;
using Models.Entries.Repository;
using Models.Maraphone.Repository;
using Models.Users.Repository;
using Sprint = Models.Maraphone.Sprint;

namespace EventGenerator
{
    public class EventGenerator
    {
        private UserRepository userRepository;
        private EntryRepository entryRepository;
        private ActivityRepository activityRepository;
        private MaraphoneRepository maraphoneRepository;

        private SettingsRepository settingsRepository;

        private RegistrationEventInfoRepository registrationEventInfoRepository;
        private SubscribeEventInfoRepository subscribeEventInfoRepository;
        private ActivityFinishedRepository activityFinishedInfoRepository;
        private StartSprintEventRepository startSprintEventRepository;

        public EventGenerator(UserRepository userRepository, EntryRepository entryRepository,
            SettingsRepository settingsRepository,
            RegistrationEventInfoRepository registrationEventInfoRepository,
            SubscribeEventInfoRepository subscribeEventInfoRepository,
            ActivityFinishedRepository activityFinishedInfoRepository,
            StartSprintEventRepository startSprintEventRepository)
        {
            this.userRepository = userRepository;
            this.entryRepository = entryRepository;
            this.settingsRepository = settingsRepository;
            this.registrationEventInfoRepository = registrationEventInfoRepository;
            this.subscribeEventInfoRepository = subscribeEventInfoRepository;
            this.activityFinishedInfoRepository = activityFinishedInfoRepository;
            this.startSprintEventRepository = startSprintEventRepository;
        }


        public async void GenerateEventForUnRegistrationUser()
        {
            var dateOfLastCheckedRegistrationUser = settingsRepository.GetLastRegistrationSettings();
            var users = await userRepository.GetAllAsync();
            foreach (var user in users)
            {
                if (user.RegisteredAt.CompareTo(dateOfLastCheckedRegistrationUser) > 0)
                {
                    var registrationEventInfo = new RegistrationEventInfo()
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        RegisteredAt = user.RegisteredAt,
                        IsChecked = false
                    };
                    await registrationEventInfoRepository.CreateRegistrationEventInfoAsync(registrationEventInfo);

                    var settingOfEventsRegistration = new SettingsEventOfRegistration()
                    {
                        DateOfLastNotificationUser = user.RegisteredAt
                    };
                    await settingsRepository.CreateRegistrationEventSettingsAsync(settingOfEventsRegistration);
                }
            }
        }


        public async void GenerateEventOfSubscription()
        {
            var dateOfLastCheckedEntry = settingsRepository.GetLastSubscribeSettings();
            var entries = await entryRepository.GetAllAsync();
            foreach (var entry in entries)
            {
                if (entry.CreatedAt.CompareTo(dateOfLastCheckedEntry) > 0)
                {
                    var userByEntry = await userRepository.GetByIdAsync(entry.UserId);
                    var activity = await activityRepository.GetByIdAsync(entry.ActivityId);
                    var maraphone = await maraphoneRepository.GetAsync(activity.MaraphoneId, cancellationToken: CancellationToken.None);
                    var subscribeEventInfo = new SubscribeEventInfo()
                    {
                        FirstName = userByEntry.FirstName,
                        LastName = userByEntry.LastName,
                        Email = userByEntry.Email,
                        CreatedAt = entry.CreatedAt,
                        Title = maraphone.Title,
                        Description = maraphone.Description,
                        IsChecked = false
                    };
                    await subscribeEventInfoRepository.CreateSubscribeEventInfoAsync(subscribeEventInfo);

                    var settingOfEventSubscribe = new SettingsEventOfSubscribeToActivity()
                    {
                        CreatedAt = entry.CreatedAt
                    };
                    await settingsRepository.CreateSubscribeToActivitySettings(settingOfEventSubscribe);
                }
            }
        }

        public async void GenerateEventOfEndActivityAsync()
        {
            var dateOfLastCheckedActivity = settingsRepository.GetLastActivitySettings();
            var activities = await activityRepository.GetAsync();
            foreach (var activity in activities)
            {
                if (activity.EndAt.CompareTo(dateOfLastCheckedActivity) > 0 &&
                    activity.EndAt.CompareTo(DateTime.Now) > 0)
                {
                    var maraphoneByActivity =
                        await maraphoneRepository.GetAsync(activity.MaraphoneId, CancellationToken.None);
                    var activityFinishedInfo = new ActivityFinishedInfo()
                    {
                        ActivityId = activity.Id,
                        Title = maraphoneByActivity.Title,
                        Description = maraphoneByActivity.Description,
                        IsChecked = false
                    };
                    await activityFinishedInfoRepository.CreateActivityFinishedInfoAsync(activityFinishedInfo);
                    var settingOfActivityFinishedInfo = new SettingsEventOfActivity()
                    {
                        DateOfLastCheckedActivity = activity.EndAt
                    };
                    await settingsRepository.CreateLastActivitySettings(settingOfActivityFinishedInfo);
                }
            }
        }

        public async void GenerateEventOfStartSprintAsync()
        {
            var dateOfLastChecked = await settingsRepository.GetLastStartSprintSettings();
            var activities = await activityRepository.GetAsync();
            foreach (var activity in activities)
            {
                var allSprintsByActivity = await GetAllSprintsByActivity(activity);
                var numberOfSprint = await GetNumberOfSprintAsync(activity, dateOfLastChecked.DateOfLastCheckedSprint);
                if (numberOfSprint == -1)
                {
                    continue;
                }

                var timeStartOfSprint = (numberOfSprint == 0)
                    ? activity.StartAt
                    : activity.StartAt.Add(allSprintsByActivity[numberOfSprint-1].Duration);
                
                var startSprintEventInfo = new StartSprintEventInfo()
                {
                    Number = numberOfSprint,
                    StartAt = timeStartOfSprint,
                    Description = allSprintsByActivity[numberOfSprint].Description,
                    Tasks = allSprintsByActivity[numberOfSprint].Tasks,
                    CreatedAt = allSprintsByActivity[numberOfSprint].CreatedAt,
                    Duration = allSprintsByActivity[numberOfSprint].Duration,
                    IsChecked = false
                };
                await startSprintEventRepository.CreateStartSprintEventInfo(startSprintEventInfo);
                var settingOfStartSprintInfo = new SettingsEventOfStartSprint()
                {
                    DateOfLastCheckedSprint = timeStartOfSprint
                };
                await settingsRepository.CreateLastStartSprintSettings(settingOfStartSprintInfo);
            }
        }

        private async Task<int> GetNumberOfSprintAsync(Activity activity, DateTime dateOfLastChecked)
        {
            var maraphoneByActivity = await maraphoneRepository.GetAsync(activity.MaraphoneId, CancellationToken.None);
            var sprints = maraphoneByActivity.Sprints;
            if (activity.StartAt.CompareTo(DateTime.Now) > 0 && activity.StartAt.CompareTo(dateOfLastChecked) > 0)
            {
                return await Task.FromResult(0);
            }

            for (var i = 1; i < sprints.Length; i++)
            {
                if (activity.StartAt.Add(sprints[i - 1].Duration).CompareTo(DateTime.Now) > 0 &&
                    activity.StartAt.Add(sprints[i - 1].Duration).CompareTo(dateOfLastChecked) > 0)
                {
                    return await Task.FromResult(i);
                }
            }

            return await Task.FromResult(-1);
        }

        private async Task<Sprint[]> GetAllSprintsByActivity(Activity activity)
        {
            
            var maraphoneByActivity = await maraphoneRepository.GetAsync(activity.MaraphoneId, CancellationToken.None);
            var sprints = maraphoneByActivity.Sprints;
            return sprints;

        }
    }
}