using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace MiniNotepad
{
    public partial class Form1 : Form
    {
        RichTextBox editor;
        ToolStripStatusLabel status;

        public Form1()
        {
            InitializeComponent();

            this.Text = "Mini Notepad";
            this.Size = new Size(800, 550);
            this.StartPosition = FormStartPosition.CenterScreen;

            MenuStrip menu = new MenuStrip();

            string[] fileItems = { "New", "Open", "Save", "Exit" };
            string[] editItems = { "Cut", "Copy", "Paste" };

            ToolStripMenuItem file = new ToolStripMenuItem("File");
            ToolStripMenuItem edit = new ToolStripMenuItem("Edit");

            foreach (string item in fileItems)
            {
                ToolStripMenuItem option = new ToolStripMenuItem(item);
                option.Click += FileMenu_Click;
                file.DropDownItems.Add(option);
            }

            foreach (string item in editItems)
            {
                ToolStripMenuItem option = new ToolStripMenuItem(item);
                option.Click += EditMenu_Click;
                edit.DropDownItems.Add(option);
            }

            menu.Items.Add(file);
            menu.Items.Add(edit);

            editor = new RichTextBox();
            editor.Dock = DockStyle.Fill;
            editor.Font = new Font("Arial", 12);
            editor.BorderStyle = BorderStyle.None;
            editor.TextChanged += UpdateStatus;

            StatusStrip bar = new StatusStrip();
            status = new ToolStripStatusLabel();
            bar.Items.Add(status);

            this.Controls.Add(editor);
            this.Controls.Add(bar);
            this.Controls.Add(menu);

            this.MainMenuStrip = menu;

            UpdateStatus(null, null);
        }

        private void FileMenu_Click(object sender, EventArgs e)
        {
            string choice = ((ToolStripMenuItem)sender).Text;

            if (choice == "New")
            {
                editor.Clear();
            }
            else if (choice == "Open")
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Text Files (*.txt)|*.txt";

                if (open.ShowDialog() == DialogResult.OK)
                    editor.Text = File.ReadAllText(open.FileName);
            }
            else if (choice == "Save")
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Text Files (*.txt)|*.txt";
                save.DefaultExt = "txt";

                if (save.ShowDialog() == DialogResult.OK)
                    File.WriteAllText(save.FileName, editor.Text);
            }
            else if (choice == "Exit")
            {
                this.Close();
            }
        }

        private void EditMenu_Click(object sender, EventArgs e)
        {
            string choice = ((ToolStripMenuItem)sender).Text;

            if (choice == "Cut")
                editor.Cut();
            else if (choice == "Copy")
                editor.Copy();
            else if (choice == "Paste")
                editor.Paste();
        }

        private void UpdateStatus(object sender, EventArgs e)
        {
            int characters = editor.Text.Length;

            int words = editor.Text.Split(
                new char[] { ' ', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries
            ).Length;

            status.Text = "Words: " + words +
                          "     Characters: " + characters;
        }
    }
}
