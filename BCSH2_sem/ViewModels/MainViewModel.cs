using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCSH2_sem.ViewModels
{
    public class MainViewModel
    {
        public GameViewModel GameViewModel { get; set; }
        public ReviewViewModel ReviewViewModel { get; set; }
        public ReviewerViewModel ReviewerViewModel { get; set; }

        public MainViewModel()
        {
            GameViewModel = new GameViewModel();
            ReviewViewModel = new ReviewViewModel();
            ReviewerViewModel = new ReviewerViewModel();
        }
    }
}
