using System.Text;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Caching;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;

namespace API.Parking
{
    public class ParkingService
    {
        private WebClient syncClient;
        private DataContractJsonSerializer serializer;
        private ObjectCache cache;

        public ParkingService()
        {
            syncClient = new WebClient();
            syncClient.Encoding = Encoding.UTF8;
            serializer = new DataContractJsonSerializer(typeof(ParkingPlaces));
            cache = MemoryCache.Default;
        }

        public List<ListOfPlace> GetParkingPlaces(double lat, double longitude)
        {
            //If the cache is empty then populate with new entry. Entry will be automatically deleted after 5 seconds.
            if (!cache.Contains("0"))
                cache.Add("0", GetPlaces(), System.DateTime.Now.AddMilliseconds(5000));
            ParkingPlaces places = (ParkingPlaces)cache.Get("0");

            //Calculates all distances between given coordinates and parking coordinates and stores them as pairs in dictionary
            GeoCoordinate coord = new GeoCoordinate(lat, longitude);
            Dictionary<ListOfPlace, double> closestPlaces = new Dictionary<ListOfPlace, double>();
            foreach (ListOfPlace item in places.listOfPlaces)
            {
                double distance = coord.GetDistanceTo(new GeoCoordinate(item.latitude, item.longitude));
                closestPlaces.Add(item, distance);
            }

            //Sorts dictionary
            var items = from pair in closestPlaces orderby pair.Value ascending select pair;
            List<ListOfPlace> parkingPlaces = new List<ListOfPlace>();
            int i = 0;

            //Takes 5 closest available parking places
            foreach (KeyValuePair<ListOfPlace, double> pair in items)
            {
                if (i < 5)
                {
                    if (pair.Key.free_count > 0)
                    {
                        parkingPlaces.Add(pair.Key);
                        i++;
                    }
                }
                else break;
            }

            return parkingPlaces;
        }

        //Get new JSON file and convert to ParkingPlaces
        private ParkingPlaces GetPlaces()
        {
            string content = syncClient.DownloadString("http://ucn-parking.herokuapp.com/places.json");
            content = "{\"listOfPlaces\":" + content + "}";
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(content)))
            {
                return (ParkingPlaces)serializer.ReadObject(ms);
            }
        }
    }
}
