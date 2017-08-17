using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSG401_120407005
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                listView1.Items.Clear();
                var eqn = ParseInput(textBox1.Text);
                var h = double.Parse(textBox2.Text);
                var y0 = double.Parse(textBox3.Text);
                var xStart = double.Parse(textBox4.Text);
                var xEnd = double.Parse(textBox5.Text);
                var sum = Sum(eqn, Math.E, 2);
                int n = 0;
                double y = y0;
                var line = listView1.Items.Add("0");
                line.SubItems.Add(xStart.ToString());
                line.SubItems.Add(y0.ToString());

                for (double x = xStart, i = 1; x <= xEnd; i += 1)
                {
                    var An = h * Sum(eqn, x, y);
                    line.SubItems.Add(An.ToString());

                    var Bn = h * Sum(eqn, x + h / 2, y + An / 2);
                    line.SubItems.Add(Bn.ToString());
                    var Cn = h * Sum(eqn, x + h / 2, y + Bn / 2);
                    line.SubItems.Add(Cn.ToString());
                    var Dn = h * Sum(eqn, x + h, y + Cn);
                    line.SubItems.Add(Dn.ToString());
                    y = y + (1.0 / 6.0) * (An + 2 * Bn + 2 * Cn + Dn);
                    if (x == xEnd)
                    {
                        break;
                    }
                    line = listView1.Items.Add(i.ToString());
                    x += h;
                    line.SubItems.Add(x.ToString());
                    line.SubItems.Add(y.ToString());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid User Input.");
            }
        }

        private double Func(string input, double x, double y)
        {
            bool isNegative = false;
            double ans = 0;
            if (input[0] == '-')
            {
                isNegative = true;
                input = input.Substring(1);
            }
            if (input.Contains("(x)"))
            {
                ans = Func2(input, x);
            }
            else if (input.Contains("(y)"))
            {
                ans = Func2(input, y);
            }
            else if (input.Contains("x"))
            {
                var a = input.Split(new[] { 'x' }, StringSplitOptions.RemoveEmptyEntries);
                if (a.Length == 0)
                {
                    ans = x;

                }
                else
                {
                    if (a[0][0] == '^')
                    {
                        ans = Math.Pow(x, double.Parse(a[0].Substring(1)));
                    }
                    else if (a[1][0]=='^')
                    {
                        ans = Math.Pow(x,double.Parse(a[1].Substring(1))) * double.Parse(a[0]);
                    }
                    else
                    {
                        ans = x * double.Parse(a[0]);
                    }
                }
            }
            else if (input.Contains("y"))
            {
                var a = input.Split(new[] { 'y' }, StringSplitOptions.RemoveEmptyEntries);
                if (a.Length == 0)
                {
                    ans = y;

                }
                else
                {
                    if (a[0][0] == '^')
                    {
                        ans = Math.Pow(y, double.Parse(a[0].Substring(1)));
                    }
                    else if (a[1][0] == '^')
                    {
                        ans = Math.Pow(y, double.Parse(a[1].Substring(1))) * double.Parse(a[0]);
                    }
                    else
                    {
                        ans = y * double.Parse(a[0]);
                    }
                }

            }
            else
            {
                ans = double.Parse(input);
            }
            if (isNegative)
            {
                return -ans;
            }
            return ans;
        }

        private static double Func2(string input, double x)
        {
            var m = typeof(Math);
            
            var function = input.Substring(0, input.IndexOf('('));

            foreach (var item in m.GetMethods())
            {
                if (item.Name ==function)
                {
                    return (double)item.Invoke(null, new object[] { x });
                }
            }

            return 0;
        }

        private static List<string> ParseInput(string line)
        {
            var sb = new StringBuilder();
            foreach (var item in line)
            {
                if (item != ' ')
                {
                    sb.Append(item);
                }
            }
            var g = sb.ToString().Split(new[] { '+', '-', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> l = new List<string>();
            foreach (var item in g)
            {
                l.Add(item);
            }
            for (int i = 0; i < l.Count; i++)
            {
                try
                {
                    var ind = line.LastIndexOf(l[i]) - 1;
                    if (line[ind] == '-')
                    {
                        l[i] = "-" + l[i];
                    }
                }
                catch (IndexOutOfRangeException)
                {

                }

            }

            return l;
        }

        double Sum(List<string> list,double x,double y)
        {
            double ans = 0;
            foreach (var item in list)
            {
                ans += Func(item, x, y);
            }
            return ans;
        }

    }
}
