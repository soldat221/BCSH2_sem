using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCSH2_sem.Models
{
    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Reviewer Reviewer { get; set; }
        public Game Game { get; set; }
        public int Stars { get; set; }
        public string Content { get; set; }

        public Review() { }

        public Review(Reviewer reviewer, Game game, int stars, string content)
        {
            Reviewer = reviewer;
            Game = game;
            Stars = stars;
            Content = content;
        }
    }
}
