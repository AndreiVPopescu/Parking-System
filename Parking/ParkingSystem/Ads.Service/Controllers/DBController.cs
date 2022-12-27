using System;
using System.Collections.Generic;
using System.Linq;
using Ads.Service.EDMX;

namespace Ads.Service
{
    class DBController
    {
     public List<Models.Ads> GetAdsList()
        {
            List<Models.Ads> adsList = new List<Models.Ads>();
            try
            {
                using (ParkingSystemDKEntities database = new ParkingSystemDKEntities())
                {
                    var ads = from m in database.Ads
                                   select m;

                    foreach (var a in ads)
                    {
                        adsList.Add(new Models.Ads
                        {
                            adsId = a.adsId,
                            adsBody = a.adsBody,
                            active = (bool) a.active,
                        });
                    }
                }
            }
            catch (Exception e)
            {
            }
            return adsList;
        }
    }
}
