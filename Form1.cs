namespace tema7
{
    struct ToolStrips
    {
        ToolStripMenuItem newFile;
        ToolStripMenuItem openFile;
        ToolStripMenuItem saveFile;
        ToolStripMenuItem cut;
        ToolStripMenuItem copy;
        ToolStripMenuItem paste;
        ToolStripMenuItem undo;
        ToolStripMenuItem redo;
    }

    public partial class Form1 : Form
    {
        ToolStrips toolStrips = new ToolStrips();
        public Form1()
        {
            InitializeComponent();
            var colorPicker = new Cyotek.Windows.Forms.ColorEditor();
            colorPicker.Location = new Point(30, 30);
            colorPicker.Visible = true;
            colorPicker.Size = new Size(300, 200);
            Controls.Add(colorPicker);
        }
    }
}
