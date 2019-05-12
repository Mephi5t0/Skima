using System;
using System.Runtime.Serialization;

namespace Client.Models.Maraphone
{
    public class MaraphoneBuildInfo
    {
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
        public SprintBuildInfo[] SprintsBuildInfo { get; set; }
        
        /// <summary>
        /// Продолжительность марафона
        /// </summary>
        [DataMember(IsRequired = true)]
        public TimeSpan Duration { get; set; }   
    }
}