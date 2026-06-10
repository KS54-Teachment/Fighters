Using System;
Using System.Drawing;
Using System.Windows.Forms;
Namespace CenterDigitalSupport
{
    Partial Public Class MainForm :  Form
    {
        Private DataGridView dgvRequests;
        Private TextBox txtFullName;
        Private TextBox txtDescription;
        Private ComboBox cmbStatus;
        Private DateTimePicker dtpDate;
        Private Button btnAdd;
        Private Button btnDelete;
        Private Button btnUpdate;
        Private Label lblFullName, lblDescription, lblStatus, lblDate;
        Private BindingSource bindingSource;
        Private DataTable dataTable;
        Public MainForm()
        {
            InitializeComponent();
            SetupDataTable();
            SetupBindings();
        }
        Private void InitializeComponent()
        {
            this.Text = "Центр цифровой поддержки";
            this.Size = New Size(800, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            
            lblFullName = New Label() { Text = "ФИО пользователя:", Location = New Point(20, 20), Width = 120 };
            lblDescription = New Label() { Text = "Описание проблемы:", Location = New Point(20, 60), Width = 120 };
            lblStatus = New Label() { Text = "Статус обращения:", Location = New Point(20, 100), Width = 120 };
            lblDate = New Label() { Text = "Дата обращения:", Location = New Point(20, 140), Width = 120 };
            
            txtFullName = New TextBox() { Location = New Point(150, 18), Width = 250 };
            
            txtDescription = New TextBox() { Location = New Point(150, 58), Width = 350, Height = 60, Multiline = true };
            
            cmbStatus = New ComboBox() { Location = New Point(150, 98), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatus.Items.AddRange(New string[] { "Новое", "В работе", "Решено", "Закрыто" });
            cmbStatus.SelectedIndex = 0;
            
            dtpDate = New DateTimePicker() { Location = New Point(150, 138), Width = 150, Format = DateTimePickerFormat.Short };
            
            btnAdd = New Button() { Text = "Добавить", Location = New Point(150, 180), Width = 100 };
            btnDelete = New Button() { Text = "Удалить", Location = New Point(270, 180), Width = 100 };
            btnUpdate = New Button() { Text = "Изменить", Location = New Point(390, 180), Width = 100 };
            
            dgvRequests = New DataGridView()
            {
                Location = New Point(20, 220),
                Size = New Size(740, 280),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true
            };
            
            Controls.Add(lblFullName);
            Controls.Add(lblDescription);
            Controls.Add(lblStatus);
            Controls.Add(lblDate);
            Controls.Add(txtFullName);
            Controls.Add(txtDescription);
            Controls.Add(cmbStatus);
            Controls.Add(dtpDate);
            Controls.Add(btnAdd);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(dgvRequests);
            
            btnAdd.Click
 += BtnAdd_Click;
            btnDelete.Click
 += BtnDelete_Click;
            btnUpdate.Click
 += BtnUpdate_Click;
            dgvRequests.SelectionChanged += DgvRequests_SelectionChanged;
        }
        Private void SetupDataTable()
        {
            dataTable = New DataTable();
            dataTable.Columns.Add("ID", TypeOf(int));
            dataTable.Columns.Add("FullName", TypeOf(string));
            dataTable.Columns.Add("Description", TypeOf(string));
dataTable.Columns.Add("Status", TypeOf(string));
            dataTable.Columns.Add("Date", TypeOf(DateTime));
            
            dataTable.Rows.Add(1, "Иванов Иван Иванович", "Не включается компьютер", "Новое", DateTime.Now
);
            dataTable.Rows.Add(2, "Петрова Анна Сергеевна", "Не работает принтер", "В работе", DateTime.Now.AddDays(-1));
            dataTable.Rows.Add(3, "Сидоров Петр Алексеевич", "Проблема с интернетом", "Решено", DateTime.Now.AddDays(-2));
        }
        private void SetupBindings()
        {
            bindingSource = new BindingSource();
            bindingSource.DataSource = dataTable;
            dgvRequests.DataSource = bindingSource;
            
            dgvRequests.Columns["ID"].HeaderText = "№";
            dgvRequests.Columns["FullName"].HeaderText = "ФИО пользователя";
            dgvRequests.Columns["Description"].HeaderText = "Описание проблемы";
            dgvRequests.Columns["Status"].HeaderText = "Статус";
            dgvRequests.Columns["Date"].HeaderText = "Дата обращения";
            dgvRequests.Columns["ID"].Width = 50;
            dgvRequests.Columns["FullName"].Width = 180;
            dgvRequests.Columns["Description"].Width = 250;
            dgvRequests.Columns["Status"].Width = 100;
            dgvRequests.Columns["Date"].Width = 100;
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show
("Введите ФИО пользователя!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show
("Введите описание проблемы!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int newId = dataTable.Rows.Count + 1;
            dataTable.Rows.Add(newId, txtFullName.Text.Trim(), txtDescription.Text.Trim(), cmbStatus.SelectedItem.ToString(), dtpDate.Value);
            ClearInputFields();
            MessageBox.Show
("Запись успешно добавлена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRequests.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvRequests.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                if (MessageBox.Show
("Вы уверены, что хотите удалить эту запись?", "Подтверждение", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataRow[] rows = dataTable.Select
($"ID = {id}");
                    if (rows.Length > 0)
                        dataTable.Rows.Remove(rows[0]);
                    
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                        dataTable.Rows[i]["ID"] = i + 1;
                    ClearInputFields();
                    MessageBox.Show
("Запись успешно удалена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show
("Выберите запись для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvRequests.SelectedRows.Count == 0)
            {
                MessageBox.Show
("Выберите запись для изменения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
MessageBox.Show
("Введите ФИО пользователя!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show
("Введите описание проблемы!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataGridViewRow selectedRow = dgv
        Requests.SelectedRows[0];
            int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
            DataRow[] rows = dataTable.Select
($"ID = {id}");
            if (rows.Length > 0)
            {
                rows[0]["FullName"] = txtFullName.Text.Trim();
                rows[0]["Description"] = txtDescription.Text.Trim();
                rows[0]["Status"] = cmbStatus.SelectedItem.ToString();
                rows[0]["Date"] = dtpDate.Value;
                ClearInputFields();
                MessageBox.Show
("Запись успешно изменена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void DgvRequests_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRequests.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvRequests.SelectedRows[0];
                txtFullName.Text = row.Cells["FullName"].Value.ToString();
                txtDescription.Text = row.Cells["Description"].Value.ToString();
                cmbStatus.SelectedItem = row.Cells["Status"].Value.ToString();
                dtpDate.Value = Convert.ToDateTime(row.Cells["Date"].Value);
            }
        }
        private void ClearInputFields()
        {
            txtFullName.Text = "";
            txtDescription.Text = "";
            cmbStatus.SelectedIndex = 0;
            dtpDate.Value = DateTime.Now
;
        }
    }
    
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run
(new MainForm());
        }
    }
}