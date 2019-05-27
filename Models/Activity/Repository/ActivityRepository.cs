using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client.Models.Activity;
using MongoDB.Driver;

namespace Models.Activity.Repository
{
    public class ActivityRepository
    {
        private readonly IMongoCollection<Activity> activities;

        public ActivityRepository(Configuration config)
        {
            var client = new MongoClient(config.GetConnectionString("SkimaDb"));
            var database = client.GetDatabase("SkimaDb");
            activities = database.GetCollection<Activity>("Activities");
        }

        public Task<List<Activity>> GetAsync()
        {
            var search = activities.Find(activity => true);
            var result = search.ToList();

            return Task.FromResult(result);
        }

        public Task<Activity> GetByIdAsync(string id)
        {
            var result = activities.Find(activity => activity.Id == id).FirstOrDefault();
            
            return Task.FromResult(result);
        }

        public Task<List<Activity>> SearchAsync(ActivityInfoSearchQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var search = activities.AsQueryable().ToEnumerable();
            
            if (query.Tags != null)
            {
                search = search.Where(activity => activity.Tags == query.Tags);
            }

            if (query.Experts != null)
            {
                search = search.Where(activity => activity.Experts == query.Experts);
            }
            
            if (query.CreatedBy != null)
            {
                search = search.Where(activity => activity.CreatedBy == query.CreatedBy);
            }

            if (query.MaraphoneId != null)
            {
                search = search.Where(activity => activity.MaraphoneId == query.MaraphoneId);
            }

            if (query.StartAt != null)
            {
                search = search.Where(activity => activity.StartAt == query.StartAt.Value);
            }

            if (query.CreatedAt!= null)
            {
                search = search.Where(activity => activity.CreatedAt == query.CreatedAt.Value);
            }

            if (query.Offset != null)
            {
                search = search.Skip(query.Offset.Value);
            }

            if (query.Limit != null)
            {
                search = search.Take(query.Limit.Value);
            }

            var result = search.ToList();

            return Task.FromResult(result);
        }
        
        public Task<Activity> CreateAsync(ActivityCreationInfo activityCreationInfo, DateTime endAt, CancellationToken cancellationToken)
        {
            if (activityCreationInfo == null)
            {
                throw new ArgumentNullException(nameof(activityCreationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var activity = new Activity
            {
                Status = Status.New,
                CreatedAt = DateTime.Now,
                Tags = activityCreationInfo.Tags,
                Experts = activityCreationInfo.Experts,
                StartAt = activityCreationInfo.StartAt,
                MaraphoneId = activityCreationInfo.MaraphoneId,
                CreatedBy = activityCreationInfo.CreatedBy,
                EndAt = endAt
            };
            
            activities.InsertOne(activity, cancellationToken: cancellationToken);

            return Task.FromResult(activity);
        }
        
        public Task RemoveActivityAsync(string id)
        {
            activities.DeleteOne(activity => activity.Id == id);

            return Task.CompletedTask;
        }

        public void ChangeActivitiesStatus()
        {
            var allActivities = this.activities.Find(activity => true).ToList();
            
            foreach (var activity in allActivities)
            {
                if (activity.Status == Status.Canceled || activity.Status == Status.Announced)
                {
                    continue;
                }
                if (activity.StartAt <= DateTime.Now && activity.EndAt > DateTime.Now && activity.Status == Status.New)
                {
                    activity.Status = Status.Running;
                    activities.ReplaceOne(x => x.Id == activity.Id, activity);
                }
                else if (activity.EndAt <= DateTime.Now && activity.Status == Status.Running)
                {
                    activity.Status = Status.Finished;
                    activities.ReplaceOne(x => x.Id == activity.Id, activity);
                }
            }
        }
    }
}