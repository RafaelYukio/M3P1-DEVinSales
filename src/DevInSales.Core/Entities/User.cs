using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInSales.Core.Entities
{
    public class User
    {

        public string Id { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Name { get; private set; }
        public DateTime BirthDate { get; private set; }

        public User(string id, string email, string password, string name)
        {
            Id = id;
            Email = email;
            Password = password;
            Name = name;
        }
        public User(string email, string password, string name, DateTime birthDate)
        {
            Email = email;
            Password = password;
            Name = name;
            BirthDate = birthDate;
        }
        public User(string id, string email, string password, string name, DateTime birthDate)
        {
            Id = id;
            Email = email;
            Password = password;
            Name = name;
            BirthDate = birthDate;
        }
    }
}