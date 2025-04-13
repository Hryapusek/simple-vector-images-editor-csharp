namespace tema7
{
    public partial class Form1 : Form
    {
        ToolStrips toolStrips = new ToolStrips();
        public Form1()
        {
            InitializeComponent();
            InitializeUI();
            // var colorPicker = new Cyotek.Windows.Forms.ColorEditor();
            // colorPicker.Location = new Point(30, 30);
            // colorPicker.Visible = true;
            // colorPicker.Size = new Size(300, 200);
            // Controls.Add(colorPicker);
        }

        private void InitializeUI()
        {
            InitializeToolStripsUI();
        }

        private void InitializeToolStripsUI()
        {
            // Create and configure the tool strip items
            InitializeFileToolStripItems();
            InitializeEditToolStripItems();

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
            public List<ToolStripMenuItem> GetAllToolStrips()
            {
                return new List<ToolStripMenuItem> { newFile, openFile, saveFile, cut, copy, paste, undo, redo };
            }
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