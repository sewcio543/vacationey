using Backend.Models.DbModels;
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
                var Germany = new Country("Germany");
                var Poland = new Country("Poland");

                var Countries = new List<Country>()
                {
                    Spain, France, Italy, Portugal, Germany, Poland
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
                };

                var Hotels = new List<Hotel>();

                using (StreamReader sr = File.OpenText("Data/hotel_names.txt"))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        Random random = new Random();
                        var index = random.Next(Cities.Count);
                        var hotel = new Hotel(s, 3, Cities[index], true, true);
                        Hotels.Add(hotel);
                    }
                }

                for (int i = 0; i < 50; i++)
                {
                    Random random = new Random();
                    var indexHotel = random.Next(Hotels.Count);
                    var indexCity = random.Next(Cities.Count);

                    var offer = new Offer("---", Hotels[indexHotel], 2000, DateTime.Now, DateTime.Now,
                        Cities[indexCity], Hotels[indexHotel].City, true, 1);
                    context.Offer.Add(offer);

                }



                context.SaveChanges();
            }
        }
    }
}

