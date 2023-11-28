using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace SRecordizer
{
    public partial class Form1 : Form
    {
        private readonly string copiedData;
        public Form1(string data)
        {
            InitializeComponent();
            label5.Visible = false;
            MaximizeBox = false;
            richTextBox1.Text = data;
            int cursor = richTextBox1.SelectionStart;
            string checkstr = richTextBox1.Text;
            char[] checkchar = checkstr.ToCharArray();
            char[] chekedchar = new char[checkstr.Length];
            for (int i = 0, a = 0; i < checkstr.Length; i++, a++)
            {
                if ((checkchar[i] >= 'A' && checkchar[i] <= 'F') || (checkchar[i] >= 'a' && checkchar[i] <= 'f') || (checkchar[i] >= '0' && checkchar[i] <= '9'))
                {
                    chekedchar[a] = checkchar[i];
                }
                else
                {
                    if (i < checkstr.Length - 1)
                    {
                        chekedchar[a] = checkchar[i + 1];
                        ++i;
                    }
                    else
                    {
                        chekedchar[a] = '\0';
                    }
                }
            }
            checkstr = String.Join("", chekedchar);
            richTextBox1.Text = checkstr;
            richTextBox1.SelectionStart = cursor;
        }

        public Byte[] GetBytesFromHexString(string strInput)
        {
            Byte[] bytArOutput = new Byte[] { };
            if (!string.IsNullOrEmpty(strInput) && strInput.Length % 2 == 0)
            {
                SoapHexBinary hexBinary = null;
                try
                {
                    hexBinary = SoapHexBinary.Parse(strInput);
                    if (hexBinary != null)
                    {
                        bytArOutput = hexBinary.Value;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return bytArOutput;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string strInput = Convert.ToString(richTextBox1.Text);
                if (strInput.Length % 2 == 0)
                {
                    ushort crc = Convert.ToUInt16(textBox2.Text, 16);
                    ushort poly = Convert.ToUInt16(textBox1.Text, 16);


                    byte[] data = GetBytesFromHexString(strInput);
                    for (int i = 0; i < data.Length; i++)
                    {
                        crc ^= (ushort)(data[i] << 8);
                        for (int j = 0; j < 8; j++)
                        {
                            if ((crc & 0x8000) > 0)
                                crc = (ushort)((crc << 1) ^ poly);
                            else
                                crc <<= 1;
                        }
                    }
                    string CRC_CCIT = crc.ToString("X4");
                    char[] checkchar = CRC_CCIT.ToCharArray();
                    char[] chekedchar = new char[CRC_CCIT.Length];
                    chekedchar[0] = checkchar[2];
                    chekedchar[1] = checkchar[3];
                    chekedchar[2] = checkchar[0];
                    chekedchar[3] = checkchar[1];
                    label5.Visible = true;
                    string reverse_CRC_CCIT = new string(chekedchar);
                    label5.Text = reverse_CRC_CCIT;
                }
                else
                {
                    MessageBox.Show("Data submitted is not even or empty!", "Warning", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void KeyPress(object sender, KeyPressEventArgs e)
        {

            if ((e.KeyChar >= 'A' && e.KeyChar <= 'F') || (e.KeyChar >= 'a' && e.KeyChar <= 'f') || (e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (char)Keys.Back)
            {

            }
            else
            {
                e.Handled = true;
            }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e){}
        private void Menu_Paste(System.Object sender, System.EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {

            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            int cursor = richTextBox1.SelectionStart;
            string checkstr = richTextBox1.Text;
            char[] checkchar = checkstr.ToCharArray();
            char[] chekedchar = new char[checkstr.Length];
            for (int i = 0,a=0; i < checkstr.Length; i++, a++)
            {
                if((checkchar[i] >= 'A' && checkchar[i] <= 'F') || (checkchar[i] >= 'a' && checkchar[i] <= 'f') || (checkchar[i] >= '0' && checkchar[i] <= '9'))
                {
                    chekedchar[a] = checkchar[i];
                }
                else
                {
                    if (i < checkstr.Length-1)
                    {
                        chekedchar[a] = checkchar[i + 1];
                        ++i;
                    }
                    else
                    {
                        chekedchar[a] = '\0';
                    }
                }
            }
            checkstr = String.Join("", chekedchar);
            richTextBox1.Text = checkstr;
            richTextBox1.SelectionStart = cursor;
        }
    }
}
