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

namespace PharmacyAutomation
{
    public partial class FrmMedicineAndDetail : Form
    {
        public FrmMedicineAndDetail()
        {
            InitializeComponent();
        }
        public string personID;
        public string username;
        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-2H5V0KB\\SQLEXPRESS;Initial Catalog=DbPharmacy;Integrated Security=True");

        public void CategoryList()
        {
            SqlCommand command = new SqlCommand("Select * From TblMedicineCategory", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryID";
            cmbCategory.DataSource = dataTable;
        }

        public void MedicineList()
        {
            SqlCommand command = new SqlCommand("Select MedicineID,MedicineName,Quantity,Stock,ConsumptionDate,Country,PurchasePrice,SalePrice,CategoryName,TblMedicine.Situation from TblMedicine inner join TblMedicineCategory on TblMedicine.CategoryID=TblMedicineCategory.CategoryID where TblMedicine.Situation=1", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void FrmSeller_Load(object sender, EventArgs e)
        {
            MedicineList();
            CategoryList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("insert into TblMedicine (MedicineName,Quantity,Stock,ConsumptionDate,Country,PurchasePrice,SalePrice,Situation,CategoryID) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)", connection);
            command.Parameters.AddWithValue("@p1", txtMedicineName.Text);
            command.Parameters.AddWithValue("@p2", txtQuantity.Text);
            command.Parameters.AddWithValue("@p3", txtMedicineStock.Text);
            command.Parameters.AddWithValue("@p4", DateTime.Parse(dtpConsumptionDate.Text));
            command.Parameters.AddWithValue("@p5", txtCountry.Text);
            command.Parameters.AddWithValue("@p6", txtPurchasePrice.Text);
            command.Parameters.AddWithValue("@p7", txtSalePrice.Text);
            command.Parameters.AddWithValue("@p9", cmbCategory.SelectedValue);
            if (rdbActive.Checked == true)
            {
                command.Parameters.AddWithValue("@p8", "True");
            }
            if (rdbPassive.Checked == true)
            {
                command.Parameters.AddWithValue("@p8", "False");
            }
            command.ExecuteNonQuery();
            MessageBox.Show("İlaç başarılı bir şekilde sisteme kaydedildi");
            connection.Close();
            MedicineList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Update TblMedicine set Situation=0 where  MedicineID=@p1", connection);
            command.Parameters.AddWithValue("@p1", txtMedicineID.Text);
            command.ExecuteNonQuery();
            MessageBox.Show("Ürün sistemden başarılı bir şekilde silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            connection.Close();
            MedicineList();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
            FrmAdminDashboard frmAdminDashboard = new FrmAdminDashboard();
            frmAdminDashboard.personID = personID;
            frmAdminDashboard.username = username;
            frmAdminDashboard.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            txtMedicineID.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            txtMedicineName.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            txtQuantity.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            txtMedicineStock.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            dtpConsumptionDate.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            txtCountry.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();
            txtPurchasePrice.Text = dataGridView1.Rows[secilen].Cells[6].Value.ToString();
            txtSalePrice.Text = dataGridView1.Rows[secilen].Cells[7].Value.ToString();
            MessageBox.Show(dataGridView1.Rows[secilen].Cells[9].Value.ToString());
            if (Convert.ToBoolean(dataGridView1.Rows[secilen].Cells[9].Value.ToString()))
            {
                rdbActive.Checked = true;
            }
            else
            {
                rdbPassive.Checked = true;
            }
            connection.Open();
            SqlCommand command =
                new SqlCommand("select * from TblMedicineCategory where CategoryID=(Select CategoryID From TblMedicine Where MedicineID=@p1)", connection);
            command.Parameters.AddWithValue("@p1", dataGridView1.Rows[secilen].Cells[0].Value.ToString());
            SqlDataReader dataReader2 = command.ExecuteReader();
            while (dataReader2.Read())
            {
                cmbCategory.SelectedValue = Int32.Parse(dataReader2["CategoryID"].ToString());
            }
            connection.Close();
        }

        private void txtSearchMedicine_TextChanged(object sender, EventArgs e)
        {
            if (txtSearchMedicine.Text != "" && txtSearchMedicine.Text != "İlaç ismi giriniz...")
            {
                SqlCommand command =
                    new SqlCommand(
                        "Select MedicineID,MedicineName,Quantity,Stock,ConsumptionDate,Country,PurchasePrice,SalePrice,CategoryName,TblMedicine.Situation from TblMedicine inner join TblMedicineCategory on TblMedicine.CategoryID=TblMedicineCategory.CategoryID where TblMedicine.Situation=1 and MedicineName Like '" + txtSearchMedicine.Text + "%'",
                        connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            else
            {
                MedicineList();
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("update TblMedicine set MedicineName=@p1,Quantity=@p2,Stock=@p3,ConsumptionDate=@p4,Country=@p5,PurchasePrice=@p6,SalePrice=@p7,Situation=@p8,CategoryID=@p9 where MedicineID=@p10", connection);
            command.Parameters.AddWithValue("@p1", txtMedicineName.Text);
            command.Parameters.AddWithValue("@p2", txtQuantity.Text);
            command.Parameters.AddWithValue("@p3", Convert.ToInt32(txtMedicineStock.Text));
            command.Parameters.AddWithValue("@p4", DateTime.Parse(dtpConsumptionDate.Text));
            command.Parameters.AddWithValue("@p5", txtCountry.Text);
            command.Parameters.AddWithValue("@p6", Convert.ToDecimal(txtPurchasePrice.Text));
            command.Parameters.AddWithValue("@p7", Convert.ToDecimal(txtSalePrice.Text));
            command.Parameters.AddWithValue("@p9", cmbCategory.SelectedValue);
            command.Parameters.AddWithValue("@p10", txtMedicineID.Text);
            if (rdbActive.Checked == true)
            {
                command.Parameters.AddWithValue("@p8", "True");
            }
            if (rdbPassive.Checked == true)
            {
                command.Parameters.AddWithValue("@p8", "False");
            }
            command.ExecuteNonQuery();
            MessageBox.Show("İlaç başarılı bir şekilde güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            connection.Close();
            MedicineList();
        }
    }
}
