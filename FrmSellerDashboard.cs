using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PharmacyAutomation
{
    public partial class FrmSellerDashboard : Form
    {
        public FrmSellerDashboard()
        {
            InitializeComponent();
        }
        public string username;
        public string personID;
        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-2H5V0KB\\SQLEXPRESS;Initial Catalog=DbPharmacy;Integrated Security=True");
        static string myApi = "2cd512a81a207c80a98260dea6a3d0e9";
        private static string city = "istanbul";
        static string conc =
            "https://api.openweathermap.org/data/2.5/weather?q=" + city + "&mode=xml&lang=tr&units=metric&appid=" + myApi;
        static XDocument weather = XDocument.Load(conc);
        string temp = weather.Descendants("temperature").ElementAt(0).Attribute("value").Value;
        string weatherstate = weather.Descendants("weather").ElementAt(0).Attribute("value").Value;

        public void MedicineList()
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("Select MedicineID, MedicineName, Quantity, Stock, ConsumptionDate, Country, PurchasePrice, SalePrice, CategoryName, TblMedicine.Situation from TblMedicine inner join TblMedicineCategory on TblMedicine.CategoryID = TblMedicineCategory.CategoryID where TblMedicine.Situation = 1", connection);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dataTable.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[1].Value = item["MedicineID"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["MedicineName"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["Quantity"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["Stock"].ToString();
                dataGridView1.Rows[n].Cells[5].Value = item["ConsumptionDate"].ToString();
                dataGridView1.Rows[n].Cells[6].Value = item["SalePrice"].ToString();
                dataGridView1.Rows[n].Cells[7].Value = item["CategoryName"].ToString();
            }
        }
        private void FrmSellerDashboard_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Enabled = true;
            lblWeather.Text = temp + " " + weatherstate;
            lblCity.Text = city.ToUpper();
            lblUserName.Text = username;
            MedicineList();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            lblDate.Text = DateTime.Now.ToShortDateString();
            lblTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (lblWeather.Text.Length >= 4)
            {
                if (lblWeather.Left > 0)
                {
                    lblWeather.Left -= 3;
                }
                else
                {
                    lblWeather.Left = 151;
                }
            }
        }

        private void txtSearchMedicine_Enter(object sender, EventArgs e)
        {
            if (txtSearchMedicine.Text == "İlaç ismi giriniz...")
            {
                txtSearchMedicine.Text = "";
            }
        }

        private void txtSearchMedicine_Leave(object sender, EventArgs e)
        {
            if (txtSearchMedicine.Text == "")
            {
                txtSearchMedicine.Text = "İlaç ismi giriniz...";
            }
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            FrmEditProfile frmEditProfile = new FrmEditProfile();
            frmEditProfile.personID = personID;
            frmEditProfile.Show();
            Close();
        }

        private void SignOut_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtSearchMedicine_TextChanged(object sender, EventArgs e)
        {
            if (txtSearchMedicine.Text != "" && txtSearchMedicine.Text != "İlaç ismi giriniz...")
            {
                SqlCommand command =
                    new SqlCommand(
                        "Select * From TblMedicine where MedicineName Like '" + txtSearchMedicine.Text + "%'",
                        connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.Rows.Clear();
                foreach (DataRow item in dataTable.Rows)
                {
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[1].Value = item["MedicineID"].ToString();
                    dataGridView1.Rows[n].Cells[2].Value = item["MedicineName"].ToString();
                    dataGridView1.Rows[n].Cells[3].Value = item["Quantity"].ToString();
                    dataGridView1.Rows[n].Cells[4].Value = item["Stock"].ToString();
                    dataGridView1.Rows[n].Cells[5].Value = item["ConsumptionDate"].ToString();
                    dataGridView1.Rows[n].Cells[6].Value = item["SalePrice"].ToString();
                }
            }
            else
            {
                MedicineList();
            }
        }

        private void btnSelectedCopy_Click(object sender, EventArgs e)
        {
            copyData();
        }
        double total = 0;
        private Dictionary<string, string> medicineIDsAndCount = new Dictionary<string, string>();
        private int stock = 0;
        private int stock1 = 0;
        public void copyData()
        {
            foreach (DataGridViewRow drv in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(drv.Cells[0].Value))
                {
                    if (txtMedicinePiece.Text == "")
                    {
                        txtMedicinePiece.Text = "1";
                    }
                    int n = dataGridView2.Rows.Add();
                    dataGridView2.Rows[n].Cells[0].Value = drv.Cells[1].Value.ToString();
                    dataGridView2.Rows[n].Cells[1].Value = drv.Cells[2].Value.ToString();
                    dataGridView2.Rows[n].Cells[2].Value = drv.Cells[3].Value.ToString();
                    dataGridView2.Rows[n].Cells[3].Value = drv.Cells[6].Value.ToString();
                    dataGridView2.Rows[n].Cells[4].Value = txtMedicinePiece.Text;
                    int secilen = dataGridView2.SelectedCells[0].RowIndex;
                    if (medicineIDsAndCount.ContainsKey(drv.Cells[1].Value.ToString()))
                    {
                        
                        stock1 = int.Parse(medicineIDsAndCount[drv.Cells[1].Value.ToString()]);
                        stock1 += Int32.Parse(txtMedicinePiece.Text);
                        connection.Open();
                        SqlCommand command2 = new SqlCommand("select Stock from TblMedicine where MedicineID=@p1", connection);
                        command2.Parameters.AddWithValue("@p1", drv.Cells[1].Value.ToString());
                        SqlDataReader reader = command2.ExecuteReader();
                        while (reader.Read())
                        {
                            stock = int.Parse(reader[0].ToString());
                        }
                        connection.Close();
                        if (stock1 > stock)
                        {
                            MessageBox.Show("Yeteri kadar ilaç sayısı depoda yok, ilaçtan " + stock + " adet kaldı", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridView2.Rows.RemoveAt(n);
                        }
                        else
                        {
                            total += Convert.ToDouble(drv.Cells[6].Value.ToString()) * Int32.Parse(txtMedicinePiece.Text);
                            lblTotal.Text = total.ToString();
                            medicineIDsAndCount[drv.Cells[1].Value.ToString()] = (Int32.Parse(medicineIDsAndCount[drv.Cells[1].Value.ToString()]) + Int32.Parse(txtMedicinePiece.Text)).ToString();
                            stock1 = 0;
                        }
                    }
                    else
                    {
                        connection.Open();
                        SqlCommand command2 = new SqlCommand("select Stock from TblMedicine where MedicineID=@p1", connection);
                        command2.Parameters.AddWithValue("@p1", drv.Cells[1].Value.ToString());
                        SqlDataReader reader = command2.ExecuteReader();
                        while (reader.Read())
                        {
                            stock = int.Parse(reader[0].ToString());
                        }
                        connection.Close();
                        if (int.Parse(txtMedicinePiece.Text) > stock)
                        {
                            dataGridView2.Rows.RemoveAt(n);
                            MessageBox.Show("Yeteri kadar ilaç sayısı depoda yok, ilaçtan " + stock + " adet kaldı", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            total += Convert.ToDouble(drv.Cells[6].Value.ToString()) * Int32.Parse(txtMedicinePiece.Text);
                            lblTotal.Text = total.ToString();
                            medicineIDsAndCount.Add(drv.Cells[1].Value.ToString(), txtMedicinePiece.Text);
                        }
                    }
                    drv.Cells[0].Value = false;
                    txtMedicinePiece.Text = "";
                }
            }
        }

        private void txtPaidMoney_TextChanged(object sender, EventArgs e)
        {
            if (txtPaidMoney.Text != "")
            {
                if (lblTotal.Text != "0")
                {
                    lblReturnMoney.Text = (Convert.ToDouble(txtPaidMoney.Text) - Convert.ToDouble(lblTotal.Text)).ToString();
                }
                else
                {
                    lblTotal.Text = "0";
                    lblReturnMoney.Text = "0";
                }
            }
            else
            {
                lblReturnMoney.Text = "0";
            }
        }

        public void Sales()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("insert into TblSales (Gain) values (@p1)", connection);
            command.Parameters.AddWithValue("@p1", Convert.ToDouble(lblTotal.Text));
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void SalesSellerMedicine()
        {
            foreach (var m in medicineIDsAndCount)
            {
                connection.Open();
                SqlCommand command = new SqlCommand("insert into TblSalesSellerMedicine (SalesID,SellerID,MedicineID,MedicineCount,Situation) values((Select Top(1) SalesID from TblSales order by Date desc),@p1,@p2,@p3,1)", connection);
                command.Parameters.AddWithValue("@p1", personID);
                command.Parameters.AddWithValue("@p2", m.Key);
                command.Parameters.AddWithValue("@p3", m.Value);
                command.ExecuteNonQuery();
                connection.Close();
                connection.Open();
                SqlCommand command1 = new SqlCommand("Update TblMedicine set Stock=Stock-@p2 where MedicineID=@p1", connection);
                command1.Parameters.AddWithValue("@p1", m.Key);
                command1.Parameters.AddWithValue("@p2", m.Value);
                command1.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Satış başarıyla gerçekleştirildi");
            medicineIDsAndCount.Clear();
            lblTotal.Text = "0";
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DeleteMedicine();
        }

        public void DeleteMedicine()
        {
            total = Convert.ToDouble(lblTotal.Text);
            int secilen = dataGridView2.SelectedCells[0].RowIndex;
            total -= Convert.ToDouble(dataGridView2.Rows[secilen].Cells[3].Value.ToString()) * Int32.Parse(dataGridView2.Rows[secilen].Cells[4].Value.ToString());
            lblTotal.Text = total.ToString();
            medicineIDsAndCount.Remove(dataGridView2.Rows[secilen].Cells[0].Value.ToString());
            dataGridView2.Rows.RemoveAt(secilen);
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            Sales();
            SalesSellerMedicine();
            lblTotal.Text = "0";
            txtPaidMoney.Text = "0";
            lblReturnMoney.Text = "0";
            dataGridView2.Rows.Clear();
            MedicineList();
        }

        private void lblTotal_TextChanged(object sender, EventArgs e)
        {
            if (txtPaidMoney.Text != "")
            {
                lblReturnMoney.Text = (Convert.ToDouble(txtPaidMoney.Text) - Convert.ToDouble(lblTotal.Text)).ToString();
            }
        }
    }
}
