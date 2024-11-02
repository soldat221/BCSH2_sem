using BCSH2_sem.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void InsertReview(Review review)
        {
            using var db = new LiteDatabase(_connectionString);
            db.GetCollection<Review>("reviews").Insert(review);
        }

        public List<Review> GetReviews()
        {
            using var db = new LiteDatabase(_connectionString);
            return db.GetCollection<Review>("reviews").FindAll().ToList();
        }

        public void InsertReviewer(Reviewer reviewer)
        {
            using var db = new LiteDatabase(_connectionString);
            db.GetCollection<Reviewer>("reviewers").Insert(reviewer);
        }

        public List<Reviewer> GetReviewers()
        {
            using var db = new LiteDatabase(_connectionString);
            return db.GetCollection<Reviewer>("reviewers").FindAll().ToList();
        }

        public void UpdateReviewer(Reviewer reviewer)
        {
            using var db = new LiteDatabase(_connectionString);
            db.GetCollection<Reviewer>("reviewers").Update(reviewer);
        }
    }
}
