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
        private List<Employees> employees = new List<Employees>();
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

        // Универсальный метод фильтрации
        private void ApplyCurrentFilter()
        {
            if (comboBox1.SelectedIndex < 0)
            {
                System.Diagnostics.Debug.WriteLine("ComboBox не имеет выбранного элемента.");
                return;
            }

            List<Employees> filteredEmployees = null;

            // Выполняем фильтрацию
            if (comboBox1.SelectedIndex == 0)
            {
                filteredEmployees = employees.Where(p => p.country == "USA").ToList();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                filteredEmployees = employees.Where(p => p.extension > 999).ToList();
            }

            // Обновляем DataGridView2
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = filteredEmployees;

            // Обновляем график
            UpdateChart(filteredEmployees);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyCurrentFilter();
        }

        private void UpdateChart(List<Employees> filteredEmployees)
        {
            if (filteredEmployees == null || filteredEmployees.Count == 0)
            {
                chart.Series = new ISeries[0];
                return;
            }

            chart.Series = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Values = filteredEmployees.Select(p => p.extension).ToArray(),
                    Name = "Extension (значение больше 999)"
                }
            };

            chart.XAxes = new[]
            {
                new Axis
                {
                    Labels = filteredEmployees.Select(p => p.lastName).ToArray()
                }
            };

            System.Diagnostics.Debug.WriteLine("График обновлён.");
        }

        private void btnAddEmployees_Click(object sender, EventArgs e)
        {
            ReloadProducts();
            ApplyCurrentFilter();
        }

        private void ReloadProducts()
        {
            employees.Clear();

            using (var connection = new SQLiteConnection("Data Source=northwind.db;Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT EmployeeID, LastName, FirstName, Extension, Country FROM Employees", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Чтение данных через свойства
                        int employeeID = reader.GetInt32(0);
                        string lastName = reader.GetString(1);
                        string firstName = reader.GetString(2);

                        // Преобразование текста из Extension в int
                        int extension = 0;
                        if (int.TryParse(reader.GetString(3), out int parsedExtension))
                        {
                            extension = parsedExtension;
                        }

                        string country = reader.GetString(4);

                        // Добавление сотрудника в список
                        employees.Add(new Employees(employeeID, lastName, firstName, extension, country));
                    }
                }
            }

            // Обновляем DataGridView1
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = employees
                .Select(e => new
                {
                    ID = e.employeeID,
                    Фамилия = e.lastName,
                    Имя = e.firstName,
                    Extension = e.extension,
                    Страна = e.country
                }).ToList();

            // Обновляем ComboBox
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Работники из USA");
            comboBox1.Items.Add("Extension > 999");

            // Устанавливаем первый элемент и применяем фильтрацию
            comboBox1.SelectedIndex = 0;
            ApplyCurrentFilter();
        }
    }
}

