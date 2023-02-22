using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace Sifrovani
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void SifrujBase64()
        {
            byte[] inputMessage = Encoding.Unicode.GetBytes(textBox1.Text);
            textBox2.Text = Convert.ToBase64String(inputMessage);
        }

        public string DesifrujBase64(string inputString)
        {
            string outputMessage = "";
            if(outputMessage == null)
            {
                return outputMessage;
            }

            byte[] inputCoverted = Convert.FromBase64String(inputString);
            outputMessage = Encoding.Unicode.GetString(inputCoverted);

            return outputMessage;
        }

        public string EncryptSymentric(string inputMessage, string pass)
        {
            if (String.IsNullOrEmpty(inputMessage))
            {
                throw new ArgumentException("Zadej neco k sifrovani? :)");
            }
            if(pass.Length != 8)
            {
                MessageBox.Show("Heslo musi byt dlouhe 8 znaku");
                return "";
            }
            byte[] bytes = ASCIIEncoding.Unicode.GetBytes(pass);
            SymmetricAlgorithm cryptoProvider = SymmetricAlgorithm.Create("TripleDES");
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cryptoStream);

            sw.Write(inputMessage);
            sw.Flush();
            cryptoStream.FlushFinalBlock();
            sw.Flush();

            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        public string DecryptSymentric(string cryptedString, string pass)
        {
            if (String.IsNullOrEmpty(cryptedString))
            {
                MessageBox.Show("Zadej retezec k desifrovani");
                return "";
            }
            if (pass.Length != 8)
            {
                MessageBox.Show("Heslo musi byt 8 znaku");
                return "";
            }
            byte[] bytes = ASCIIEncoding.Unicode.GetBytes(pass);
            TripleDESCryptoServiceProvider cryptoProvider = new TripleDESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cryptoStream);

            return sr.ReadToEnd();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != null)
            {
                SifrujBase64();
            }
            else
            {
                MessageBox.Show("Není co šifrovat");
            }

            textBox3.Text = DesifrujBase64(textBox2.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox5.Text = EncryptSymentric(textBox4.Text, textBox7.Text);
            textBox6.Text = DecryptSymentric(textBox5.Text, textBox7.Text);
        }
    }
}
