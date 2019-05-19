using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Tokens
{
    public class Token
    {
        /// <summary>
        /// Идентификатор токена
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        /// <summary>
        /// Логин пользователя
        /// </summary>
        [BsonElement("UserId")]
        public string UserId { get; set; }
        
        /// <summary>
        /// Refresh Токен
        /// </summary>
        [BsonElement("RefreshToken")]
        public string RefreshToken { get; set; }
        
        /// <summary>
        /// Дата создания Refresh токена
        /// </summary>
        [BsonElement("CreatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; }
        
        public static int LIFE_TIME = 60;
    }
}