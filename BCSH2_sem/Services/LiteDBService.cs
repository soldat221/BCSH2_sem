using BCSH2_sem.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BCSH2_sem.Services
{
    public class LiteDBService
    {
        private readonly string _connectionString = "Filename=games.db; Mode=Shared";

        private LiteDatabase _db;

        public LiteDBService()
        {
            _db = new LiteDatabase("Filename=games.db; Connection=shared");
        }

        public List<Game> GetAllGames() => _db.GetCollection<Game>("games").FindAll().ToList();

        public void AddGame(Game game) => _db.GetCollection<Game>("games").Insert(game);

        public void UpdateGame(Game game) => _db.GetCollection<Game>("games").Update(game);

        public void DeleteGame(Guid gameId) => _db.GetCollection<Game>("games").Delete(gameId);

        public List<Review> GetAllReviews() => _db.GetCollection<Review>("reviews").FindAll().ToList();

        public void AddReview(Review review) => _db.GetCollection<Review>("reviews").Insert(review);

        public void UpdateReview(Review review) => _db.GetCollection<Review>("reviews").Update(review);

        public void DeleteReview(Guid reviewId) => _db.GetCollection<Review>("reviews").Delete(reviewId);

        public List<Reviewer> GetAllReviewers() => _db.GetCollection<Reviewer>("reviewers").FindAll().ToList();

        public void AddReviewer(Reviewer reviewer) => _db.GetCollection<Reviewer>("reviewers").Insert(reviewer);

        public void UpdateReviewer(Reviewer reviewer) => _db.GetCollection<Reviewer>("reviewers").Update(reviewer);

        public void DeleteReviewer(Guid reviewerId) => _db.GetCollection<Reviewer>("reviewers").Delete(reviewerId);

    }
}
