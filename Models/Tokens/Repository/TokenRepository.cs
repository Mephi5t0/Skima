using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Models.Tokens.Repository
{
    public class TokenRepository
    {
        private readonly IMongoCollection<Token> tokens;

        public TokenRepository(Configuration config)
        {
            var client = new MongoClient(config.GetConnectionString("SkimaDb"));
            var database = client.GetDatabase("SkimaDb");
            tokens = database.GetCollection<Token>("Tokens");
        }

        public Task<List<Token>> GetAsync()
        {
            var search = tokens.Find(token => true);
            var result = search.ToList();

            return Task.FromResult(result);
        }

        public Task SaveRefreshTokenAsync(string userId, string refreshToken)
        {
            var token = new Token
            {
                UserId = userId,
                RefreshToken = refreshToken,
                CreatedAt = DateTime.Now
            };
            tokens.InsertOne(token);

            return Task.CompletedTask;
        }

        public Task<bool> IsRefreshTokenExistAsync(string userId, string refreshToken)
        {
            var count = tokens.CountDocuments(token => token.UserId == userId && token.RefreshToken == refreshToken);
            var result = count > 0;
            
            return Task.FromResult(result);
        }

        public Task RemoveRefreshTokenAsync(string userId, string refreshToken)
        {
            tokens.DeleteOne(token => token.UserId == userId && token.RefreshToken == refreshToken);

            return Task.CompletedTask;
        }
        
        public Task RemoveRefreshTokenAsync(string refreshToken)
        {
            tokens.DeleteOne(token => token.RefreshToken == refreshToken);

            return Task.CompletedTask;
        }
        
        public void DeleteExpiredTokes()
        {
            var allTokens = tokens.Find(token => true).ToList();

            foreach (var token in allTokens)
            {
                if ((DateTime.Now - token.CreatedAt).TotalDays >= Token.LIFE_TIME)
                {
                    tokens.DeleteOne(t => t.RefreshToken == token.RefreshToken);
                }
            }
        }
    }
}