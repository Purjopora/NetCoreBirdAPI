
using NetCoreBirdAPI.Algorithms;
using NetCoreBirdAPI.DBConnections;
using NetCoreBirdAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetCoreBirdAPI.Processors
{
    public class UserProcessor
    {
        public static bool ProcessUser(User user)
        {
            //maybe later encrypt credentials here

            user.passwordhash = PasswordHash.CreateHash(user.passwordhash);
            return DbConnector.AddUserToDB(user);
        }
    }
}