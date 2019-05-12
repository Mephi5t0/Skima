using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Maraphone.Task
{
    public class Answer
    {
        /// <summary>
        /// Идентификатор ответа
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        /// <summary>
        /// Дата ответа
        /// </summary>
        [BsonElement("CreatedAt")]
        public string CreatedAt { get; set; }
        
        /// <summary>
        /// Идентификатор задачи, к которой относится ответ
        /// </summary>
        [BsonElement("TaskId")]
        public string TaskId { get; set; }
        
        /// <summary>
        /// Идентификатор пользователя, отправившего ответ
        /// </summary>
        [BsonElement("UserId")]
        public string UserId { get; set; }
        
        /// <summary>
        /// Идентификатор активности
        /// </summary>
        [BsonElement("ActivityId")]
        public string ActivityId { get; set; }
        
        /// <summary>
        /// Идентификатор спринта
        /// </summary>
        [BsonElement("SprintId")]
        public string SprintId { get; set; }
        
        /// <summary>
        /// Содержание задачи
        /// </summary>
        [BsonElement("Content")]
        public string Content { get; set; }
        
        /// <summary>
        /// Тип содержимого задачи
        /// </summary>
        [BsonElement("ContentType")]
        public string ContentType { get; set; }
    }
}