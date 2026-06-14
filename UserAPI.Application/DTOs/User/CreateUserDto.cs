using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Application.DTOs.User
{
    public class CreateUserDto
    {
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Pincode { get; set; } = string.Empty;
    }
}
