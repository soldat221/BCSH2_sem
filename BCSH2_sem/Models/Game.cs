using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCSH2_sem.Models
{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public double Price { get; set; }
        public string Genre { get; set; }
        public string URL { get; set; }

        [BsonIgnore]
        public double AverageRating { get; set; } = 0;

        public Game() { }

        public Game(string name, double price, string genre, string url)
        {
            Name = name;
            Price = price;
            Genre = genre;
            URL = url;
        }
    }
}
