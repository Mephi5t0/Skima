using System;

namespace Models.Users
{
    /// <summary>
    /// Информация для создания пользователя
    /// </summary>
    public class UserCreationInfo
    {
        /// <summary>
        /// Инийиализирует новый экземпляр описания для создания пользователя
        /// </summary>
        /// <param name="passwordHash">Хэш пароля</param>
        /// <param name="firstName">Имя пользователя</param>
        /// <param name="lastName">Фамилия пользователя</param>
        /// <param name="email">Почта пользователя</param>
        /// <param name="phone">Номер телефона пользователя</param>
        public UserCreationInfo(string passwordHash, string firstName, string lastName, string email, string phone)
        {
            this.PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email ?? throw new ArgumentNullException(nameof(email));
            this.Phone = phone;
        }
        
        /// <summary>
        /// Хэш пароля
        /// </summary>
        public string PasswordHash { get; }
        
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Телефон пользователя
        /// </summary>
        public string Phone { get; set; }
    }
}
