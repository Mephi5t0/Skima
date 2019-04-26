using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Users
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        [BsonElement("Login")]
        public string Login { get; set; }

        /// <summary>
        /// Хэш пароля
        /// </summary>
        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [BsonElement("FirstName")]
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [BsonElement("LastName")]
        public string LastName { get; set; }
        
        /// <summary>
        /// Почта пользователя
        /// </summary>
        [BsonElement("Email")]
        public string Email { get; set; }
        
        /// <summary>
        /// Телефон пользователя
        /// </summary>
        [BsonElement("Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Дата регистрации пользователя
        /// </summary>
        [BsonElement("RegisteredAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime RegisteredAt { get; set; }
        
        /// <summary>
        /// Дата обновления информации о пользователе
        /// </summary>
        [BsonElement("LastUpdatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime LastUpdatedAt { get; set; }
    }
}