using System.Runtime.Serialization;

namespace Client.Models.Maraphone.Task
{
    public class Content
    {
        /// <summary>
        /// Идентификатор контента
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Данные
        /// </summary>
        public byte[] Data { get; set; }
    }
}