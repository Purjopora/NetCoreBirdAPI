using Microsoft.AspNetCore.Mvc;
using NetCoreBirdAPI.Algorithms;
using NetCoreBirdAPI.Common;
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
    public class TowerController : ControllerBase
    {

        // GET api/GetTowers
        [HttpGet]
        [Route("GetTowers")]
        public IEnumerable<Tower> GetTowers()
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

        [HttpGet]
        [Route("GetTowers/clustered")]
        public IEnumerable<Tower> getTowersClustered()
        {
            //TODO: Only needs to fetch coordinates from database?
            IEnumerable<Tower> Towers = GetTowers();
            double[][] clusters = KekMeansLocationProviderAdapter.ClusterPartitions(Towers.ToList<LocationProvider>(), KekMeansLocationProviderAdapter.DEFAULT_CLUSTER_AMOUNT);

            var ResultList = new List<Tower>();
            for (int i = 0; i < clusters.Length; i++)
            {
                Tower cluster = new Tower
                {
                    id = i.ToString(),
                    municipal = "testipesti",
                    towername = "Cluster" + i.ToString(),
                    latitudecoord = clusters[i][0].ToString(),
                    longitudecoord = clusters[i][1].ToString()
                };
                ResultList.Add(cluster);
            }
            return ResultList;
        }

        [HttpGet]
        [Route("GetTowers/municipal")]
        public IEnumerable<Tower> Get(string municipal)
        {
            DataTable resultdt = TowerProcessor.RemoveWhiteSpace(municipal);
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
