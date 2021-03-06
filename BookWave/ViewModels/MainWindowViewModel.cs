﻿using BookWave.Desktop;
using BookWave.Desktop.Models.AudiobookManagement;
using BookWave.Desktop.Models.MenuNavigation;
using BookWave.Desktop.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookWave.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Public Properties

        private string mSelectedPageTitle;
        public string SelectedPageTitle
        {
            get { return mSelectedPageTitle; }
            set { Set<string>(() => this.SelectedPageTitle, ref mSelectedPageTitle, value); }
        }

        private ControlTemplate mSharedPageTitleTemplate;
        public ControlTemplate SharedPageTitleTemplate
        {
            get { return mSharedPageTitleTemplate; }
            set { Set<ControlTemplate>(() => this.SharedPageTitleTemplate, ref mSharedPageTitleTemplate, value); }
        }

        private HistoryList<PageItem> mNavigationHistory;
        public HistoryList<PageItem> NavigationHistory
        {
            get { return mNavigationHistory; }
            set { mNavigationHistory = value; }
        }

        private bool mDarkenBackground;
        public bool DarkenBackground
        {
            get { return mDarkenBackground; }
            set { Set<bool>(() => this.DarkenBackground, ref mDarkenBackground, value); }
        }

        private MainWindow mMainWindow;

        public MainWindow MainWindow
        {
            get { return mMainWindow; }
            set { mMainWindow = value; }
        }


        public PlayerViewModel PlayerViewModel
        {
            get { return ViewModelLocator.Instance.PlayerViewModel; }
        }


        #endregion

        #region Commands

        /// <summary>
        /// The command to minimize the window
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize the window
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// The command to close the window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            SelectedPageTitle = string.Empty;
            NavigationHistory = new HistoryList<PageItem>();
        }

        #endregion

        #region Methods

        public void SetupBorderlessWindow(Window window)
        {
            mWindow = window;

            // Listen out for the window resizing
            mWindow.StateChanged += (sender, e) =>
            {
                // Fire off events for all properties that are affected by a resize
                WindowResized();
            };

            window.Height = WindowMinimumHeight;
            window.Width = WindowMinimumWidth;

            // Create commands
            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => mWindow.Close());

            // Fix window resize issue
            var resizer = new WindowResizer(mWindow);

            // Listen out for dock changes
            resizer.WindowDockChanged += (dock) =>
            {
                // Store last position
                mDockPosition = dock;

                // Fire off resize events
                WindowResized();
            };
        }

        public void SwitchToEditLibraryPage(Audiobook audiobook)
        {
            EditLibraryViewModel editLibraryViewModel = ViewModelLocator.Instance.EditLibraryViewModel;
            editLibraryViewModel.Audiobook = audiobook;

            MainWindow.SwitchPage(new PageItem(MainWindow.btnEditLibrary));
        }

        public void SwitchToSplitChapterPage(Chapter chapter)
        {
            SplitChapterViewModel splitChapterViewModel = ViewModelLocator.Instance.SplitChapterViewModel;
            splitChapterViewModel.AudioFilePath = chapter.AudioPath.Path;

            MainWindow.OpenInvisiblePage("/Views/Pages/SplitChapterPage.xaml", "Split Chapter");
        }

        #endregion

        #region Private Look Properties

        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        private int mOuterMarginSize = 3;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;

        #endregion

        #region Public Look Properties

        /// <summary>
        /// The smallest width the window can go to
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 855;

        /// <summary>
        /// The smallest height the window can go to
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 620;

        /// <summary>
        /// True if the window should be borderless because it is docked or maximized
        /// </summary>
        public bool Borderless { get { return (mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked); } }

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int ResizeBorder { get; set; } = 1;

        /// <summary>
        /// The size of the resize border around the window, taking into account the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder + OuterMarginSize); } }

        /// <summary>
        /// The padding of the inner content of the main window
        /// </summary>
        public Thickness InnerContentPadding { get { return new Thickness(ResizeBorder); } }

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public int OuterMarginSize
        {
            get
            {
                // If it is maximized or docked, no border
                return Borderless ? 0 : mOuterMarginSize;
            }
            set
            {
                mOuterMarginSize = value;
            }
        }

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } }

        #endregion

        #region Look Helper Methods

        /// <summary>
        /// Gets the current mouse position on the screen
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            // Position of the mouse relative to the window
            var position = Mouse.GetPosition(mWindow);

            // Add the window position so its a "ToScreen"
            return new Point(position.X + mWindow.Left, position.Y + mWindow.Top);
        }

        /// <summary>
        /// If the window resizes to a special position (docked or maximized)
        /// this will update all required property change events to set the borders and radius values
        /// </summary>
        private void WindowResized()
        {
            // Fire off events for all properties that are affected by a resize
            RaisePropertyChanged(nameof(Borderless));
            RaisePropertyChanged(nameof(ResizeBorderThickness));
            RaisePropertyChanged(nameof(OuterMarginSize));
            RaisePropertyChanged(nameof(OuterMarginSizeThickness));
        }


        #endregion

    }
}
