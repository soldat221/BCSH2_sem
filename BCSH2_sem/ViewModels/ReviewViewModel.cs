using BCSH2_sem.Models;
using BCSH2_sem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BCSH2_sem.ViewModels
{
    public class ReviewViewModel : INotifyPropertyChanged
    {
        private LiteDBService _database;
        private ReviewerViewModel _reviewerViewModel;
        private GameViewModel _gameViewModel;
        private Review _selectedReview;

        public ObservableCollection<Review> Reviews { get; set; }
        public ObservableCollection<Game> Games { get; set; }
        public ObservableCollection<Reviewer> Reviewers { get; set; }

        public Review SelectedReview
        {
            get => _selectedReview;
            set
            {
                _selectedReview = value;

                if (_selectedReview != null)
                {
                    _selectedReview.Game = Games.FirstOrDefault(g => g.Id == _selectedReview.Game.Id);
                    _selectedReview.Reviewer = Reviewers.FirstOrDefault(r => r.Id == _selectedReview.Reviewer.Id);

                    OnPropertyChanged(nameof(SelectedReview));
                    OnPropertyChanged(nameof(SelectedReview.Game));
                    OnPropertyChanged(nameof(SelectedReview.Reviewer));
                }

                (UpdateReviewCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteReviewCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand AddReviewCommand { get; }
        public ICommand UpdateReviewCommand { get; }
        public ICommand DeleteReviewCommand { get; }

        public ReviewViewModel(ReviewerViewModel reviewerViewModel, GameViewModel gameViewModel)
        {
            _database = new LiteDBService();
            _reviewerViewModel = reviewerViewModel;
            _gameViewModel = gameViewModel;
            Reviews = new ObservableCollection<Review>(_database.GetAllReviews());
            Games = new ObservableCollection<Game>(_database.GetAllGames());
            Reviewers = new ObservableCollection<Reviewer>(_database.GetAllReviewers());

            UpdateRelatedData();

            AddReviewCommand = new RelayCommand(AddReview);
            UpdateReviewCommand = new RelayCommand(UpdateReview, () => SelectedReview != null);
            DeleteReviewCommand = new RelayCommand(DeleteReview, () => SelectedReview != null);
        }

        private void AddReview()
        {
            var newReview = new Review(Reviewers.FirstOrDefault(), Games.FirstOrDefault(), 0, "New Content");
            _database.AddReview(newReview);
            Reviews.Add(newReview);
            UpdateRelatedData();
            SelectedReview = newReview;
        }

        private void UpdateReview()
        {
            if (SelectedReview != null)
            {
                _database.UpdateReview(SelectedReview);
                var index = Reviews.IndexOf(SelectedReview);
                try 
                {
                    Reviews[index] = SelectedReview;
                } 
                catch { }
                UpdateRelatedData();
                OnPropertyChanged(nameof(Reviews));
            }
        }

        private void DeleteReview()
        {
            if (SelectedReview != null)
            {
                _database.DeleteReview(SelectedReview.Id);
                Reviews.Remove(SelectedReview);
                UpdateRelatedData();
                SelectedReview = null;
            }
        }

        private void UpdateRelatedData()
        {
            var reviews = _database.GetAllReviews();

            foreach (var review in reviews)
            {
                review.Game = _gameViewModel.Games.FirstOrDefault(g => g.Id == review.Game.Id);
                review.Reviewer = _reviewerViewModel.Reviewers.FirstOrDefault(r => r.Id == review.Reviewer.Id);
            }

            // Aktualizace datových kolekcí
            Reviews.Clear();
            foreach (var review in reviews)
            {
                Reviews.Add(review);
            }

            _gameViewModel.UpdateAverageRatings(reviews);

            _reviewerViewModel.UpdateReviewCounts(reviews);

            OnPropertyChanged(nameof(Reviews));
        }

        public void UpdateGameCollection()
        {
            Games.Clear();
            foreach (var game in _gameViewModel.Games)
            {
                Games.Add(game);
            }
            OnPropertyChanged(nameof(Games));
        }

        public void UpdateReviewerCollection()
        {
            Reviewers.Clear();
            foreach (var reviewer in _reviewerViewModel.Reviewers)
            {
                Reviewers.Add(reviewer);
            }
            OnPropertyChanged(nameof(Reviewers));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
