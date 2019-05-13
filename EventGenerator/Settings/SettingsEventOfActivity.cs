using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventGenerator
{
    public class SettingsEventOfActivity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        /// <summary>
        /// Дата последней обработанной активности.
        /// </summary>
        [BsonElement("DateOfLastCheckedActivity")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DateOfLastCheckedActivity { get; set; }
    }
}