using System.Runtime.Serialization;

namespace Client.Models.Maraphone.Task
{
    public class TaskBuildInfo
    {
        /// <summary>
        /// Название задачи
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Title { get; set; }
        
        /// <summary>
        /// Содержание задачи
        /// </summary>
        [DataMember(IsRequired = false)]
        public string ContentId { get; set; }
    }
}