using System;

namespace Models.Converters.Entry
{
    using Client = global::Client.Models.Entry;
    using Model = global::Models.Entries;
    
    /// <summary>
    /// Предоставляет методы конвертирования entry между клиентской и серверной моделями
    /// </summary>
    public static class EntryConverter
    {   
        /// <summary>
        /// Переводит entry из серверной модели в клиентскую
        /// </summary>
        /// <param name="modelEntry">Entry в клиентской модели</param>
        /// <returns>Entry в клиентской модели</returns>
        public static Client.Entry Convert(Model.Entry modelEntry)
        {
            if (modelEntry == null)
            {
                throw new ArgumentNullException();
            }

            Client.Status status;

            switch (modelEntry.Status)
            {
                case Model.Status.Active:
                    status = Client.Status.Active;
                    break;
                case Model.Status.Pending:
                    status = Client.Status.Pending;
                    break;
                case Model.Status.Revoked:
                    status = Client.Status.Revoked;
                    break;
                case Model.Status.Finished:
                    status = Client.Status.Finished;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(modelEntry.Status.ToString());
            }
            
            var clientEntry = new Client.Entry
            {
                UserId = modelEntry.UserId,
                Id = modelEntry.Id,
                ActivityId = modelEntry.ActivityId,
                Status = status
            };

            return clientEntry;
        }
    }
}