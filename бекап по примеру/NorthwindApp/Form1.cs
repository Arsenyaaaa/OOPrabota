using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Forms;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;

namespace NorthwindApp
{
    public partial class Form1 : Form
    {
        private List<Products> products = new List<Products>();
        private CartesianChart chart;

        public Form1()
        {
            InitializeComponent();
            SetupChart();
        }

        private void SetupChart()
        {
            chart = new CartesianChart
            {
                Dock = DockStyle.Fill
            };
            panelChart.Controls.Add(chart);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReloadProducts();
        }

        // ������������� ����� ����������
        private void ApplyCurrentFilter()
        {
            if (comboBox1.SelectedIndex < 0)
            {
                System.Diagnostics.Debug.WriteLine("ComboBox �� ����� ���������� ��������.");
                return;
            }

            List<Products> filteredProducts = null;

            // ��������� ����������
            if (comboBox1.SelectedIndex == 0)
            {
                filteredProducts = products.Where(p => p.ProductName.Length < 7).ToList();
                System.Diagnostics.Debug.WriteLine("������: �������� < 7 ��������");
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                filteredProducts = products.Where(p => p.UnitPrice < 15).ToList();
                System.Diagnostics.Debug.WriteLine("������: ���� < 15");
            }

            // ��������� DataGridView2
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = filteredProducts;

            // ��������� ������
            UpdateChart(filteredProducts);

            System.Diagnostics.Debug.WriteLine($"���������� ���������: ������� {filteredProducts?.Count ?? 0} ���������.");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"ComboBox ������������ �� ������: {comboBox1.SelectedIndex}");
            ApplyCurrentFilter();
        }

        private void UpdateChart(List<Products> filteredProducts)
        {
            if (filteredProducts == null || filteredProducts.Count == 0)
            {
                chart.Series = new ISeries[0];
                System.Diagnostics.Debug.WriteLine("��� ������ ��� ���������� �������.");
                return;
            }

            chart.Series = new ISeries[]
            {
                new ColumnSeries<decimal>
                {
                    Values = filteredProducts.Select(p => p.UnitPrice).ToArray(),
                    Name = "���� ���������"
                }
            };

            chart.XAxes = new[]
            {
                new Axis
                {
                    Labels = filteredProducts.Select(p => p.ProductName).ToArray()
                }
            };

            System.Diagnostics.Debug.WriteLine("������ �������.");
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            ReloadProducts();
            ApplyCurrentFilter();
        }

        private void ReloadProducts()
        {
            products.Clear();

            using (var connection = new SQLiteConnection("Data Source=northwind.db;Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT ProductID, ProductName, CategoryID, UnitPrice FROM Products", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Products(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetInt32(2),
                            reader.GetDecimal(3)
                        ));
                    }
                }
            }

            // ��������� DataGridView1
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = products;

            // ��������� ComboBox
            comboBox1.Items.Clear();
            comboBox1.Items.Add("�������� < 7 ��������");
            comboBox1.Items.Add("���� < 15");

            // ������������� ������ ������� � ���������
            comboBox1.SelectedIndex = 0;
            ApplyCurrentFilter();
        }
    }
}

