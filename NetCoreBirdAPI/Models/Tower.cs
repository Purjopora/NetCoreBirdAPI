using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetCoreBirdAPI.Common;
namespace NetCoreBirdAPI.Models
{
    public class Tower : LocationProvider
    {

        public string id { get; set; }
        public string municipal { get; set; }
        public string towername { get; set; }
        //TODO: location data in doubles
        public string longitudecoord { get; set; }
        public string latitudecoord { get; set; }

        public double getX()
        {
            return double.Parse(latitudecoord, System.Globalization.CultureInfo.InvariantCulture);
        }

        public double getY()
        {
            return Convert.ToDouble(longitudecoord, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}