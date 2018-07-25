using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using PCSC;

namespace RFID_ReaderGUI
{
    public partial class Form1 : Form
    {
        private static SmartCardReaderHelper _reader;

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            _reader = new SmartCardReaderHelper();
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            _reader.Enabled = true;
        }
    }
}
