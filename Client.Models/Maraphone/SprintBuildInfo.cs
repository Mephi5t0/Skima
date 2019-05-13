using System;
using System.Runtime.Serialization;
using Client.Models.Maraphone.Task;

namespace Client.Models.Maraphone
{
    public class SprintBuildInfo
    {
        /// <summary>
        /// Номер спринта в марафоне
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Number { get; set; }
        
        /// <summary>
        /// Название спринта
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Title { get; set; }
        
        /// <summary>
        /// Описание спринта
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Description { get; set; }
        
        /// <summary>
        /// Продолжительность спринта
        /// </summary>
        [DataMember(IsRequired = true)]
        public TimeSpan Duration { get; set; }
        
        /// <summary>
        /// Коллекция идентификаторов задач
        /// </summary>
        [DataMember(IsRequired = true)]
        public TaskBuildInfo[] Tasks { get; set; }
    }
}