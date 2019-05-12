using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Maraphone.Task
{
    public class Content
    {
        /// <summary>
        /// Идентификатор контента
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        /// <summary>
        /// Тип данных
        /// </summary>
        [BsonElement("Type")]
        public string Type { get; set; }
        
        /// <summary>
        /// Данные
        /// </summary>
        [BsonElement("Data")]
        public byte[] Data { get; set; }
    }
}