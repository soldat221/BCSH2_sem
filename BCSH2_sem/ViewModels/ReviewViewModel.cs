using BCSH2_sem.Models;
using BCSH2_sem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCSH2_sem.ViewModels
{
    public class ReviewViewModel : INotifyPropertyChanged
    {
        private LiteDBService _dbService = new LiteDBService();
        public ObservableCollection<Review> Reviews { get; set; } = new ObservableCollection<Review>();

        public Review SelectedReview { get; set; }
        public Reviewer SelectedReviewer { get; set; }
        public Game SelectedGame { get; set; }
        public int NewReviewStars { get; set; }
        public string NewReviewContent { get; set; }

        public RelayCommand AddReviewCommand { get; }

        public ReviewViewModel()
        {
            AddReviewCommand = new RelayCommand(AddReview);
            LoadReviews();
        }

        private void LoadReviews()
        {
            Reviews.Clear();
            foreach (var review in _dbService.GetReviews())
            {
                Reviews.Add(review);
            }
        }

        private void AddReview()
        {
            var newReview = new Review(SelectedReviewer, SelectedGame, NewReviewStars, NewReviewContent);
            _dbService.InsertReview(newReview);
            LoadReviews();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
