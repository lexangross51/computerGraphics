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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.GL = new SharpGL.OpenGLControl();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusXPosName = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusXPosValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusYPosName = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusYPosValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.ChangeSet = new System.Windows.Forms.NumericUpDown();
            this.Sets = new System.Windows.Forms.GroupBox();
            this.SetColorS = new System.Windows.Forms.Button();
            this.ChangeWidthS = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.AddSet = new System.Windows.Forms.Button();
            this.DeleteSet = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.DeletePrimitive = new System.Windows.Forms.Button();
            this.ChangePrimitive = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.Primitives = new System.Windows.Forms.GroupBox();
            this.SetColorP = new System.Windows.Forms.Button();
            this.ChangeWidthP = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GL)).BeginInit();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangeSet)).BeginInit();
            this.Sets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangeWidthS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChangePrimitive)).BeginInit();
            this.Primitives.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangeWidthP)).BeginInit();
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
            this.GL.Size = new System.Drawing.Size(933, 644);
            this.GL.TabIndex = 0;
            this.GL.OpenGLInitialized += new System.EventHandler(this.GL_OpenGLInitialized);
            this.GL.OpenGLDraw += new SharpGL.RenderEventHandler(this.GL_OpenGLDraw);
            this.GL.Resized += new System.EventHandler(this.GL_Resized);
            this.GL.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GL_MouseClick);
            this.GL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GL_MouseMove);
            // 
            // statusBar
            // 
            this.statusBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusXPosName,
            this.statusXPosValue,
            this.statusYPosName,
            this.statusYPosValue});
            this.statusBar.Location = new System.Drawing.Point(0, 650);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1309, 30);
            this.statusBar.TabIndex = 1;
            this.statusBar.Text = "statusStrip1";
            // 
            // statusXPosName
            // 
            this.statusXPosName.Name = "statusXPosName";
            this.statusXPosName.Size = new System.Drawing.Size(21, 24);
            this.statusXPosName.Text = "X:";
            // 
            // statusXPosValue
            // 
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
            this.statusYPosValue.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.statusYPosValue.Name = "statusYPosValue";
            this.statusYPosValue.Size = new System.Drawing.Size(45, 24);
            this.statusYPosValue.Text = "0000";
            // 
            // ChangeSet
            // 
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
            this.Sets.Controls.Add(this.SetColorS);
            this.Sets.Controls.Add(this.ChangeWidthS);
            this.Sets.Controls.Add(this.label3);
            this.Sets.Controls.Add(this.AddSet);
            this.Sets.Controls.Add(this.DeleteSet);
            this.Sets.Controls.Add(this.label1);
            this.Sets.Controls.Add(this.ChangeSet);
            this.Sets.Location = new System.Drawing.Point(952, 2);
            this.Sets.Name = "Sets";
            this.Sets.Size = new System.Drawing.Size(343, 124);
            this.Sets.TabIndex = 3;
            this.Sets.TabStop = false;
            this.Sets.Text = "Управление наборами";
            // 
            // SetColorS
            // 
            this.SetColorS.Location = new System.Drawing.Point(209, 64);
            this.SetColorS.Name = "SetColorS";
            this.SetColorS.Size = new System.Drawing.Size(104, 46);
            this.SetColorS.TabIndex = 11;
            this.SetColorS.Text = "Выбрать цвет";
            this.SetColorS.UseVisualStyleBackColor = true;
            // 
            // ChangeWidthS
            // 
            this.ChangeWidthS.Location = new System.Drawing.Point(119, 62);
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
            this.ChangeWidthS.TabIndex = 10;
            this.ChangeWidthS.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Ширина линий";
            // 
            // AddSet
            // 
            this.AddSet.Location = new System.Drawing.Point(264, 25);
            this.AddSet.Name = "AddSet";
            this.AddSet.Size = new System.Drawing.Size(49, 23);
            this.AddSet.TabIndex = 8;
            this.AddSet.Text = "Добавить";
            this.AddSet.UseVisualStyleBackColor = true;
            this.AddSet.Click += new System.EventHandler(this.AddSet_Click);
            // 
            // DeleteSet
            // 
            this.DeleteSet.Location = new System.Drawing.Point(209, 25);
            this.DeleteSet.Name = "DeleteSet";
            this.DeleteSet.Size = new System.Drawing.Size(49, 23);
            this.DeleteSet.TabIndex = 6;
            this.DeleteSet.Text = "Удалить";
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
            this.DeletePrimitive.Location = new System.Drawing.Point(243, 26);
            this.DeletePrimitive.Name = "DeletePrimitive";
            this.DeletePrimitive.Size = new System.Drawing.Size(75, 23);
            this.DeletePrimitive.TabIndex = 10;
            this.DeletePrimitive.Text = "Удалить";
            this.DeletePrimitive.UseVisualStyleBackColor = true;
            // 
            // ChangePrimitive
            // 
            this.ChangePrimitive.Location = new System.Drawing.Point(143, 26);
            this.ChangePrimitive.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ChangePrimitive.Name = "ChangePrimitive";
            this.ChangePrimitive.ReadOnly = true;
            this.ChangePrimitive.Size = new System.Drawing.Size(84, 22);
            this.ChangePrimitive.TabIndex = 9;
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
            this.Primitives.Controls.Add(this.SetColorP);
            this.Primitives.Controls.Add(this.ChangeWidthP);
            this.Primitives.Controls.Add(this.label4);
            this.Primitives.Controls.Add(this.label2);
            this.Primitives.Controls.Add(this.DeletePrimitive);
            this.Primitives.Controls.Add(this.ChangePrimitive);
            this.Primitives.Location = new System.Drawing.Point(952, 151);
            this.Primitives.Name = "Primitives";
            this.Primitives.Size = new System.Drawing.Size(343, 123);
            this.Primitives.TabIndex = 11;
            this.Primitives.TabStop = false;
            this.Primitives.Text = "Управление примитивами";
            // 
            // SetColorP
            // 
            this.SetColorP.Location = new System.Drawing.Point(243, 62);
            this.SetColorP.Name = "SetColorP";
            this.SetColorP.Size = new System.Drawing.Size(75, 46);
            this.SetColorP.TabIndex = 13;
            this.SetColorP.Text = "Выбрать цвет";
            this.SetColorP.UseVisualStyleBackColor = true;
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1309, 680);
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
            ((System.ComponentModel.ISupportInitialize)(this.ChangeWidthS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChangePrimitive)).EndInit();
            this.Primitives.ResumeLayout(false);
            this.Primitives.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangeWidthP)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

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
        private System.Windows.Forms.NumericUpDown ChangePrimitive;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox Primitives;
        private System.Windows.Forms.Button SetColorS;
        private System.Windows.Forms.NumericUpDown ChangeWidthS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button SetColorP;
        private System.Windows.Forms.NumericUpDown ChangeWidthP;
        private System.Windows.Forms.Label label4;
    }
}

