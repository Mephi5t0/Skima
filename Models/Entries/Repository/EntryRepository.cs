using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Models.Entries.Repository
{
    public class EntryRepository
    {
        private readonly IMongoCollection<Entry> entries;

        public EntryRepository(Configuration config)
        {
            var client = new MongoClient(config.GetConnectionString("SkimaDb"));
            var database = client.GetDatabase("SkimaDb");
            entries = database.GetCollection<Entry>("Entries");
        }
        
        public Task<Entry> CreateAsync(EntryCreationInfo entryCreationInfo, CancellationToken cancellationToken)
        {
            if (entryCreationInfo == null)
            {
                throw new ArgumentNullException(nameof(entryCreationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var countSameEntries = entries.Find(ent => ent.UserId == entryCreationInfo.UserId && ent.ActivityId == entryCreationInfo.ActivityId).CountDocuments();

            if (countSameEntries > 0)
            {
                throw new EntryDuplicationException(entryCreationInfo.UserId, entryCreationInfo.ActivityId);
            }

            var entry = new Entry
            {
                Status = entryCreationInfo.Status,
                UserId = entryCreationInfo.UserId,
                CreatedAt = DateTime.Now,
                ActivityId = entryCreationInfo.ActivityId
            };
            
            entries.InsertOne(entry, cancellationToken: cancellationToken);

            return Task.FromResult(entry);
        }

        public Task<Entry> GetAsync(string id)
        {
            var result = entries.Find(entry => entry.Id == id).FirstOrDefault();
            
            return Task.FromResult(result);
        }
        
        public Task<List<Entry>> GetAllAsync()
        {
            var result = entries.Find(x => true).ToList();
            
            return Task.FromResult(result);
        }

        public Task ReplaceOne(string id, Entry entryIn)
        {
            entries.ReplaceOne(entry => entry.Id == id, entryIn);

            return Task.CompletedTask;
        }
    }
}