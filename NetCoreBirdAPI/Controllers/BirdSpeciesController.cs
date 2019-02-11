using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using NetCoreBirdAPI.Models;
using NetCoreBirdAPI.DBConnections;

namespace NetCoreBirdAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirdSpeciesController : ControllerBase
    {

        [HttpGet]
        public ActionResult<IEnumerable<Specie>> Get()
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
    }
}