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
            this.ChangeSet = new System.Windows.Forms.NumericUpDown();
            this.Sets = new System.Windows.Forms.GroupBox();
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
            this.Scene = new System.Windows.Forms.GroupBox();
            this.ResetBtn = new System.Windows.Forms.Button();
            this.RightBtn = new System.Windows.Forms.Button();
            this.DownBtn = new System.Windows.Forms.Button();
            this.LeftBtn = new System.Windows.Forms.Button();
            this.UpBtn = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.GL)).BeginInit();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangeSet)).BeginInit();
            this.Sets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangePrimitive)).BeginInit();
            this.Primitives.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangeWidthP)).BeginInit();
            this.Scene.SuspendLayout();
            this.SuspendLayout();
            // 
            // GL
            // 
            this.GL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
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
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.statusXPosName, this.statusXPosValue, this.statusYPosName, this.statusYPosValue });
            this.statusBar.Location = new System.Drawing.Point(0, 651);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1309, 29);
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
            this.statusXPosValue.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
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
            this.statusYPosValue.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.statusYPosValue.Name = "statusYPosValue";
            this.statusYPosValue.Size = new System.Drawing.Size(45, 24);
            this.statusYPosValue.Text = "0000";
            // 
            // ChangeSet
            // 
            this.ChangeSet.Location = new System.Drawing.Point(119, 26);
            this.ChangeSet.Maximum = new decimal(new int[] { 0, 0, 0, 0 });
            this.ChangeSet.Name = "ChangeSet";
            this.ChangeSet.ReadOnly = true;
            this.ChangeSet.Size = new System.Drawing.Size(84, 22);
            this.ChangeSet.TabIndex = 2;
            this.ChangeSet.ValueChanged += new System.EventHandler(this.ChangeSet_ValueChanged);
            // 
            // Sets
            // 
            this.Sets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sets.Controls.Add(this.AddSet);
            this.Sets.Controls.Add(this.DeleteSet);
            this.Sets.Controls.Add(this.label1);
            this.Sets.Controls.Add(this.ChangeSet);
            this.Sets.Location = new System.Drawing.Point(954, 213);
            this.Sets.Name = "Sets";
            this.Sets.Size = new System.Drawing.Size(343, 71);
            this.Sets.TabIndex = 3;
            this.Sets.TabStop = false;
            this.Sets.Text = "Управление наборами";
            // 
            // AddSet
            // 
            this.AddSet.Image = ((System.Drawing.Image)(resources.GetObject("AddSet.Image")));
            this.AddSet.Location = new System.Drawing.Point(283, 26);
            this.AddSet.Name = "AddSet";
            this.AddSet.Size = new System.Drawing.Size(57, 27);
            this.AddSet.TabIndex = 8;
            this.AddSet.UseVisualStyleBackColor = true;
            this.AddSet.Click += new System.EventHandler(this.AddSet_Click);
            // 
            // DeleteSet
            // 
            this.DeleteSet.Image = ((System.Drawing.Image)(resources.GetObject("DeleteSet.Image")));
            this.DeleteSet.Location = new System.Drawing.Point(220, 26);
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
            this.label1.Size = new System.Drawing.Size(110, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Текущий набор";
            // 
            // DeletePrimitive
            // 
            this.DeletePrimitive.Image = ((System.Drawing.Image)(resources.GetObject("DeletePrimitive.Image")));
            this.DeletePrimitive.Location = new System.Drawing.Point(250, 26);
            this.DeletePrimitive.Name = "DeletePrimitive";
            this.DeletePrimitive.Size = new System.Drawing.Size(65, 28);
            this.DeletePrimitive.TabIndex = 10;
            this.DeletePrimitive.UseVisualStyleBackColor = true;
            // 
            // ChangePrimitive
            // 
            this.ChangePrimitive.Location = new System.Drawing.Point(143, 26);
            this.ChangePrimitive.Maximum = new decimal(new int[] { 0, 0, 0, 0 });
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
            this.label2.Size = new System.Drawing.Size(133, 17);
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
            this.Primitives.Location = new System.Drawing.Point(954, 309);
            this.Primitives.Name = "Primitives";
            this.Primitives.Size = new System.Drawing.Size(343, 123);
            this.Primitives.TabIndex = 11;
            this.Primitives.TabStop = false;
            this.Primitives.Text = "Управление примитивами";
            // 
            // SetColorP
            // 
            this.SetColorP.Image = ((System.Drawing.Image)(resources.GetObject("SetColorP.Image")));
            this.SetColorP.Location = new System.Drawing.Point(250, 62);
            this.SetColorP.Name = "SetColorP";
            this.SetColorP.Size = new System.Drawing.Size(65, 28);
            this.SetColorP.TabIndex = 13;
            this.SetColorP.UseVisualStyleBackColor = true;
            this.SetColorP.Click += new System.EventHandler(this.SetColorP_Click);
            // 
            // ChangeWidthP
            // 
            this.ChangeWidthP.Location = new System.Drawing.Point(143, 62);
            this.ChangeWidthP.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            this.ChangeWidthP.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.ChangeWidthP.Name = "ChangeWidthP";
            this.ChangeWidthP.ReadOnly = true;
            this.ChangeWidthP.Size = new System.Drawing.Size(84, 22);
            this.ChangeWidthP.TabIndex = 12;
            this.ChangeWidthP.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Ширина линии";
            // 
            // Scene
            // 
            this.Scene.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Scene.Controls.Add(this.ResetBtn);
            this.Scene.Controls.Add(this.RightBtn);
            this.Scene.Controls.Add(this.DownBtn);
            this.Scene.Controls.Add(this.LeftBtn);
            this.Scene.Controls.Add(this.UpBtn);
            this.Scene.Location = new System.Drawing.Point(954, 12);
            this.Scene.Name = "Scene";
            this.Scene.Size = new System.Drawing.Size(343, 180);
            this.Scene.TabIndex = 12;
            this.Scene.TabStop = false;
            this.Scene.Text = "Сцена";
            // 
            // ResetBtn
            // 
            this.ResetBtn.Image = ((System.Drawing.Image)(resources.GetObject("ResetBtn.Image")));
            this.ResetBtn.Location = new System.Drawing.Point(159, 69);
            this.ResetBtn.Name = "ResetBtn";
            this.ResetBtn.Size = new System.Drawing.Size(34, 35);
            this.ResetBtn.TabIndex = 4;
            this.ResetBtn.UseVisualStyleBackColor = true;
            this.ResetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
            // 
            // RightBtn
            // 
            this.RightBtn.Image = ((System.Drawing.Image)(resources.GetObject("RightBtn.Image")));
            this.RightBtn.Location = new System.Drawing.Point(199, 69);
            this.RightBtn.Name = "RightBtn";
            this.RightBtn.Size = new System.Drawing.Size(34, 35);
            this.RightBtn.TabIndex = 3;
            this.RightBtn.UseVisualStyleBackColor = true;
            this.RightBtn.Click += new System.EventHandler(this.RightBtn_Click);
            // 
            // DownBtn
            // 
            this.DownBtn.Image = ((System.Drawing.Image)(resources.GetObject("DownBtn.Image")));
            this.DownBtn.Location = new System.Drawing.Point(159, 110);
            this.DownBtn.Name = "DownBtn";
            this.DownBtn.Size = new System.Drawing.Size(34, 35);
            this.DownBtn.TabIndex = 2;
            this.DownBtn.UseVisualStyleBackColor = true;
            this.DownBtn.Click += new System.EventHandler(this.DownBtn_Click);
            // 
            // LeftBtn
            // 
            this.LeftBtn.Image = ((System.Drawing.Image)(resources.GetObject("LeftBtn.Image")));
            this.LeftBtn.Location = new System.Drawing.Point(119, 69);
            this.LeftBtn.Name = "LeftBtn";
            this.LeftBtn.Size = new System.Drawing.Size(34, 35);
            this.LeftBtn.TabIndex = 1;
            this.LeftBtn.UseVisualStyleBackColor = true;
            this.LeftBtn.Click += new System.EventHandler(this.LeftBtn_Click);
            // 
            // UpBtn
            // 
            this.UpBtn.Image = ((System.Drawing.Image)(resources.GetObject("UpBtn.Image")));
            this.UpBtn.Location = new System.Drawing.Point(159, 28);
            this.UpBtn.Name = "UpBtn";
            this.UpBtn.Size = new System.Drawing.Size(34, 35);
            this.UpBtn.TabIndex = 0;
            this.UpBtn.UseVisualStyleBackColor = true;
            this.UpBtn.Click += new System.EventHandler(this.UpBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1309, 680);
            this.Controls.Add(this.Scene);
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
            ((System.ComponentModel.ISupportInitialize)(this.ChangePrimitive)).EndInit();
            this.Primitives.ResumeLayout(false);
            this.Primitives.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChangeWidthP)).EndInit();
            this.Scene.ResumeLayout(false);
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
        private System.Windows.Forms.Button SetColorP;
        private System.Windows.Forms.NumericUpDown ChangeWidthP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox Scene;
        private System.Windows.Forms.Button ResetBtn;
        private System.Windows.Forms.Button RightBtn;
        private System.Windows.Forms.Button DownBtn;
        private System.Windows.Forms.Button LeftBtn;
        private System.Windows.Forms.Button UpBtn;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}

