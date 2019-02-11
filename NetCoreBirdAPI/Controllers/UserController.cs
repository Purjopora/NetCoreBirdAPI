using Microsoft.AspNetCore.Mvc;
using NetCoreBirdAPI.Algorithms;
using NetCoreBirdAPI.DBConnections;
using NetCoreBirdAPI.Models;
using NetCoreBirdAPI.Processors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBirdAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpGet]
        [Route("VerifyUser")]
        public bool VerifyUser(string username, string password)
        {
            DataTable resultdt = DbConnector.GetUserFromDB(username);
            if (resultdt == null)
            {
                return false;
            }
            DataRow row = resultdt.Rows[0];
            var parts = row["passwordhash"].ToString();
            return PasswordHash.VerifyPassword(password, parts);
            //TODO: SEND JSON WEBTOKEN INSTEAD OF TRUE/FALSE CONFIRMATION
        }

        //POST NEW User
        [HttpPost]
        [Route("SaveUser")]
        public bool SaveUser(User user)
        {
            if (user == null)
            {
                return false;
            }
            return UserProcessor.ProcessUser(user);

        }
    }
}
