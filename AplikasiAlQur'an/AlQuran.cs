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
    public partial class AlQuran: Form
    {
        public AlQuran()
        {
            InitializeComponent();
            this.Load += AlQuran_Load;
        }

        private List<Surah> allSurah = new List<Surah>();

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Ignore;
            this.Close();
        }

        private void AlQuran_Load(object sender, EventArgs e)
        {
            _ = LoadSurahFromApiAsync(); // abaikan warning karena kita tidak tunggu hasilnya
        }

        private async Task LoadSurahFromApiAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync("https://equran.id/api/surat");

                    allSurah = JsonConvert.DeserializeObject<List<Surah>>(response);

                    listSurah.Items.Clear();
                    foreach (var surah in allSurah)
                    {
                        listSurah.Items.Add($"{surah.nomor}. {surah.nama_latin} - {surah.arti}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal mengambil data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string keyword = txtSearchSurah.Text.Trim().ToLower();
            var filteredSurah = allSurah
            .Where(s => s.nama_latin.ToLower().Contains(keyword))
            .ToList();

            listSurah.Items.Clear();
            foreach (var surah in filteredSurah)
            {
                listSurah.Items.Add($"{surah.nomor}. {surah.nama_latin} - {surah.arti}");
            }

            if (filteredSurah.Count == 0)
            {
                MessageBox.Show("Surah tidak ditemukan.", "Pencarian", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void listSurah_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listSurah.SelectedIndex == -1) return;

            string selected = listSurah.SelectedItem.ToString();
            int dotIndex = selected.IndexOf('.');
            if (dotIndex == -1) return;

            int nomorSurah = int.Parse(selected.Substring(0, dotIndex));

            FormAyat formAyat = new FormAyat(nomorSurah);
            formAyat.ShowDialog(); // tampilkan form baru
        }
    }
}
