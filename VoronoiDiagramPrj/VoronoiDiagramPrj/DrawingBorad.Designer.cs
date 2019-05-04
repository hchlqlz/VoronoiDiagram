namespace VoronoiDiagram
{
	partial class MainForm
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.ControlPanel = new System.Windows.Forms.Panel();
			this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.DrawingBroad = new System.Windows.Forms.PictureBox();
			this.ControlPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.DrawingBroad)).BeginInit();
			this.SuspendLayout();
			// 
			// ControlPanel
			// 
			this.ControlPanel.Controls.Add(this.numericUpDown5);
			this.ControlPanel.Controls.Add(this.label5);
			this.ControlPanel.Controls.Add(this.label4);
			this.ControlPanel.Controls.Add(this.numericUpDown4);
			this.ControlPanel.Controls.Add(this.label3);
			this.ControlPanel.Controls.Add(this.numericUpDown3);
			this.ControlPanel.Controls.Add(this.label2);
			this.ControlPanel.Controls.Add(this.numericUpDown2);
			this.ControlPanel.Controls.Add(this.numericUpDown1);
			this.ControlPanel.Controls.Add(this.label1);
			this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.ControlPanel.Location = new System.Drawing.Point(0, 0);
			this.ControlPanel.Name = "ControlPanel";
			this.ControlPanel.Size = new System.Drawing.Size(200, 503);
			this.ControlPanel.TabIndex = 0;
			// 
			// numericUpDown5
			// 
			this.numericUpDown5.Font = new System.Drawing.Font("幼圆", 11F);
			this.numericUpDown5.Location = new System.Drawing.Point(73, 132);
			this.numericUpDown5.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.numericUpDown5.Name = "numericUpDown5";
			this.numericUpDown5.Size = new System.Drawing.Size(124, 24);
			this.numericUpDown5.TabIndex = 10;
			this.numericUpDown5.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numericUpDown5.ValueChanged += new System.EventHandler(this.HandleInput);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("幼圆", 11F);
			this.label5.Location = new System.Drawing.Point(12, 134);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(55, 15);
			this.label5.TabIndex = 9;
			this.label5.Text = "精度：";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("幼圆", 11F);
			this.label4.Location = new System.Drawing.Point(12, 104);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(55, 15);
			this.label4.TabIndex = 8;
			this.label4.Text = "点数：";
			// 
			// numericUpDown4
			// 
			this.numericUpDown4.Font = new System.Drawing.Font("幼圆", 11F);
			this.numericUpDown4.Location = new System.Drawing.Point(73, 102);
			this.numericUpDown4.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.numericUpDown4.Name = "numericUpDown4";
			this.numericUpDown4.Size = new System.Drawing.Size(124, 24);
			this.numericUpDown4.TabIndex = 7;
			this.numericUpDown4.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.numericUpDown4.ValueChanged += new System.EventHandler(this.HandleInput);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("幼圆", 11F);
			this.label3.Location = new System.Drawing.Point(12, 74);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(39, 15);
			this.label3.TabIndex = 6;
			this.label3.Text = "宽：";
			// 
			// numericUpDown3
			// 
			this.numericUpDown3.Font = new System.Drawing.Font("幼圆", 11F);
			this.numericUpDown3.Location = new System.Drawing.Point(73, 72);
			this.numericUpDown3.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.numericUpDown3.Name = "numericUpDown3";
			this.numericUpDown3.Size = new System.Drawing.Size(124, 24);
			this.numericUpDown3.TabIndex = 5;
			this.numericUpDown3.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numericUpDown3.ValueChanged += new System.EventHandler(this.HandleInput);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("幼圆", 11F);
			this.label2.Location = new System.Drawing.Point(12, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(39, 15);
			this.label2.TabIndex = 4;
			this.label2.Text = "长：";
			// 
			// numericUpDown2
			// 
			this.numericUpDown2.Font = new System.Drawing.Font("幼圆", 11F);
			this.numericUpDown2.Location = new System.Drawing.Point(73, 42);
			this.numericUpDown2.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new System.Drawing.Size(124, 24);
			this.numericUpDown2.TabIndex = 3;
			this.numericUpDown2.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numericUpDown2.ValueChanged += new System.EventHandler(this.HandleInput);
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Font = new System.Drawing.Font("幼圆", 11F);
			this.numericUpDown1.Location = new System.Drawing.Point(73, 12);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(124, 24);
			this.numericUpDown1.TabIndex = 2;
			this.numericUpDown1.Value = new decimal(new int[] {
            123456789,
            0,
            0,
            0});
			this.numericUpDown1.ValueChanged += new System.EventHandler(this.HandleInput);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("幼圆", 11F);
			this.label1.Location = new System.Drawing.Point(12, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "种子：";
			// 
			// DrawingBroad
			// 
			this.DrawingBroad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.DrawingBroad.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DrawingBroad.Location = new System.Drawing.Point(200, 0);
			this.DrawingBroad.Name = "DrawingBroad";
			this.DrawingBroad.Size = new System.Drawing.Size(579, 503);
			this.DrawingBroad.TabIndex = 1;
			this.DrawingBroad.TabStop = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(779, 503);
			this.Controls.Add(this.DrawingBroad);
			this.Controls.Add(this.ControlPanel);
			this.Name = "MainForm";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ControlPanel.ResumeLayout(false);
			this.ControlPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DrawingBroad)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel ControlPanel;
		private System.Windows.Forms.NumericUpDown numericUpDown5;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numericUpDown4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numericUpDown3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numericUpDown2;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox DrawingBroad;
	}
}

