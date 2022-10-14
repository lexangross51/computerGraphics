namespace cg_2
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GL = new SharpGL.OpenGLControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Scene = new System.Windows.Forms.GroupBox();
            this.ProjectionModeLabel = new System.Windows.Forms.Label();
            this.ProjectionModeList = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.GL)).BeginInit();
            this.Scene.SuspendLayout();
            this.SuspendLayout();
            // 
            // GL
            // 
            this.GL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GL.DrawFPS = false;
            this.GL.Location = new System.Drawing.Point(0, 7);
            this.GL.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GL.Name = "GL";
            this.GL.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.GL.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.GL.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.GL.Size = new System.Drawing.Size(1250, 724);
            this.GL.TabIndex = 0;
            this.GL.OpenGLInitialized += new System.EventHandler(this.GL_OpenGLInitialized);
            this.GL.OpenGLDraw += new SharpGL.RenderEventHandler(this.GL_OpenGLDraw);
            this.GL.Resized += new System.EventHandler(this.GL_Resized);
            this.GL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GL_KeyPress);
            this.GL.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GL_MouseClick);
            this.GL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GL_MouseDown);
            this.GL.MouseEnter += new System.EventHandler(this.GL_MouseEnter);
            this.GL.MouseLeave += new System.EventHandler(this.GL_MouseLeave);
            this.GL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GL_MouseMove);
            this.GL.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GL_MouseUp);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 736);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1521, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Scene
            // 
            this.Scene.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Scene.Controls.Add(this.ProjectionModeLabel);
            this.Scene.Controls.Add(this.ProjectionModeList);
            this.Scene.Location = new System.Drawing.Point(1257, 7);
            this.Scene.Name = "Scene";
            this.Scene.Size = new System.Drawing.Size(252, 132);
            this.Scene.TabIndex = 2;
            this.Scene.TabStop = false;
            this.Scene.Text = "Сцена";
            // 
            // ProjectionModeLabel
            // 
            this.ProjectionModeLabel.AutoSize = true;
            this.ProjectionModeLabel.Location = new System.Drawing.Point(6, 23);
            this.ProjectionModeLabel.Name = "ProjectionModeLabel";
            this.ProjectionModeLabel.Size = new System.Drawing.Size(79, 20);
            this.ProjectionModeLabel.TabIndex = 4;
            this.ProjectionModeLabel.Text = "Проекция";
            // 
            // ProjectionModeList
            // 
            this.ProjectionModeList.FormattingEnabled = true;
            this.ProjectionModeList.Items.AddRange(new object[] {
            "Перспективная",
            "Ортографическая"});
            this.ProjectionModeList.Location = new System.Drawing.Point(91, 20);
            this.ProjectionModeList.Name = "ProjectionModeList";
            this.ProjectionModeList.Size = new System.Drawing.Size(151, 28);
            this.ProjectionModeList.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1521, 758);
            this.Controls.Add(this.Scene);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.GL);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.GL)).EndInit();
            this.Scene.ResumeLayout(false);
            this.Scene.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpGL.OpenGLControl GL;
        private StatusStrip statusStrip1;
        private GroupBox Scene;
        private Label ProjectionModeLabel;
        private ComboBox ProjectionModeList;
    }
}