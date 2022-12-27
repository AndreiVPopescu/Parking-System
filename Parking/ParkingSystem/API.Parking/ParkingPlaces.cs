using System.Runtime.Serialization;
using System.Collections.Generic;

namespace API.Parking
{
    public class ListOfPlace
    {
        public string name { get; set; }

        public string is_open { get; set; }

        public string is_payment_active { get; set; }

        public string status_park_place { get; set; }

        public float longitude { get; set; }

        public float latitude { get; set; }

        public int max_count { get; set; }

        public int free_count { get; set; }
    }

       [DataContract]
    public class ParkingPlaces
    {
        [DataMember]
        public List<ListOfPlace> listOfPlaces { get; set; }
    }
}
