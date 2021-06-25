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
    }
}
