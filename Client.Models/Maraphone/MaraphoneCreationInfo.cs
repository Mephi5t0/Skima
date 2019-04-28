using System;
using System.Runtime.Serialization;

namespace Client.Models.Maraphone
{
    public class MaraphoneCreationInfo
    {
        /// <summary>
        /// Идентификатор марафона
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Id создателя марафона
        /// </summary> 
        public string CreatedBy { get; set; }
        
        /// <summary>
        /// Название марафона
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// Описание марафона
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Description { get; set; }
        
        /// <summary>
        /// Спринты марафона
        /// </summary>
        [DataMember(IsRequired = true)]
        public Sprint[] Sprints { get; set; }
        
        /// <summary>
        /// Продолжительность марафона
        /// </summary>
        [DataMember(IsRequired = true)]
        public TimeSpan Duration { get; set; }   
    }
}