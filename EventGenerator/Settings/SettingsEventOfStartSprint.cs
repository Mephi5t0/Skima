using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventGenerator.Settings
{
    public class SettingsEventOfStartSprint
    {
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        /// <summary>
        /// Дата последней обработанного спринта.
        /// </summary>
        [BsonElement("DateOfLastCheckedSprint")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DateOfLastCheckedSprint { get; set; }
        
    }
}