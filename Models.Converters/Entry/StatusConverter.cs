using System;

namespace Models.Converters.Entry
{
    using Client = global::Client.Models.Entry;
    using Model = global::Models.Entries;
    
    /// <summary>
    /// Предоставляет методы конвертирования статуса записи на марафон между клиентской и серверной моделями
    /// </summary>
    public class StatusConverter
    {
        /// <summary>
        /// Переводит статус entry из клиентской модели в серверную
        /// </summary>
        /// <param name="clientStatus">Статус записи в клиентской модели</param>
        /// <returns>Статус записи в серверной модели</returns>
        public static Model.Status Convert(Client.Status clientStatus)
        {
            Model.Status status;

            switch (clientStatus)
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
                    throw new ArgumentOutOfRangeException(clientStatus.ToString());
            }

            return status;
        }
    }
}