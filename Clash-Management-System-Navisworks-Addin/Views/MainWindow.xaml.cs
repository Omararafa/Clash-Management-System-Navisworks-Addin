using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Clash_Management_System_Navisworks_Addin.ViewModels;

namespace Clash_Management_System_Navisworks_Addin.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int expandedTrueHeight = 200;
        public int expandedfalseHeight = 25;
        /*
        public Brush expanderNormalBackground = (Brush)ColorConverter.ConvertFromString("MintCream");
        public Brush expanderNormalForeground = (Brush)ColorConverter.ConvertFromString("LightSlateGray");
        public Brush expanderHighlightBackground = (Brush)ColorConverter.ConvertFromString("MintCream");
        public Brush expanderHighlightForeground = (Brush)ColorConverter.ConvertFromString("LightSlateGray");
        */
        public Brush expanderNormalBackground = Brushes.MintCream;
        public Brush expanderNormalForeground = Brushes.LightSlateGray;
        public Brush expanderHighlightBackground = Brushes.MintCream;
        public Brush expanderHighlightForeground = Brushes.LightSlateGray;
        List<Expander> SidebarExpanders = new List<Expander>();



        public MainWindow()
        {
            InitializeComponent();

            SidebarExpanders.AddRange(new List<Expander>
            {
                LoginExpander,
                SelectProjectExpander,
                SelectClashMatrixExpander,
                SelectFunctionExpander
            });


            //Set initial values for expanders
            InitialLoginLoadState();
        }



        private void InitialLoginLoadState()
        {
            Expander expander = LoginExpander;
            ActivateExpander(expander);
            List<Expander> expandersToDeactivate = new List<Expander>();
            expandersToDeactivate.AddRange(SidebarExpanders);
            expandersToDeactivate.Remove(expander);
            foreach (var ex in expandersToDeactivate)
            {
                DeactivateExpander(ex);
            }

        }


        public bool PresentSearchSetsOnDataGrid(DataGrid datagrid, List<ASearchSet> data)
        {
            DataTable dataTable = new DataTable("Search Sets");

            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Project");
            dataTable.Columns.Add("Clash Matrix");
            dataTable.Columns.Add("Trade Id");
            dataTable.Columns.Add("Modified By");
            dataTable.Columns.Add("Status");

            foreach (var searchSet in data)
            {
                dataTable.Rows.Add(
                    searchSet.Name,
                    searchSet.Project.Name,
                    searchSet.ClashMatrix.Name,
                    searchSet.TradeId.ToString(),
                    searchSet.ModifiedBy.ToString(),
                    searchSet.Conditon.ToString()
                    );
            }
            datagrid.ItemsSource = dataTable.DefaultView;

            return true;
        }

        public bool PresentClashTestsOnDataGrid(DataGrid datagrid, List<AClashTest> data)
        {
            DataTable dataTable = new DataTable("Clash Tests");

            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Condition");
            dataTable.Columns.Add("Id");
            dataTable.Columns.Add("Type Name");
            dataTable.Columns.Add("Tolerance");
            dataTable.Columns.Add("Trade Code");
            dataTable.Columns.Add("Add Time");
            dataTable.Columns.Add("Added By");
            dataTable.Columns.Add("Search Set 1");
            dataTable.Columns.Add("Search Set 2");

            foreach (var test in data)
            {
                dataTable.Rows.Add(
                    test.Name,
                    test.Condition,
                    test.Id,
                    test.TypeName,
                    test.Tolerance,
                    test.TradeCode,
                    test.AddedDate,
                    test.AddedBy,
                    test.SearchSet1.Name,
                    test.SearchSet2.Name
                    );
            }
            datagrid.ItemsSource = dataTable.DefaultView;

            return true;
        }

        private void Expander_PreviewMouseUp(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
            //Other condition to be added: && frameworkElement.Name == "HeaderSite"
            //frameworkElement is ToggleButton 
            if (true)
            {
                Trace.WriteLine("Clicked in expander header");
                //ActivateExpander(expander);
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
        private Expander ExpanderHeaderClicked(Expander expander)
        {
            /*
             *Functions by this method 
             * Enable Expander
             * Disable rest of expanders
             * Change this expander height to 200
             * set other Expanders height to 25
             * Highlight this expander header
             * Normalize other expanders header
             */

            expander.IsEnabled = true;
            expander.Height = 200;

            //Disable rest of expanders
            //Set other Expanders height to 25




            throw new Exception("Method ExpanderHeaderClicked: Work in progress");
            return null;
        }

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
        }
        private bool LoginProcedure(Button Btn)
        {
            /*
             * This method shall be used for the login procedure as follows;
             * 1-DB: Send user name and password to database
             * 2-[Obsolete]Return login credentials feedback
             * 3-UI: If not matching: report to user via the text block :LoginFeedbackTxt, Credentials.IsAuthorized=False
             * 4-UI: If yes, set static members of CurrentUser
             * 5-Assembly: Set the variables in step #4 to assembly resources
             * 6-UI: handle the process of activate project expander
             * 7-UI: handle the process of deactivate login expander [or hide it entirely]
             * 8-UI: study to present the username into a text block within the header/footer
             * 9-DB: Get list of projects assigned to the user from the database
             */

            //TODO: Bary, update data to be written into the Assembly
            string userName = UserNameTxt.Text.Trim();
            string userDomain = UserDomainTxt.Text.Trim();

            if (userName != string.Empty && userDomain != string.Empty)
            {
                ViewsHandler.CurrentUser = new User(userName, userDomain);
                List<Project> projects = ViewsHandler.CurrentUser.Projects;
                string tradeAbb = ViewsHandler.CurrentUser.TradeAbb;


                if (ViewsHandler.CurrentUser.Projects != null || ViewsHandler.CurrentUser.Projects.Count > 0)
                {
                    ActivateExpander(SelectProjectExpander);
                    DeactivateExpander(LoginExpander);
                    ObservableCollection<string> projectCbxDataSource = new ObservableCollection<string>
                        ( ViewsHandler.CurrentUser.Projects.Select(x => x.Name + ": " + x.Code).ToList());
                    ProjectCbx.ItemsSource = projectCbxDataSource;
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
        private bool ProjectSelectedProcedure(Button button)
        {
            //TODO: Bary Assembly: Create and store Project Class
            int currentProjectIndex = ProjectCbx.SelectedIndex;

            if (ViewsHandler.CurrentUserProjects.Count > currentProjectIndex && currentProjectIndex >= 0)
            {
                ViewsHandler.CurrentProject = ViewsHandler.CurrentUser.Projects.ElementAt(currentProjectIndex);
                if (ViewsHandler.CurrentProject != null && ViewsHandler.CurrentProjectClashMatrices != null)
                {
                    if (ViewsHandler.CurrentProjectClashMatrices.Count > 0)
                    {
                        ActivateExpander(SelectClashMatrixExpander);
                        DeactivateExpander(SelectProjectExpander);

                        ClashMatrixCbx.ItemsSource = ViewsHandler.CurrentProjectClashMatrices.Select(x => x.Name + ": " + x.Id.ToString());
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
        private bool ClashMatrixSelectedProcedure(Button button)
        {
            int currentClashMatrixIndex = ClashMatrixCbx.SelectedIndex;

            if (ViewsHandler.CurrentProjectClashMatrices.Count > currentClashMatrixIndex && currentClashMatrixIndex >= 0)
            {


                ViewsHandler.CurrentAClashMatrix = ViewsHandler.CurrentProjectClashMatrices.ElementAt(currentClashMatrixIndex);

                if (ViewsHandler.CurrentAClashMatrix != null )
                {
                    ActivateExpander(SelectFunctionExpander);
                    DeactivateExpander(SelectClashMatrixExpander);
                    return true;
                }
            }

            return false;
        }

        /*
        * This method shall be used for the Handle the event of function selection as follows;
        * 
        */
        private bool FunctionSelectedProcedure(Button button)
        {
            if (FunctionSearchSetsRBtn.IsChecked == true)
            {
                List<ASearchSet> nwSearchSets = NW.NWHandler.NWASearchSets;
                List<ASearchSet> dbSearchSets = new List<ASearchSet>();

                if (nwSearchSets == null|| nwSearchSets.Count>0)
                {
                    DB.DBHandler.SyncSearchSetsWithDB(
                        ViewsHandler.CurrentUser,
                        ViewsHandler.CurrentAClashMatrix,
                        ref dbSearchSets,
                        nwSearchSets
                        );
                    if (dbSearchSets.Count>0)
                    {
                        return PresentSearchSetsOnDataGrid(PresenterDataGrid, DBNWHandler.DBNWComparison.ASearchSetsComparisonList);
                    }
                }
            }
            if (FunctionSearchSetsRBtn.IsChecked == true)
            {
                List<ASearchSet> nwSearchSets = NW.NWHandler.NWASearchSets;
                List<ASearchSet> dbSearchSets = new List<ASearchSet>();
                //TODO: Delete line below // just for testing DataGrid
                PresentSearchSetsOnDataGrid(PresenterDataGrid, nwSearchSets);
                if (nwSearchSets==null|| nwSearchSets.Count > 0)
                {
                    DB.DBHandler.SyncSearchSetsWithDB(
                        ViewsHandler.CurrentUser,
                        ViewsHandler.CurrentAClashMatrix,
                        ref dbSearchSets,
                        nwSearchSets
                        );
                    if (dbSearchSets.Count > 0)
                    {
                        return PresentSearchSetsOnDataGrid(PresenterDataGrid, DBNWHandler.DBNWComparison.ASearchSetsComparisonList);
                    }
                }
            }
            if (FunctionClashTestsRBtn.IsChecked==true)
            {
                List<AClashTest> dbClashTests = DB.DBHandler.DBAClashTests;

            }
            return false;

        }




        #endregion


        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            bool loginStatus = LoginProcedure(sender as Button);
            UpdateFeedbackTextBlock(LoginFeedbackTxt, loginStatus);
        }

        private void SelectProjectBtn_Click(object sender, RoutedEventArgs e)
        {

            bool selectProjectStatus = ProjectSelectedProcedure(sender as Button);
            UpdateFeedbackTextBlock(ProjectsFeedbackTxt, selectProjectStatus);
            return;
        }

        private void SelectClashMatrixBtn_Click(object sender, RoutedEventArgs e)
        {
            bool selectClashMatrixStatus = ClashMatrixSelectedProcedure(sender as Button);
            UpdateFeedbackTextBlock(ClashMatrixFeedbackTxt, selectClashMatrixStatus);
            return;
        }


        private void SelectFunctionBtn_Click(object sender, RoutedEventArgs e)
        {
            bool selectFunctionStatus = FunctionSelectedProcedure(sender as Button);
            UpdateFeedbackTextBlock(FunctionFeedbackTxt, selectFunctionStatus);
            return;

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
    }
}
