using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeaBattle.MVC.Models
{
    public class RegisterModel
    {
        public LoginModel LoginModel { get; set; }
        public string Email { get; set; }
    }
}
