using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Tyuiu.PlontikovPI.Sprint7.Project.V10;
using Tyuiu.PlontikovPI.Sprint7.Lib.Models;
using Tyuiu.PlontikovPI.Sprint7.Lib.Services;

namespace Tyuiu.PlontikovPI.Sprint7.Project.V10
{
    public class MainForm_PI : Form
    {
        private readonly OrderCsvRepository repository_PI = new OrderCsvRepository();
        private readonly BindingList<Order> orders_PI = new BindingList<Order>();
        private readonly BindingList<Order> filteredOrders_PI = new BindingList<Order>();
        private readonly BindingSource bindingSourceOrders_PI = new BindingSource();
        private string currentFilePath_PI = string.Empty;

        private MenuStrip menuStripMain_PI;
        private ToolStripMenuItem fileToolStripMenuItem_PI;
        private ToolStripMenuItem openToolStripMenuItem_PI;
        private ToolStripMenuItem saveToolStripMenuItem_PI;
        private ToolStripMenuItem saveAsToolStripMenuItem_PI;
        private ToolStripMenuItem exitToolStripMenuItem_PI;
        private ToolStripMenuItem dataToolStripMenuItem_PI;
        private ToolStripMenuItem addToolStripMenuItem_PI;
        private ToolStripMenuItem deleteToolStripMenuItem_PI;
        private ToolStripMenuItem refreshToolStripMenuItem_PI;
        private ToolStripMenuItem helpToolStripMenuItem_PI;
        private ToolStripMenuItem userGuideToolStripMenuItem_PI;
        private ToolStripMenuItem aboutToolStripMenuItem_PI;

        private ToolStrip toolStripMain_PI;
        private ToolStripButton toolStripButtonOpen_PI;
        private ToolStripButton toolStripButtonSave_PI;
        private ToolStripButton toolStripButtonAdd_PI;
        private ToolStripButton toolStripButtonDelete_PI;
        private ToolStripButton toolStripButtonRefresh_PI;
        private ToolStripButton toolStripButtonStatistics_PI;

        private TableLayoutPanel rootLayout_PI;
        private Panel panelFilter_PI;
        private FlowLayoutPanel filterFlowLayoutPanel_PI;
        private Label labelSearch_PI;
        private TextBox textBoxSearch_PI;
        private Label labelFilterField_PI;
        private ComboBox comboBoxFilterField_PI;
        private Label labelMinCost_PI;
        private NumericUpDown numericUpDownMinCost_PI;
        private Button buttonApplyFilter_PI;
        private Button buttonResetFilter_PI;

        private SplitContainer splitContainerMain_PI;
        private DataGridView dataGridViewOrders_PI;
        private TableLayoutPanel panelRightLayout_PI;
        private Label labelStatistics_PI;
        private PictureBox pictureBoxHistogram_PI;

        private StatusStrip statusStripMain_PI;
        private ToolStripStatusLabel statusLabelInfo_PI;
        private ToolTip toolTipMain_PI;

        public MainForm_PI()
        {
            InitializeComponent();

            bindingSourceOrders_PI.DataSource = filteredOrders_PI;
            dataGridViewOrders_PI.DataSource = bindingSourceOrders_PI;

            Load += MainForm_PI_Load;

            toolTipMain_PI.SetToolTip(buttonApplyFilter_PI, "Применить фильтр по введённым условиям");
            toolTipMain_PI.SetToolTip(buttonResetFilter_PI, "Сбросить все фильтры");
            toolTipMain_PI.SetToolTip(textBoxSearch_PI, "Поиск по выбранному полю");
            toolTipMain_PI.SetToolTip(comboBoxFilterField_PI, "Выберите поле для поиска");
            toolTipMain_PI.SetToolTip(numericUpDownMinCost_PI, "Минимальная стоимость заказа");
            toolTipMain_PI.SetToolTip(pictureBoxHistogram_PI, "Гистограмма суммы стоимости заказов по товарам");

            UpdateStatistics();
        }

        private void MainForm_PI_Load(object sender, EventArgs e)
        {
            splitContainerMain_PI.SplitterDistance = Math.Max(400, splitContainerMain_PI.Width / 2);
        }

        private void InitializeComponent()
        {
            Text = "Заказы — Tyuiu.PlontikovPI.Sprint7 (Вариант 10)";
            Size = new Size(1200, 800);
            MinimumSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;

            rootLayout_PI = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5
            };

            rootLayout_PI.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            rootLayout_PI.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            rootLayout_PI.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            rootLayout_PI.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout_PI.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Меню
            menuStripMain_PI = new MenuStrip { Dock = DockStyle.Fill };

            fileToolStripMenuItem_PI = new ToolStripMenuItem("Файл");
            openToolStripMenuItem_PI = new ToolStripMenuItem("Открыть CSV...");
            saveToolStripMenuItem_PI = new ToolStripMenuItem("Сохранить");
            saveAsToolStripMenuItem_PI = new ToolStripMenuItem("Сохранить как...");
            exitToolStripMenuItem_PI = new ToolStripMenuItem("Выход");

            openToolStripMenuItem_PI.Click += OpenToolStripMenuItem_PI_Click;
            saveToolStripMenuItem_PI.Click += SaveToolStripMenuItem_PI_Click;
            saveAsToolStripMenuItem_PI.Click += SaveAsToolStripMenuItem_PI_Click;
            exitToolStripMenuItem_PI.Click += (s, e) => Close();

            fileToolStripMenuItem_PI.DropDownItems.AddRange(new ToolStripItem[]
            {
                openToolStripMenuItem_PI,
                saveToolStripMenuItem_PI,
                saveAsToolStripMenuItem_PI,
                new ToolStripSeparator(),
                exitToolStripMenuItem_PI
            });

            dataToolStripMenuItem_PI = new ToolStripMenuItem("Данные");
            addToolStripMenuItem_PI = new ToolStripMenuItem("Добавить заказ");
            deleteToolStripMenuItem_PI = new ToolStripMenuItem("Удалить заказ");
            refreshToolStripMenuItem_PI = new ToolStripMenuItem("Обновить");

            addToolStripMenuItem_PI.Click += AddToolStripMenuItem_PI_Click;
            deleteToolStripMenuItem_PI.Click += DeleteToolStripMenuItem_PI_Click;
            refreshToolStripMenuItem_PI.Click += RefreshToolStripMenuItem_PI_Click;

            dataToolStripMenuItem_PI.DropDownItems.AddRange(new ToolStripItem[]
            {
                addToolStripMenuItem_PI,
                deleteToolStripMenuItem_PI,
                refreshToolStripMenuItem_PI
            });

            helpToolStripMenuItem_PI = new ToolStripMenuItem("Справка");
            userGuideToolStripMenuItem_PI = new ToolStripMenuItem("Руководство пользователя");
            aboutToolStripMenuItem_PI = new ToolStripMenuItem("О программе");

            userGuideToolStripMenuItem_PI.Click += (s, e) => new HelpForm_PI().ShowDialog(this);
            aboutToolStripMenuItem_PI.Click += (s, e) => new AboutForm_PI().ShowDialog(this);

            helpToolStripMenuItem_PI.DropDownItems.AddRange(new ToolStripItem[]
            {
                userGuideToolStripMenuItem_PI,
                aboutToolStripMenuItem_PI
            });

            menuStripMain_PI.Items.AddRange(new ToolStripItem[]
            {
                fileToolStripMenuItem_PI,
                dataToolStripMenuItem_PI,
                helpToolStripMenuItem_PI
            });

            MainMenuStrip = menuStripMain_PI;
            rootLayout_PI.Controls.Add(menuStripMain_PI, 0, 0);

            // Панель инструментов
            toolStripMain_PI = new ToolStrip
            {
                Dock = DockStyle.Fill,
                GripStyle = ToolStripGripStyle.Hidden
            };

            toolStripButtonOpen_PI = new ToolStripButton("Открыть") { DisplayStyle = ToolStripItemDisplayStyle.Text };
            toolStripButtonSave_PI = new ToolStripButton("Сохранить") { DisplayStyle = ToolStripItemDisplayStyle.Text };
            toolStripButtonAdd_PI = new ToolStripButton("Добавить") { DisplayStyle = ToolStripItemDisplayStyle.Text };
            toolStripButtonDelete_PI = new ToolStripButton("Удалить") { DisplayStyle = ToolStripItemDisplayStyle.Text };
            toolStripButtonRefresh_PI = new ToolStripButton("Обновить") { DisplayStyle = ToolStripItemDisplayStyle.Text };
            toolStripButtonStatistics_PI = new ToolStripButton("Статистика") { DisplayStyle = ToolStripItemDisplayStyle.Text };

            toolStripButtonOpen_PI.Click += OpenToolStripMenuItem_PI_Click;
            toolStripButtonSave_PI.Click += SaveToolStripMenuItem_PI_Click;
            toolStripButtonAdd_PI.Click += AddToolStripMenuItem_PI_Click;
            toolStripButtonDelete_PI.Click += DeleteToolStripMenuItem_PI_Click;
            toolStripButtonRefresh_PI.Click += RefreshToolStripMenuItem_PI_Click;
            toolStripButtonStatistics_PI.Click += (s, e) => UpdateStatistics();

            toolStripMain_PI.Items.AddRange(new ToolStripItem[]
            {
                toolStripButtonOpen_PI,
                toolStripButtonSave_PI,
                new ToolStripSeparator(),
                toolStripButtonAdd_PI,
                toolStripButtonDelete_PI,
                toolStripButtonRefresh_PI,
                new ToolStripSeparator(),
                toolStripButtonStatistics_PI
            });

            rootLayout_PI.Controls.Add(toolStripMain_PI, 0, 1);

            // Панель фильтров
            panelFilter_PI = new Panel { Dock = DockStyle.Fill, Height = 44, Padding = new Padding(6) };
            filterFlowLayoutPanel_PI = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = false,
                AutoScroll = true
            };

            labelSearch_PI = new Label { Text = "Поиск:", AutoSize = true, Margin = new Padding(3, 9, 3, 3) };
            textBoxSearch_PI = new TextBox { Width = 220 };

            labelFilterField_PI = new Label { Text = "Поле:", AutoSize = true, Margin = new Padding(12, 9, 3, 3) };
            comboBoxFilterField_PI = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 160
            };
            comboBoxFilterField_PI.Items.AddRange(new object[]
            {
                "Все поля",
                "ФИО клиента",
                "Номер заказа",
                "Товар",
                "Адрес",
                "Телефон"
            });
            comboBoxFilterField_PI.SelectedIndex = 0;

            labelMinCost_PI = new Label { Text = "Мин. стоимость:", AutoSize = true, Margin = new Padding(12, 9, 3, 3) };
            numericUpDownMinCost_PI = new NumericUpDown
            {
                DecimalPlaces = 2,
                Maximum = 1000000000,
                Width = 120
            };

            buttonApplyFilter_PI = new Button { Text = "Фильтр", Width = 90 };
            buttonResetFilter_PI = new Button { Text = "Сброс", Width = 90 };

            buttonApplyFilter_PI.Click += ButtonApplyFilter_PI_Click;
            buttonResetFilter_PI.Click += ButtonResetFilter_PI_Click;
            textBoxSearch_PI.TextChanged += TextBoxSearch_PI_TextChanged;
            comboBoxFilterField_PI.SelectedIndexChanged += ComboBoxFilterField_PI_SelectedIndexChanged;
            numericUpDownMinCost_PI.ValueChanged += NumericUpDownMinCost_PI_ValueChanged;

            filterFlowLayoutPanel_PI.Controls.AddRange(new Control[]
            {
                labelSearch_PI,
                textBoxSearch_PI,
                labelFilterField_PI,
                comboBoxFilterField_PI,
                labelMinCost_PI,
                numericUpDownMinCost_PI,
                buttonApplyFilter_PI,
                buttonResetFilter_PI
            });

            panelFilter_PI.Controls.Add(filterFlowLayoutPanel_PI);
            rootLayout_PI.Controls.Add(panelFilter_PI, 0, 2);

            // Основная область
            splitContainerMain_PI = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical
            };

            splitContainerMain_PI.Panel1.Padding = new Padding(4);
            splitContainerMain_PI.Panel2.Padding = new Padding(4);

            dataGridViewOrders_PI = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };

            dataGridViewOrders_PI.CellValidating += DataGridViewOrders_PI_CellValidating;
            dataGridViewOrders_PI.CellEndEdit += DataGridViewOrders_PI_CellEndEdit;
            dataGridViewOrders_PI.DataError += DataGridViewOrders_PI_DataError;

            splitContainerMain_PI.Panel1.Controls.Add(dataGridViewOrders_PI);

            panelRightLayout_PI = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };

            panelRightLayout_PI.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            panelRightLayout_PI.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            labelStatistics_PI = new Label
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                Padding = new Padding(8),
                BorderStyle = BorderStyle.FixedSingle,
                Text = "Статистика"
            };

            pictureBoxHistogram_PI = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            pictureBoxHistogram_PI.Paint += PictureBoxHistogram_PI_Paint;
            pictureBoxHistogram_PI.Resize += (s, e) => pictureBoxHistogram_PI.Invalidate();

            panelRightLayout_PI.Controls.Add(labelStatistics_PI, 0, 0);
            panelRightLayout_PI.Controls.Add(pictureBoxHistogram_PI, 0, 1);

            splitContainerMain_PI.Panel2.Controls.Add(panelRightLayout_PI);
            rootLayout_PI.Controls.Add(splitContainerMain_PI, 0, 3);

            // Статусная строка
            statusStripMain_PI = new StatusStrip { Dock = DockStyle.Fill };
            statusLabelInfo_PI = new ToolStripStatusLabel("Готово");
            statusStripMain_PI.Items.Add(statusLabelInfo_PI);
            rootLayout_PI.Controls.Add(statusStripMain_PI, 0, 4);

            toolTipMain_PI = new ToolTip();

            Controls.Add(rootLayout_PI);
        }

        private void OpenToolStripMenuItem_PI_Click(object sender, EventArgs e)
        {
            using (var openFileDialogOrders_PI = new OpenFileDialog())
            {
                openFileDialogOrders_PI.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

                if (openFileDialogOrders_PI.ShowDialog() == DialogResult.OK)
                {
                    LoadFromFile(openFileDialogOrders_PI.FileName);
                }
            }
        }

        private void SaveToolStripMenuItem_PI_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath_PI))
            {
                SaveAs();
                return;
            }

            SaveToFile(currentFilePath_PI);
        }

        private void SaveAsToolStripMenuItem_PI_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void AddToolStripMenuItem_PI_Click(object sender, EventArgs e)
        {
            int nextId = orders_PI.Count == 0 ? 1 : orders_PI.Max(o => o.OrderId) + 1;

            var order = new Order
            {
                OrderId = nextId,
                ClientFullName = "Новый клиент",
                AccountNumber = string.Empty,
                Address = string.Empty,
                Phone = string.Empty,
                OrderDate = DateTime.Today,
                ExecutionDate = DateTime.Today.AddDays(7),
                Cost = 0m,
                ProductName = "Новый товар",
                ProductPrice = 0m,
                Quantity = 1
            };

            orders_PI.Add(order);
            ApplyFilter();

            if (dataGridViewOrders_PI.Rows.Count > 0)
            {
                int lastIndex = dataGridViewOrders_PI.Rows.Count - 1;
                dataGridViewOrders_PI.CurrentCell = dataGridViewOrders_PI.Rows[lastIndex].Cells[0];
                dataGridViewOrders_PI.BeginEdit(true);
            }
        }

        private void DeleteToolStripMenuItem_PI_Click(object sender, EventArgs e)
        {
            if (bindingSourceOrders_PI.Current is Order order)
            {
                if (MessageBox.Show(
                        "Удалить выбранный заказ?",
                        "Подтверждение",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    orders_PI.Remove(order);
                    ApplyFilter();
                }
            }
        }

        private void RefreshToolStripMenuItem_PI_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ButtonApplyFilter_PI_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ButtonResetFilter_PI_Click(object sender, EventArgs e)
        {
            textBoxSearch_PI.Text = string.Empty;
            comboBoxFilterField_PI.SelectedIndex = 0;
            numericUpDownMinCost_PI.Value = 0m;
            ApplyFilter();
        }

        private void TextBoxSearch_PI_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ComboBoxFilterField_PI_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void NumericUpDownMinCost_PI_ValueChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void DataGridViewOrders_PI_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateStatistics();
        }

        private void DataGridViewOrders_PI_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridViewOrders_PI.Rows[e.RowIndex].DataBoundItem is not Order)
                return;

            string propertyName = dataGridViewOrders_PI.Columns[e.ColumnIndex].DataPropertyName;
            string value = e.FormattedValue == null ? string.Empty : e.FormattedValue.ToString();
            bool cancel = false;

            switch (propertyName)
            {
                case nameof(Order.OrderId):
                    cancel = !int.TryParse(value, out int id) || id <= 0;
                    break;

                case nameof(Order.Cost):
                    cancel = !decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out _);
                    break;

                case nameof(Order.ProductPrice):
                    cancel = !decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out _);
                    break;

                case nameof(Order.Quantity):
                    cancel = !int.TryParse(value, out int quantity) || quantity < 0;
                    break;

                case nameof(Order.OrderDate):
                case nameof(Order.ExecutionDate):
                    cancel = !DateTime.TryParse(value, out _);
                    break;
            }

            if (cancel)
            {
                e.Cancel = true;
                dataGridViewOrders_PI.Rows[e.RowIndex].ErrorText = "Некорректное значение";
            }
            else
            {
                dataGridViewOrders_PI.Rows[e.RowIndex].ErrorText = string.Empty;
            }
        }

        private void DataGridViewOrders_PI_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            statusLabelInfo_PI.Text = "Ошибка ввода данных: " + e.Exception.Message;
        }

        private void LoadFromFile(string filePath)
        {
            try
            {
                var loadedOrders = repository_PI.Load(filePath);

                orders_PI.Clear();
                foreach (var order in loadedOrders)
                {
                    orders_PI.Add(order);
                }

                currentFilePath_PI = filePath;
                statusLabelInfo_PI.Text =
                    "Загружено: " + filePath + " (записей: " + loadedOrders.Count + ")";

                ApplyFilter();          // <-- вместо ResetBindings
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка загрузки файла: " + ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void SaveAs()
        {
            using (var saveFileDialogOrders_PI = new SaveFileDialog())
            {
                saveFileDialogOrders_PI.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialogOrders_PI.FileName = "orders.csv";

                if (saveFileDialogOrders_PI.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath_PI = saveFileDialogOrders_PI.FileName;
                    SaveToFile(currentFilePath_PI);
                }
            }
        }

        private void SaveToFile(string filePath)
        {
            try
            {
                repository_PI.Save(filePath, orders_PI);
                statusLabelInfo_PI.Text = "Сохранено: " + filePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка сохранения файла: " + ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter()
        {
            if (dataGridViewOrders_PI == null || bindingSourceOrders_PI == null)
                return;

            string search = (textBoxSearch_PI?.Text ?? string.Empty).Trim().ToLowerInvariant();
            string field = comboBoxFilterField_PI?.SelectedItem?.ToString() ?? "Все поля";
            decimal minCost = numericUpDownMinCost_PI?.Value ?? 0m;

            filteredOrders_PI.RaiseListChangedEvents = false;

            try
            {
                filteredOrders_PI.Clear();

                foreach (var order in orders_PI)
                {
                    if (order == null)
                        continue;

                    // Фильтр по минимальной стоимости
                    if (order.Cost < minCost)
                        continue;

                    // Поиск по выбранному полю
                    if (search.Length > 0)
                    {
                        string haystack;

                        switch (field)
                        {
                            case "ФИО клиента":
                                haystack = order.ClientFullName;
                                break;

                            case "Номер заказа":
                                haystack = order.OrderId.ToString(CultureInfo.CurrentCulture);
                                break;

                            case "Товар":
                                haystack = order.ProductName;
                                break;

                            case "Адрес":
                                haystack = order.Address;
                                break;

                            case "Телефон":
                                haystack = order.Phone;
                                break;

                            default: // "Все поля"
                                haystack = string.Join(" ",
                                    order.OrderId,
                                    order.ClientFullName,
                                    order.AccountNumber,
                                    order.Address,
                                    order.Phone,
                                    order.ProductName,
                                    order.Cost,
                                    order.ProductPrice,
                                    order.Quantity);
                                break;
                        }

                        if (!(haystack ?? string.Empty).ToLowerInvariant().Contains(search))
                            continue;
                    }

                    filteredOrders_PI.Add(order);
                }
            }
            finally
            {
                filteredOrders_PI.RaiseListChangedEvents = true;
                filteredOrders_PI.ResetBindings();
            }

            UpdateStatistics();
        }

        private List<Order> GetVisibleOrders_PI()
        {
            var result = new List<Order>();

            if (dataGridViewOrders_PI == null)
                return result;

            foreach (DataGridViewRow row in dataGridViewOrders_PI.Rows)
            {
                if (row.IsNewRow || !row.Visible)
                    continue;

                if (row.DataBoundItem is Order order)
                {
                    result.Add(order);
                }
            }

            return result;
        }

        private void UpdateStatistics()
        {
            if (labelStatistics_PI == null || pictureBoxHistogram_PI == null)
                return;

            var visibleOrders = GetVisibleOrders_PI();
            var stats = OrderStatistics.Calculate(visibleOrders);

            labelStatistics_PI.Text = string.Format(
                CultureInfo.CurrentCulture,
                "Показано заказов: {0}\n" +
                "Сумма стоимости: {1:N2}\n" +
                "Средняя стоимость: {2:N2}\n" +
                "Минимум: {3:N2}\n" +
                "Максимум: {4:N2}\n" +
                "Всего единиц товара: {5}",
                stats.Count,
                stats.TotalCost,
                stats.AverageCost,
                stats.MinCost,
                stats.MaxCost,
                stats.TotalQuantity);

            pictureBoxHistogram_PI.Invalidate();
        }

        private void PictureBoxHistogram_PI_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            var visibleOrders = GetVisibleOrders_PI();
            var data = OrderStatistics.GetCostByProduct(visibleOrders)
                .OrderByDescending(kv => kv.Value)
                .Take(8)
                .ToList();

            if (data.Count == 0)
            {
                e.Graphics.DrawString("Нет данных для гистограммы", Font, Brushes.Gray, 10, 10);
                return;
            }

            const int left = 45;
            const int top = 20;
            const int right = 15;
            const int bottom = 45;

            int width = pictureBoxHistogram_PI.ClientSize.Width - left - right;
            int height = pictureBoxHistogram_PI.ClientSize.Height - top - bottom;

            if (width <= 0 || height <= 0)
                return;

            e.Graphics.DrawLine(Pens.Black, left, top, left, top + height);
            e.Graphics.DrawLine(Pens.Black, left, top + height, left + width, top + height);

            decimal max = data.Max(kv => kv.Value);
            if (max <= 0)
                max = 1;

            int slot = width / data.Count;
            int barWidth = Math.Max(8, slot / 2);

            using (var labelFont = new Font("Segoe UI", 7f))
            using (var valueFont = new Font("Segoe UI", 7f))
            {
                for (int i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    float barHeight = (float)(item.Value / max * height);
                    float x = left + i * slot + (slot - barWidth) / 2f;
                    float y = top + height - barHeight;

                    e.Graphics.FillRectangle(Brushes.SteelBlue, x, y, barWidth, barHeight);

                    string label = item.Key.Length > 10
                        ? item.Key.Substring(0, 10) + "…"
                        : item.Key;

                    e.Graphics.DrawString(label, labelFont, Brushes.Black, x, top + height + 3);
                    e.Graphics.DrawString(item.Value.ToString("N0"), valueFont, Brushes.Black, x, y - 14);
                }
            }
        }
    }
}