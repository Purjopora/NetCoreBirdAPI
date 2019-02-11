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

        [HttpGet]
        [Route("GetSightings/Bird")]
        public IEnumerable<BirdSighting> GetSightingsForBird(string bird)
        {
            DataTable resultdt = DbConnector.GetSightingsForBird(bird);
            var SightingList = new List<BirdSighting>();
            foreach (DataRow row in resultdt.Rows)
            {
                var sighting = new BirdSighting
                {
                    username = "",
                    specie = "",
                    longitudecoord = Double.Parse(row["longtitude"].ToString()),
                    latitudecoord = Double.Parse(row["latitude"].ToString()),
                    comment = "",
                    timestamp = DateTime.Now

                };
                SightingList.Add(sighting);
            }
            return SightingList;
        }

        [HttpGet]
        [Route("UpdateSightings")]
        public bool updateSightings()
        {
            return DbConnector.updateSightings();
        }

        [HttpGet]
        [Route("SpoofSightings")]
        public bool SpoofSightings(int amount, string user, string bird)
        {
            Random rnd = new Random();
            DateTime now = DateTime.Now;
            List<Specie> species = GetSpecies();
            List<Tower> towers = GetTowers();
            List<BirdSighting> spoofed = new List<BirdSighting>();
            for (int i = 0; i < amount; i++)
            {
                Tower spoofLatitude = towers[rnd.Next(towers.Count)];
                Tower spoofLongitude = towers[rnd.Next(towers.Count)];
                var sighting = new BirdSighting
                {
                    username = user,
                    comment = "chiterboi",
                    specie = bird,
                    latitudecoord = spoofLatitude.getLatitude(),
                    longitudecoord = spoofLongitude.getLongitude(),
                    timestamp = now
                };
                spoofed.Add(sighting);
            }
            foreach (BirdSighting s in spoofed)
            {
                DbConnector.UpdateSightingsToDB(s);
            }
            return true;
        }

        private List<Specie> GetSpecies()
        {
            DataTable resultdt = DbConnector.GetSpeciesFromDB();
            if (resultdt == null)
            {
                return null;
            }
            var SpecieList = new List<Specie>();
            foreach (DataRow row in resultdt.Rows)
            {
                Specie specie = new Specie
                {
                    Id = row[0].ToString(), //TODO: fix to get items with row["id"] and row["speciename"]
                    Speciename = row[1].ToString()
                };
                SpecieList.Add(specie);
            }
            return SpecieList;
        }

        private List<Tower> GetTowers()
        {
            DataTable resultdt = DbConnector.GetTowersFromDB(null);
            if (resultdt == null)
            {
                return null;
            }
            var TowerList = new List<Tower>();
            foreach (DataRow row in resultdt.Rows)
            {
                var tower = new Tower
                {
                    id = row["id"].ToString(),
                    municipal = row["municipal"].ToString(),
                    towername = row["towername"].ToString(),
                    latitudecoord = row["latitudecoord"].ToString().Replace(",", "."),

                    longitudecoord = row["longitudecoord"].ToString().Replace(",", ".")
                };
                TowerList.Add(tower);
            }
            return TowerList;
        }
    }
}
