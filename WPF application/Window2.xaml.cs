using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WPF_application;
using System.Collections.ObjectModel;

namespace WPF_application
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window2 : Window
    {

        private readonly MainWindow MyParentWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

        

        public int MyID;

        public string RegBOSSConn;

        public Window2()
        {
            InitializeComponent();

            var MySettingsBuilder = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("connsettings.json", optional: true, reloadOnChange: true)
        .Build();

        RegBOSSConn = MySettingsBuilder.GetValue<string>("RegBOSS");

   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string MyUS = USTXB2.Text;
            string MyDIN = DINTXB2.Text;
            string MyUK = UKTXB2.Text;
            string MyJIS = JISTXB2.Text;
            string MyClassification = CTXB2.Text;


            SqlConnection MyConnection = null;
            SqlCommand MyCommand;
            MyConnection = new SqlConnection(RegBOSSConn);
            MyConnection.Open();

            


            try
            {
                MyCommand = new SqlCommand("UPDATE CONVERT_Material SET US = @US,DIN = @DIN,UK = @UK,JIS = @JIS,Classification = @Classification Where MaterialID = '" + MyID + "'", MyConnection);

                if (string.IsNullOrEmpty(MyUS))
                {
                    MyCommand.Parameters.AddWithValue("@US", DBNull.Value);
                }
                else
                {
                    MyCommand.Parameters.AddWithValue("@US", MyUS);
                }

                if (string.IsNullOrEmpty(MyDIN))
                {
                    MyCommand.Parameters.AddWithValue("@DIN", DBNull.Value);
                }
                else
                {
                    MyCommand.Parameters.AddWithValue("@DIN", MyDIN);
                }

                if (string.IsNullOrEmpty(MyUK))
                {
                    MyCommand.Parameters.AddWithValue("@UK", DBNull.Value);
                }
                else
                {
                    MyCommand.Parameters.AddWithValue("@UK", MyUK);
                }

                if (string.IsNullOrEmpty(MyJIS))
                {
                    MyCommand.Parameters.AddWithValue("@JIS", DBNull.Value);
                }
                else
                {
                    MyCommand.Parameters.AddWithValue("@JIS", MyJIS);
                }

                if (string.IsNullOrEmpty(MyClassification))
                {
                    MyCommand.Parameters.AddWithValue("@Classification", DBNull.Value);
                }
                else
                {
                    MyCommand.Parameters.AddWithValue("@Classification", MyClassification);
                }

                MyCommand.ExecuteNonQuery();

                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

           

            MyConnection.Close();
            MyParentWindow.Bring_Back_Number();
            this.Close();
            
           

        }
    }
}

