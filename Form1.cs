using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;

namespace Launcher_new2
{
    public partial class Form1 : Form
    {
        //global
        private Cvars[] gamecontent = new Cvars[1];
        private List<String> listData;


        public Form1()
        {
            this.listData = new List<String>();
            InitializeComponent();
        }

        //Start a configuration file - inputing them to text boxes..

        private void Write(Cvars obj)
        {
            StreamWriter sw = new StreamWriter("configuration.ini");
            sw.WriteLine(gamecontent.Length + 1);
            sw.WriteLine(obj.Port);
            sw.WriteLine(obj.Iwads);
            sw.WriteLine(obj.Files);
            sw.WriteLine(obj.Commands);

            for (int x = 0; x < gamecontent.Length; x++)
            {
                sw.WriteLine(gamecontent[x].Port);
                sw.WriteLine(gamecontent[x].Iwads);
                sw.WriteLine(gamecontent[x].Files);
                sw.WriteLine(gamecontent[x].Commands);
            }

            sw.Close();
        }
        // Read the text boxes .. read them from the configuration file..
        private void Read()
        {
            StreamReader sr = new StreamReader("configuration.ini");
            gamecontent = new Cvars[Convert.ToInt32(sr.ReadLine())];

            for (int x = 0; x < gamecontent.Length; x++)
            {
                gamecontent[x] = new Cvars();
                gamecontent[x].Port = sr.ReadLine();
                gamecontent[x].Iwads = sr.ReadLine();
                gamecontent[x].Files = sr.ReadLine();
                gamecontent[x].Commands = sr.ReadLine();

            }

            sr.Close();

        }

        private void Display()
        {
            listBox1.Items.Clear();

            for (int x = 0; x < gamecontent.Length; x++)
            {
                listBox1.Items.Add(gamecontent[x].ToString());
            }
        }

        private void ClearForm()
        {
            //   textBox1.Text = String.Empty;
            //   textBox2.Text = String.Empty;
            //   textBox3.Text = String.Empty;
            //   textBox4.Text = String.Empty;

        }
        // Add info from text boxes and put them in ListBox1..
        private void btnAddParameters_Click(object sender, EventArgs e)
        {
            Cvars obj = new Cvars();
            obj.Port = textBox1.Text;
            obj.Iwads = textBox2.Text;
            obj.Files = textBox3.Text;
            obj.Commands = textBox4.Text;

            Write(obj);
            Read();
            Display();
            ClearForm();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Read();
            Display();


            if (File.Exists("Settings.xml"))
            {
                XmlSerializer xs = new XmlSerializer(typeof(Information));
                FileStream read = new FileStream("Settings.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
                Information info = (Information)xs.Deserialize(read);
                textBox1.Text = info.Data1;
                textBox2.Text = info.Data2;
                textBox3.Text = info.Data3;
                textBox4.Text = info.Data4;
                comboBox1.Text = info.Data5;
              //  checkBox1.Text = info.Data6;
              //  checkBox2.Text = info.Data7;
             //   checkBox3.Text = info.Data8;
              //  checkBox4.Text = info.Data9;
            }

        }

        // Load info from ListBox1 and execute it!....

        // LAUNCH GAME 


        private void button5_Click(object sender, EventArgs e)

        {

            StringBuilder str = new StringBuilder();
            foreach (object Cvars in listBox1.Items)

            {
                str.AppendLine(Cvars.ToString());
            }

            // MessageBox.Show("" + str, "Launch game with these parameters?");
            string file = listBox1.SelectedItems.ToString();
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("cmd");
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.CreateNoWindow = true;
                psi.RedirectStandardInput = true;
                var proc = Process.Start(psi);

                proc.StandardInput.WriteLine("" + str, "");
                proc.StandardInput.WriteLine("exit");
                string s = proc.StandardOutput.ReadToEnd();

                textBox5.Text = s;
                //  Process.Start("" + str, "");
            }
            // Added soft error message...
            catch (Exception ex)
            {
                MessageBox.Show("Error, " + ex.Message);
            }
        }

        //////////////////////////////////////////////////




        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear(); //need to clear the ini
            StreamWriter sw = new StreamWriter("configuration.ini");
            sw.Close();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Read();
            Display();
        }
        //                 ADD EXE location into textbox1  ///////////
        private void button1_Click(object sender, EventArgs e)
        {
            {
                Stream sourceport = null;
                OpenFileDialog Port = new OpenFileDialog();

                Port.InitialDirectory = "C:/Legacy/";
                Port.Filter = "exe files (*.exe)|*.exe|DooM Engine (*.*)|*.*";
                Port.DefaultExt = ".exe";
                Port.FileName = "doomlegacy.exe";


                if (Port.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((sourceport = Port.OpenFile()) != null)
                        {
                            using (sourceport)
                            {   //Loading file path into textbox..
                                textBox1.Text = string.Format(Port.FileName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
            }
        }
        //                ADD IWAD location into textbox2  ////////////////////
        private void button2_Click(object sender, EventArgs e)
        {
            {
                Stream mainwadfile = null;
                OpenFileDialog Iwad = new OpenFileDialog();

                Iwad.InitialDirectory = "C:/Legacy/";
                Iwad.Filter = "wad files (*.wad)|*.wad|Iwad (*.*)|*.*";
                Iwad.DefaultExt = ".wad";
                Iwad.FileName = "";

                if (Iwad.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((mainwadfile = Iwad.OpenFile()) != null)
                        {
                            using (mainwadfile)
                            {   //Loading file path into textbox..
                                textBox2.Text = string.Format(Iwad.FileName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
            }
        }
        //                 ADD PWAD location in to textbox3  //////////////////
        private void button3_Click(object sender, EventArgs e)
        {
            {
                Stream pwadfile = null;
                OpenFileDialog Pwad = new OpenFileDialog();

                Pwad.InitialDirectory = "C:/doomports/pwads/";
                Pwad.Filter = "wad files (*.wad)|*.wad|Pwad (*.*)|*.*";
                Pwad.DefaultExt = ".wad";
                Pwad.FileName = "";

                if (Pwad.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((pwadfile = Pwad.OpenFile()) != null)
                        {
                            using (pwadfile)
                            {   //Loading file path into textbox..
                                textBox3.Text = string.Format(Pwad.FileName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox theCheckBoxChanged = sender as CheckBox;

            // verify the event is from a checkbox
            if (theCheckBoxChanged != null)
            {
                if (theCheckBoxChanged.Checked)
                {
                    // add data
                    this.listData.Add(theCheckBoxChanged.Text);
                }
                else
                {
                    // remove data
                    this.listData.Remove(theCheckBoxChanged.Text);
                }

                this.UpdateTextBox();
            }
        }

        private void UpdateTextBox()
        {
            // be carefull, you can't concatenate if there is no data.
            if (this.listData.Count > 0)
            {
                // sort and concatenate
                this.textBox4.Text = this.listData
                                         .OrderBy(s => s)
                                         .Aggregate((s1, s2) => s1 + " -" + s2);
            }
            else
            {
                this.textBox4.Text = String.Empty;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            {
                CheckBox theCheckBoxChanged = sender as CheckBox;

                // verify the event is from a checkbox
                if (theCheckBoxChanged != null)
                {
                    if (theCheckBoxChanged.Checked)
                    {
                        // add data
                        this.listData.Add(theCheckBoxChanged.Text);
                    }
                    else
                    {
                        // remove data
                        this.listData.Remove(theCheckBoxChanged.Text);
                    }

                    this.UpdateTextBox2();
                }
            }
        }
        private void UpdateTextBox2()
        {
            // be carefull, you can't concatenate if there is no data.
            if (this.listData.Count > 0)
            {
                // sort and concatenate
                this.textBox4.Text = this.listData
                                         .OrderBy(s => s)
                                         .Aggregate((s1, s2) => s1 + " -" + s2);
            }
            else
            {
                this.textBox4.Text = String.Empty;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            {
                CheckBox theCheckBoxChanged = sender as CheckBox;

                // verify the event is from a checkbox
                if (theCheckBoxChanged != null)
                {
                    if (theCheckBoxChanged.Checked)
                    {
                        // add data
                        this.listData.Add(theCheckBoxChanged.Text);
                    }
                    else
                    {
                        // remove data
                        this.listData.Remove(theCheckBoxChanged.Text);
                    }

                    this.UpdateTextBox3();
                }
            }
        }
        private void UpdateTextBox3()
        {
            // be carefull, you can't concatenate if there is no data.
            if (this.listData.Count > 0)
            {
                // sort and concatenate
                this.textBox4.Text = this.listData
                                         .OrderBy(s => s)
                                         .Aggregate((s1, s2) => s1 + " -" + s2);
            }
            else
            {
                this.textBox4.Text = String.Empty;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            {
                CheckBox theCheckBoxChanged = sender as CheckBox;

                // verify the event is from a checkbox
                if (theCheckBoxChanged != null)
                {
                    if (theCheckBoxChanged.Checked)
                    {
                        // add data
                        this.listData.Add(theCheckBoxChanged.Text);
                    }
                    else
                    {
                        // remove data
                        this.listData.Remove(theCheckBoxChanged.Text);
                    }

                    this.UpdateTextBox4();
                }
            }
        }
        private void UpdateTextBox4()
        {
            // be carefull, you can't concatenate if there is no data.
            if (this.listData.Count > 0)
            {
                // sort and concatenate
                this.textBox4.Text = this.listData
                                         .OrderBy(s => s)
                                         .Aggregate((s1, s2) => s1 + " -" + s2);
            }
            else
            {
                this.textBox4.Text = String.Empty;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextBox4();
            {
                //  comboBox1.Items.Add("MAP01");
                if (comboBox1.SelectedItem == "MAP01")
                    textBox4.Text += (" -warp 1");

                {
                    //   comboBox1.Items.Add("MAP02");
                    if (comboBox1.SelectedItem == "MAP02")
                        textBox4.Text += (" -warp 2");


                    {
                        //      comboBox1.Items.Add("MAP03");

                        if (comboBox1.SelectedItem == "MAP03")
                            textBox4.Text += (" -warp 3");
                    }
                    {
                        //      comboBox1.Items.Add("MAP04");

                        if (comboBox1.SelectedItem == "MAP04")
                            textBox4.Text += (" -warp 4");
                    }
                    {
                        //       comboBox1.Items.Add("MAP05");

                        if (comboBox1.SelectedItem == "MAP05")
                            textBox4.Text += (" -warp 5");
                    }
                    {
                        //       comboBox1.Items.Add("MAP06");

                        if (comboBox1.SelectedItem == "MAP06")
                            textBox4.Text += (" -warp 6");
                    }
                    {
                        //       comboBox1.Items.Add("MAP07");

                        if (comboBox1.SelectedItem == "MAP07")
                            textBox4.Text += (" -warp 7");
                    }
                    {
                        //       comboBox1.Items.Add("MAP08");

                        if (comboBox1.SelectedItem == "MAP08")
                            textBox4.Text += (" -warp 8");
                    }
                    {
                        //       comboBox1.Items.Add("MAP09");

                        if (comboBox1.SelectedItem == "MAP09")
                            textBox4.Text += (" -warp 9");
                    }
                    {
                        //         comboBox1.Items.Add("MAP10");

                        if (comboBox1.SelectedItem == "MAP10")
                            textBox4.Text += (" -warp 10");
                    }
                    {
                        //       comboBox1.Items.Add("MAP11");

                        if (comboBox1.SelectedItem == "MAP11")
                            textBox4.Text += (" -warp 11");
                    }
                    {
                        //      comboBox1.Items.Add("MAP12");

                        if (comboBox1.SelectedItem == "MAP12")
                            textBox4.Text += (" -warp 12");
                    }
                    {
                        //     comboBox1.Items.Add("MAP13");

                        if (comboBox1.SelectedItem == "MAP13")
                            textBox4.Text += (" -warp 13");
                    }
                    {
                        //     comboBox1.Items.Add("MAP14");

                        if (comboBox1.SelectedItem == "MAP14")
                            textBox4.Text += (" -warp 14");
                    }
                    {
                        //      comboBox1.Items.Add("MAP15");

                        if (comboBox1.SelectedItem == "MAP15")
                            textBox4.Text += (" -warp 15");
                    }
                    {
                        //       comboBox1.Items.Add("MAP16");

                        if (comboBox1.SelectedItem == "MAP16")
                            textBox4.Text += (" -warp 16");
                    }
                    {
                        //       comboBox1.Items.Add("MAP17");

                        if (comboBox1.SelectedItem == "MAP17")
                            textBox4.Text += (" -warp 17");
                    }
                    {
                        //       comboBox1.Items.Add("MAP18");

                        if (comboBox1.SelectedItem == "MAP18")
                            textBox4.Text += (" -warp 18");
                    }
                    {
                        //       comboBox1.Items.Add("MAP19");

                        if (comboBox1.SelectedItem == "MAP19")
                            textBox4.Text += (" -warp 19");
                    }
                    {
                        //       comboBox1.Items.Add("MAP20");

                        if (comboBox1.SelectedItem == "MAP20")
                            textBox4.Text += (" -warp 20");
                    }
                    {
                        //       comboBox1.Items.Add("MAP21");

                        if (comboBox1.SelectedItem == "MAP21")
                            textBox4.Text += (" -warp 21");
                    }
                    {
                        //       comboBox1.Items.Add("MAP22");

                        if (comboBox1.SelectedItem == "MAP22")
                            textBox4.Text += (" -warp 22");
                    }
                    {
                        //       comboBox1.Items.Add("MAP23");

                        if (comboBox1.SelectedItem == "MAP23")
                            textBox4.Text += (" -warp 23");
                    }
                    {
                        //       comboBox1.Items.Add("MAP24");

                        if (comboBox1.SelectedItem == "MAP24")
                            textBox4.Text += (" -warp 24");
                    }
                    {
                        //       comboBox1.Items.Add("MAP25");

                        if (comboBox1.SelectedItem == "MAP25")
                            textBox4.Text += (" -warp 25");
                    }
                    {
                        //       comboBox1.Items.Add("MAP26");

                        if (comboBox1.SelectedItem == "MAP26")
                            textBox4.Text += (" -warp 26");
                    }
                    {
                        //        comboBox1.Items.Add("MAP27");

                        if (comboBox1.SelectedItem == "MAP27")
                            textBox4.Text += (" -warp 27");
                    }
                    {
                        //        comboBox1.Items.Add("MAP28");

                        if (comboBox1.SelectedItem == "MAP28")
                            textBox4.Text += (" -warp 28");
                    }
                    {
                        //        comboBox1.Items.Add("MAP29");

                        if (comboBox1.SelectedItem == "MAP29")
                            textBox4.Text += (" -warp 29");
                    }
                    {
                        //        comboBox1.Items.Add("MAP30");

                        if (comboBox1.SelectedItem == "MAP30")
                            textBox4.Text += (" -warp 30");
                    }
                    {
                        //        comboBox1.Items.Add("MAP31");

                        if (comboBox1.SelectedItem == "MAP31")
                            textBox4.Text += (" -warp 31");
                    }
                    {
                        //        comboBox1.Items.Add("MAP32");

                        if (comboBox1.SelectedItem == "MAP32")
                            textBox4.Text += (" -warp 32");
                    }

                    Read();
                    Display();
                }
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                Information info = new Information();
                info.Data1 = textBox1.Text;
                info.Data2 = textBox2.Text;
                info.Data3 = textBox3.Text;
                info.Data4 = textBox4.Text;
                info.Data5 = comboBox1.Text;
              //  info.Data6 = checkBox1.Text;
              //  info.Data7 = checkBox2.Text;
              //  info.Data8 = checkBox3.Text;
             //   info.Data9 = checkBox4.Text;
                Settings.SaveData(info, "Settings.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        { /// TODO test - FIX
            test_parameters.Properties.Settings.Default.Save();
        }
    }
}
          
       
