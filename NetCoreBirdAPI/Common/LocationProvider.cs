using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBirdAPI.Common
{
    public interface LocationProvider
    {
        double getLatitude();

        double getLongitude();
    }
}
