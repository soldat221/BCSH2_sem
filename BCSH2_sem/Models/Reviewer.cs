using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCSH2_sem.Models
{
    public class Reviewer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        [BsonIgnore]
        public int ReviewCount { get; set; }

        public Reviewer() { }

        public Reviewer(string name)
        {
            Name = name;
        }
    }
}
