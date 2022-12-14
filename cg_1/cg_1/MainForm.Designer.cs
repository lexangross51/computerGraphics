using System.Drawing;

namespace ComputerGraphics
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.GL = new SharpGL.OpenGLControl();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusXPosName = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusXPosValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusYPosName = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusYPosValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusXShiftName = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusXShiftValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusYShiftName = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusYShiftValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.PrimitiveScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.PrimitiveAngle = new System.Windows.Forms.ToolStripStatusLabel();
            this.ChangeSet = new System.Windows.Forms.NumericUpDown();
            this.Sets = new System.Windows.Forms.GroupBox();
            this.SetMove = new System.Windows.Forms.GroupBox();
            this.ResetBtn = new System.Windows.Forms.Button();
            this.RightBtn = new System.Windows.Forms.Button();
            this.DownBtn = new System.Windows.Forms.Button();
            this.LeftBtn = new System.Windows.Forms.Button();
            this.UpBtn = new System.Windows.Forms.Button();
            this.comboBoxSet = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ChangeColorS = new System.Windows.Forms.Button();
            this.ChangeWidthS = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.AddSet = new System.Windows.Forms.Button();
            this.DeleteSet = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.DeletePrimitive = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Primitives = new System.Windows.Forms.GroupBox();
            this.ScaleResetBtn = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.RotateResetBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.RotateRight = new System.Windows.Forms.Button();
            this.RotateLeft = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxLine = new System.Windows.Forms.ComboBox();
            this.ChangePrimitive = new System.Windows.Forms.NumericUpDown();
            this.ChangeColorP = new System.Windows.Forms.Button();
            this.ChangeWidthP = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.reg = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.regLabel = new System.Windows.Forms.Label();
            this.infoBox = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.GL)).BeginInit();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangeSet)).BeginInit();
            this.Sets.SuspendLayout();
            this.SetMove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangeWidthS)).BeginInit();
            this.Primitives.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangePrimitive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChangeWidthP)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.infoBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // GL
            // 
            this.GL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GL.DrawFPS = false;
            this.GL.Location = new System.Drawing.Point(0, 2);
            this.GL.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GL.Name = "GL";
            this.GL.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.GL.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.GL.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.GL.Size = new System.Drawing.Size(1259, 836);
            this.GL.TabIndex = 0;
            this.GL.OpenGLInitialized += new System.EventHandler(this.GL_OpenGLInitialized);
            this.GL.OpenGLDraw += new SharpGL.RenderEventHandler(this.GL_OpenGLDraw);
            this.GL.Resized += new System.EventHandler(this.GL_Resized);
            this.GL.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GL_MouseClick);
            this.GL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GL_MouseMove);
            this.GL.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.GL_MouseScroll);
            // 
            // statusBar
            // 
            this.statusBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusXPosName,
            this.statusXPosValue,
            this.statusYPosName,
            this.statusYPosValue,
            this.toolStripStatusLabel1,
            this.statusXShiftName,
            this.statusXShiftValue,
            this.statusYShiftName,
            this.statusYShiftValue,
            this.toolStripStatusLabel2,
            this.PrimitiveScale,
            this.toolStripStatusLabel3,
            this.PrimitiveAngle});
            this.statusBar.Location = new System.Drawing.Point(0, 842);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1635, 30);
            this.statusBar.TabIndex = 1;
            this.statusBar.Text = " ";
            // 
            // statusXPosName
            // 
            this.statusXPosName.Name = "statusXPosName";
            this.statusXPosName.Size = new System.Drawing.Size(21, 24);
            this.statusXPosName.Text = "X:";
            // 
            // statusXPosValue
            // 
            this.statusXPosValue.AutoSize = false;
            this.statusXPosValue.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.statusXPosValue.Name = "statusXPosValue";
            this.statusXPosValue.Size = new System.Drawing.Size(45, 24);
            this.statusXPosValue.Text = "0000";
            // 
            // statusYPosName
            // 
            this.statusYPosName.Name = "statusYPosName";
            this.statusYPosName.Size = new System.Drawing.Size(20, 24);
            this.statusYPosName.Text = "Y:";
            // 
            // statusYPosValue
            // 
            this.statusYPosValue.AutoSize = false;
            this.statusYPosValue.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.statusYPosValue.Name = "statusYPosValue";
            this.statusYPosValue.Size = new System.Drawing.Size(45, 24);
            this.statusYPosValue.Text = "0000";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(69, 24);
            this.toolStripStatusLabel1.Text = "               ";
            // 
            // statusXShiftName
            // 
            this.statusXShiftName.Name = "statusXShiftName";
            this.statusXShiftName.Size = new System.Drawing.Size(87, 24);
            this.statusXShiftName.Text = "Сдвиг по X:";
            this.statusXShiftName.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // statusXShiftValue
            // 
            this.statusXShiftValue.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.statusXShiftValue.Name = "statusXShiftValue";
            this.statusXShiftValue.Size = new System.Drawing.Size(21, 24);
            this.statusXShiftValue.Text = "0";
            // 
            // statusYShiftName
            // 
            this.statusYShiftName.Name = "statusYShiftName";
            this.statusYShiftName.Size = new System.Drawing.Size(98, 24);
            this.statusYShiftName.Text = "   Сдвиг по Y:";
            // 
            // statusYShiftValue
            // 
            this.statusYShiftValue.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.statusYShiftValue.Name = "statusYShiftValue";
            this.statusYShiftValue.Size = new System.Drawing.Size(21, 24);
            this.statusYShiftValue.Text = "0";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(208, 24);
            this.toolStripStatusLabel2.Text = "               Масшаб примитива";
            // 
            // PrimitiveScale
            // 
            this.PrimitiveScale.AutoSize = false;
            this.PrimitiveScale.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.PrimitiveScale.Name = "PrimitiveScale";
            this.PrimitiveScale.Size = new System.Drawing.Size(35, 24);
            this.PrimitiveScale.Text = "1x";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(257, 24);
            this.toolStripStatusLabel3.Text = "               Угол поворота примитива:";
            // 
            // PrimitiveAngle
            // 
            this.PrimitiveAngle.AutoSize = false;
            this.PrimitiveAngle.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.PrimitiveAngle.Name = "PrimitiveAngle";
            this.PrimitiveAngle.Size = new System.Drawing.Size(30, 24);
            this.PrimitiveAngle.Text = "0";
            // 
            // ChangeSet
            // 
            this.ChangeSet.Enabled = false;
            this.ChangeSet.Location = new System.Drawing.Point(119, 26);
            this.ChangeSet.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ChangeSet.Name = "ChangeSet";
            this.ChangeSet.ReadOnly = true;
            this.ChangeSet.Size = new System.Drawing.Size(84, 22);
            this.ChangeSet.TabIndex = 2;
            this.ChangeSet.ValueChanged += new System.EventHandler(this.ChangeSet_ValueChanged);
            // 
            // Sets
            // 
            this.Sets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sets.Controls.Add(this.SetMove);
            this.Sets.Controls.Add(this.comboBoxSet);
            this.Sets.Controls.Add(this.label6);
            this.Sets.Controls.Add(this.ChangeColorS);
            this.Sets.Controls.Add(this.ChangeWidthS);
            this.Sets.Controls.Add(this.label3);
            this.Sets.Controls.Add(this.AddSet);
            this.Sets.Controls.Add(this.DeleteSet);
            this.Sets.Controls.Add(this.label1);
            this.Sets.Controls.Add(this.ChangeSet);
            this.Sets.Location = new System.Drawing.Point(1280, 12);
            this.Sets.Name = "Sets";
            this.Sets.Size = new System.Drawing.Size(343, 269);
            this.Sets.TabIndex = 3;
            this.Sets.TabStop = false;
            this.Sets.Text = "Управление наборами";
            // 
            // SetMove
            // 
            this.SetMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SetMove.Controls.Add(this.ResetBtn);
            this.SetMove.Controls.Add(this.RightBtn);
            this.SetMove.Controls.Add(this.DownBtn);
            this.SetMove.Controls.Add(this.LeftBtn);
            this.SetMove.Controls.Add(this.UpBtn);
            this.SetMove.Enabled = false;
            this.SetMove.Location = new System.Drawing.Point(38, 125);
            this.SetMove.Name = "SetMove";
            this.SetMove.Size = new System.Drawing.Size(265, 138);
            this.SetMove.TabIndex = 19;
            this.SetMove.TabStop = false;
            this.SetMove.Text = "Перемещение";
            // 
            // ResetBtn
            // 
            this.ResetBtn.Image = ((System.Drawing.Image)(resources.GetObject("ResetBtn.Image")));
            this.ResetBtn.Location = new System.Drawing.Point(115, 55);
            this.ResetBtn.Name = "ResetBtn";
            this.ResetBtn.Size = new System.Drawing.Size(34, 35);
            this.ResetBtn.TabIndex = 4;
            this.ResetBtn.UseVisualStyleBackColor = true;
            this.ResetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
            // 
            // RightBtn
            // 
            this.RightBtn.Image = ((System.Drawing.Image)(resources.GetObject("RightBtn.Image")));
            this.RightBtn.Location = new System.Drawing.Point(155, 55);
            this.RightBtn.Name = "RightBtn";
            this.RightBtn.Size = new System.Drawing.Size(34, 35);
            this.RightBtn.TabIndex = 3;
            this.RightBtn.UseVisualStyleBackColor = true;
            this.RightBtn.Click += new System.EventHandler(this.RightBtn_Click);
            // 
            // DownBtn
            // 
            this.DownBtn.Image = ((System.Drawing.Image)(resources.GetObject("DownBtn.Image")));
            this.DownBtn.Location = new System.Drawing.Point(115, 96);
            this.DownBtn.Name = "DownBtn";
            this.DownBtn.Size = new System.Drawing.Size(34, 35);
            this.DownBtn.TabIndex = 2;
            this.DownBtn.UseVisualStyleBackColor = true;
            this.DownBtn.Click += new System.EventHandler(this.DownBtn_Click);
            // 
            // LeftBtn
            // 
            this.LeftBtn.Image = ((System.Drawing.Image)(resources.GetObject("LeftBtn.Image")));
            this.LeftBtn.Location = new System.Drawing.Point(75, 55);
            this.LeftBtn.Name = "LeftBtn";
            this.LeftBtn.Size = new System.Drawing.Size(34, 35);
            this.LeftBtn.TabIndex = 1;
            this.LeftBtn.UseVisualStyleBackColor = true;
            this.LeftBtn.Click += new System.EventHandler(this.LeftBtn_Click);
            // 
            // UpBtn
            // 
            this.UpBtn.Image = ((System.Drawing.Image)(resources.GetObject("UpBtn.Image")));
            this.UpBtn.Location = new System.Drawing.Point(115, 14);
            this.UpBtn.Name = "UpBtn";
            this.UpBtn.Size = new System.Drawing.Size(34, 35);
            this.UpBtn.TabIndex = 0;
            this.UpBtn.UseVisualStyleBackColor = true;
            this.UpBtn.Click += new System.EventHandler(this.UpBtn_Click);
            // 
            // comboBoxSet
            // 
            this.comboBoxSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSet.FormattingEnabled = true;
            this.comboBoxSet.Items.AddRange(new object[] {
            "Сплошной",
            "Точечный",
            "Штриховой",
            "Штрихпунктирный"});
            this.comboBoxSet.Location = new System.Drawing.Point(119, 95);
            this.comboBoxSet.Name = "comboBoxSet";
            this.comboBoxSet.Size = new System.Drawing.Size(121, 24);
            this.comboBoxSet.TabIndex = 18;
            this.comboBoxSet.SelectedIndexChanged += new System.EventHandler(this.comboBoxSet_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 23);
            this.label6.TabIndex = 17;
            this.label6.Text = "Тип линиий";
            // 
            // ChangeColorS
            // 
            this.ChangeColorS.Image = ((System.Drawing.Image)(resources.GetObject("ChangeColorS.Image")));
            this.ChangeColorS.Location = new System.Drawing.Point(220, 58);
            this.ChangeColorS.Name = "ChangeColorS";
            this.ChangeColorS.Size = new System.Drawing.Size(57, 28);
            this.ChangeColorS.TabIndex = 16;
            this.ChangeColorS.UseVisualStyleBackColor = true;
            this.ChangeColorS.Click += new System.EventHandler(this.ChangeColorS_Click);
            // 
            // ChangeWidthS
            // 
            this.ChangeWidthS.Location = new System.Drawing.Point(119, 61);
            this.ChangeWidthS.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ChangeWidthS.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ChangeWidthS.Name = "ChangeWidthS";
            this.ChangeWidthS.ReadOnly = true;
            this.ChangeWidthS.Size = new System.Drawing.Size(84, 22);
            this.ChangeWidthS.TabIndex = 15;
            this.ChangeWidthS.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ChangeWidthS.ValueChanged += new System.EventHandler(this.ChangeWidthS_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "Ширина линий";
            // 
            // AddSet
            // 
            this.AddSet.Image = ((System.Drawing.Image)(resources.GetObject("AddSet.Image")));
            this.AddSet.Location = new System.Drawing.Point(283, 23);
            this.AddSet.Name = "AddSet";
            this.AddSet.Size = new System.Drawing.Size(57, 27);
            this.AddSet.TabIndex = 8;
            this.AddSet.UseVisualStyleBackColor = true;
            this.AddSet.Click += new System.EventHandler(this.AddSet_Click);
            // 
            // DeleteSet
            // 
            this.DeleteSet.Image = ((System.Drawing.Image)(resources.GetObject("DeleteSet.Image")));
            this.DeleteSet.Location = new System.Drawing.Point(220, 23);
            this.DeleteSet.Name = "DeleteSet";
            this.DeleteSet.Size = new System.Drawing.Size(57, 27);
            this.DeleteSet.TabIndex = 6;
            this.DeleteSet.UseVisualStyleBackColor = true;
            this.DeleteSet.Click += new System.EventHandler(this.DeleteSet_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Текущий набор";
            // 
            // DeletePrimitive
            // 
            this.DeletePrimitive.Image = ((System.Drawing.Image)(resources.GetObject("DeletePrimitive.Image")));
            this.DeletePrimitive.Location = new System.Drawing.Point(250, 23);
            this.DeletePrimitive.Name = "DeletePrimitive";
            this.DeletePrimitive.Size = new System.Drawing.Size(65, 28);
            this.DeletePrimitive.TabIndex = 10;
            this.DeletePrimitive.UseVisualStyleBackColor = true;
            this.DeletePrimitive.Click += new System.EventHandler(this.DeletePrimitive_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Текущий примитив";
            // 
            // Primitives
            // 
            this.Primitives.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Primitives.Controls.Add(this.numericUpDown1);
            this.Primitives.Controls.Add(this.ScaleResetBtn);
            this.Primitives.Controls.Add(this.label8);
            this.Primitives.Controls.Add(this.RotateResetBtn);
            this.Primitives.Controls.Add(this.label7);
            this.Primitives.Controls.Add(this.RotateRight);
            this.Primitives.Controls.Add(this.RotateLeft);
            this.Primitives.Controls.Add(this.label5);
            this.Primitives.Controls.Add(this.comboBoxLine);
            this.Primitives.Controls.Add(this.ChangePrimitive);
            this.Primitives.Controls.Add(this.ChangeColorP);
            this.Primitives.Controls.Add(this.ChangeWidthP);
            this.Primitives.Controls.Add(this.label4);
            this.Primitives.Controls.Add(this.label2);
            this.Primitives.Controls.Add(this.DeletePrimitive);
            this.Primitives.Location = new System.Drawing.Point(1280, 298);
            this.Primitives.Name = "Primitives";
            this.Primitives.Size = new System.Drawing.Size(343, 209);
            this.Primitives.TabIndex = 11;
            this.Primitives.TabStop = false;
            this.Primitives.Text = "Управление примитивами";
            // 
            // ScaleResetBtn
            // 
            this.ScaleResetBtn.Image = ((System.Drawing.Image)(resources.GetObject("ScaleResetBtn.Image")));
            this.ScaleResetBtn.Location = new System.Drawing.Point(143, 170);
            this.ScaleResetBtn.Name = "ScaleResetBtn";
            this.ScaleResetBtn.Size = new System.Drawing.Size(34, 22);
            this.ScaleResetBtn.TabIndex = 22;
            this.ScaleResetBtn.UseVisualStyleBackColor = true;
            this.ScaleResetBtn.Click += new System.EventHandler(this.ScaleResetBtn_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 174);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 16);
            this.label8.TabIndex = 21;
            this.label8.Text = "Сброс масштаба";
            // 
            // RotateResetBtn
            // 
            this.RotateResetBtn.Image = ((System.Drawing.Image)(resources.GetObject("RotateResetBtn.Image")));
            this.RotateResetBtn.Location = new System.Drawing.Point(183, 123);
            this.RotateResetBtn.Name = "RotateResetBtn";
            this.RotateResetBtn.Size = new System.Drawing.Size(34, 35);
            this.RotateResetBtn.TabIndex = 20;
            this.RotateResetBtn.UseVisualStyleBackColor = true;
            this.RotateResetBtn.Click += new System.EventHandler(this.RotateResetBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 16);
            this.label7.TabIndex = 19;
            this.label7.Text = "Вращение";
            // 
            // RotateRight
            // 
            this.RotateRight.Image = ((System.Drawing.Image)(resources.GetObject("RotateRight.Image")));
            this.RotateRight.Location = new System.Drawing.Point(223, 124);
            this.RotateRight.Name = "RotateRight";
            this.RotateRight.Size = new System.Drawing.Size(34, 35);
            this.RotateRight.TabIndex = 18;
            this.RotateRight.UseVisualStyleBackColor = true;
            this.RotateRight.Click += new System.EventHandler(this.RotateRight_Click);
            // 
            // RotateLeft
            // 
            this.RotateLeft.Image = ((System.Drawing.Image)(resources.GetObject("RotateLeft.Image")));
            this.RotateLeft.Location = new System.Drawing.Point(143, 123);
            this.RotateLeft.Name = "RotateLeft";
            this.RotateLeft.Size = new System.Drawing.Size(34, 35);
            this.RotateLeft.TabIndex = 17;
            this.RotateLeft.UseVisualStyleBackColor = true;
            this.RotateLeft.Click += new System.EventHandler(this.RotateLeft_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "Тип линии";
            // 
            // comboBoxLine
            // 
            this.comboBoxLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLine.FormattingEnabled = true;
            this.comboBoxLine.Items.AddRange(new object[] {
            "Сплошной",
            "Точечный",
            "Штриховой",
            "Штрихпунктирный"});
            this.comboBoxLine.Location = new System.Drawing.Point(143, 93);
            this.comboBoxLine.Name = "comboBoxLine";
            this.comboBoxLine.Size = new System.Drawing.Size(121, 24);
            this.comboBoxLine.TabIndex = 15;
            this.comboBoxLine.SelectedIndexChanged += new System.EventHandler(this.comboBoxLine_SelectedIndexChanged);
            // 
            // ChangePrimitive
            // 
            this.ChangePrimitive.Enabled = false;
            this.ChangePrimitive.Location = new System.Drawing.Point(143, 27);
            this.ChangePrimitive.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ChangePrimitive.Name = "ChangePrimitive";
            this.ChangePrimitive.ReadOnly = true;
            this.ChangePrimitive.Size = new System.Drawing.Size(84, 22);
            this.ChangePrimitive.TabIndex = 14;
            this.ChangePrimitive.ValueChanged += new System.EventHandler(this.ChangePrimitive_ValueChanged);
            // 
            // ChangeColorP
            // 
            this.ChangeColorP.Image = ((System.Drawing.Image)(resources.GetObject("ChangeColorP.Image")));
            this.ChangeColorP.Location = new System.Drawing.Point(250, 59);
            this.ChangeColorP.Name = "ChangeColorP";
            this.ChangeColorP.Size = new System.Drawing.Size(65, 28);
            this.ChangeColorP.TabIndex = 13;
            this.ChangeColorP.UseVisualStyleBackColor = true;
            this.ChangeColorP.Click += new System.EventHandler(this.ChangeColorP_Click);
            // 
            // ChangeWidthP
            // 
            this.ChangeWidthP.Location = new System.Drawing.Point(143, 62);
            this.ChangeWidthP.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ChangeWidthP.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ChangeWidthP.Name = "ChangeWidthP";
            this.ChangeWidthP.ReadOnly = true;
            this.ChangeWidthP.Size = new System.Drawing.Size(84, 22);
            this.ChangeWidthP.TabIndex = 12;
            this.ChangeWidthP.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ChangeWidthP.ValueChanged += new System.EventHandler(this.ChangeWidthP_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Ширина линии";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.reg);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.regLabel);
            this.groupBox1.Location = new System.Drawing.Point(1280, 513);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(343, 88);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Режимы";
            // 
            // reg
            // 
            this.reg.AutoSize = true;
            this.reg.Location = new System.Drawing.Point(57, 29);
            this.reg.Name = "reg";
            this.reg.Size = new System.Drawing.Size(72, 16);
            this.reg.TabIndex = 4;
            this.reg.Text = "Просмотр";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.checkBox1.Location = new System.Drawing.Point(9, 62);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(130, 20);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Редактировать";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // regLabel
            // 
            this.regLabel.AutoSize = true;
            this.regLabel.Location = new System.Drawing.Point(6, 29);
            this.regLabel.Name = "regLabel";
            this.regLabel.Size = new System.Drawing.Size(50, 16);
            this.regLabel.TabIndex = 0;
            this.regLabel.Text = "Режим";
            // 
            // infoBox
            // 
            this.infoBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoBox.Controls.Add(this.richTextBox1);
            this.infoBox.Location = new System.Drawing.Point(1280, 617);
            this.infoBox.Name = "infoBox";
            this.infoBox.Size = new System.Drawing.Size(343, 199);
            this.infoBox.TabIndex = 13;
            this.infoBox.TabStop = false;
            this.infoBox.Text = "Информация";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(6, 22);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(331, 171);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(183, 170);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(74, 22);
            this.numericUpDown1.TabIndex = 23;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1635, 872);
            this.Controls.Add(this.infoBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Primitives);
            this.Controls.Add(this.Sets);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.GL);
            this.Name = "MainForm";
            this.Text = "Отрисовка примитивов";
            ((System.ComponentModel.ISupportInitialize)(this.GL)).EndInit();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangeSet)).EndInit();
            this.Sets.ResumeLayout(false);
            this.Sets.PerformLayout();
            this.SetMove.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ChangeWidthS)).EndInit();
            this.Primitives.ResumeLayout(false);
            this.Primitives.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangePrimitive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChangeWidthP)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.infoBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ComboBox comboBoxLine;
        private System.Windows.Forms.Label label5;

        #endregion

        private SharpGL.OpenGLControl GL;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusXPosName;
        private System.Windows.Forms.ToolStripStatusLabel statusXPosValue;
        private System.Windows.Forms.ToolStripStatusLabel statusYPosName;
        private System.Windows.Forms.ToolStripStatusLabel statusYPosValue;
        private System.Windows.Forms.NumericUpDown ChangeSet;
        private System.Windows.Forms.GroupBox Sets;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DeleteSet;
        private System.Windows.Forms.Button AddSet;
        private System.Windows.Forms.Button DeletePrimitive;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox Primitives;
        private System.Windows.Forms.Button ChangeColorP;
        private System.Windows.Forms.NumericUpDown ChangeWidthP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button ChangeColorS;
        private System.Windows.Forms.NumericUpDown ChangeWidthS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripStatusLabel statusXShiftName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel statusXShiftValue;
        private System.Windows.Forms.ToolStripStatusLabel statusYShiftName;
        private System.Windows.Forms.ToolStripStatusLabel statusYShiftValue;
        private System.Windows.Forms.NumericUpDown ChangePrimitive;
        private System.Windows.Forms.ComboBox comboBoxSet;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox SetMove;
        private System.Windows.Forms.Button ResetBtn;
        private System.Windows.Forms.Button RightBtn;
        private System.Windows.Forms.Button DownBtn;
        private System.Windows.Forms.Button LeftBtn;
        private System.Windows.Forms.Button UpBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label regLabel;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox infoBox;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label reg;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button RotateRight;
        private System.Windows.Forms.Button RotateLeft;
        private System.Windows.Forms.Button RotateResetBtn;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel PrimitiveScale;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel PrimitiveAngle;
        private System.Windows.Forms.Button ScaleResetBtn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}

