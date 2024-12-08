﻿using BCSH2_sem.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BCSH2_sem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new BCSH2_sem.ViewModels.MainViewModel();
            var viewModel = DataContext as MainViewModel;

            if (viewModel != null)
            {
                viewModel.ReviewerViewModel.AttachDataGrid(ReviewerDataGrid);
            }
        }
    }
}