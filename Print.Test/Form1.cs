
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Print.Test.Printer;

namespace Print.Test
{
    public partial class Form1 : Form
    {
        private PrinterManager printerManager;

        public Form1()
        {
            InitializeComponent();

            printerManager = new PrinterManager();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            printerManager.printTestOrder();
        }
    }
}
