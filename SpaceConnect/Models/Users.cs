using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyXaN.Service.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; } // int
        [Required(ErrorMessage = "Имя пользователя не установлен")]
        public string FIO { get; set; }

        [Required(ErrorMessage = "Логин не установлен")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Пароль не установлен")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Выберите Роль")]
        public int RoleID { get; set; } // varchar(100)
    }

}
