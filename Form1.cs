namespace tema7
{
    public partial class Form1 : Form
    {
        ToolStrips toolStrips = new();
        Cyotek.Windows.Forms.ColorGrid? colorGrid;
        GridView gridView = new();
        GroupBox groupBox = new();
        Scene scene = new();
        Figure? figureInProgress;
        private string? currentFilePath = null;

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
            InitializeSceneUI();
        }

        private void InitializeGridViewUI()
        {

            this.groupBox.Size = new Size(ClientSize.Width - colorGrid.Width - 30, ClientSize.Height);
            this.groupBox.Location = new Point(0, 30);
            this.groupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            this.gridView.Size = new Size(groupBox.Width, groupBox.Height);
            this.gridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.groupBox.Controls.Add(gridView);
            this.Controls.Add(groupBox);
        }

        private void InitializeColorPickerUI()
        {
            colorGrid = new Cyotek.Windows.Forms.ColorGrid
            {
                Size = new Size(300, 200)
            };
            colorGrid.Location = new Point(ClientSize.Width - colorGrid.Width - 30, ClientSize.Height - colorGrid.Height - 30);
            colorGrid.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            colorGrid.Visible = true;
            colorGrid.ColorChanged += (sender, e) => scene.SetColor(colorGrid.Color);
            Controls.Add(colorGrid);
        }

        private void InitializeToolStripsUI()
        {
            // Create and configure the tool strip items
            InitializeFileToolStripItems();
            InitializeEditToolStripItems();
            InitializeViewToolStripItems();
            InitializeFiguresToolStripItems();

            // Add event handlers
            AttachEventHandlers();
        }

        private void InitializeSceneUI()
        {
            gridView.BackColor = Color.White;
            gridView.Paint += (s, e) => scene.Draw(e.Graphics);
            gridView.MouseDown += DrawingPanel_MouseDown;
            gridView.MouseMove += DrawingPanel_MouseMove;
            gridView.MouseUp += DrawingPanel_MouseUp;
            gridView.scene = scene;
            scene.SceneChanged += () => gridView.Invalidate();
            gridView.BringToFront();
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
            this.toolStrip.Items.AddRange([
                this.toolStrips.newFile,
                this.toolStrips.openFile,
                this.toolStrips.saveFile,
                new ToolStripSeparator()
            ]);
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
            this.toolStrip.Items.AddRange([
                this.toolStrips.undo,
                this.toolStrips.redo,
                separator1,
                this.toolStrips.cut,
                this.toolStrips.copy,
                this.toolStrips.paste
            ]);
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
            viewMenu.DropDownItems.AddRange([
                this.toolStrips.toggleGrid,
                this.toolStrips.gridSettings
            ]);

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

                form.Controls.AddRange([
                    sizeLabel, sizeNumeric,
                    snapCheck,
                    colorLabel, colorButton,
                    okButton
                ]);

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
            public ToolStripMenuItem? newFile;
            public ToolStripMenuItem? openFile;
            public ToolStripMenuItem? saveFile;
            public ToolStripMenuItem? cut;
            public ToolStripMenuItem? copy;
            public ToolStripMenuItem? paste;
            public ToolStripMenuItem? undo;
            public ToolStripMenuItem? redo;
            public ToolStripMenuItem? toggleGrid;
            public ToolStripMenuItem? gridSettings;
            public ToolStripMenuItem? triangle;
            public ToolStripMenuItem? square;
            public ToolStripMenuItem? pentagon;
            public ToolStripMenuItem? hexagon;
        }

        private void NewDocument()
        {
            if (scene.Figures.Count > 0)
            {
                var result = MessageBox.Show("Save current document before creating new one?",
                                           "Unsaved Changes",
                                           MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    SaveDocument();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            scene = new Scene();
            gridView.scene = scene;
            scene.SceneChanged += () => gridView.Invalidate();
            UpdateWindowTitle("Untitled");
            MessageBox.Show("New document created", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OpenDocument()
        {
            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "Scene Files (*.scene)|*.scene|All Files (*.*)|*.*";
                openDialog.DefaultExt = "scene";
                openDialog.Title = "Open Vector Drawing";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var newScene = SceneSerializer.LoadFromFile(openDialog.FileName);
                        scene = newScene;
                        scene.SceneChanged += () => gridView.Invalidate();
                        gridView.scene = scene;
                        gridView.Invalidate();
                        Console.WriteLine($"Loaded scene with {scene.Figures.Count} figures");
                        UpdateWindowTitle(openDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to open file: {ex.Message}",
                                      "Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveDocument()
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveDocumentAs();
            }
            else
            {
                try
                {
                    SceneSerializer.SaveToFile(scene, currentFilePath);
                    UpdateWindowTitle(currentFilePath);
                    MessageBox.Show("Document saved successfully",
                                   "Success",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save file: {ex.Message}",
                                  "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
        }

        private void SaveDocumentAs()
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Scene Files (*.scene)|*.scene|All Files (*.*)|*.*";
                saveDialog.DefaultExt = "scene";
                saveDialog.Title = "Save Vector Drawing";
                saveDialog.AddExtension = true;

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveDialog.FileName;
                    SaveDocument();
                }
            }
        }

        private void UpdateWindowTitle(string filePath)
        {
            string title = Path.GetFileNameWithoutExtension(filePath);
            if (string.IsNullOrEmpty(title)) title = "Untitled";
            this.Text = $"{title} - Vector Editor";
        }

        private void UndoAction()
        {
            scene.Undo();
        }

        private void RedoAction()
        {
            // Call your command manager's redo
            scene.Redo();
        }

        private void CutSelected()
        {
            scene.CutSelected();
        }

        private void CopySelected()
        {
            scene.CopySelected();
        }

        private void PasteFromClipboard()
        {
            scene.Paste();
        }

        private void DrawingPanel_MouseDown(object? sender, MouseEventArgs e)
        {
            Console.WriteLine($"Mouse down at {e.Location}");
            if (e.Button == MouseButtons.Left)
            {
                if (figureInProgress != null)
                {
                    Console.WriteLine($"Adding figure at {e.Location}");
                    figureInProgress.Position = e.Location;
                    scene.AddFigure(figureInProgress);
                    gridView.Invalidate();
                    figureInProgress = null;
                    return;
                }
                scene.HandleMouseDown(e.Location);
            }
        }

        private void DrawingPanel_MouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                scene.HandleMouseMove(e.Location);
            }
        }

        private void DrawingPanel_MouseUp(object? sender, MouseEventArgs e)
        {
            Console.WriteLine($"Mouse up at {e.Location}");
            if (e.Button == MouseButtons.Left)
            {
                scene.HandleMouseUp(e.Location);
            }
        }

        private void InitializeFiguresToolStripItems()
        {
            this.toolStrips.square = CreateToolStripMenuItem(
                "SquareImage",
                "square",
                "",
                "");

            this.toolStrips.square.Click += (s, e) =>
            {
                Console.WriteLine("Square selected");
                figureInProgress = new Square
                {
                    Position = Point.Empty, // Will be set on mouse down
                    StrokeColor = Color.Blue,
                    StrokeWidth = 2f
                };
            };
            this.toolStrip.Items.Add(this.toolStrips.square);
        }
    }
}