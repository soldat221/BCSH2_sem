﻿using BCSH2_sem.Models;
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
    public class ReviewerViewModel : INotifyPropertyChanged
    {
        private LiteDBService _database;
        private Reviewer _selectedReviewer;

        public ObservableCollection<Reviewer> Reviewers { get; set; }

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
            DeleteReviewerCommand = new RelayCommand(DeleteReviewer, () => SelectedReviewer != null);
        }

        private void AddReviewer()
        {
            var newReviewer = new Reviewer("New Reviewer");
            _database.AddReviewer(newReviewer);
            Reviewers.Add(newReviewer);
            SelectedReviewer = newReviewer;
        }

        private void UpdateReviewer()
        {
            if (SelectedReviewer != null)
            {
                _database.UpdateReviewer(SelectedReviewer);
                var index = Reviewers.IndexOf(SelectedReviewer);
                Reviewers[index] = SelectedReviewer;
                OnPropertyChanged(nameof(Reviewers));
            }
        }

        private void DeleteReviewer()
        {
            if (SelectedReviewer != null)
            {
                _database.DeleteReviewer(SelectedReviewer.Id);
                Reviewers.Remove(SelectedReviewer);
                SelectedReviewer = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
