using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3DEngine {
    public partial class Form1 : Form {
        private Graphics g;

        public Form1() {
            InitializeComponent();
            g = CreateGraphics();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e) {
            Point[] triangle = new Point[3] { new Point(30, 50), new Point(100, 200), new Point(25, 85) };
            g.FillPolygon(new SolidBrush(Color.Green), triangle);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e) {
            g.Clear(SystemColors.Control);
            
        }
    }
}
