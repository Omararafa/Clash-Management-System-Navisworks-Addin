using System;
using System.Collections.Generic;
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
                SelectFunctionExpander,
                TestExpander
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


        public void PresentSearchSetsOnDataGrid(DataGrid datagrid, List<ASearchSet> data)
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
                    searchSet.SearchSetName,
                    searchSet.Project.Name,
                    searchSet.clashMatrix.Name,
                    searchSet.TradeId.ToString(),
                    searchSet.ModifiedBy.ToString(),
                    searchSet.Status.ToString()
                    );
            }
            datagrid.ItemsSource = dataTable.DefaultView;






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

        private bool LoginProcedure(Button Btn)
        {
            /*
             * This method shall be used for the login procedure as follows;
             * 1-DB: Send user name and password to database
             * 2-Return login credentials feedback
             * 3-UI: If not matching: report to user via the text block :LoginFeedbackTxt, Credentials.IsAuthorized=False
             * 4-UI: If yes, set static members of credentials, CurrentUserName, CurrentUserPassword, Credentials.IsAuthorized=True
             * 5-Assembly: Set the variables in step #4 to assembly resources
             * 6-UI: handle the process of activate project expander
             * 7-UI: handle the process of deactivate login expander [or hide it entirely]
             * 8-UI: study to present the username into a text block within the header/footer
             * 9-DB: Get list of projects assigned to the user from the database
             */
            throw new Exception("Method LoginProcedure: Work in progress");
            return false;
        }



        /*
        * This method shall be used for the Handle the event of project selection as follows;
        * 1-DB: Get associated clash matrices with this project
        * 2-Assembly: Create and store Project Class
        * 3-UI: Handle the process of activate Clash Matrices expander
        * 4-UI: Handle the process of deactivate project expander
        */
        private bool ProjectSelectedHandler(Button button)
        {
            throw new Exception("Method ProjectSelectedHandler: Work in progress");
            return false;
        }

        /*
        * This method shall be used for the Handle the event of clash matrix selection as follows;
        * 1-DB: Get associated clash tests with this clash matrix
        * 2-Assembly: Create and store clash matrix Class
        * 3-UI: Handle the process of activate select function expander
        * 4-UI: Handle the process of deactivate Clash Matrices expander
        */
        private bool ClashMatrixSelectedHandler(Button button)
        {
            throw new Exception("Method ClashMatrixSelectedHandler: Work in progress");
            return false;
        }

        /*
        * This method shall be used for the Handle the event of function selection as follows;
        * 
        */
        private bool FunctionSelectedHandler(Button button)
        {
            throw new Exception("Method FunctionSelectedHandler: Work in progress");
            return false;
        }




        #endregion

        private void SelectFunctionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FunctionSearchSetsRBtn.IsChecked == true)
            {
                PresentSearchSetsOnDataGrid(PresenterDataGrid, ViewsHandler.SearchSetsFromNW);
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string userName = UserNameTxt.Text.Trim();
            string userDomain = UserDomainTxt.Text.Trim();

            if (userName != string.Empty && userDomain != string.Empty)
            {
                ViewsHandler.CurrentUser = new User(userName, userDomain);

                if (ViewsHandler.CurrentUser.Projects==null||ViewsHandler.CurrentUser.Projects.Count<1)
                {
                    LoginFeedbackTxt.Text = "Invalid User Input";
                }

                ProjectCbx.ItemsSource = ViewsHandler.CurrentUser.Projects.Select(x=>x.Name+": " +x.Code);

                ActivateExpander(SelectProjectExpander);
            }

            LoginFeedbackTxt.Text = "Invalid User Input";
        }

        //TODO: Delete lines below




    }
}
