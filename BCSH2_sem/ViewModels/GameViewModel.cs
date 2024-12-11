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
    public class GameViewModel : INotifyPropertyChanged
    {
        private LiteDBService _database;
        private Game _selectedGame;
        private DataGrid _dataGrid;

        public ObservableCollection<Game> Games { get; set; }
        public event Action GamesUpdated;

        public Game SelectedGame
        {
            get => _selectedGame;
            set
            {
                _selectedGame = value;
                OnPropertyChanged(nameof(SelectedGame));
                (UpdateGameCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteGameCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand AddGameCommand { get; }
        public ICommand UpdateGameCommand { get; }
        public ICommand DeleteGameCommand { get; }

        public GameViewModel()
        {
            _database = new LiteDBService();
            Games = new ObservableCollection<Game>(_database.GetAllGames());

            AddGameCommand = new RelayCommand(AddGame);
            UpdateGameCommand = new RelayCommand(UpdateGame, () => SelectedGame != null);
            DeleteGameCommand = new RelayCommand(DeleteGame, CanDeleteGame);
        }

        private void NotifyGamesUpdated()
        {
            GamesUpdated?.Invoke();
        }

        private void AddGame()
        {
            var newGame = new Game("New Game", 0, "Genre", "URL");
            _database.AddGame(newGame);
            Games.Add(newGame);
            NotifyGamesUpdated();
            SelectedGame = newGame;
        }

        private void UpdateGame()
        {
            if (SelectedGame != null)
            {
                _database.UpdateGame(SelectedGame);
                var index = Games.IndexOf(SelectedGame);
                Games[index] = SelectedGame;
                NotifyGamesUpdated();
                OnPropertyChanged(nameof(Games));
            }
        }

        public void AttachDataGrid(DataGrid dataGrid)
        {
            _dataGrid = dataGrid;
        }

        public void UpdateAverageRatings(IEnumerable<Review> reviews)
        {
            foreach (var game in Games)
            {
                var gameReviews = reviews.Where(r => r.Game.Id == game.Id).ToList();
                game.AverageRating = gameReviews.Any() ? gameReviews.Average(r => r.Stars) : 0;
            }

            // Explicitní refresh DataGridu
            _dataGrid?.Items.Refresh();
            OnPropertyChanged(nameof(Games));
        }

        private void DeleteGame()
        {
            if (SelectedGame != null)
            {
                var reviewsForGame = _database.GetAllReviews().Any(r => r.Game.Id == SelectedGame.Id);

                if (reviewsForGame)
                {
                    MessageBox.Show("Cannot delete this game because it has reviews associated with it.", "Delete Game", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _database.DeleteGame(SelectedGame.Id);
                Games.Remove(SelectedGame);
                NotifyGamesUpdated();
                SelectedGame = null;
            }
        }

        private bool CanDeleteGame()
        {
            if (SelectedGame == null) return false;

            var reviewsForGame = _database.GetAllReviews().Any(r => r.Game.Id == SelectedGame.Id);
            return !reviewsForGame;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
