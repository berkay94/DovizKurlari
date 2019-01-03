using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DovizKurlari
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Doviz secilenDoviz = (Doviz)listBox1.SelectedItem;
            labelAlis.Text = secilenDoviz.ForexBuying.ToString();
            labelSatis.Text = secilenDoviz.ForexSelling.ToString();
            labelDovizKodu.Text = secilenDoviz.CurrencyCode;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load("http://www.tcmb.gov.tr/kurlar/today.xml");
            XmlElement rooteleman = xmldoc.DocumentElement;
            XmlNodeList liste = rooteleman.GetElementsByTagName("Currency");
            List<Doviz> dlist = new List<Doviz>();

            foreach (XmlElement item in liste)
            {
                Doviz d = new Doviz();
                XmlElement currency = item;

                string isim = currency.GetElementsByTagName("Isim").Item(0).InnerText;
                d.CurrencyName = isim;

                string kod = currency.Attributes["CurrencyCode"].Value;
                d.CurrencyCode = kod;

                string alisFiyat = currency.GetElementsByTagName("ForexBuying").Item(0).InnerText;
                string satisFiyat = currency.GetElementsByTagName("ForexSelling").Item(0).InnerText;

                if (!(kod is null))
                {
                    d.CurrencyCode = kod;
                }

                if (!string.IsNullOrEmpty(alisFiyat))
                    d.ForexBuying = Convert.ToDecimal(alisFiyat) / 10000;
                
                if (!string.IsNullOrEmpty(satisFiyat))
                    d.ForexSelling = Convert.ToDecimal(satisFiyat) / 10000;

                listBox1.Items.Add(d);

                dlist.Add(d);

            }
            dataGridView1.DataSource = dlist;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataSet dovız = new DataSet();
            dovız.ReadXml("http://www.tcmb.gov.tr/kurlar/today.xml");
            dataGridView1.DataSource = dovız.Tables[1];
        }
    }
}
