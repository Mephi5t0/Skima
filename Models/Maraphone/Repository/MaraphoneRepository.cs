using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client.Models.Maraphone;
using MongoDB.Driver;

namespace Models.Maraphone.Repository
{
    public class MaraphoneRepository
    {
        private readonly IMongoCollection<Maraphone> maraphones;

        public MaraphoneRepository(Configuration config)
        {
            var client = new MongoClient(config.GetConnectionString("SkimaDb"));
            var database = client.GetDatabase("SkimaDb");
            maraphones = database.GetCollection<Maraphone>("Maraphones");
        }

        public Task<Maraphone> CreateAsync(MaraphoneCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var maraphone = new Maraphone
            {
                Title = creationInfo.Title,
                Sprints = creationInfo.Sprints,
                Duration = creationInfo.Duration,
                Description = creationInfo.Description,
                CreatedAt = DateTime.Now,
                CreatedBy = creationInfo.CreatedBy
            };

            maraphones.InsertOne(maraphone, cancellationToken: cancellationToken);

            return System.Threading.Tasks.Task.FromResult(maraphone);
        }

        public Task<List<Maraphone>> SearchAsync(MaraphoneInfoSearchQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var search = maraphones.AsQueryable().ToEnumerable();
            
            if (query.Offset != null)
            {
                search = search.Skip(query.Offset.Value);
            }

            if (query.Limit != null)
            {
                search = search.Take(query.Limit.Value);
            }

            var result = search.ToList();

            return System.Threading.Tasks.Task.FromResult(result);
        }
        
        public Task<Maraphone> GetAsync(string id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                throw new ArgumentNullException(id);
            }

            cancellationToken.ThrowIfCancellationRequested();

            var result = maraphones.Find(maraphone => maraphone.Id == id).FirstOrDefault();

            return System.Threading.Tasks.Task.FromResult(result);
        }
    }
}