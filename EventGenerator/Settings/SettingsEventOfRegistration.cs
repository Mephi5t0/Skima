using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventGenerator.Settings
{
    public class SettingsEventOfRegistration
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        /// <summary>
        /// Дата регистрации пользователя
        /// </summary>
        [BsonElement("DateOfLastNotificationUser")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DateOfLastNotificationUser { get; set; }
    }
    
}