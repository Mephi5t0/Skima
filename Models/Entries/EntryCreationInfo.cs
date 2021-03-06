using System;

namespace Models.Entries
{
    /// <summary>
    /// Информация для создания записи на марафон
    /// </summary>
    public class EntryCreationInfo
    {
        /// <summary>
        /// Создает запись на марафон
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="activityId">Идентификатор активности</param>
        public EntryCreationInfo(string userId, string activityId)
        {
            this.UserId = userId ?? throw new ArgumentNullException();
            this.ActivityId = activityId ?? throw new ArgumentNullException();
            this.Status = Status.Active;
        }
        
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Идентификатор активности
        /// </summary>
        public string ActivityId { get; set; }
        
        /// <summary>
        /// Статус активности
        /// </summary>
        public Status Status { get; set; }
    }
}