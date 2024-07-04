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

namespace Otobüs
{
    public partial class OtobusBilet : Form
    {
        public OtobusBilet()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-71IKNM6\\SQLEXPRESS;Initial Catalog=OtobusBilet;Integrated Security=True");

        private void cmbOtobüs_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbOtobus.Text)
            {
                case "Buzlu": KoltukDoldur(6, false); break;
                case "KamilKoç": KoltukDoldur(10, true); break;
                case "Metro": KoltukDoldur(8, true); break;
                case "Kale": KoltukDoldur(7, false); break;
            }
            void KoltukDoldur(int sira, bool arkaBesliMi)
            {
            yavaslat:
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Button)
                    {
                        Button btn = ctrl as Button;
                        if (btn.Text == "Kaydet")
                        {
                            continue;
                        }
                        else
                        {
                            this.Controls.Remove(ctrl);
                            goto yavaslat;
                        }
                    }
                }
                int koltukNo = 1;
                for (int i = 0; i < sira; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (arkaBesliMi == true)
                        {
                            if (i != sira - 1 && j == 2)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            if (j == 2)
                                continue;
                        }
                        Button koltuk = new Button();
                        koltuk.Height = koltuk.Width = 40;
                        koltuk.Top = 30 + (i * 45);
                        koltuk.Left = 5 + (j * 45);
                        koltuk.Text = koltukNo.ToString();
                        koltukNo++;
                        koltuk.ContextMenuStrip = contextMenuStrip1;
                        koltuk.MouseDown += Koltuk_MouseDown;
                        this.Controls.Add(koltuk);
                    }


                }

            }
        }
        Button tiklanan;
        private void Koltuk_MouseDown(object sender, MouseEventArgs e)
        {
            tiklanan = sender as Button;
        }

        private void rezerveEtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cmbOtobus.SelectedIndex == -1 || cmbNereden.SelectedIndex == -1 || cmbNereye.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen önce gerekli alanları doldurunuz. ");
                return;
            }
            YolcuKayıt yk = new YolcuKayıt();
            DialogResult sonuc = yk.ShowDialog();
            if (sonuc == DialogResult.OK)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into Tb_Otbus (Ad,Soyad,Telefon,cinsiyet,Nereden,Nereye,KoltukNo,Tarih,Fiyat) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)", baglanti);
                komut.Parameters.AddWithValue("@p1", yk.txtAd.Text);
                komut.Parameters.AddWithValue("@p2", yk.txtSoyad.Text);
                komut.Parameters.AddWithValue("@p3", yk.mskTelefon.Text);               
                if (yk.rdnErkek.Checked)
                {
                    label4.Text = "True";
                    komut.Parameters.AddWithValue("@p4", label4.Text);
                }
                if (yk.rdnKadın.Checked)
                {
                    label5.Text = "False";
                    komut.Parameters.AddWithValue("@p4", label4.Text);
                }
                komut.Parameters.AddWithValue("@p5", cmbNereden.Text);
                komut.Parameters.AddWithValue("@p6", cmbNereye.Text);
                komut.Parameters.AddWithValue("@p7", tiklanan.Text);
                komut.Parameters.AddWithValue("@p8", dtpTarih.Text);
                komut.Parameters.AddWithValue("@p9", nudFiyat.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("yeni kayıt eklendi");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'otobusBiletDataSet.Tb_Otobus' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            //this.tb_OtobusTableAdapter.Fill(this.otobusBiletDataSet.Tb_Otobus);

        }
    }
}

