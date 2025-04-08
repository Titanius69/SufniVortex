using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SufniBrowser
{
    public class HistoryForm : Form
    {
        private ListBox listBox;
        private Button okButton;
        public string SelectedUrl { get; private set; }

        public HistoryForm(List<string> history)
        {
            Text = "History";
            Width = 400;
            Height = 300;
            BackColor = Color.Black;
            ForeColor = Color.Cyan;

            listBox = new ListBox
            {
                Dock = DockStyle.Top,
                Height = 200,
                BackColor = Color.Black,
                ForeColor = Color.Cyan
            };
            listBox.Items.AddRange(history.ToArray());
            listBox.DoubleClick += ListBox_DoubleClick;

            okButton = new Button
            {
                Text = "OK",
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Cyan
            };
            okButton.Click += OkButton_Click;

            Controls.Add(listBox);
            Controls.Add(okButton);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                SelectedUrl = listBox.SelectedItem.ToString();
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
            Close();
        }

        private void ListBox_DoubleClick(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                SelectedUrl = listBox.SelectedItem.ToString();
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
