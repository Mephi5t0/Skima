using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Client.Models.Users;
using MongoDB.Driver;

namespace Models.Users.Repository
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> users;

        public UserRepository(Configuration config)
        {
            var client = new MongoClient(config.GetConnectionString("SkimaDb"));
            var database = client.GetDatabase("SkimaDb");
            users = database.GetCollection<User>("Users");
        }

        public Task<List<User>> GetAllAsync()
        {
            var result = users.Find(user => true).ToList();

            return Task.FromResult(result);
        }

        public Task<User> GetByIdAsync(string id)
        {
            var result = users.Find(user => user.Id == id).FirstOrDefault();
            
            return Task.FromResult(result);
        }
        
        public Task<User> GetByEMailAsync(string email)
        {
            var result = users.Find(user => user.Email == email).FirstOrDefault();

            return Task.FromResult(result);
        }

        public Task<User> CreateAsync(UserCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var countUsersWithSameLogin = users.Find(usr => usr.Email == creationInfo.Email).CountDocuments();

            if (countUsersWithSameLogin > 0)
            {
                throw new UserDuplicationException(creationInfo.Email);
            }

            var user = new User
            {
                PasswordHash = creationInfo.PasswordHash,
                RegisteredAt = DateTime.Now,
                FirstName = creationInfo.FirstName,
                LastName = creationInfo.LastName,
                Email = creationInfo.Email,
                Phone = creationInfo.Phone,
                LastUpdatedAt = DateTime.Now
            };
            
            users.InsertOne(user, cancellationToken: cancellationToken);

            return Task.FromResult(user);
        }

        public Task<User> PatchAsync(UserPatchInfo patchInfo, string userId, CancellationToken cancellationToken)
        {
            if (patchInfo == null)
            {
                throw new ArgumentNullException(nameof(patchInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();
            
            var user = users.Find(item => item.Id == userId).FirstOrDefault();
            
            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            if (patchInfo.Email != null)
            {
                user.Email = patchInfo.Email;
            }

            if (patchInfo.Phone != null)
            {
                user.Phone = patchInfo.Phone;
            }

            if (patchInfo.FirstName != null)
            {
                user.FirstName = patchInfo.FirstName;
            }    
            
            if (patchInfo.LastName != null)
            {
                user.LastName = patchInfo.LastName;
            }
            users.ReplaceOne(usr => usr.Id == user.Id, user);
            
            return Task.FromResult(user);
        }
        
        public Task UpdateAsync(string id, User userIn)
        {
            users.ReplaceOne(user => user.Id == id, userIn);

            return Task.CompletedTask;
        }

        public Task RemoveAsync(User userIn)
        {
            users.DeleteOne(user => user.Id == userIn.Id);

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string id)
        {
            users.DeleteOne(user => user.Id == id);

            return Task.CompletedTask;
        }
    }
}