﻿using System.Runtime.Serialization;

namespace Client.Models.Users
{
    /// <summary>
    /// Информация для регистрации пользователя
    /// </summary>
    [DataContract]
    public class UserRegistrationInfo
    {
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Password { get; set; }
        
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [DataMember(IsRequired = false)]
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [DataMember(IsRequired = false)]
        public string LastName { get; set; }
        
        /// <summary>
        /// Почта пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Email { get; set; }
        
        /// <summary>
        /// Телефон пользователя
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Phone { get; set; }
    }
}
