using Domen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Klijent
{
    public partial class FrmKlijent : Form
    {
        BindingList<Stanica> medjustanice = new BindingList<Stanica>();
        Komunikacija k = new Komunikacija();
        public FrmKlijent()
        {
            InitializeComponent();
        }

        private void FrmKlijent_Load(object sender, EventArgs e)
        {
            if (k.poveziSe())
            {
                
                comboBox1.DataSource = k.VratiStanice(); 
                comboBox2.DataSource = k.VratiStanice(); 
                comboBox3.DataSource = k.VratiStanice(); 
                dataGridView1.DataSource = medjustanice;
            }
            else { }

        }
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stanica s = (Stanica)comboBox3.SelectedItem;
            if (!medjustanice.Contains(s))
            {
                medjustanice.Add(s);
            }
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //k = new Komunikacija();
            string nazivLinije = textBox1.Text;
            Stanica pocetna = (Stanica)comboBox1.SelectedItem;
            Stanica krajnja = (Stanica)comboBox2.SelectedItem;
            List<Stanica> medjustaniceParam = medjustanice.ToList();
            if (proveri(pocetna, krajnja, medjustaniceParam))
            {
                bool sacuvan = k.SacuvajLiniju(nazivLinije, pocetna, krajnja, medjustaniceParam);
                if (sacuvan)
                {
                    MessageBox.Show("Linija sacuvana");
                }
                else
                {
                    MessageBox.Show("Linija nije sacuvana");
                }
            }
            else return;
        }

        private bool proveri(Stanica pocetna, Stanica krajnja, List<Stanica> medjustanice)
        {
            if (pocetna.Equals(krajnja))
                return false;
            if (medjustanice.Count() == 0)
                return false;
            if (medjustanice.Contains(pocetna) || medjustanice.Contains(krajnja))
                return false;
            return true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
