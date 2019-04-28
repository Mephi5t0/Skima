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
        /// Переводит entry из клиентской модели в серверную
        /// </summary>
        /// <param name="clientEntryCreationInfo">Entry в клиентской модели</param>
        /// <returns>Entry в серверной модели</returns>
        public static Model.Entry Convert(Client.EntryCreationInfo clientEntryCreationInfo)
        {
            if (clientEntryCreationInfo == null)
            {
                throw new ArgumentNullException();
            }

            Model.Status status;

            switch (clientEntryCreationInfo.Status)
            {
                case Client.Status.Active:
                    status = Model.Status.Active;
                    break;
                case Client.Status.Pending:
                    status = Model.Status.Pending;
                    break;
                case Client.Status.Revoked:
                    status = Model.Status.Revoked;
                    break;
                case Client.Status.Finished:
                    status = Model.Status.Finished;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(clientEntryCreationInfo.Status.ToString());
            }
            
            var modelEntry = new Model.Entry
            {
                UserId = clientEntryCreationInfo.UserId,
                CreatedAt = DateTime.Now,
                ActivityId = clientEntryCreationInfo.ActivityId,
                Status = status
            };

            return modelEntry;
        }
        
        /// <summary>
        /// Переводит entry из серверной модели в клиентскую
        /// </summary>
        /// <param name="modelEntry">Entry в клиентской модели</param>
        /// <returns>Entry в клиентской модели</returns>
        public static Client.Entry ConvertToClientModel(Model.Entry modelEntry)
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