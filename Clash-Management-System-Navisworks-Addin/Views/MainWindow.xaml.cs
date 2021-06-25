using System;
using System.Collections.Generic;
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

namespace Clash_Management_System_Navisworks_Addin.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int expandedTrueHeight = 200;
        public int expandedfalseHeight = 25;
        public Brush expanderNormalBackground = (Brush)ColorConverter.ConvertFromString("MintCream");
        public Brush expanderNormalForeground = (Brush)ColorConverter.ConvertFromString("LightSlateGray");
        public Brush expanderHighlightBackground = (Brush)ColorConverter.ConvertFromString("MintCream");
        public Brush expanderHighlightForeground = (Brush)ColorConverter.ConvertFromString("LightSlateGray");
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginExpander_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Expander expander = sender as Expander;
            FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
            if (frameworkElement is ToggleButton && frameworkElement.Name == "HeaderSite")
            {
                Trace.WriteLine("Clicked in expander header");
                expander.IsEnabled = true;
                expander.IsExpanded = true;
                expander.Height = expandedTrueHeight;
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
             * 
             */
            throw new Exception("Method ExpanderHeaderClicked: Work in progress");
            return null;
        }

        private bool ActivateExpander(Expander expander)
        {
            throw new Exception("Method ActivateExpander: Work in progress");
            return false;
        }

        private bool DeactivateExpander(Expander expander)
        {
            throw new Exception("Method DeactivateExpander: Work in progress");
            return false;
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


    }
}
