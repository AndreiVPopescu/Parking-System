using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Parking;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Core.Parking
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set up connection
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "52.28.57.150";
            factory.UserName = "parkingapp";
            factory.Password = "parkinguser";
            factory.Port = 5673;
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //Create event consumer
                var consumer = new EventingBasicConsumer(channel);

                //Listen for new messages
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    //New message received, do work here
                    Console.WriteLine(" [x] Received {0}", message);

                    //Converts message to email and coordinates
                    string email = message.Substring(0, message.IndexOf("|"));
                    message = message.Substring(message.IndexOf("|") + 1);
                    double latitude = Convert.ToDouble(message.Substring(0, message.IndexOf("|")));
                    message = message.Substring(message.IndexOf("|") + 1);
                    double longitude = Convert.ToDouble(message);

                    //Creates new parking service
                    ParkingService service = new ParkingService();

                    //Gets a list of 5 closest places to given coordinates
                    List<ListOfPlace> places = service.GetParkingPlaces(latitude, longitude);

                    // Gives all places on console, testing only
                    foreach (ListOfPlace place in places)
                    {
                        Console.WriteLine(place.name + " " + place.free_count + " " + place.latitude + " " + place.longitude);
                    }
                };
                //Remove message
                channel.BasicConsume(queue: "ParkRequest", noAck: true, consumer: consumer);

                //Close program
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
