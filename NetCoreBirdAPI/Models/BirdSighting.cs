using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using NetCoreBirdAPI.Common;
namespace NetCoreBirdAPI.Models
{
    public class BirdSighting : LocationProvider
    {
        public string username { get; set; }
        public string specie { get; set; }
        public double longitudecoord { get; set; }
        public double latitudecoord { get; set; }
        public string comment { get; set; }
        public DateTime timestamp { get; set; }

        public double getLatitude()
        {
            return latitudecoord;
        }

        public double getLongitude()
        {
            return longitudecoord;
        }
    }
}