using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace SufniBrowser
{
    public class BookmarksForm : Form
    {
        private ListBox listBox;
        private Button addButton;
        private Button deleteButton;
        private Button saveButton;
        private Button okButton;
        private List<string> bookmarks;
        public string SelectedUrl { get; private set; }

        /// <summary>
        /// Ha nem adsz meg listát, akkor a mentett (vagy új) bookmark–okat töltjük be.
        /// </summary>
        /// <param name="bookmarks">Bookmark lista (opcionális)</param>
        public BookmarksForm(List<string> bookmarks = null)
        {
            Text = "Bookmarks Manager";
            Width = 400;
            Height = 400;
            BackColor = Color.Black;
            ForeColor = Color.Cyan;

            // Ha nincs paraméterként megadott lista, akkor a DataManager-ből töltjük be (ha van korábbi mentés)
            this.bookmarks = bookmarks ?? DataManager.LoadBookmarks() ?? new List<string>();

            // ListBox, mely a bookmark–okat jeleníti meg
            listBox = new ListBox
            {
                Dock = DockStyle.Top,
                Height = 250,
                BackColor = Color.Black,
                ForeColor = Color.Cyan
            };
            UpdateListBox();

            // Gombok egy alsó panelen
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.FromArgb(30, 30, 30)
            };

            addButton = new Button
            {
                Text = "Add Bookmark",
                Width = 100,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Cyan
            };
            addButton.Click += AddButton_Click;

            deleteButton = new Button
            {
                Text = "Delete Selected",
                Width = 100,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Cyan
            };
            deleteButton.Click += DeleteButton_Click;

            saveButton = new Button
            {
                Text = "Save Changes",
                Width = 100,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Cyan
            };
            saveButton.Click += SaveButton_Click;

            okButton = new Button
            {
                Text = "OK",
                Width = 100,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Cyan
            };
            okButton.Click += OkButton_Click;

            buttonPanel.Controls.Add(addButton);
            buttonPanel.Controls.Add(deleteButton);
            buttonPanel.Controls.Add(saveButton);
            buttonPanel.Controls.Add(okButton);

            Controls.Add(listBox);
            Controls.Add(buttonPanel);
        }

        /// <summary>
        /// Frissíti a ListBox tartalmát a jelenlegi bookmark lista alapján.
        /// </summary>
        private void UpdateListBox()
        {
            listBox.Items.Clear();
            foreach (var bm in bookmarks)
            {
                listBox.Items.Add(bm);
            }
        }

        /// <summary>
        /// Új bookmark hozzáadása egy InputBox segítségével.
        /// </summary>
        private void AddButton_Click(object sender, EventArgs e)
        {
            string newBookmark = Interaction.InputBox("Enter bookmark URL:", "Add Bookmark", "");
            if (!string.IsNullOrWhiteSpace(newBookmark))
            {
                bookmarks.Add(newBookmark);
                UpdateListBox();
            }
        }

        /// <summary>
        /// A kiválasztott bookmark törlése.
        /// </summary>
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex >= 0)
            {
                bookmarks.RemoveAt(listBox.SelectedIndex);
                UpdateListBox();
            }
        }

        /// <summary>
        /// A változásokat lementi a DataManager segítségével.
        /// </summary>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            DataManager.SaveBookmarks(bookmarks);
            MessageBox.Show("Bookmarks saved.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Ha egy bookmark ki van választva, azt visszaadja a SelectedUrl property-ben.
        /// </summary>
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
    }
}
