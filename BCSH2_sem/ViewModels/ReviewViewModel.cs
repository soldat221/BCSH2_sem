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
                OnPropertyChanged(nameof(SelectedReview));
                (UpdateReviewCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteReviewCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand AddReviewCommand { get; }
        public ICommand UpdateReviewCommand { get; }
        public ICommand DeleteReviewCommand { get; }

        public ReviewViewModel()
        {
            _database = new LiteDBService();
            Reviews = new ObservableCollection<Review>(_database.GetAllReviews());
            Games = new ObservableCollection<Game>(_database.GetAllGames());
            Reviewers = new ObservableCollection<Reviewer>(_database.GetAllReviewers());

            AddReviewCommand = new RelayCommand(AddReview);
            UpdateReviewCommand = new RelayCommand(UpdateReview, () => SelectedReview != null);
            DeleteReviewCommand = new RelayCommand(DeleteReview, () => SelectedReview != null);
        }

        private void AddReview()
        {
            var newReview = new Review(new Reviewer("Reviewer Name"), new Game("Game Name", 0, "Genre", "URL"), 0, "Content");
            _database.AddReview(newReview);
            Reviews.Add(newReview);
            SelectedReview = newReview;
        }

        private void UpdateReview()
        {
            if (SelectedReview != null)
            {
                _database.UpdateReview(SelectedReview);
                var index = Reviews.IndexOf(SelectedReview);
                Reviews[index] = SelectedReview;
                OnPropertyChanged(nameof(Reviews));
            }
        }

        private void DeleteReview()
        {
            if (SelectedReview != null)
            {
                _database.DeleteReview(SelectedReview.Id);
                Reviews.Remove(SelectedReview);
                SelectedReview = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
