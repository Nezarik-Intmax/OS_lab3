using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS_lab3 {
    public partial class Form1 : Form {
        public delegate void del();
        public Form1() {
            InitializeComponent();
            Buyer.form = this;

        }


        class Buyer {
            public static Form1 form;
            static Semaphore sem = new Semaphore(3, 3);
            Thread myThread;
            int count = 3;
            int arrive_time = 0;
            int buy_time;

            public Buyer(int i) {
                Random rnd = new Random();
                myThread = new Thread(Buy);
                myThread.Name = $"Покупатель {i}";
                arrive_time = rnd.Next(500, 1200);
                buy_time = rnd.Next(500, 1200);
                myThread.Start();
            }

            public void Buy() {
                while (count > 0) {
                    sem.WaitOne();

                    string str = Thread.CurrentThread.Name;
                    form.richTextBox1.Invoke(new del(() => form.richTextBox1.Text += $"{str} входит в магазин\n"));

                    form.richTextBox1.Invoke(new del(() => form.richTextBox1.Text += $"{str} покупает\n"));
                    Thread.Sleep(buy_time);

                    form.richTextBox1.Invoke(new del(() => form.richTextBox1.Text += $"{str} покидает магазин\n"));

                    sem.Release();

                    count--;
                    Thread.Sleep(arrive_time);
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e) {
            for (int i = 1; i < 6; i++) {
                Buyer buyer = new Buyer(i);
            }
        }

        private void Form1_Load(object sender, EventArgs e) {

        }
    }
}
