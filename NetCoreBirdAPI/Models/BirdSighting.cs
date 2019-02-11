using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NetCoreBirdAPI.Models
{
    public class BirdSighting
    {
        public string username { get; set; }
        public string specie { get; set; }
        public double longitudecoord { get; set; }
        public double latitudecoord { get; set; }
        public string comment { get; set; }
        public DateTime timestamp { get; set; }

    }
}