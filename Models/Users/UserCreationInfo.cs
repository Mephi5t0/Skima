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
        /// <param name="login">Логин пользователя</param>
        /// <param name="passwordHash">Хэш пароля</param>
        /// <param name="firstName">Имя пользователя</param>
        /// <param name="lastName">Фамилия пользователя</param>
        /// <param name="email">Почта пользователя</param>
        /// <param name="phone">Номер телефона пользователя</param>
        public UserCreationInfo(string login, string passwordHash, string firstName, string lastName, string email, string phone)
        {
            this.Login = login ?? throw new ArgumentNullException(nameof(login));
            this.PasswodHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            this.FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            this.LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            this.Email = email ?? throw new ArgumentNullException(nameof(email));
            this.Phone = phone ?? throw new ArgumentNullException(nameof(phone));
        }
        
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; }

        /// <summary>
        /// Хэш пароля
        /// </summary>
        public string PasswodHash { get; }
        
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
