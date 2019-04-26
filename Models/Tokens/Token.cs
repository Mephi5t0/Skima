using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Tokens
{
    public class Token
    {
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
        
//        /// <summary>
//        /// Access Токен
//        /// </summary>
//        [BsonElement("RefreshToken")]
//        public string AccessToken { get; set; }
        
        /// <summary>
        /// Дата создания Refresh токена
        /// </summary>
        [BsonElement("CreatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime RefreshTokenCreatedAt { get; set; }
        
//        /// <summary>
//        /// Дата создания Access токена
//        /// </summary>
//        [BsonElement("CreatedAt")]
//        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
//        public DateTime AccessTokenCreatedAt { get; set; }
    }
}