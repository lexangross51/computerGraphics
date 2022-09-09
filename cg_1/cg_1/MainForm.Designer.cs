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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ChangePrimitive = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DeleteSet = new System.Windows.Forms.Button();
            this.DeletePrimitive = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.GL)).BeginInit();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangeSet)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangePrimitive)).BeginInit();
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
            this.GL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GL_KeyDown);
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
            this.ChangeSet.Location = new System.Drawing.Point(143, 17);
            this.ChangeSet.Name = "ChangeSet";
            this.ChangeSet.ReadOnly = true;
            this.ChangeSet.Size = new System.Drawing.Size(84, 22);
            this.ChangeSet.TabIndex = 2;
            this.ChangeSet.ValueChanged += new System.EventHandler(this.ChangeSet_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.DeletePrimitive);
            this.groupBox1.Controls.Add(this.DeleteSet);
            this.groupBox1.Controls.Add(this.ChangePrimitive);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ChangeSet);
            this.groupBox1.Location = new System.Drawing.Point(952, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 85);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Режим набора";
            // 
            // ChangePrimitive
            // 
            this.ChangePrimitive.Location = new System.Drawing.Point(143, 49);
            this.ChangePrimitive.Name = "ChangePrimitive";
            this.ChangePrimitive.ReadOnly = true;
            this.ChangePrimitive.Size = new System.Drawing.Size(84, 22);
            this.ChangePrimitive.TabIndex = 5;
            this.ChangePrimitive.ValueChanged += new System.EventHandler(this.ChangePrimitive_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Текущий примитив";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Текущий набор";
            // 
            // DeleteSet
            // 
            this.DeleteSet.Location = new System.Drawing.Point(243, 16);
            this.DeleteSet.Name = "DeleteSet";
            this.DeleteSet.Size = new System.Drawing.Size(75, 23);
            this.DeleteSet.TabIndex = 6;
            this.DeleteSet.Text = "Удалить";
            this.DeleteSet.UseVisualStyleBackColor = true;
            this.DeleteSet.Click += new System.EventHandler(this.DeleteSet_Click);
            // 
            // DeletePrimitive
            // 
            this.DeletePrimitive.Location = new System.Drawing.Point(243, 49);
            this.DeletePrimitive.Name = "DeletePrimitive";
            this.DeletePrimitive.Size = new System.Drawing.Size(75, 23);
            this.DeletePrimitive.TabIndex = 7;
            this.DeletePrimitive.Text = "Удалить";
            this.DeletePrimitive.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1309, 680);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.GL);
            this.Name = "MainForm";
            this.Text = "Отрисовка примитивов";
            ((System.ComponentModel.ISupportInitialize)(this.GL)).EndInit();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangeSet)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangePrimitive)).EndInit();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown ChangePrimitive;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DeleteSet;
        private System.Windows.Forms.Button DeletePrimitive;
    }
}

