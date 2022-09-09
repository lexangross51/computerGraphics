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
            ((System.ComponentModel.ISupportInitialize)(this.GL)).BeginInit();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // GL
            // 
            this.GL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GL.DrawFPS = false;
            this.GL.Location = new System.Drawing.Point(13, 2);
            this.GL.Margin = new System.Windows.Forms.Padding(4);
            this.GL.Name = "GL";
            this.GL.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.GL.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.GL.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.GL.Size = new System.Drawing.Size(920, 644);
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
            this.statusXPosName.Size = new System.Drawing.Size(21, 20);
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
            this.statusYPosName.Size = new System.Drawing.Size(20, 20);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1309, 680);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.GL);
            this.Name = "MainForm";
            this.Text = "Отрисовка примитивов";
            ((System.ComponentModel.ISupportInitialize)(this.GL)).EndInit();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
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
    }
}

