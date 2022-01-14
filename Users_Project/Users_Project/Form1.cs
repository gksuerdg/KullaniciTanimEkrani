using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace Users_Project
{
    public partial class Form1 : Form
    {
        DataTable dt = new DataTable();
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "YVnZ6JI7p9cPH4QmKjAJ2kjzcTmyYq9UPA6MbwD7",
            
            BasePath= "https://users-project-78bc4-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);
            
            if(client != null)
            {
                MessageBox.Show("CONNECTION SUCCESSFUL !");
            }

            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("LastName");

            dataGridView1.DataSource = dt;
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            FirebaseResponse resp = await client.GetTaskAsync("Counter/Node");
            Counter get = resp.ResultAs<Counter>();

            MessageBox.Show(get.cnt);

            var data = new Data
            {
                Id =(Convert.ToInt32(get.cnt)+1).ToString(),
                Name = textBox4.Text,
                LastName = textBox3.Text
            };

            SetResponse response = await client.SetTaskAsync("Users/" + data.Id, data);
            Data result = response.ResultAs<Data>();

            MessageBox.Show("User Inserted:" + result.Id);

            var obj = new Counter
            {
                cnt = data.Id
            };

            SetResponse response1 = await client.SetTaskAsync("Counter/Node",obj);

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var data = new Data
            {
                Id = textBox5.Text,
                Name = textBox4.Text,
                LastName = textBox3.Text
            };

            FirebaseResponse response = await client.UpdateTaskAsync("Users/" + textBox5.Text, data);
            Data result = response.ResultAs<Data>();

            MessageBox.Show("User Updated : " + result.Id);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.DeleteTaskAsync("Users/" + textBox5.Text);
            MessageBox.Show("User ID Deleted" + textBox5.Text);
        }
      

        private  void button4_Click(object sender, EventArgs e)
        {
            export();
        }
        private async void export ()
        {
            dt.Rows.Clear();
            int i = 0;
            FirebaseResponse resp1 = await client.GetTaskAsync("Counter/Node");
            Counter obj1 = resp1.ResultAs<Counter>();
            int cnt = Convert.ToInt32(obj1.cnt);

           
            while (true)
            {
                if (i == cnt)
                {
                    break;
                }
                i++;
                try
                {
                    FirebaseResponse resp2 = await client.GetTaskAsync("Users/" + i);
                    Data obj2 = resp2.ResultAs<Data>();
                   
                    DataRow row = dt.NewRow();
                    row["Id"] = resp2.ResultAs<Data>().Id;
                    row["Name"] = resp2.ResultAs<Data>().Name;
                    row["LastName"] = resp2.ResultAs<Data>().LastName;

                    dt.Rows.Add(row);
                }
                catch
                {

                }
            }
        }

      
    }
}
