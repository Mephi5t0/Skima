using System;

namespace Models.Entries
{
    /// <summary>
    /// Исключение, которое возникает при попытке повторно записаться на марафон
    /// </summary>
    public class EntryDuplicationException : Exception
    {
        /// <summary>
        /// Инициализировать эксземпляр исключения по индентификаторам пользователя и активности
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="activityId"></param>
        public EntryDuplicationException(string userId, string activityId)
            : base($"A user with id \"{userId}\" already participate in activity with id  \"{activityId}\".")
        {
        }
    }
}