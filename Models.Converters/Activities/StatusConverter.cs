using System;

namespace Models.Converters.Activities
{
    using Client = global::Client.Models.Activity;
    using Model = global::Models.Activity;
    
    /// <summary>
    /// Предоставляет методы конвертирования статуса активности на марафон между клиентской и серверной моделями
    /// </summary>
    public class StatusConverter
    {
        /// <summary>
        /// Переводит статус активности из клиентской модели в серверную
        /// </summary>
        /// <param name="clientStatus">Статус активности в клиентской модели</param>
        /// <returns>Статус активности в серверной модели</returns>
        public static Model.Status Convert(Client.Status clientStatus)
        {
            Model.Status status;

            switch (clientStatus)
            {
                case Client.Status.New:
                    status = Model.Status.New;
                    break;
                case Client.Status.Running:
                    status = Model.Status.Running;
                    break;
                case Client.Status.Announced:
                    status = Model.Status.Announced;
                    break;
                case Client.Status.Finished:
                    status = Model.Status.Finished;
                    break;
                case Client.Status.Canceled:
                    status = Model.Status.Canceled;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(clientStatus.ToString());
            }

            return status;
        }
    }
}