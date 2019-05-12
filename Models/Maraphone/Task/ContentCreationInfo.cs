namespace Models.Maraphone.Task
{
    public class ContentCreationInfo
    {
        /// <summary>
        /// Тип данных
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// Данные
        /// </summary>
        public byte[] Data { get; set; }

        /// <param name="type">Тип данных</param>
        /// <param name="data">Данные</param>
        public ContentCreationInfo(string type, byte[] data)
        {
            this.Type = type;
            this.Data = data;
        }
    }
}