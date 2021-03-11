using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LibraryShips;

namespace Program2
{
    public partial class Form1 : Form
    {
        private List<Ship> ships = new List<Ship>();

        private string path = @"D:\source\repos\Program2\XML\data.xml";

        public Form1()
        {
            InitializeComponent();
            XmlDownloadAsync();
        }

        //Очистить таблицы вывода
        private void DataGridViewClear1()
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
        }

        //Очистить таблицы поиска
        private void DataGridViewClear2()
        {
            dataGridView4.Rows.Clear();
            dataGridView5.Rows.Clear();
            dataGridView6.Rows.Clear();
        }

        //Обновить таблицу
        private void UpdateTable()
        {
            try
            {
                DataGridViewClear1();
                labelErrorShow.Text = "";
                foreach (var item in ships)
                {
                    if (item is Steamer)
                    {
                        dataGridView1.Rows.Add(item.Name, item.Weight, item.MaxSpeed, ((Steamer)item).MassOfCoal, ((Steamer)item).RangeOfTravel);
                    }
                    if (item is Sailboat)
                    {
                        dataGridView2.Rows.Add(item.Name, item.Weight, item.MaxSpeed, ((Sailboat)item).SailMaterial, ((Sailboat)item).SailArea);
                    }
                    if (item is Corvette)
                    {
                        dataGridView3.Rows.Add(item.Name, item.Weight, item.MaxSpeed, ((Corvette)item).Armament, ((Corvette)item).Equipment);
                    }
                }
            }
            catch (Exception ex)
            {
                labelErrorShow.Text = ex.Message;
            }
        }

        //Обновить таблицу по нажатию
        private void button1_Click(object sender, EventArgs e)
        {
            UpdateTable();
        }

        //Добавить обьект
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    var name = textBox1.Text;
                    var maxSpeed = int.Parse(textBox2.Text);
                    var weight = int.Parse(textBox3.Text);
                    var massOfCoal = int.Parse(textBox4.Text);
                    var rangeOfTravel = int.Parse(textBox5.Text);
                    ships.Add(new Steamer(name, maxSpeed, weight, massOfCoal, rangeOfTravel));
                }
                else if (radioButton2.Checked)
                {
                    var name = textBox1.Text;
                    var maxSpeed = int.Parse(textBox2.Text);
                    var weight = int.Parse(textBox3.Text);
                    var sailMaterial = textBox4.Text;
                    var sailArea = int.Parse(textBox5.Text);
                    ships.Add(new Sailboat(name, maxSpeed, weight, sailMaterial, sailArea));
                }
                else if (radioButton3.Checked)
                {
                    var name = textBox1.Text;
                    var maxSpeed = int.Parse(textBox2.Text);
                    var weight = int.Parse(textBox3.Text);
                    var armament = textBox4.Text;
                    var equipment = textBox5.Text;
                    ships.Add(new Corvette(name, maxSpeed, weight, armament, equipment));
                }
                else
                {
                    throw new Exception("Не выбран тип судна!");
                }

                UpdateTable();
                labelError.Text = "";
            }
            catch (Exception ex)
            {
                labelError.Text = ex.Message;
            }
        }

        //Сохранение при выходе
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            XmlSaveAsync();
        }

        //Сохранить в XML
        private async Task XmlSaveAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Ship>));
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        serializer.Serialize(fs, ships);
                    }
                });
            }
            catch (Exception ex)
            {
                labelErrorShow.Text = ex.Message;
            }
        }

        //Загрузить с XML
        private async Task XmlDownloadAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(List<Ship>));
                    using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        ships = (List<Ship>)formatter.Deserialize(fs);
                    }
                    UpdateTable();
                });
            }
            catch (Exception ex)
            {
                labelErrorShow.Text = ex.Message;
            }
        }

        //Удалить элемент
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if ((dataGridView1.SelectedRows.Count + dataGridView2.SelectedRows.Count + dataGridView3.SelectedRows.Count) < 1)
                {
                    throw new Exception("Выберите строки");
                }
                else
                {
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        dataGridView1.Rows.RemoveAt(row.Index);
                    }

                    foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                    {
                        dataGridView2.Rows.RemoveAt(row.Index);
                    }

                    foreach (DataGridViewRow row in dataGridView3.SelectedRows)
                    {
                        dataGridView3.Rows.RemoveAt(row.Index);
                    }
                }
                labelErrorShow.Text = "";
            }
            catch(Exception ex)
            {
                labelErrorShow.Text = ex.Message;
            }
        }

        //Очистить список
        private void button4_Click_1(object sender, EventArgs e)
        {
            ships.Clear();
            XmlSaveAsync();
            XmlDownloadAsync();
        }

        //Найти элемент
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewClear2();
                foreach (var item in ships)
                {
                    if (item.IsSearchContains(textBox6.Text))
                    {
                        if (item is Steamer)
                        {
                            dataGridView4.Rows.Add(item.Name, item.Weight, item.MaxSpeed, ((Steamer)item).MassOfCoal, ((Steamer)item).RangeOfTravel);
                        }
                        if (item is Sailboat)
                        {
                            dataGridView5.Rows.Add(item.Name, item.Weight, item.MaxSpeed, ((Sailboat)item).SailMaterial, ((Sailboat)item).SailArea);
                        }
                        if (item is Corvette)
                        {
                            dataGridView6.Rows.Add(item.Name, item.Weight, item.MaxSpeed, ((Corvette)item).Armament, ((Corvette)item).Equipment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                labelErrorShow.Text = ex.Message;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            label5.Text = "Вооружение";
            label6.Text = "Оборудование";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label5.Text = "Материал паруса";
            label6.Text = "Площадь паруса";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label5.Text = "Масса угля";
            label6.Text = "Дальность хода";
        }
    }
}
