using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PharmacyAutomation
{
    public partial class FrmAdminDashboard : Form
    {
        public FrmAdminDashboard()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-2H5V0KB\\SQLEXPRESS;Initial Catalog=DbPharmacy;Integrated Security=True");

        public string username;
        public string personID;

        static string myApi = "2cd512a81a207c80a98260dea6a3d0e9";
        private static string city = "istanbul";
        static string conc =
            "https://api.openweathermap.org/data/2.5/weather?q=" + city + "&mode=xml&lang=tr&units=metric&appid=" + myApi;
        static XDocument weather = XDocument.Load(conc);
        string temp = weather.Descendants("temperature").ElementAt(0).Attribute("value").Value;
        string weatherstate = weather.Descendants("weather").ElementAt(0).Attribute("value").Value;


        public void SellerCount()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select COUNT(*) From TblSeller", connection);
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                lblSellerCount.Text = dataReader[0].ToString();
            }
            connection.Close();
        }
        public void SalesCount()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select COUNT(*) From TblSales", connection);
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                lblSalesCount.Text = dataReader[0].ToString();
            }
            connection.Close();
        }
        public void MoneyEarned()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("select SUM(Gain) from TblSales", connection);
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                lblMoneyEarned.Text = dataReader[0].ToString() + " TL";
            }
            connection.Close();
        }
        public void MedicineCount()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select COUNT(*) From TblMedicine", connection);
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                lblMedicineCount.Text = dataReader[0].ToString();
            }
            connection.Close();
        }
        public void SalesMedicineCount()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select SUM(MedicineCount) from TblSalesSellerMedicine", connection);
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                lblSalesMedicineCount.Text = dataReader[0].ToString();
            }
            connection.Close();
        }
        public void Gain()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select SUM(MedicineCount*(SalePrice-PurchasePrice)) from TblMedicine inner join TblSalesSellerMedicine on TblMedicine.MedicineID=TblSalesSellerMedicine.MedicineID", connection);
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                lblGain.Text = dataReader[0].ToString() + " TL";
            }
            connection.Close();
        }

        public void Notification()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select Count(*) from TblPerson where PersonID not in (select PersonID from TblPersonAuthority)", connection);
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                lblNotification.Text = dataReader[0].ToString();
            }
            connection.Close();
        }

        private void FrmAdminDashboard_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblWeather.Text = temp + " " + weatherstate;
            lblCity.Text = city.ToUpper();
            timer2.Enabled = true;
            SqlCommand command = new SqlCommand("Select * From ");
            lblUserName.Text = username;
            SalesCount();
            SellerCount();
            MoneyEarned();
            MedicineCount();
            SalesMedicineCount();
            Gain();
            Notification();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToShortDateString();
            lblTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (label13.Left > -295)
            {
                label13.Left -= 3;
            }
            else
            {
                label13.Left = 13;
            }

            if (lblUserName.Text.Length > 5)
            {
                if (lblUserName.Left > 30)
                {
                    lblUserName.Left -= 1;
                }
                else
                {
                    lblUserName.Left = 59;
                }
            }

        }

        private void SignOut_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            FrmEditProfile frmEditProfile = new FrmEditProfile();
            frmEditProfile.personID = personID;
            frmEditProfile.Show();
            Close();
        }

        private void btnSellerInfo_Click(object sender, EventArgs e)
        {
            
        }

        private void btnMedicineDetails_Click(object sender, EventArgs e)
        {
            FrmMedicineAndDetail frmMedicineAndDetail = new FrmMedicineAndDetail();
            frmMedicineAndDetail.personID = personID;
            frmMedicineAndDetail.username = username;
            frmMedicineAndDetail.Show();
            Close();
        }

        private void lblNotification_Click(object sender, EventArgs e)
        {
            FrmAuthorize frmAuthorize = new FrmAuthorize();
            frmAuthorize.personID = personID;
            frmAuthorize.username = username;
            frmAuthorize.Show();
            Close();
        }
    }
}