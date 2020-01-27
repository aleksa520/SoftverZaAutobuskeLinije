using Domen;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Server
{
    public partial class FrmServer : Form
    {
        BindingList<Linija> linije = new BindingList<Linija>();
        public FrmServer()
        {
            InitializeComponent();
        }
        Server s;

        private void FrmServer_Load(object sender, EventArgs e)
        {
            s = new Server();
            if (s.pokreniServer())
            {
                linije = Broker.Instance.PovuciSveLinije();
                foreach (Linija l in linije)
                {
                    l.Medjustanice = Broker.Instance.VratiStaniceZaLiniju(l);
                }
                label1.Text = "Pokrenut server!";
                dataGridView1.DataSource = linije;
                Thread t = new Thread(osvezi);
                t.IsBackground = true;
                t.Start();
            }
            else { label1.Text = "Server nije pokrenut!"; }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void osvezi()
        {
            while (true)
            {

                BindingList<Linija> osvezeneLinije = new BindingList<Linija>();
                osvezeneLinije = Broker.Instance.PovuciSveLinije();
                foreach (Linija l in osvezeneLinije)
                {
                    l.Medjustanice = Broker.Instance.VratiStaniceZaLiniju(l);
                }
                linije = osvezeneLinije;
                dataGridView1.Invoke(new Action(() =>
                {
                    dataGridView1.DataSource = linije;

                }));
                Thread.Sleep(10000);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                BindingList<Linija> filtriraneLinije = new BindingList<Linija>();
                string filter = txtFilter.Text;
                foreach (Linija l in linije)
                {
                    if (l.NazivLinije.Contains(filter))
                    {
                        filtriraneLinije.Add(l);
                    }
                }
                dataGridView1.DataSource = filtriraneLinije;
            }
            else
            {
                if (checkBox1.Checked == false)
                {
                    dataGridView1.DataSource = linije;

                }
            }
        }
    }
}
