using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MCD_MSSQLCRUIDIslemleri
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static string baglanti = @"Server=DESKTOP-TUMHS1A\ESRA; Database=NorthWind; Trusted_connection=True;";
        SqlConnection conn = new SqlConnection(baglanti);
        SqlCommand cmd;

        private void UrunGetir(string sorgu)
        {
            SqlDataAdapter adap = new SqlDataAdapter(sorgu, conn);
            DataTable tablo = new DataTable();
            adap.Fill(tablo);
            dataGridView1.DataSource = tablo;
        }

        private void btnUrunGetir_Click(object sender, EventArgs e)
        {
            string sorgu = "select * from Products";
            UrunGetir(sorgu);
        }

        private void btnGetirId_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from Products where ProductID = @pID", conn);
                cmd.Parameters.AddWithValue("@pID", txtBulId.Text);
                object varMi = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                if (varMi != "")
                {
                    

                    SqlDataAdapter adap = new SqlDataAdapter(cmd);
                    DataTable tablo = new DataTable();
                    adap.Fill(tablo);
                    dataGridView1.DataSource = tablo;
                }
                else
                {
                    MessageBox.Show("ID yok");
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAd.Text != "")
                {
                    conn.Open();
                    cmd = new SqlCommand("insert into Products(ProductName,UnitsInStock,UnitPrice) values(@ProductName, @UnitsInStock, @UnitPrice)", conn);
                        cmd.Parameters.AddWithValue("@ProductName", txtAd.Text);
                        cmd.Parameters.AddWithValue("@UnitsInStock", txtstok.Text);
                        cmd.Parameters.AddWithValue("@UnitPrice", txtFiyat.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Ürün Eklendi..");

                    UrunGetir("select * from products");
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        int id;
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            conn.Open();
            string updateSorgu = "update Products set ProductName = @name, UnitPrice = @uprice, UnitsInStock = @Stock where ProductID = @pID";
            cmd = new SqlCommand(updateSorgu, conn);
            cmd.Parameters.AddWithValue("@pID", id);
            cmd.Parameters.AddWithValue("@name", txtAd.Text);
            cmd.Parameters.AddWithValue("@uprice", txtFiyat.Text);
            cmd.Parameters.AddWithValue("@Stock", txtstok.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Güncellendi...");
            UrunGetir("select * from products");
            conn.Close();

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            txtAd.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtstok.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            txtFiyat.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            conn.Open();
            string deleteSorgu = "delete from products where ProductID = @pID";
            cmd = new SqlCommand(deleteSorgu, conn);
            cmd.Parameters.AddWithValue("@pID", id);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Silindi...");
            UrunGetir("Select * from Products");
            conn.Close();
        }
    }
}
