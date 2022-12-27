using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Service
{
    public class AdsController
    {
        public Models.Ads getRandomAds()
        {
          var dbCon = new DBController();
          var rnd = new Random();
          int index = rnd.Next(dbCon.GetAdsList().Count);
            return dbCon.GetAdsList()[index];
        }
    }
}
