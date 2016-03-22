using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Questor
{
    public partial class FindingNemoForm : Form
    {
        private TextBox inputTextBox;
        private TextBox locationTextBox;
        private Button browseButton;
        private Button searchButton;
        private Label searchResultLabel;
        private TextBox searchResultTextBox;
        private int dirCount;
        private int fileCount;

        public FindingNemoForm()
        {
            InitializeComponent();

            // Form: FindingNemoForm
            this.Text = "Finding Nemo";
            this.Font = new Font("Segoe UI", 9);
            this.Size = new Size(350, 495);
            this.Icon = (Icon)Properties.Resources.find;
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;

            // Panel: mainPanel
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;

            // Label: inputLabel
            Label inputLabel = new Label();
            inputLabel.Text = "Search for:";
            inputLabel.AutoSize = true;
            inputLabel.Location = new Point(10, 10);

            // TextBox: inputTextBox
            inputTextBox = new TextBox();
            inputTextBox.Location = new Point(90, 10);
            inputTextBox.Size = new Size(230, 25);
            inputTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

            // Label: locationLabel
            Label locationLabel = new Label();
            locationLabel.Text = "Location:";
            locationLabel.AutoSize = true;
            locationLabel.Location = new Point(10, 40);

            // TextBox: locationTextBox
            locationTextBox = new TextBox();
            locationTextBox.Location = new Point(90, 40);
            locationTextBox.Size = new Size(190, 25);
            locationTextBox.ReadOnly = true;
            locationTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

            // Button: browseButton
            browseButton = new Button();
            browseButton.Text = "...";
            browseButton.Location = new Point(290, 40);
            browseButton.Size = new Size(30, 25);
            browseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Button: searchButton
            searchButton = new Button();
            searchButton.Text = "Search";
            searchButton.Location = new Point(245, 70);
            searchButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Label: searchResultLabel
            searchResultLabel = new Label();
            searchResultLabel.Text = "Search result:";
            searchResultLabel.AutoSize = true;
            searchResultLabel.Location = new Point(10, 100);

            // TextBox: searchResultTextBox
            searchResultTextBox = new TextBox();
            searchResultTextBox.Location = new Point(10, 120);
            searchResultTextBox.Multiline = true;
            searchResultTextBox.Size = new Size(310, 325);
            searchResultTextBox.ScrollBars = ScrollBars.Both;
            searchResultTextBox.WordWrap = true;
            searchResultTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

            // Adding components
            this.Controls.Add(mainPanel);
            mainPanel.Controls.Add(inputLabel);
            mainPanel.Controls.Add(inputTextBox);
            mainPanel.Controls.Add(locationLabel);
            mainPanel.Controls.Add(locationTextBox);
            mainPanel.Controls.Add(browseButton);
            mainPanel.Controls.Add(searchButton);
            mainPanel.Controls.Add(searchResultLabel);
            mainPanel.Controls.Add(searchResultTextBox);

            // Adding EventHandler
            this.Load += new EventHandler(FindingNemoForm_Load);
        }

        private void FindingNemoForm_Load(object sender, EventArgs evnt)
        {
            locationTextBox.Text = Directory.GetCurrentDirectory();

            // Adding EventHandler
            searchButton.Click += new EventHandler(searchButton_Click);
            browseButton.Click += new EventHandler(browseButton_Click);
        }

        private void browseButton_Click(object sender, EventArgs evnt)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.ShowNewFolderButton = false;
                folderBrowserDialog.ShowDialog();
                locationTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void searchButton_Click(object sender, EventArgs evnt)
        {
            searchResultTextBox.Clear();
            dirCount = 0;
            fileCount = 0;
            searchResultLabel.Text = "Searching...";

            if ((inputTextBox.Text.Trim().Length != 0) & (locationTextBox.Text.Trim().Length != 0))
            {
                searchResultTextBox.AppendText("[START SEARCHING FOR " + inputTextBox.Text.Trim() + " AT " + DateTime.Now + " ]\n");
                searchResultTextBox.AppendText("\n");
                search(locationTextBox.Text.Trim(), inputTextBox.Text.Trim());
                searchResultTextBox.AppendText("<----------[ END ]---------->");
            }
            else
                MessageBox.Show("Enter file name and path correctly", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
        }

        // Go through all folders and files to match specified input from user
        private void search(string fileOrDirectoryName, string input)
        {
            try
            {
                if (Directory.Exists(fileOrDirectoryName))
                {
                    foreach (var directory in Directory.GetDirectories(fileOrDirectoryName))
                    {
                        if (Regex.Match(onlyFileName(directory).ToLower(), input.ToLower()).Length != 0)
                        {
                            searchResultLabel.Text = "Search Result: " + (++dirCount) + " folder(s) and " + fileCount + " file(s) found";
                            searchResultTextBox.AppendText("<DIR> " + directory + "\n");
                            searchResultTextBox.AppendText("\n");
                        }

                        search(directory, input);
                    }

                    foreach (var file in Directory.GetFiles(fileOrDirectoryName))
                    {
                        if (Regex.Match(onlyFileName(file).ToLower(), input.ToLower()).Length != 0)
                        {
                            searchResultLabel.Text = "Search Result: " + dirCount + " folder(s) and " + (++fileCount) + " file(s) found";
                            searchResultTextBox.AppendText(file + "\n");
                            searchResultTextBox.AppendText("\n");
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        // make file or directory name short
        private string onlyFileName(string fileName)
        {
            while (((Regex.Match(fileName, @"\\.*")).ToString()).Substring(1).Length != 0)
            {
                fileName = ((Regex.Match(fileName, @"\\.*")).ToString()).Substring(1);

                if (((Regex.Match(fileName, @"\\.*")).ToString()).Substring(0).Length == 0)
                    break;
            }

            return fileName;
        }
    }
}
