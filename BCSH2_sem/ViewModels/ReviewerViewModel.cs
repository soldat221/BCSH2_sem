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
    public class ReviewerViewModel : INotifyPropertyChanged
    {
        private LiteDBService _dbService = new LiteDBService();
        public ObservableCollection<Reviewer> Reviewers { get; set; } = new ObservableCollection<Reviewer>();

        public Reviewer SelectedReviewer { get; set; }
        public string NewReviewerName { get; set; }

        public RelayCommand AddReviewerCommand { get; }

        public ReviewerViewModel()
        {
            AddReviewerCommand = new RelayCommand(AddReviewer);
            LoadReviewers();
        }

        private void LoadReviewers()
        {
            Reviewers.Clear();
            foreach (var reviewer in _dbService.GetReviewers())
            {
                reviewer.ReviewCount = _dbService.GetReviews().Count(r => r.Reviewer.Id == reviewer.Id);
                Reviewers.Add(reviewer);
            }
        }

        private void AddReviewer()
        {
            var newReviewer = new Reviewer(NewReviewerName);
            _dbService.InsertReviewer(newReviewer);
            LoadReviewers();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
