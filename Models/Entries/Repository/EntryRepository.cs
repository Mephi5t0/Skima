using System;
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
        
        public Task<Entry> CreateAsync(Entry entryInfo, CancellationToken cancellationToken)
        {
            if (entryInfo == null)
            {
                throw new ArgumentNullException(nameof(entryInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var countSameEntries = entries.Find(ent => ent.UserId == entryInfo.UserId && ent.ActivityId == entryInfo.ActivityId).CountDocuments();

            if (countSameEntries > 0)
            {
                throw new EntryDuplicationException(entryInfo.UserId, entryInfo.ActivityId);
            }

            var entry = new Entry
            {
                Status = entryInfo.Status,
                UserId = entryInfo.UserId,
                CreatedAt = DateTime.Now,
                ActivityId = entryInfo.ActivityId
            };
            
            entries.InsertOne(entry, cancellationToken: cancellationToken);

            return Task.FromResult(entry);
        }

        public Task<Entry> GetAsync(string id)
        {
            var result = entries.Find(entry => entry.Id == id).FirstOrDefault();
            
            return Task.FromResult(result);
        }
    }
}