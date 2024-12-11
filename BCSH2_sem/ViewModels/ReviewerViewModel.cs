using BCSH2_sem.Models;
using BCSH2_sem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BCSH2_sem.ViewModels
{
    public class ReviewerViewModel : INotifyPropertyChanged
    {
        private LiteDBService _database;
        private Reviewer _selectedReviewer;
        private DataGrid _dataGrid;

        public ObservableCollection<Reviewer> Reviewers { get; set; }
        public event Action ReviewersUpdated;
        public Reviewer SelectedReviewer
        {
            get => _selectedReviewer;
            set
            {
                _selectedReviewer = value;
                OnPropertyChanged(nameof(SelectedReviewer));
                (UpdateReviewerCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteReviewerCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand AddReviewerCommand { get; }
        public ICommand UpdateReviewerCommand { get; }
        public ICommand DeleteReviewerCommand { get; }

        public ReviewerViewModel()
        {
            _database = new LiteDBService();
            Reviewers = new ObservableCollection<Reviewer>(_database.GetAllReviewers());
            AddReviewerCommand = new RelayCommand(AddReviewer);
            UpdateReviewerCommand = new RelayCommand(UpdateReviewer, () => SelectedReviewer != null);
            DeleteReviewerCommand = new RelayCommand(DeleteReviewer, CanDeleteReviewer);
        }

        private void NotifyReviewersUpdated()
        {
            ReviewersUpdated?.Invoke();
        }

        public void AttachDataGrid(DataGrid dataGrid)
        {
            _dataGrid = dataGrid;
        }

        public void UpdateReviewCounts(IEnumerable<Review> reviews)
        {
            foreach (var reviewer in Reviewers)
            {
                reviewer.ReviewCount = reviews.Count(r => r.Reviewer.Id == reviewer.Id);
            }

            // refresh DataGridu
            _dataGrid?.Items.Refresh();
            OnPropertyChanged(nameof(Reviewers));
        }

        private void AddReviewer()
        {
            var newReviewer = new Reviewer("New Reviewer");
            _database.AddReviewer(newReviewer);
            Reviewers.Add(newReviewer);
            NotifyReviewersUpdated();
            SelectedReviewer = newReviewer;
        }

        private void UpdateReviewer()
        {
            if (SelectedReviewer != null)
            {
                _database.UpdateReviewer(SelectedReviewer);
                var index = Reviewers.IndexOf(SelectedReviewer);
                Reviewers[index] = SelectedReviewer;
                NotifyReviewersUpdated();
                OnPropertyChanged(nameof(Reviewers));
            }
        }

        private void DeleteReviewer()
        {
            if (SelectedReviewer != null)
            {
                var reviewsForReviewer = _database.GetAllReviews().Any(r => r.Reviewer.Id == SelectedReviewer.Id);

                if (reviewsForReviewer)
                {
                    MessageBox.Show("Cannot delete this reviewer because they have reviews associated with them.", "Delete Reviewer", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _database.DeleteReviewer(SelectedReviewer.Id);
                Reviewers.Remove(SelectedReviewer);
                NotifyReviewersUpdated();
                SelectedReviewer = null;
            }
        }

        private bool CanDeleteReviewer()
        {
            if (SelectedReviewer == null) return false;

            var reviewsForReviewer = _database.GetAllReviews().Any(r => r.Reviewer.Id == SelectedReviewer.Id);
            return !reviewsForReviewer;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
