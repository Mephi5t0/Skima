using System.Runtime.Serialization;

namespace Client.Models.Entry
{
    public class EntryBuildInfo
    {    
        /// <summary>
        /// Идентификатор активности
        /// </summary>
        [DataMember(IsRequired = true)]
        public string ActivityId { get; set; }
    }
}