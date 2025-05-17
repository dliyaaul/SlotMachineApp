using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AplikasiAlQur_an
{
    public partial class FormAyat: Form
    {
        private int nomorSurah;

        public FormAyat(int nomorSurah)
        {
            InitializeComponent();
            this.nomorSurah = nomorSurah;
            this.Load += FormAyat_Load;
        }

        private async void FormAyat_Load(object sender, EventArgs e)
        {
            await LoadSurahAsync();
        }

        private async Task LoadSurahAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"https://equran.id/api/surat/{nomorSurah}";
                    string response = await client.GetStringAsync(url);

                    SurahDetail detail = JsonConvert.DeserializeObject<SurahDetail>(response);

                    label1.Text = $"{detail.nama_latin} - {detail.nama}";

                    richAyat.Clear();
                    foreach (var ayat in detail.ayat)
                    {
                        richAyat.SelectionFont = new Font("Segoe UI", 12, FontStyle.Bold);
                        richAyat.AppendText($"[{ayat.nomor}]\n");

                        richAyat.SelectionFont = new Font("Traditional Arabic", 16, FontStyle.Regular);
                        richAyat.SelectionAlignment = HorizontalAlignment.Right;
                        richAyat.AppendText($"{ayat.ar}\n");

                        richAyat.SelectionAlignment = HorizontalAlignment.Left;
                        richAyat.SelectionFont = new Font("Segoe UI", 10, FontStyle.Italic);
                        richAyat.AppendText($"{ayat.idn}\n\n");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal mengambil ayat: " + ex.Message);
                }
            }
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if(nomorSurah < 114)
            {
                nomorSurah++;
                await LoadSurahAsync();
            }
            else
            {
                MessageBox.Show("Ini adalah surah terakhir.");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (nomorSurah > 1)
            {
                nomorSurah--;
                await LoadSurahAsync();
            }
            else
            {
                MessageBox.Show("Ini adalah surah pertama.");
            }         
        }
    }
}
