/*
 * Created by SharpDevelop.
 * User: user
 * Date: 12.11.2024
 * Time: 11:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace NewTaraTools
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Button Unpack;
		private System.Windows.Forms.Button Pack;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.Unpack = new System.Windows.Forms.Button();
			this.Pack = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// Unpack
			// 
			this.Unpack.Location = new System.Drawing.Point(12, 12);
			this.Unpack.Name = "Unpack";
			this.Unpack.Size = new System.Drawing.Size(146, 34);
			this.Unpack.TabIndex = 0;
			this.Unpack.Text = "Unpack";
			this.Unpack.UseVisualStyleBackColor = true;
			this.Unpack.Click += new System.EventHandler(this.UnpackClick);
			// 
			// Pack
			// 
			this.Pack.Location = new System.Drawing.Point(12, 52);
			this.Pack.Name = "Pack";
			this.Pack.Size = new System.Drawing.Size(146, 36);
			this.Pack.TabIndex = 1;
			this.Pack.Text = "Pack";
			this.Pack.UseVisualStyleBackColor = true;
			this.Pack.Click += new System.EventHandler(this.PackClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(170, 111);
			this.Controls.Add(this.Pack);
			this.Controls.Add(this.Unpack);
			this.Name = "MainForm";
			this.Text = "NewTaraTools";
			this.ResumeLayout(false);

		}
	}
}
