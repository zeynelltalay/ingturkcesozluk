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
using System.Drawing.Text;


namespace ingturkcesozluk
{

    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=ZETUS\\SQLZETUS;Initial Catalog=ingturkce;Integrated Security=True;");

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                SqlCommand silkomutu = new SqlCommand("delete from sozluk where ing ='" + textBox1.Text + "'", baglanti);
                silkomutu.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("Sözcük veritabanından silindi", "Veri Tabanı İşlemleri");
                textBox1.Clear();
                textBox2.Clear();
                listBox1.Items.Clear();
            }
            catch (Exception aciklama)
            {

                MessageBox.Show(aciklama.Message, "Veritabanı İşlemleri");
                baglanti.Close();
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                SqlCommand guncellesorgu = new SqlCommand("insert  into sozluk (ing,türkce) values ('" + textBox1.Text + "','" + textBox2.Text + "')", baglanti);
                guncellesorgu.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("Sözcük  Veri Tabanına Eklendi ...", "Veri Tabanı İşlemleri");
                textBox1.Clear();
                textBox2.Clear();


            }
            catch (Exception aciklama)
            {
                MessageBox.Show(aciklama.Message, "Veri Tabanı İşlemleri");
                baglanti.Close();
            }




        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                baglanti.Open();
                SqlCommand guncellesorgu = new SqlCommand("update sozluk set türkce ='" + textBox2.Text + "' where ing ='" + textBox1.Text + "'", baglanti);
                guncellesorgu.ExecuteNonQuery();


                MessageBox.Show("Sözcük  Veri Tabanında Güncellendi...", "Veri Tabanı İşlemleri");
                textBox1.Clear();
                textBox2.Clear();

            }
            catch (Exception aciklama)
            {
                MessageBox.Show(aciklama.Message, "Veri Tabanı İşlemleri");
                baglanti.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                baglanti.Open();
                SqlCommand aramasorgu = new SqlCommand("select ing,türkce from sozluk where ing like'" + textBox1.Text + "%'", baglanti);

                SqlDataReader read = aramasorgu.ExecuteReader();

                while (read.Read())
                {
                    listBox1.Items.Add(read["ing"].ToString() + "=" + read["türkce"].ToString());
                }
                baglanti.Close();
            }
            catch (Exception aciklama)
            {
                MessageBox.Show(aciklama.Message, "veri tabanı işlemleri");
                baglanti.Close();

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand vericekme = new SqlCommand("SELECT TOP 1 * FROM sozluk ORDER BY NEWID();", baglanti);
            SqlDataReader reader = vericekme.ExecuteReader();

            try
            {
                if (reader.Read())
                {
                    listBox1.Items.Add(reader["ing"].ToString() + " = " + reader["türkce"].ToString());

                }


                baglanti.Close();
                reader.Close();
            }
            catch (Exception aciklama)
            {

                MessageBox.Show(aciklama.Message, "Veri Tabanı İşlemleri");
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {



            string ingilizceKelime = textBox1.Text.Trim();  // İlk TextBox'tan kelimeyi al
            string cumle = textBox2.Text.Trim();             // İkinci TextBox'tan cümleyi al

            // Eğer kelime ve cümle boş değilse işlemi yap
            if (!string.IsNullOrEmpty(ingilizceKelime) && !string.IsNullOrEmpty(cumle))
            {

                baglanti.Open();

                // 1. Adım: Veritabanındaki kelimeyi bul ve cümleyi güncelle
                string updateQuery = "UPDATE sozluk SET cumle = @cumle WHERE ing = @ingilizceKelime";
                SqlCommand updateCmd = new SqlCommand(updateQuery, baglanti);
                updateCmd.Parameters.AddWithValue("@cumle", cumle);
                updateCmd.Parameters.AddWithValue("@ingilizceKelime", ingilizceKelime);

                int rowsAffected = updateCmd.ExecuteNonQuery();  // Güncelleme işlemi yap ve kaç satır etkilendiğini kontrol et
                baglanti.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Cümle başarıyla güncellendi!");
                    textBox1.Clear();
                    textBox2.Clear();
                }
                else
                {
                    MessageBox.Show("İlgili kelime bulunamadı.");
                }

            }
            else
            {
                MessageBox.Show("Lütfen hem kelimeyi hem de cümleyi girin.");
            }

        }
    }
}
// there are more columns in the ınsert statement than values specified in the values clause. the number of values in the values clause must match the number of columns specified in the ınsert statement.