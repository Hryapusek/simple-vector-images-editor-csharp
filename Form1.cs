namespace tema7
{
    public partial class Form1 : Form
    {
        ToolStrips toolStrips = new();
        Cyotek.Windows.Forms.ColorGrid colorGrid;
        GridView gridView = new();

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.MinimumSize = new Size(1400, 800);
            this.MaximumSize = new Size(1400, 800);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeToolStripsUI();
            InitializeColorPickerUI();
            InitializeGridViewUI();
        }

        private void InitializeGridViewUI()
        {
            gridView.Size = new Size(ClientSize.Width - colorGrid.Width - 30, ClientSize.Height);
            gridView.Location = new Point(0, 60);
            gridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(gridView);
        }

        private void InitializeColorPickerUI()
        {
            colorGrid = new Cyotek.Windows.Forms.ColorGrid();
            colorGrid.Size = new Size(300, 200);
            colorGrid.Location = new Point(ClientSize.Width - colorGrid.Width - 30, ClientSize.Height - colorGrid.Height - 30);
            colorGrid.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            colorGrid.Visible = true;
            Controls.Add(colorGrid);
        }

        private void InitializeToolStripsUI()
        {
            // Create and configure the tool strip items
            InitializeFileToolStripItems();
            InitializeEditToolStripItems();
            InitializeViewToolStripItems();

            // Add event handlers
            AttachEventHandlers();
        }

        private void InitializeFileToolStripItems()
        {
            // New File
            this.toolStrips.newFile = CreateToolStripMenuItem(
                "NewDocumentImage",
                "newFile",
                "New",
                "Ctrl+N");

            // Open File (placeholder - no icon in your resources)
            this.toolStrips.openFile = CreateToolStripMenuItem(
                "AddFolderImage", // Using folder icon as placeholder
                "openFile",
                "Open",
                "Ctrl+O");

            // Save File
            this.toolStrips.saveFile = CreateToolStripMenuItem(
                "SaveImage",
                "saveFile",
                "Save",
                "Ctrl+S");

            // Add to toolstrip
            this.toolStrip.Items.AddRange(new ToolStripItem[] {
                this.toolStrips.newFile,
                this.toolStrips.openFile,
                this.toolStrips.saveFile,
                new ToolStripSeparator()
            });
        }

        private void InitializeEditToolStripItems()
        {
            // Undo
            this.toolStrips.undo = CreateToolStripMenuItem(
                "UndoImage",
                "undo",
                "Undo",
                "Ctrl+Z");

            // Redo
            this.toolStrips.redo = CreateToolStripMenuItem(
                "RedoImage",
                "redo",
                "Redo",
                "Ctrl+Y");

            // Separator
            var separator1 = new ToolStripSeparator();

            // Cut
            this.toolStrips.cut = CreateToolStripMenuItem(
                "CutImage",
                "cut",
                "Cut",
                "Ctrl+X");

            // Copy
            this.toolStrips.copy = CreateToolStripMenuItem(
                "CopyImage",
                "copy",
                "Copy",
                "Ctrl+C");

            // Paste
            this.toolStrips.paste = CreateToolStripMenuItem(
                "PasteImage",
                "paste",
                "Paste",
                "Ctrl+V");

            // Add to toolstrip
            this.toolStrip.Items.AddRange(new ToolStripItem[] {
                this.toolStrips.undo,
                this.toolStrips.redo,
                separator1,
                this.toolStrips.cut,
                this.toolStrips.copy,
                this.toolStrips.paste
            });
        }

        private void InitializeViewToolStripItems()
        {
            // Toggle Grid Visibility
            this.toolStrips.toggleGrid = new ToolStripMenuItem();
            this.toolStrips.toggleGrid.Text = "Toggle Grid";
            this.toolStrips.toggleGrid.CheckOnClick = true;
            this.toolStrips.toggleGrid.Checked = true;
            this.toolStrips.toggleGrid.Click += (s, e) =>
            {
                gridView.ShowGrid = !gridView.ShowGrid;
                gridView.Invalidate();
            };

            // Grid Settings
            this.toolStrips.gridSettings = new ToolStripMenuItem();
            this.toolStrips.gridSettings.Text = "Grid Settings...";
            this.toolStrips.gridSettings.Click += ShowGridSettingsDialog;

            // Add to menu
            var viewMenu = new ToolStripMenuItem("Grid");
            var res = Properties.Resources.ResourceManager.GetObject("GridImage") as byte[];
            if (res != null)
            {
                viewMenu.Image = Image.FromStream(new MemoryStream(res));
                viewMenu.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            }
            viewMenu.DropDownItems.AddRange(new ToolStripItem[] {
                this.toolStrips.toggleGrid,
                this.toolStrips.gridSettings
            });

            this.toolStrip.Items.Add(viewMenu);
        }

        private void ShowGridSettingsDialog(object? sender, EventArgs e)
        {
            using (var form = new Form())
            {
                form.Text = "Grid Settings";
                form.Size = new Size(300, 200);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;

                var sizeLabel = new Label { Text = "Grid Size:", Left = 20, Top = 20 };
                var sizeNumeric = new NumericUpDown
                {
                    Minimum = 5,
                    Maximum = 100,
                    Value = gridView.GridSize,
                    Left = 120,
                    Top = 20
                };

                var snapCheck = new CheckBox
                {
                    Text = "Snap to Grid",
                    Checked = gridView.SnapToGrid,
                    Left = 20,
                    Top = 60
                };

                var colorLabel = new Label { Text = "Grid Color:", Left = 20, Top = 90 };
                var colorButton = new Button
                {
                    Text = "Choose...",
                    Left = 120,
                    Top = 90,
                    BackColor = gridView.GridColor
                };

                colorButton.Click += (s, ev) =>
                {
                    using (var colorDialog = new ColorDialog())
                    {
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            colorButton.BackColor = colorDialog.Color;
                        }
                    }
                };

                var okButton = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Left = 120,
                    Top = 130
                };

                form.Controls.AddRange(new Control[] {
                    sizeLabel, sizeNumeric,
                    snapCheck,
                    colorLabel, colorButton,
                    okButton
                });

                if (form.ShowDialog() == DialogResult.OK)
                {
                    gridView.GridSize = (int)sizeNumeric.Value;
                    gridView.SnapToGrid = snapCheck.Checked;
                    gridView.GridColor = colorButton.BackColor;
                    gridView.Invalidate();
                }
            }
        }
        private ToolStripMenuItem CreateToolStripMenuItem(
            string resourceName,
            string name,
            string text,
            string shortcutText)
        {
            var item = new ToolStripMenuItem();

            // Load image from resources
            var res = Properties.Resources.ResourceManager.GetObject(resourceName) as byte[];
            if (res != null)
            {
                item.Image = Image.FromStream(new MemoryStream(res));
                item.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            }

            item.Name = name;
            item.Text = text;
            item.Size = new Size(24, 24);
            item.ToolTipText = $"{text} ({shortcutText})";
            item.ShortcutKeyDisplayString = shortcutText;

            return item;
        }

        private void AttachEventHandlers()
        {
            // File operations
            this.toolStrips.newFile.Click += (s, e) => NewDocument();
            this.toolStrips.openFile.Click += (s, e) => OpenDocument();
            this.toolStrips.saveFile.Click += (s, e) => SaveDocument();

            // Edit operations
            this.toolStrips.undo.Click += (s, e) => UndoAction();
            this.toolStrips.redo.Click += (s, e) => RedoAction();
            this.toolStrips.cut.Click += (s, e) => CutSelected();
            this.toolStrips.copy.Click += (s, e) => CopySelected();
            this.toolStrips.paste.Click += (s, e) => PasteFromClipboard();
        }

        class ToolStrips
        {
            public ToolStripMenuItem newFile;
            public ToolStripMenuItem openFile;
            public ToolStripMenuItem saveFile;
            public ToolStripMenuItem cut;
            public ToolStripMenuItem copy;
            public ToolStripMenuItem paste;
            public ToolStripMenuItem undo;
            public ToolStripMenuItem redo;
            public ToolStripMenuItem toggleGrid;
            public ToolStripMenuItem gridSettings;
        }

        private void NewDocument()
        {
            // Clear the drawing canvas
            MessageBox.Show("New document created");
        }

        private void OpenDocument()
        {
            // Implement file open logic
            MessageBox.Show("Open document");
        }

        private void SaveDocument()
        {
            // Implement file save logic
            MessageBox.Show("Document saved");
        }

        private void UndoAction()
        {
            // Call your command manager's undo
            MessageBox.Show("Undo action");
        }

        private void RedoAction()
        {
            // Call your command manager's redo
            MessageBox.Show("Redo action");
        }

        private void CutSelected()
        {
            // Implement cut logic
            MessageBox.Show("Cut selected");
        }

        private void CopySelected()
        {
            // Implement copy logic
            MessageBox.Show("Copied to clipboard");
        }

        private void PasteFromClipboard()
        {
            // Implement paste logic
            MessageBox.Show("Pasted from clipboard");
        }
    }
}