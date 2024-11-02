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
    public class GameViewModel : INotifyPropertyChanged
    {
        private LiteDBService _database;
        private Game _selectedGame;

        public ObservableCollection<Game> Games { get; set; }

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
            DeleteGameCommand = new RelayCommand(DeleteGame, () => SelectedGame != null);
        }

        private void AddGame()
        {
            var newGame = new Game("New Game", 0, "Genre", "URL");
            _database.AddGame(newGame);
            Games.Add(newGame);
            SelectedGame = newGame;
        }

        private void UpdateGame()
        {
            if (SelectedGame != null)
            {
                _database.UpdateGame(SelectedGame);
                var index = Games.IndexOf(SelectedGame);
                Games[index] = SelectedGame;
                OnPropertyChanged(nameof(Games));
            }
        }

        private void DeleteGame()
        {
            if (SelectedGame != null)
            {
                _database.DeleteGame(SelectedGame.Id);
                Games.Remove(SelectedGame);
                SelectedGame = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
