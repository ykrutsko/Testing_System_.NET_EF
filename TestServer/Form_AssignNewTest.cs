﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestServer
{
    public partial class AssignNewTestForm : Form
    {
        public DALTestingSystemDB.Test Test  { get; set; }
        OpenMode openMode;
        public AssignNewTestForm(OpenMode mode)
        {
            openMode = mode;
            InitializeComponent();
        }

        private async void AssignNewTestForm_Load(object sender, EventArgs e)
        {
            this.Text = openMode == OpenMode.User ? "Assign new test for User" : "Assign new test for Group";
            dataGridView.Columns.Clear();
            bindingSource.DataSource = await Task.Run(() => Globals.repoTest.GetAll());
            dataGridView.DataSource = bindingSource;
            for (int i = 4; i <= 9; i++)
                if (i != 6 && i != 7)
                    dataGridView.Columns[i].Visible = false;
            dataGridView.Columns[0].Width = 50;
            dataGridView.Columns[1].Width = 260;
            dataGridView.Columns[2].Width = 170;
            dataGridView.Columns[3].Width = 260;
            dataGridView.Columns[6].Width = 80;
            dataGridView.Columns[7].Width = 120;
            dataGridView.Columns[6].HeaderText = "Archived";
            dataGridView.Columns[7].HeaderText = "Loaded date";
            dataGridView.ClearSelection();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Test = dataGridView.CurrentRow.DataBoundItem as DALTestingSystemDB.Test;
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = dataGridView.SelectedRows.Count != 0;
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnOk_Click(sender, e);
                this.DialogResult = DialogResult.OK;
            }
        }

        private async void textBox_TextChanged(object sender, EventArgs e)
        {
            bindingSource.DataSource = textBox.Text.Any() ?
                await Task.Run(() => Globals.repoTest.FindAll(x => x.Title.Contains(textBox.Text))) 
                : await Task.Run(() => Globals.repoTest.GetAll());
        }
    }
}