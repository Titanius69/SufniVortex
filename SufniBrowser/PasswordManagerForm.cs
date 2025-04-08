using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SufniBrowser;

namespace SufniBrowser
{
    public class PasswordManagerForm : Form
    {
        private ListBox credentialsListBox;
        private Button addButton;
        private Button deleteButton;
        private Button saveButton;
        private Button closeButton;
        private List<Credential> credentials;

        public PasswordManagerForm()
        {
            Text = "Password Manager";
            Width = 600;
            Height = 400;
            BackColor = Color.Black;
            ForeColor = Color.Cyan;

            // Töltsük be a korábbi mentett adatok (ha nincs, hozzunk létre egy üres listát)
            credentials = DataManager.LoadPasswords() ?? new List<Credential>();

            // ListBox beállítása
            credentialsListBox = new ListBox()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                ForeColor = Color.Cyan
            };
            UpdateListBox();

            // Gombok elrendezése egy FlowLayoutPanel–ben
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.FromArgb(30, 30, 30)
            };

            addButton = new Button()
            {
                Text = "Add Credential",
                Width = 120,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Cyan
            };
            addButton.Click += AddButton_Click;

            deleteButton = new Button()
            {
                Text = "Delete Selected",
                Width = 120,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Cyan
            };
            deleteButton.Click += DeleteButton_Click;

            saveButton = new Button()
            {
                Text = "Save Changes",
                Width = 120,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Cyan
            };
            saveButton.Click += SaveButton_Click;

            closeButton = new Button()
            {
                Text = "Close",
                Width = 120,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Cyan
            };
            closeButton.Click += (s, e) => Close();

            buttonPanel.Controls.Add(addButton);
            buttonPanel.Controls.Add(deleteButton);
            buttonPanel.Controls.Add(saveButton);
            buttonPanel.Controls.Add(closeButton);

            // Hozzáadjuk a ListBox–ot és a gombpanelt az űrlaphoz
            Controls.Add(credentialsListBox);
            Controls.Add(buttonPanel);
        }

        private void UpdateListBox()
        {
            credentialsListBox.Items.Clear();
            foreach (var cred in credentials)
            {
                credentialsListBox.Items.Add($"{cred.Website} - {cred.Username}");
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddCredentialForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    credentials.Add(addForm.Credential);
                    UpdateListBox();
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (credentialsListBox.SelectedIndex >= 0)
            {
                credentials.RemoveAt(credentialsListBox.SelectedIndex);
                UpdateListBox();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            DataManager.SavePasswords(credentials);
            MessageBox.Show("Changes saved.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
