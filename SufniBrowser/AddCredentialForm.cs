using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SufniBrowser
{
    public class AddCredentialForm : Form
    {
        private TextBox websiteTextBox;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private Button okButton;
        private Button cancelButton;

        public Credential Credential { get; private set; }

        public AddCredentialForm()
        {
            Text = "Add Credential";
            Width = 400;
            Height = 250;
            BackColor = Color.Black;
            ForeColor = Color.Cyan;

            Label websiteLabel = new Label() { Text = "Website:", Left = 10, Top = 20, ForeColor = Color.Cyan };
            websiteTextBox = new TextBox() { Left = 100, Top = 20, Width = 250, BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.Cyan };

            Label usernameLabel = new Label() { Text = "Username:", Left = 10, Top = 60, ForeColor = Color.Cyan };
            usernameTextBox = new TextBox() { Left = 100, Top = 60, Width = 250, BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.Cyan };

            Label passwordLabel = new Label() { Text = "Password:", Left = 10, Top = 100, ForeColor = Color.Cyan };
            passwordTextBox = new TextBox() { Left = 100, Top = 100, Width = 250, BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.Cyan, PasswordChar = '*' };

            okButton = new Button() { Text = "OK", Left = 100, Top = 150, Width = 100, BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.Cyan };
            okButton.Click += OkButton_Click;

            cancelButton = new Button() { Text = "Cancel", Left = 210, Top = 150, Width = 100, BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.Cyan };
            cancelButton.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(websiteLabel);
            Controls.Add(websiteTextBox);
            Controls.Add(usernameLabel);
            Controls.Add(usernameTextBox);
            Controls.Add(passwordLabel);
            Controls.Add(passwordTextBox);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(websiteTextBox.Text) ||
                string.IsNullOrEmpty(usernameTextBox.Text) ||
                string.IsNullOrEmpty(passwordTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Credential = new Credential
            {
                Website = websiteTextBox.Text,
                Username = usernameTextBox.Text,
                Password = passwordTextBox.Text
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
