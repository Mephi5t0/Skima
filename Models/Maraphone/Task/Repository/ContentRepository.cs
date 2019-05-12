using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Models.Maraphone.Task.Repository
{
    public class ContentRepository
    {
        private readonly IMongoCollection<Content> content;

        public ContentRepository(Configuration config)
        {
            var client = new MongoClient(config.GetConnectionString("SkimaDb"));
            var database = client.GetDatabase("SkimaDb");
            content = database.GetCollection<Content>("Content");
        }
    
        public Task<Content> CreateAsync(ContentCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var document = new Content
            {
                Data = creationInfo.Data,
                Type = creationInfo.Type
            };

            content.InsertOne(document, cancellationToken: cancellationToken);

            return System.Threading.Tasks.Task.FromResult(document);
        }

        public Task<Content> GetAsync(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var result = content.Find(document => document.Id == id).FirstOrDefault();
            
            return System.Threading.Tasks.Task.FromResult(result);
        }
    }
}