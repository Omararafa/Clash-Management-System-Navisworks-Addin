using Clash_Management_System_Navisworks_Addin.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Clash_Management_System_Navisworks_Addin.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int expandedTrueHeight = 200;
        public int expandedfalseHeight = 25;
        public Brush expanderNormalBackground = Brushes.Gainsboro;
        public Brush expanderNormalForeground = Brushes.DimGray;
        public Brush expanderHighlightBackground = Brushes.Gainsboro;
        public Brush expanderHighlightForeground = Brushes.DimGray;
        List<Expander> SidebarExpanders = new List<Expander>();
        bool IsRunClicked = false;


        public MainWindow()
        {
            InitializeComponent();

            SidebarExpanders.AddRange(new List<Expander>
            {
                SelectProjectExpander,
                SelectFunctionExpander
            });
            WindowGrid.Background = expanderNormalBackground;

            //Set initial values for expanders
            try
            {
                InitialLoginLoadState();
            }
            catch (Exception e)
            {
                string reportContent = "Method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                reportContent += e.Message;
                Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
            }

        }



        private void InitialLoginLoadState()
        {
            Expander expander = SelectProjectExpander;
            ActivateExpander(expander);
            List<Expander> expandersToDeactivate = new List<Expander>();
            expandersToDeactivate.AddRange(SidebarExpanders);
            expandersToDeactivate.Remove(expander);
            foreach (var ex in expandersToDeactivate)
            {
                DeactivateExpander(ex);
            }

            try
            {

                LoginProcedure();
            }
            catch (Exception e)
            {
                string reportContent = "Method Name: Login: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                reportContent += e.Message;
                Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
            }
            ClashMatrixCbx.IsEnabled = false;
            RunBtn.Visibility = Visibility.Hidden;
            StatusBarMessage.Visibility = Visibility.Hidden;
            PresenterDataGrid.Visibility = Visibility.Hidden;
            HeaderDockPanel1.Visibility = Visibility.Hidden;
            HeaderDockPanel2.Visibility = Visibility.Hidden;
        }


        public bool PresentSearchSetsOnDataGrid(DataGrid datagrid, List<ASearchSet> data)
        {
            if (data == null)
            {
                return false;
            }
            if (data.Count < 1)
            {
                return false;
            }
            var checkBoxColumn = datagrid.Columns.First();
            datagrid.Columns.Clear();
            datagrid.Columns.Add(checkBoxColumn);
            datagrid.ItemsSource = data;
            PresenterDataGrid.Visibility = Visibility.Visible;
            datagrid.Columns.ElementAt(0).Visibility = Visibility.Hidden;

            List<string> searchSetBindingProperties = new List<string>
            {
                "Name", "Project.Name", "ClashMatrix.Name"
            };


            foreach (var colProperty in searchSetBindingProperties)
            {
                var col = new DataGridTextColumn();
                string colName = colProperty;
                switch (colProperty)
                {
                    case "Name":
                        colName = "Search Set";
                        break;
                    case "Project.Name":
                        colName = "Project";
                        continue;
                    case "ClashMatrix.Name":
                        colName = "Clash Matrix";
                        continue;
                    default:
                        break;
                }
                col.Header = colName;

                col.Binding = new Binding(colProperty);
                datagrid.Columns.Add(col);
            }

            return true;
        }

        public bool PresentClashTestsOnDataGrid(DataGrid datagrid, ref List<AClashTest> data)
        {
            if (data == null || FunctionClashTestsRBtn.IsChecked == true)
            {
                for (int i = 1; i < datagrid.Columns.Count; i++)
                {
                    datagrid.Columns.RemoveAt(i);
                }
                datagrid.ItemsSource = new List<AClashTest>();
                return true;
            }

            var checkBoxColumn = datagrid.Columns.First();
            datagrid.Columns.Clear();
            datagrid.Columns.Add(checkBoxColumn);
            datagrid.ItemsSource = data;
            PresenterDataGrid.Visibility = Visibility.Visible;
            datagrid.Columns.ElementAt(0).Visibility = Visibility.Hidden;

            if (FunctionClashResultsRBtn.IsChecked == true)
            {
                datagrid.Columns.ElementAt(0).Visibility = Visibility.Visible;
            }



            List<string> clashTestBindingProperties = new List<string>
            {
                "Name","ClashTest.Status", "AClashTestResults.Count","SearchSet1.Name","SearchSet2.Name"
            };

            foreach (var colProperty in clashTestBindingProperties)
            {
                var col = new DataGridTextColumn();
                string colName = colProperty;
                switch (colProperty)
                {
                    case "Name":
                        colName = "Clash Test";
                        break;
                    case "ClashTest.Status":
                        colName = "Status";
                        break;
                    case "AClashTestResults.Count":
                        colName = "Results Count";
                        break;
                    case "SearchSet1.Name":
                        colName = "Search Set 1";
                        break;
                    case "SearchSet2.Name":
                        colName = "Search Set 2";
                        break;
                    default:
                        break;
                }
                col.Header = colName;
                col.Binding = new Binding(colProperty);
                datagrid.Columns.Add(col);
            }
            var uncheckedCount = data.Where(x => !x.IsSelected).Count();
            if (uncheckedCount>0)
            {
                SelectAllChkBox.IsChecked = false;
            }

            return true;

        }

        private void Expander_PreviewMouseUp(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
            if (true)
            {
                Trace.WriteLine("Clicked in expander header");
                List<Expander> expandersToDeactivate = new List<Expander>();
                expandersToDeactivate.AddRange(SidebarExpanders);
                expandersToDeactivate.Remove(expander);
                foreach (var ex in expandersToDeactivate)
                {
                    DeactivateExpander(ex);
                }
            }
        }

        #region View Moderator Methods

        private bool ActivateExpander(Expander expander)
        {
            expander.IsEnabled = true;
            expander.IsExpanded = true;
            //expander.Height = expandedTrueHeight;
            expander.Background = expanderHighlightBackground;
            expander.Foreground = expanderHighlightForeground;
            return true;
        }

        private bool DeactivateExpander(Expander expander)
        {
            expander.IsEnabled = true;
            expander.IsExpanded = false;
            //expander.Height = expandedfalseHeight;
            expander.Background = expanderNormalBackground;
            expander.Foreground = expanderNormalForeground;
            return true;
        }
        private void UpdateFeedbackTextBlock(TextBlock feedbackTextBlock, bool isSuccess)
        {
            /*
            feedbackTextBlock.Visibility = Visibility.Visible;
            if (isSuccess)
            {
                feedbackTextBlock.Text = "Success!";
                feedbackTextBlock.Background = Brushes.Green;

                return;
            }

            feedbackTextBlock.Text = "Invalid User Input!";
            feedbackTextBlock.Background = Brushes.Red;

            return;
            */
        }
        private bool LoginProcedure()
        {
            string userName = Environment.UserName;
            string userDomain = Environment.UserDomainName;

            //TODO deploy: remove two lines below for dynamic login access
            userName = "AMM";
            userDomain = "CIVIL";


            if (userName != string.Empty && userDomain != string.Empty)
            {
                ViewsHandler.CurrentUser = new User(userName, userDomain);
                var projects = ViewsHandler.CurrentUser.Projects;
                string tradeAbb = ViewsHandler.CurrentUser.TradeAbb;

                if (projects == null)
                {
                    System.Windows.Forms.MessageBox.Show("Failed to get projects from database.");
                    return false;
                }

                if (ViewsHandler.CurrentUser.Projects != null || ViewsHandler.CurrentUser.Projects.Count > 0)
                {
                    ActivateExpander(SelectProjectExpander);

                    ObservableCollection<string> projectCbxDataSource = new ObservableCollection<string>
                        (ViewsHandler.CurrentUser.Projects.Select(x => x.Name + ": " + x.Code).ToList());
                    ProjectCbx.ItemsSource = projectCbxDataSource;
                    ProjectCbx.Focus();
                    return true;
                }
            }

            return false;

        }

        /*
        * This method shall be used for the Handle the event of project selection as follows;
        * 1-DB: Get associated clash matrices with this project
        * 2-Assembly: Create and store Project Class
        * 3-UI: Handle the process of activate Clash Matrices expander
        * 4-UI: Handle the process of deactivate project expander
        */
        private bool ProjectSelectedProcedure()
        {
            int currentProjectIndex = ProjectCbx.SelectedIndex;

            if (ViewsHandler.CurrentUserProjects.Count > currentProjectIndex && currentProjectIndex >= 0)
            {
                ViewsHandler.CurrentProject = ViewsHandler.CurrentUser.Projects.ElementAt(currentProjectIndex);
                if (ViewsHandler.CurrentProject != null && ViewsHandler.CurrentProjectClashMatrices != null)
                {
                    HeaderDockPanel1.Visibility = Visibility.Visible;
                    ProjectNameTxtBox.Text = ViewsHandler.CurrentProject.Name;

                    if (ViewsHandler.CurrentProjectClashMatrices.Count > 0)
                    {
                        ClashMatrixCbx.IsEnabled = true;
                        ClashMatrixCbx.ItemsSource = ViewsHandler.CurrentProjectClashMatrices.Select(x => x.Name + ": " + x.Id.ToString());
                        ClashMatrixCbx.Focus();

                        return true;
                    }
                }
            }
            return false;
        }

        /*
        * This method shall be used for the Handle the event of clash matrix selection as follows;
        * 1-DB: Get associated clash tests with this clash matrix
        * 2-Assembly: Create and store clash matrix Class
        * 3-UI: Handle the process of activate select function expander
        * 4-UI: Handle the process of deactivate Clash Matrices expander
        */
        private bool ClashMatrixSelectedProcedure()
        {
            int currentClashMatrixIndex = ClashMatrixCbx.SelectedIndex;

            if (ViewsHandler.CurrentProjectClashMatrices.Count > currentClashMatrixIndex && currentClashMatrixIndex >= 0)
            {


                ViewsHandler.CurrentAClashMatrix = ViewsHandler.CurrentProjectClashMatrices.ElementAt(currentClashMatrixIndex);
                HeaderDockPanel2.Visibility = Visibility.Visible;
                ClashMatrixTxtBox.Text = ViewsHandler.CurrentAClashMatrix.Name;

                if (ViewsHandler.CurrentAClashMatrix != null)
                {
                    ActivateExpander(SelectFunctionExpander);
                    DeactivateExpander(SelectProjectExpander);
                    FunctionSearchSetsRBtn.Focus();

                    return true;
                }
            }

            return false;
        }

        private bool FunctionSelectedProcedure()
        {

            if (FunctionSearchSetsRBtn.IsChecked == true)
            {
                PresenterDataGrid.Visibility = Visibility.Visible;
                try
                {
                    List<ASearchSet> nwSearchSets = NW.NWHandler.NWASearchSets;

                    if (IsRunClicked)
                    {
                        IsRunClicked = false;

                        List<ASearchSet> dbSearchSets = new List<ASearchSet>();
                        List<ASearchSet> comparedSearchSets = new List<ASearchSet>();
                        if (nwSearchSets != null || nwSearchSets.Count > 0)
                        {
                            DB.DBHandler.SyncSearchSetsWithDB(
                                ViewsHandler.CurrentUser,
                                ViewsHandler.CurrentAClashMatrix,
                                ref dbSearchSets,
                                nwSearchSets
                                );
                            if (dbSearchSets.Count > 0)
                            {
                                comparedSearchSets = DBNWHandler.DBNWComparison.ASearchSetsComparisonList;
                                if (comparedSearchSets == null || comparedSearchSets.Count < 1)
                                {
                                    return false;
                                }
                                int newSearchSets = comparedSearchSets.Count();
                                string msg = string.Format("Search Sets: {0} Total synchronized successfully", newSearchSets);
                                System.Windows.Forms.MessageBox.Show(msg, "Clash Management", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                                return PresentSearchSetsOnDataGrid(PresenterDataGrid, comparedSearchSets);
                            }
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return PresentSearchSetsOnDataGrid(PresenterDataGrid, nwSearchSets);
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    return false;
                }


            }
            if (FunctionClashTestsRBtn.IsChecked == true)
            {
                PresenterDataGrid.Visibility = Visibility.Hidden;

                try
                {
                    List<ASearchSet> nwSearchSets = NW.NWHandler.NWASearchSets;
                    if (nwSearchSets == null || nwSearchSets.Count < 1)
                    {
                        return false;
                    }

                    List<AClashTest> nwClashTests = NW.NWHandler.NWAClashTests;

                    if (IsRunClicked)
                    {
                        IsRunClicked = false;

                        List<AClashTest> comparedAClashTests = DBNWHandler.DBNWComparison.CompareNWDBAClashTests();

                        if (comparedAClashTests == null || comparedAClashTests.Count < 1)
                        {
                            System.Windows.Forms.MessageBox.Show("Clash Tests were not Synchronized or not found!");
                            return false;
                        }


                        int comparedClashTestsCount = comparedAClashTests.Count();

                        string msg = string.Format("{0} clash tests have been synchronized successfully:", comparedClashTestsCount);

                        msg += Environment.NewLine;
                        //msg += "Do you want to open the log file?";
                        /*
                        System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(
                            msg, "Clash Management",
                            System.Windows.Forms.MessageBoxButtons.YesNo,
                            System.Windows.Forms.MessageBoxIcon.Information);
                        */

                        System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(
                            msg, "Clash Management",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);



                        System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                        saveFileDialog.DefaultExt = "csv";
                        saveFileDialog.CheckPathExists = true;
                        saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        saveFileDialog.Filter = "Comma Separated Values (*.csv)|*.csv";
                        saveFileDialog.Title = "Save Clash Tests Report";
                        saveFileDialog.FileName = DateTime.Now.ToString("yyyy-dd-M") + " - Clash Tests Report";

                        var saveDialogResult = saveFileDialog.ShowDialog();

                        if (saveDialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            Reporting.ReportHandler.Path = saveFileDialog.FileName;
                            Reporting.ReportHandler.WriteReport(comparedAClashTests);

                            msg = "Do you want to open the log file?";
                            System.Windows.Forms.DialogResult openConfirmationDialogResult = System.Windows.Forms.MessageBox.Show(
                                msg, "Clash Management",
                                System.Windows.Forms.MessageBoxButtons.YesNo,
                                System.Windows.Forms.MessageBoxIcon.Information);
                            if (openConfirmationDialogResult == System.Windows.Forms.DialogResult.Yes)
                            {
                                Process.Start(Reporting.ReportHandler.Path);
                            }
                        }
                        nwClashTests = NW.NWHandler.NWAClashTests;
                        var result = PresentClashTestsOnDataGrid(this.PresenterDataGrid, ref nwClashTests);


                        return result;
                    }

                    return PresentClashTestsOnDataGrid(this.PresenterDataGrid, ref nwClashTests);
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    return false;
                }
            }
            if (FunctionClashResultsRBtn.IsChecked == true)
            {
                PresenterDataGrid.Visibility = Visibility.Visible;
                try
                {
                    /*
                    List<ASearchSet> nwSearchSets = NW.NWHandler.NWASearchSets;
                    if (nwSearchSets == null || nwSearchSets.Count < 1)
                    {
                        return false;
                    }
                    */
                    List<AClashTest> nwClashTests = new List<AClashTest>();
                    if (!IsRunClicked)
                    {
                        nwClashTests = NW.NWHandler.NWAClashTests;
                        if (nwClashTests == null || nwClashTests.Count < 1)
                        {
                            return false;
                        }
                        //Make all clash tests selected by default
                        nwClashTests.ForEach(x => x.IsSelected = true);
                        SelectAllChkBox.IsChecked = true;
                    }

                    if (IsRunClicked)
                    {
                        IsRunClicked = false;

                        ViewsHandler.SelectedClashTests = (PresenterDataGrid.ItemsSource as List<AClashTest>).Where(x => x.IsSelected).ToList();
                        List<AClashTest> selectedClashTests = ViewsHandler.SelectedClashTests;
                        if (selectedClashTests.Count < 1)
                        {
                            System.Windows.Forms.MessageBox.Show(
                                "No Clash Test was selected.",
                                "Error",
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Error);
                            return false;
                        }

                        try
                        {
                            nwClashTests = NW.NWHandler.NWAClashTests;
                            if (nwClashTests == null || nwClashTests.Count < 1)
                            {
                                return false;
                            }
                            bool syncStatus = DB.DBHandler.SyncClashResultToDB(ViewsHandler.CurrentAClashMatrix, selectedClashTests.Where(x => x.AClashTestResults != null).ToList());
                            bool presentStatus = PresentClashTestsOnDataGrid(this.PresenterDataGrid, ref nwClashTests);
                            int newClashTests = selectedClashTests.Count();
                            string msg = string.Format("Clash Tests: {0} Total with valid results were synchronized successfully", selectedClashTests.Where(x => x.AClashTestResults != null).ToList().Count);
                            System.Windows.Forms.MessageBox.Show(
                                msg, "Clash Management",
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);

                            return syncStatus;

                        }
                        catch (Exception e)
                        {
                            System.Windows.Forms.MessageBox.Show("Sync With Database Exception: " + e.Message);
                            return false;
                        }

                    }
                    return PresentClashTestsOnDataGrid(this.PresenterDataGrid, ref nwClashTests);

                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    return false;
                }
            }
            return false;

        }

        private bool RunSyncButton()
        {
            StatusBarMessage.Text = "Sync with database in progress...";
            StatusBarMessage.Visibility = Visibility.Visible;

            IsRunClicked = true;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.DataBind,
               new Action(() =>
               {
                   StatusBarMessage.Text = "Sync with database in progress...";
                   StatusBarMessage.Visibility = Visibility.Visible;
               }));
            //StatusBarMessage.Visibility = Visibility.Visible;
            bool result = FunctionSelectedProcedure();
            return result;
        }


        #endregion


        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            /*
            bool loginStatus = LoginProcedure(sender as Button);
            UpdateFeedbackTextBlock(LoginFeedbackTxt, loginStatus);
            */
        }

        private void SelectProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            /*
            bool selectProjectStatus = ProjectSelectedProcedure();
            UpdateFeedbackTextBlock(ProjectsFeedbackTxt, selectProjectStatus);
            */
        }

        private void SelectClashMatrixBtn_Click(object sender, RoutedEventArgs e)
        {
            /*
            bool selectClashMatrixStatus = ClashMatrixSelectedProcedure();
            UpdateFeedbackTextBlock(ClashMatrixFeedbackTxt, selectClashMatrixStatus);
            return;
            */
        }



        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textBox = sender as TextBox;
                if (textBox.Text.ToLower().StartsWith("enter"))
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Run_Click");

            bool result = RunSyncButton();
            StatusBarMessage.Visibility = Visibility.Hidden;
            Debug.WriteLine("After : Run_Click");

            return;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }


        private void ProjectCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool selectProjectStatus = ProjectSelectedProcedure();
        }

        private void ClashMatrixCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool selectClashMatrixStatus = ClashMatrixSelectedProcedure();
        }

        void UpdateStatusBarMessage(TextBlock textBlock, string message)
        {
            textBlock.Visibility = Visibility.Visible;
            textBlock.Text = message;
            Refresh.RefreshUI(this);
            return;
        }
        bool FunctionSelectionChangedReaction()
        {
            return FunctionSelectedProcedure();

        }
        private void SelectedFunctionChanged(object sender, RoutedEventArgs e)
        {
            string statusBarMessage = "Collecting data from Navisworks in progress...";
            UpdateStatusBarMessage(StatusBarMessage, statusBarMessage);
            RunBtn.Visibility = Visibility.Visible;

            bool selectFunctionStatus = FunctionSelectionChangedReaction();
            if (FunctionClashResultsRBtn.IsChecked == true)
            {
                selectFunctionStatus = true;
            }
            StatusBarMessage.Visibility = Visibility.Hidden;

            if (FunctionClashTestsRBtn.IsChecked == false)
            {
               // UpdateFeedbackTextBlock(FunctionFeedbackTxt, selectFunctionStatus);
            }
            else
            {
                //FunctionFeedbackTxt.Visibility = Visibility.Hidden;
            }

            RunBtn.Focus();
        }

        private void AboutBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO Deploy: Change tool version
            System.Windows.Forms.MessageBox.Show(
                "Clash Management Tool Version: 1.0.0",
                "Clash Management",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Information);

        }


        private void HelpDeskBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO Deploy: Change help desk path
            Process.Start("https://dar.com/error/NotFound");
        }

        private void HelpFileBtn_Click(object sender, MouseButtonEventArgs e)
        {
            //TODO Deploy: Change help file path
            Process.Start("https://dar.com/error/NotFound");
        }

        private void SelectAllBtn_Click(object sender, RoutedEventArgs e)
        {
            var items = PresenterDataGrid.ItemsSource;
            if (items==null)
            {
                return;
            }
            foreach (var item in items)
            {
                AClashTest test = item as AClashTest;
                if (test != null)
                {
                    test.IsSelected = true;
                }

            }
            PresenterDataGrid.ItemsSource = null;
            PresenterDataGrid.ItemsSource = items;
        }

        private void DeselectAllBtn_Click(object sender, RoutedEventArgs e)
        {
            var items = PresenterDataGrid.ItemsSource;
            foreach (var item in items)
            {
                AClashTest test = item as AClashTest;
                if (test != null)
                {
                    test.IsSelected = false;
                }

            }
            PresenterDataGrid.ItemsSource = null;
            PresenterDataGrid.ItemsSource = items;
        }



        private void RunBtn_GotFocus(object sender, RoutedEventArgs e)
        {
            //StatusBarMessage.Visibility = Visibility.Hidden;

            //Refresh.RefreshUI(this);
        }

        private void FunctionClashTestsRBtn_GotFocus(object sender, RoutedEventArgs e)
        {
            StatusBarMessage.Text = "Collecting data from Navisworks in progress...";

            StatusBarMessage.Visibility = Visibility.Visible;
        }

        private void RunBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            StatusBarMessage.Visibility = Visibility.Visible;

        }

        private void RunBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            StatusBarMessage.Visibility = Visibility.Hidden;

        }


        private void RunBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("RunBtn_PreviewMouseLeftButtonDown");
            StatusBarMessage.Text = "Sync with database in progress...";
            StatusBarMessage.Visibility = Visibility.Visible;
        }
    }
    public static class Refresh
    {
        static Action EmptyDelegate = delegate () { };


        public static void RefreshUI(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }

    }
}
