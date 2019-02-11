using NetCoreBirdAPI.DBConnections;
using NetCoreBirdAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NetCoreBirdAPI.Processors
{
    public class TowerProcessor
    {
        public static DataTable RemoveWhiteSpace(string municipal)
        {
            municipal = municipal.Replace(" ", String.Empty);
            return DbConnector.GetTowersFromDB(municipal);
        }
    }
}