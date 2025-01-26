using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schlime_Mobile_App.Repos
{
    public class FullFarmRepo
    {
        public GeoLocationRepo GeoLocationRepo { get; set; }
        public PlantRepo PlantRepo { get; set; }
        public SecurityRepo SecurityRepo { get; set; }
      
        public FullFarmRepo()
        {
            GeoLocationRepo = new GeoLocationRepo();
            SecurityRepo = new SecurityRepo();
            PlantRepo = new PlantRepo();
        }
    }
}
