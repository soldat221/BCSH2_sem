using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCSH2_sem.ViewModels
{
    public class MainViewModel
    {
        public GameViewModel GameViewModel { get; }
        public ReviewViewModel ReviewViewModel { get; }
        public ReviewerViewModel ReviewerViewModel { get; }

        public MainViewModel()
        {
            ReviewerViewModel = new ReviewerViewModel();
            GameViewModel = new GameViewModel();
            ReviewViewModel = new ReviewViewModel(ReviewerViewModel, GameViewModel);
        }
    }
}
