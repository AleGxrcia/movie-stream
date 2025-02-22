﻿using MovieStream.Core.Application.Enums;
using System.Text.Json.Serialization;

namespace MovieStream.Core.Application.DTOs.Account
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Roles Role { get; set; }
    }
}
