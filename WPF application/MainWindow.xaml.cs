using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using System.Printing;
using System.IO;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Win32;
using WPF_application;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using Path = System.IO.Path;



namespace WPF_application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // setting dictionaries and boolean flags 
        #region

        public ObservableCollection<MatRef> MatRefCollection { get; set; }
        public ObservableCollection<Hardness> HardnessCollection { get; set; }
        public ObservableCollection<Surface> SurfaceCollection { get; set; }

        public string RegBOSSConn; //holders for database connection strings
        public string RegBOSSConn2;

        public bool flag1 = false; // these are for window ones radio buttons (flag1 and 2)
        public bool flag2 = false;
        public bool flag3 = true; //These flags are for Maximizing the window and setting back to normal if they are already set (flag3 and 4)
        public bool flag4 = true;

        private readonly PaletteHelper MyPaletteHelper = new(); //This is used for setting the window to dark mode
        public Dictionary<string, string> MySettingsDict = new(); //This is used to save the settings of the application
        #endregion

        public MainWindow()
        {

            InitializeComponent();

            //Starting Connections for settings and adding the dictionaries
            #region


            MatRefCollection = new ObservableCollection<MatRef>();
            HardnessCollection = new ObservableCollection<Hardness>();
            SurfaceCollection = new ObservableCollection<Surface>();


            var MySettingsBuilder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) //These are used for the settings 
            .AddJsonFile("connsettings.json", optional: true, reloadOnChange: true)
            .Build();

            MySettingsDict.Add("Theme", MySettingsBuilder.GetValue<string>("Theme")); // These are used for the settings
            MySettingsDict.Add("TAB", MySettingsBuilder.GetValue<string>("TAB"));
            RegBOSSConn = MySettingsBuilder.GetValue<string>("RegBOSS");

            DataContext = this;

            Bring_Back_Number2();
            Bring_Back_Number3();



            #endregion

            //Loading Settings 
            #region

            if (MySettingsDict["Theme"] == "dark") // this is the main method for loading the settings for light and dark mode
            {
                tog_LightDark.IsChecked = true;
                ChangeThemeMode();
            }
            else
            {
                tog_LightDark.IsChecked = false;
                ChangeThemeMode();
            }

            if (MySettingsDict["TAB"] == "Tab1") // This is used for loading the settings of the last tab opened
            {
                TopTab.SelectedItem = Tab1;
            }
            if (MySettingsDict["TAB"] == "Tab2")
            {
                TopTab.SelectedItem = Tab2;
            }
            if (MySettingsDict["TAB"] == "Tab3")
            {
                TopTab.SelectedItem = Tab3;
            }
            if (MySettingsDict["TAB"] == "Tab4")
            {
                TopTab.SelectedItem = Tab4;
            }

            #endregion
        }


        // Window Operations
        #region
        private void MoveWindow(object sender, MouseButtonEventArgs e) //This section is just for the window movement when moving the window
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
            if (Top < SystemParameters.VirtualScreenTop) { Top = SystemParameters.VirtualScreenTop; }
            if (Left < SystemParameters.VirtualScreenLeft) { Left = SystemParameters.VirtualScreenLeft; }
            if (Left + Width > SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth) { Left = SystemParameters.VirtualScreenWidth + SystemParameters.VirtualScreenLeft - Width; }
            if (Top + Height > SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight) { Top = SystemParameters.VirtualScreenHeight + SystemParameters.VirtualScreenTop - Height; }
        }

        private void MinWindow(object sender, RoutedEventArgs e) //this is for minimizing the window (since we are making our own window)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        private void MaxWindow(object sender, RoutedEventArgs e) // This is for the maximization of the window and setting the window back to normal if pressing when the window is already maximized
        {
            if (flag1 == false && flag4 == true)
            {
                WindowState = System.Windows.WindowState.Maximized;
                flag1 = true;
            }
            else
            {
                WindowState = System.Windows.WindowState.Normal;
                flag1 = false;
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)  // this is just to make the window closed since we are making our own window
        {
            Close();
        }

        private void ChangeThemeMode() // this is for changing the window into dark mode
        {
            bool isDark;
            if (tog_LightDark.IsChecked == true) isDark = true; else { isDark = false; }
            ITheme theme = MyPaletteHelper.GetTheme();
            IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            MyPaletteHelper.SetTheme(theme);
        }

        private void ToggleDark(object sender, RoutedEventArgs e) //this is also for making the window change into dark mode
        {
            if (tog_LightDark.IsChecked == true) MySettingsDict["Theme"] = "dark"; else { MySettingsDict["Theme"] = "light"; }
            SaveSettings();
            ChangeThemeMode();
        }

        #endregion  window operations   

        // Tab1 Operations
        #region
        private void RadioButton_Click(object sender, RoutedEventArgs e) //These two radio buttons are for choosing inches to mm or mm to inches
        {
            flag2 = true;
            flag3 = false;
            TXBCN1.Clear();
        }

        private void RadioButton_Click1(object sender, RoutedEventArgs e)
        {
            flag3 = true;
            flag2 = false;
            TXBCN1.Clear();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) //these is for the button that does the actual conversion
        {
            if (TXBCN1.Text != "")
            {
                if (flag2 == true && flag3 == false)
                {
                    double CNU = 25.4;
                    // String CN1 = TXBCN1.Text;
                    Double CN1 = Convert.ToDouble(TXBCN1.Text);
                    double end_number1 = (CN1 / CNU);
                    TXBCN2.Text = end_number1.ToString();

                }
                else if (flag2 == false && flag3 == true)
                {
                    double CNU = 25.4;
                    // String CN1 = TXBCN1.Text;
                    Double CN1 = Convert.ToDouble(TXBCN1.Text);
                    double end_number1 = (CN1 * CNU);
                    TXBCN2.Text = end_number1.ToString();
                }
                else if (flag3 == false && flag2 == false) //These is to make sure the app doesn't close incase a incorrect input
                {
                    MessageBox.Show("Error please hit one of the buttons");
                }
            }
            else
            {
                MessageBox.Show("Please enter a number");
            }

        }

        #endregion

        // Tab2 Operations
        #region

        public void Bring_Back_Number2()
        {
            MatRefCollection.Clear();
            SqlDataReader MyReader = null;
            SqlConnection MyConnection = null;
            SqlCommand MyCommand;
            HardnessCollection.Add(new Hardness(0, "", ""));
            try
            {
                MyConnection = new SqlConnection(RegBOSSConn);
                MyCommand = new SqlCommand("SELECT * From CONVERT_Hardness", MyConnection);
                MyConnection.Open();
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                    if (MyReader.HasRows)
                    {
                        int MyID = (int)MyReader["HardnessID"];
                        string MyKEY = MyReader["HardnessKEY"].ToString();
                        string MyValue = MyReader["HardnessVALUE"].ToString();


                        HardnessCollection.Add(new Hardness(MyID, MyKEY, MyValue));
                    }
                if (HardnessCollection.Count == 0)
                {
                    MessageBox.Show("No data found");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("it is catching");
            }
            finally
            {
                if (MyConnection.State == ConnectionState.Open)
                    MyConnection.Close();
            }
        }

        #endregion

        // Tab3 Operations
        #region


        public void Bring_Back_Number3()
        {
            SurfaceCollection.Clear();
            SqlDataReader MyReader = null;
            SqlConnection MyConnection = null;
            SqlCommand MyCommand;
            SurfaceCollection.Add(new Surface(0, "",""));
            try
            {
                MyConnection = new SqlConnection(RegBOSSConn);
                MyCommand = new SqlCommand("SELECT * From CONVERT_Surface", MyConnection);
                MyConnection.Open();
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                    if (MyReader.HasRows)
                    {
                        int MyID = (int)MyReader["SurfaceID"];
                        string MyKEY = MyReader["SurfaceKEY"].ToString();
                        string MyValue = MyReader["SurfaceVALUE"].ToString();


                        SurfaceCollection.Add(new Surface(MyID, MyKEY, MyValue));
                    }
                if (SurfaceCollection.Count == 0)
                {
                    MessageBox.Show("No data found");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("it is catching");
            }
            finally
            {
                if (MyConnection.State == ConnectionState.Open)
                    MyConnection.Close();
            }
        }






        //private void RadioButton_Click_11(object sender, RoutedEventArgs e)
        //{
        //    Dictionary<Double, string> Conversion_Table2 = new Dictionary<Double, string>(); //This method uses another dictionary to link data to eachother each of these dictionaries in this method are the same

        //    Conversion_Table2.Add(0.025, "1 Microinch");
        //    Conversion_Table2.Add(0.05, "2 Microinches");
        //    Conversion_Table2.Add(0.1, "4 Microinches");
        //    Conversion_Table2.Add(0.2, "8 Microinches");
        //    Conversion_Table2.Add(0.4, "16 Microinches");
        //    Conversion_Table2.Add(0.8, "32 Microinches");
        //    Conversion_Table2.Add(1.6, "63 Microinches");
        //    Conversion_Table2.Add(3.2, "125 Microinches");
        //    Conversion_Table2.Add(6.3, "250 Microinches");
        //    Conversion_Table2.Add(12.5, "500 Microinches");
        //    Conversion_Table2.Add(25, "1000 Microinches");
        //    Conversion_Table2.Add(50, "2000 Microinches");

        //    TXBCN6.Text = Conversion_Table2[Convert.ToDouble(Button21.Content)]; //// This is so the dictionary will understand what the user selected and outputing it
        //}
        //private void RadioButton_Click_12(object sender, RoutedEventArgs e)
        //{
        //    Dictionary<Double, string> Conversion_Table2 = new Dictionary<Double, string>();

        //    Conversion_Table2.Add(0.025, "1 Microinch");
        //    Conversion_Table2.Add(0.05, "2 Microinches");
        //    Conversion_Table2.Add(0.1, "4 Microinches");
        //    Conversion_Table2.Add(0.2, "8 Microinches");
        //    Conversion_Table2.Add(0.4, "16 Microinches");
        //    Conversion_Table2.Add(0.8, "32 Microinches");
        //    Conversion_Table2.Add(1.6, "63 Microinches");
        //    Conversion_Table2.Add(3.2, "125 Microinches");
        //    Conversion_Table2.Add(6.3, "250 Microinches");
        //    Conversion_Table2.Add(12.5, "500 Microinches");
        //    Conversion_Table2.Add(25, "1000 Microinches");
        //    Conversion_Table2.Add(50, "2000 Microinches");

        //    TXBCN6.Text = Conversion_Table2[Convert.ToDouble(Button22.Content)];
        //}
        //private void RadioButton_Click_13(object sender, RoutedEventArgs e)
        //{
        //    Dictionary<Double, string> Conversion_Table2 = new Dictionary<Double, string>(); 

        //    Conversion_Table2.Add(0.025, "1 Microinch");
        //    Conversion_Table2.Add(0.05, "2 Microinches");
        //    Conversion_Table2.Add(0.1, "4 Microinches");
        //    Conversion_Table2.Add(0.2, "8 Microinches");
        //    Conversion_Table2.Add(0.4, "16 Microinches");
        //    Conversion_Table2.Add(0.8, "32 Microinches");
        //    Conversion_Table2.Add(1.6, "63 Microinches");
        //    Conversion_Table2.Add(3.2, "125 Microinches");
        //    Conversion_Table2.Add(6.3, "250 Microinches");
        //    Conversion_Table2.Add(12.5, "500 Microinches");
        //    Conversion_Table2.Add(25, "1000 Microinches");
        //    Conversion_Table2.Add(50, "2000 Microinches");

        //    TXBCN6.Text = Conversion_Table2[Convert.ToDouble(Button23.Content)];
        //}
        //private void RadioButton_Click_14(object sender, RoutedEventArgs e)
        //{
        //    Dictionary<Double, string> Conversion_Table2 = new Dictionary<Double, string>(); 

        //    Conversion_Table2.Add(0.025, "1 Microinch");
        //    Conversion_Table2.Add(0.05, "2 Microinches");
        //    Conversion_Table2.Add(0.1, "4 Microinches");
        //    Conversion_Table2.Add(0.2, "8 Microinches");
        //    Conversion_Table2.Add(0.4, "16 Microinches");
        //    Conversion_Table2.Add(0.8, "32 Microinches");
        //    Conversion_Table2.Add(1.6, "63 Microinches");
        //    Conversion_Table2.Add(3.2, "125 Microinches");
        //    Conversion_Table2.Add(6.3, "250 Microinches");
        //    Conversion_Table2.Add(12.5, "500 Microinches");
        //    Conversion_Table2.Add(25, "1000 Microinches");
        //    Conversion_Table2.Add(50, "2000 Microinches");

        //    TXBCN6.Text = Conversion_Table2[Convert.ToDouble(Button24.Content)];
        //}
        //private void RadioButton_Click_15(object sender, RoutedEventArgs e)
        //{
        //    Dictionary<Double, string> Conversion_Table2 = new Dictionary<Double, string>(); 

        //    Conversion_Table2.Add(0.025, "1 Microinch");
        //    Conversion_Table2.Add(0.05, "2 Microinches");
        //    Conversion_Table2.Add(0.1, "4 Microinches");
        //    Conversion_Table2.Add(0.2, "8 Microinches");
        //    Conversion_Table2.Add(0.4, "16 Microinches");
        //    Conversion_Table2.Add(0.8, "32 Microinches");
        //    Conversion_Table2.Add(1.6, "63 Microinches");
        //    Conversion_Table2.Add(3.2, "125 Microinches");
        //    Conversion_Table2.Add(6.3, "250 Microinches");
        //    Conversion_Table2.Add(12.5, "500 Microinches");
        //    Conversion_Table2.Add(25, "1000 Microinches");
        //    Conversion_Table2.Add(50, "2000 Microinches");

        //    TXBCN6.Text = Conversion_Table2[Convert.ToDouble(Button25.Content)];
        //}
        //private void RadioButton_Click_16(object sender, RoutedEventArgs e)
        //{
        //    Dictionary<Double, string> Conversion_Table2 = new Dictionary<Double, string>(); 

        //    Conversion_Table2.Add(0.025, "1 Microinch");
        //    Conversion_Table2.Add(0.05, "2 Microinches");
        //    Conversion_Table2.Add(0.1, "4 Microinches");
        //    Conversion_Table2.Add(0.2, "8 Microinches");
        //    Conversion_Table2.Add(0.4, "16 Microinches");
        //    Conversion_Table2.Add(0.8, "32 Microinches");
        //    Conversion_Table2.Add(1.6, "63 Microinches");
        //    Conversion_Table2.Add(3.2, "125 Microinches");
        //    Conversion_Table2.Add(6.3, "250 Microinches");
        //    Conversion_Table2.Add(12.5, "500 Microinches");
        //    Conversion_Table2.Add(25, "1000 Microinches");
        //    Conversion_Table2.Add(50, "2000 Microinches");

        //    TXBCN6.Text = Conversion_Table2[Convert.ToDouble(Button26.Content)];
        //}
        //private void RadioButton_Click_17(object sender, RoutedEventArgs e)
        //{
        //    Dictionary<Double, string> Conversion_Table2 = new Dictionary<Double, string>(); 

        //    Conversion_Table2.Add(0.025, "1 Microinch");
        //    Conversion_Table2.Add(0.05, "2 Microinches");
        //    Conversion_Table2.Add(0.1, "4 Microinches");
        //    Conversion_Table2.Add(0.2, "8 Microinches");
        //    Conversion_Table2.Add(0.4, "16 Microinches");
        //    Conversion_Table2.Add(0.8, "32 Microinches");
        //    Conversion_Table2.Add(1.6, "63 Microinches");
        //    Conversion_Table2.Add(3.2, "125 Microinches");
        //    Conversion_Table2.Add(6.3, "250 Microinches");
        //    Conversion_Table2.Add(12.5, "500 Microinches");
        //    Conversion_Table2.Add(25, "1000 Microinches");
        //    Conversion_Table2.Add(50, "2000 Microinches");

        //    TXBCN6.Text = Conversion_Table2[Convert.ToDouble(Button27.Content)];
        //}
        //private void RadioButton_Click_18(object sender, RoutedEventArgs e)
        //{
        //    Dictionary<Double, string> Conversion_Table2 = new Dictionary<Double, string>(); 

        //    Conversion_Table2.Add(0.025, "1 Microinch");
        //    Conversion_Table2.Add(0.05, "2 Microinches");
        //    Conversion_Table2.Add(0.1, "4 Microinches");
        //    Conversion_Table2.Add(0.2, "8 Microinches");
        //    Conversion_Table2.Add(0.4, "16 Microinches");
        //    Conversion_Table2.Add(0.8, "32 Microinches");
        //    Conversion_Table2.Add(1.6, "63 Microinches");
        //    Conversion_Table2.Add(3.2, "125 Microinches");
        //    Conversion_Table2.Add(6.3, "250 Microinches");
        //    Conversion_Table2.Add(12.5, "500 Microinches");
        //    Conversion_Table2.Add(25, "1000 Microinches");
        //    Conversion_Table2.Add(50, "2000 Microinches");

        //    TXBCN6.Text = Conversion_Table2[Convert.ToDouble(Button28.Content)];
        //}
        //private void RadioButton_Click_19(object sender, RoutedEventArgs e)
        //{
        //    Dictionary<Double, string> Conversion_Table2 = new Dictionary<Double, string>(); 

        //    Conversion_Table2.Add(0.025, "1 Microinch");
        //    Conversion_Table2.Add(0.05, "2 Microinches");
        //    Conversion_Table2.Add(0.1, "4 Microinches");
        //    Conversion_Table2.Add(0.2, "8 Microinches");
        //    Conversion_Table2.Add(0.4, "16 Microinches");
        //    Conversion_Table2.Add(0.8, "32 Microinches");
        //    Conversion_Table2.Add(1.6, "63 Microinches");
        //    Conversion_Table2.Add(3.2, "125 Microinches");
        //    Conversion_Table2.Add(6.3, "250 Microinches");
        //    Conversion_Table2.Add(12.5, "500 Microinches");
        //    Conversion_Table2.Add(25, "1000 Microinches");
        //    Conversion_Table2.Add(50, "2000 Microinches");

        //    TXBCN6.Text = Conversion_Table2[Convert.ToDouble(Button29.Content)];
        //}
        //private void RadioButton_Click_20(object sender, RoutedEventArgs e)
        //{
        //    Dictionary<Double, string> Conversion_Table2 = new Dictionary<Double, string>(); 

        //    Conversion_Table2.Add(0.025, "1 Microinch");
        //    Conversion_Table2.Add(0.05, "2 Microinches");
        //    Conversion_Table2.Add(0.1, "4 Microinches");
        //    Conversion_Table2.Add(0.2, "8 Microinches");
        //    Conversion_Table2.Add(0.4, "16 Microinches");
        //    Conversion_Table2.Add(0.8, "32 Microinches");
        //    Conversion_Table2.Add(1.6, "63 Microinches");
        //    Conversion_Table2.Add(3.2, "125 Microinches");
        //    Conversion_Table2.Add(6.3, "250 Microinches");
        //    Conversion_Table2.Add(12.5, "500 Microinches");
        //    Conversion_Table2.Add(25, "1000 Microinches");
        //    Conversion_Table2.Add(50, "2000 Microinches");

        //    TXBCN6.Text = Conversion_Table2[Convert.ToDouble(Button30.Content)];
        //}
        //private void RadioButton_Click_21(object sender, RoutedEventArgs e)
        //{
        //    Dictionary<Double, string> Conversion_Table2 = new Dictionary<Double, string>(); 

        //    Conversion_Table2.Add(0.025, "1 Microinch");
        //    Conversion_Table2.Add(0.05, "2 Microinches");
        //    Conversion_Table2.Add(0.1, "4 Microinches");
        //    Conversion_Table2.Add(0.2, "8 Microinches");
        //    Conversion_Table2.Add(0.4, "16 Microinches");
        //    Conversion_Table2.Add(0.8, "32 Microinches");
        //    Conversion_Table2.Add(1.6, "63 Microinches");
        //    Conversion_Table2.Add(3.2, "125 Microinches");
        //    Conversion_Table2.Add(6.3, "250 Microinches");
        //    Conversion_Table2.Add(12.5, "500 Microinches");
        //    Conversion_Table2.Add(25, "1000 Microinches");
        //    Conversion_Table2.Add(50, "2000 Microinches");

        //    TXBCN6.Text = Conversion_Table2[Convert.ToDouble(Button31.Content)];
        //}
        //private void RadioButton_Click_22(object sender, RoutedEventArgs e)
        //{
        //    Dictionary<Double, string> Conversion_Table2 = new Dictionary<Double, string>(); 

        //    Conversion_Table2.Add(0.025, "1 Microinch");
        //    Conversion_Table2.Add(0.05, "2 Microinches");
        //    Conversion_Table2.Add(0.1, "4 Microinches");
        //    Conversion_Table2.Add(0.2, "8 Microinches");
        //    Conversion_Table2.Add(0.4, "16 Microinches");
        //    Conversion_Table2.Add(0.8, "32 Microinches");
        //    Conversion_Table2.Add(1.6, "63 Microinches");
        //    Conversion_Table2.Add(3.2, "125 Microinches");
        //    Conversion_Table2.Add(6.3, "250 Microinches");
        //    Conversion_Table2.Add(12.5, "500 Microinches");
        //    Conversion_Table2.Add(25, "1000 Microinches");
        //    Conversion_Table2.Add(50, "2000 Microinches");

        //    TXBCN6.Text = Conversion_Table2[Convert.ToDouble(Button32.Content)];







        #endregion

        // Tab4 Operations
        #region
        public void Bring_Back_Number()
        {

            MatRefCollection.Clear();
            SqlDataReader MyReader = null;
            SqlConnection MyConnection = null;
            SqlCommand MyCommand;
            try
            {
                MyConnection = new SqlConnection(RegBOSSConn);
                MyCommand = new SqlCommand("SELECT * From CONVERT_Material Where US = '" + TXBCN7.Text + "' or DIN = '" + TXBCN7.Text + "' or UK = '" + TXBCN7.Text + "' or JIS= '" + TXBCN7.Text + "' or Classification Like '" + TXBCN7.Text + "'", MyConnection);
                MyConnection.Open();
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                    if (MyReader.HasRows)
                    {
                        int MyMaterialID = (int)MyReader["MaterialID"];
                        string MyUS = MyReader["US"].ToString();
                        string MyDIN = MyReader["DIN"].ToString();
                        string MyUK = MyReader["UK"].ToString();
                        string MyJIS = MyReader["JIS"].ToString();
                        string MyClassification = MyReader["Classification"].ToString();

                        MatRefCollection.Add(new MatRef(MyMaterialID, MyUS, MyDIN, MyUK, MyJIS, MyClassification));
                    }
                if (MatRefCollection.Count == 0)
                {
                    MessageBox.Show("No data found");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (MyConnection.State == ConnectionState.Open)
                    MyConnection.Close();
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Bring_Back_Number();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            TXBCN7.Clear();
            MatRefCollection.Clear();
        }

        public void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                Window1 MyWindow = new Window1 { Owner = this };
                MyWindow.ShowDialog();
            }
            catch
            {
                MessageBox.Show("dang welp");
            }

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {
                Window2 MyWindow2 = new Window2 { Owner = this };
                MyWindow2.MyID = MatRefCollection[0].ID;
                MyWindow2.USTXB2.Text = MatRefCollection[0].US;
                MyWindow2.DINTXB2.Text = MatRefCollection[0].DIN;
                MyWindow2.UKTXB2.Text = MatRefCollection[0].UK;
                MyWindow2.JISTXB2.Text = MatRefCollection[0].JIS;
                MyWindow2.CTXB2.Text = MatRefCollection[0].Classification;
                MyWindow2.ShowDialog();

            }
            catch
            {
                MessageBox.Show("Please search a number");
            }


        }
        #endregion

        // Saving settings
        #region





        public void SaveSettings() //This is used for saving settings by writing the current setting to a json file 
        {
            var jsonWriteOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var newJson = JsonSerializer.Serialize(MySettingsDict, jsonWriteOptions);
            var appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            File.WriteAllText(appSettingsPath, newJson);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) //This is the saving of the settings for the tab selection
        {
            if (Tab1.IsSelected == true)
            {
                MySettingsDict["TAB"] = "Tab1";
                SaveSettings();
            }
            if (Tab2.IsSelected == true)
            {
                MySettingsDict["TAB"] = "Tab2";
                SaveSettings();
            }
            if (Tab3.IsSelected == true)
            {
                MySettingsDict["TAB"] = "Tab3";
                SaveSettings();
            }
            if (Tab4.IsSelected == true)
            {
                MySettingsDict["TAB"] = "Tab4";
                SaveSettings();
            }

        }

    }
        #endregion








}   


