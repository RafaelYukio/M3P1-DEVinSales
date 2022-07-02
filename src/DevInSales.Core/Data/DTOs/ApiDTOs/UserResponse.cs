using System.ComponentModel.DataAnnotations;
using DevInSales.Core.Entities;

namespace DevInSales.Core.Data.DTOs.ApiDTOs
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public UserResponse(string id, string email, string name)
        {
            Id = id;
            Email = email;
            Name = name;
        }

        public static UserResponse ConverterParaEntidade(User user)
        {
            return new UserResponse(user.Id, user.Email, user.Name);
        }
    }
}