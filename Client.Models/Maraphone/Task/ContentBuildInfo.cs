using System.Runtime.Serialization;

namespace Client.Models.Maraphone.Task
{
    public class ContentBuildInfo
    {
        /// <summary>
        /// Данные
        /// </summary>
        [DataMember(IsRequired = true)]
        public byte[] Data { get; set; }
    }
}