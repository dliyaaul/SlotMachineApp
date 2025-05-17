using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AplikasiAlQur_an
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            while (true)
            {
                using (Form1 form1 = new Form1())
                {
                    Application.Run(form1);
                    if (form1.DialogResult != DialogResult.OK)
                        break; // keluar dari aplikasi
                }

                using (Form2 form2 = new Form2())
                {
                    Application.Run(form2);
                    if (form2.DialogResult != DialogResult.Retry)
                        break; // keluar dari aplikasi
                }
            }
        }
    }
}
