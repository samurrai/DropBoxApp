﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace SecurityApp.Services
{
    public class SecurityHasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool VerifyPassword(string passwordCandidate, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(passwordCandidate, hashedPassword);
        }
    }
}
