﻿using Backend.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class SeedModel
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new DatabaseContext(serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                // Look for any movies.
                if (context.Country.Any())
                {
                    return;   // DB has been seeded
                }

                var Spain = new Country("Spain");
                var France = new Country("France");
                var Italy = new Country("Italy");
                var Portugal = new Country("Portugal");
                var Poland = new Country("Poland");
                var Germany = new Country("Germany");
                var Austria = new Country("Austria");
                var Belgium = new Country("Belgium");
                var Turkey = new Country("Turkey");
                var Brasil = new Country("Brasil");
                var England = new Country("England");
                var Ireland = new Country("Ireland");

                var Countries = new List<Country>()
                {
                    Spain, France, Italy, Portugal, Germany, Poland, Austria, Belgium, Turkey, Brasil, England, Ireland  
                };

                var Cities = new List<City>()
                {
                    new City("Barcelona", Spain),
                    new City("Valencia", Spain),
                    new City("Madryt", Spain),
                    new City("Bilbao", Spain),
                    new City("Cracow", Poland),
                    new City("Warsaw", Poland),
                    new City("Gdansk", Poland),
                    new City("Kielce", Poland),
                    new City("Milan", Italy ),
                    new City("Rome", Italy ),
                    new City("Mediolan", Italy ),
                    new City("Piza", Italy ),
                    new City("Porto", Portugal),
                    new City("Lisboa", Portugal),
                    new City("Braga", Portugal),
                    new City("Paris", France),
                    new City("Lyon", France),
                    new City("Nicea", France),
                    new City("Lille", France),
                    new City("Nantes", France),
                    new City("Bordeaux", France),
                };

                var Hotels = new List<Hotel>();

                using (StreamReader sr = File.OpenText("Data/hotel_names.txt"))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        Random random = new Random();
                        var index = random.Next(Cities.Count);
                        var hotel = new Hotel(s, random.Next(1, 6), Cities[index], 
                            random.Next(0,2) == 1 ? true : false,
                            random.Next(0, 2) == 1 ? true : false) ;
                        Hotels.Add(hotel);
                    }
                }

                for (int i = 0; i < 50; i++)
                {
                    Random random = new Random();
                    // To
                    var indexHotel = random.Next(Hotels.Count);
                    // From
                    var indexCity = random.Next(Cities.Count);

                    var offer = new Offer("---", Hotels[indexHotel], random.Next(1000,3000), DateTime.Now, DateTime.Now.AddDays(7),
                        Cities[indexCity], Hotels[indexHotel].City, true, 1);
                    context.Offer.Add(offer);

                }



                context.SaveChanges();
            }
        }
    }
}

