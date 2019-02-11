using Microsoft.AspNetCore.Mvc;
using NetCoreBirdAPI.DBConnections;
using NetCoreBirdAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBirdAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirdSightingController : ControllerBase
    {
        //POST NEW Sighting
        [HttpPost]
        [Route("SaveSighting")]
        public bool SaveSighting(BirdSighting birdsighting)
        {
            return DbConnector.UpdateSightingsToDB(birdsighting);
        }

        [HttpGet]
        [Route("GetSightings")]
        public IEnumerable<BirdSighting> GetSightingsFromUser(string user)
        {

            DataTable resultdt = DbConnector.GetSightingsFromDB(user);
            if (resultdt == null)
            {
                return null;
            }

            var SightingList = new List<BirdSighting>();
            foreach (DataRow row in resultdt.Rows)
            {

                var sighting = new BirdSighting
                {
                    username = row["username"].ToString(),
                    specie = row["specie"].ToString(),
                    longitudecoord = Convert.ToDouble(row["longitudecoord"]),
                    latitudecoord = Convert.ToDouble(row["latitudecoord"]),
                    comment = row["comment"].ToString(),
                    timestamp = Convert.ToDateTime(row["timestamp"])

                };
                SightingList.Add(sighting);
            }
            return SightingList;

        }

    }
}
