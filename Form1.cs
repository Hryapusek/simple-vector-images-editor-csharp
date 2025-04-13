namespace tema7
{
    public partial class Form1 : Form
    {
        ToolStrips toolStrips = new ToolStrips();
        public Form1()
        {
            InitializeComponent();
            InitializeUI();
            var colorPicker = new Cyotek.Windows.Forms.ColorEditor();
            colorPicker.Location = new Point(30, 30);
            colorPicker.Visible = true;
            colorPicker.Size = new Size(300, 200);
            Controls.Add(colorPicker);
        }

        private void InitializeUI()
        {
            this.toolStrips.newFile = new ToolStripMenuItem();
            var res = Properties.Resources.ResourceManager.GetObject("NewDocumentImage") as System.Byte[];
            if (res == null)
            {
                throw new Exception("Resource not found");
            }
            this.toolStrips.newFile.Image = Image.FromStream(new MemoryStream(res));
            this.toolStrips.newFile.Name = "newFile";
            this.toolStrips.newFile.Size = new System.Drawing.Size(24, 24);
            this.toolStrip.Items.Add(this.toolStrips.newFile);
        }
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
        public List<ToolStripMenuItem?> GetAllToolStrips()
        {
            return new List<ToolStripMenuItem?> { newFile, openFile, saveFile, cut, copy, paste, undo, redo };
        }
    }
}
