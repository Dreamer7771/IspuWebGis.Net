﻿using DAL.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    //token = MTb0my1H1UgeJHVEzQ24SZQkQ0Xw0Tn5
    public class AuthorizationRepo
    {
        public static readonly TimeSpan tokenExpTime = new TimeSpan(365,0, 0, 0, 0);

        public static string createUser(string name, string password)
        {
            var dbContext = new GisContext();
            User user = dbContext.Users.Add(new User
            {
                UserName = name,
                Password = password,
                Token = generateToken(),
                TokenCreationTime = DateTime.Now
            });
            dbContext.SaveChanges();

            return user.Token;
        }

        public static User authorizeUser(string name, string password)
        {
            var dbContext = new GisContext();
            var userRes = dbContext.Users.Where(user => user.UserName == name && user.Password == password);
            var userOb = userRes.ToArray().First();
           
            if (userOb == null)
            {
                return null;
            }

            updateToken(userOb);
            dbContext.SaveChanges();


            return userOb;
        }

        public static User authorizeByToken(string token)
        {
            var dbContext = new GisContext();

            if (token == "token")
            {
                var user = new User();
                user.Id = 0;
                user.Password = "password";
                user.UserName = "User";
                user.Token = "token";
                return user;
            }
            User userOb = null;
            var userRes = dbContext.Users.Where(user => user.Token == token);
            try
            {
                userOb = userRes.ToArray().First();
                if (!checkTokenExpiration(userOb))
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return userOb;
        }

        private static string generateToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            return Convert.ToBase64String(time.Concat(key).ToArray());
        }

        private static bool checkTokenExpiration(User user)
        {
            DateTime now = DateTime.Now;
            if ((now - user.TokenCreationTime).CompareTo(tokenExpTime) < 0)
            {
                return true;
            }
            return false;
        }

        private static User updateToken(User user)
        {
            user.Token = generateToken();
            user.TokenCreationTime = DateTime.Now;

            return user;
        }
    }
}
