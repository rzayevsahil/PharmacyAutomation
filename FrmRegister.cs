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
    public partial class FrmRegister : Form
    {
        public FrmRegister()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-2H5V0KB\\SQLEXPRESS;Initial Catalog=DbPharmacy;Integrated Security=True");

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == txtPasswordRepeat.Text)
            {
                connection.Open();
                SqlCommand command = new SqlCommand("insert into TblPerson (PersonName,PersonSurname,UserName,Password) values (@p1,@p2,@p3,@p4)", connection);
                command.Parameters.AddWithValue("@p1", txtName.Text);
                command.Parameters.AddWithValue("@p2", txtSurname.Text);
                command.Parameters.AddWithValue("@p3", txtUserName.Text);
                command.Parameters.AddWithValue("@p4", txtPassword.Text);
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Başarıyka kaydoldunuz", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Şifreler eşleşmiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmRegister_FormClosed(object sender, FormClosedEventArgs e)
        {
            //FrmLogin frmLogin = new FrmLogin();
            //frmLogin.Show();
        }

        private void eyePassword_Click(object sender, EventArgs e)
        {
            eyePassword.Visible = false;
            eyeClosedPassword.Visible = true;
            txtPassword.UseSystemPasswordChar = false;
        }

        private void eyeClosedPassword_Click(object sender, EventArgs e)
        {
            eyeClosedPassword.Visible = false;
            eyePassword.Visible = true;
            txtPassword.UseSystemPasswordChar = true;
        }

        private void eyeRepeatPassword_Click(object sender, EventArgs e)
        {
            eyeRepeatPassword.Visible = false;
            eyeClosedRepeatPassword.Visible = true;
            txtPasswordRepeat.UseSystemPasswordChar = false;
        }

        private void eyeClosedRepeatPassword_Click(object sender, EventArgs e)
        {
            eyeClosedRepeatPassword.Visible = false;
            eyeRepeatPassword.Visible = true;
            txtPasswordRepeat.UseSystemPasswordChar = true;
        }
    }
}
