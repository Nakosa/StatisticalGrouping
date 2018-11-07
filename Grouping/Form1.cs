using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Grouping
{
    public partial class Form1 : Form
    {
        public StatisticalGrouping G;
        private double[][] groupingArr;
        private int[] groupingIndex;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.groupingArr = new double[6]{ new double[1]};
            this.groupingIndex = new int[1] {0};

            this.Text = "READY";
            new Thread(test).Start();
            Thread.Sleep(100);
            checkReady();
        }

        private void test() {
            this.G = new StatisticalGrouping();
            this.G.Init(this.groupingArr, this.groupingIndex);
        }


        private void checkReady() {
            while (!this.G.ready) {
                Application.DoEvents();
            }
            this.Text = "READY";
        }
    }
}
