namespace SQLiteDemo
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tbSQLiteControllerTest = new System.Windows.Forms.TextBox();
			this.btnClickMe = new System.Windows.Forms.Button();
			this.tbDataTableExtensionsTest = new System.Windows.Forms.TextBox();
			this.btnDataTableExtensionsTest = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbSQLiteControllerTest
			// 
			this.tbSQLiteControllerTest.Location = new System.Drawing.Point(12, 41);
			this.tbSQLiteControllerTest.Multiline = true;
			this.tbSQLiteControllerTest.Name = "tbSQLiteControllerTest";
			this.tbSQLiteControllerTest.Size = new System.Drawing.Size(974, 104);
			this.tbSQLiteControllerTest.TabIndex = 3;
			// 
			// btnClickMe
			// 
			this.btnClickMe.Location = new System.Drawing.Point(12, 12);
			this.btnClickMe.Name = "btnClickMe";
			this.btnClickMe.Size = new System.Drawing.Size(974, 23);
			this.btnClickMe.TabIndex = 2;
			this.btnClickMe.Text = "SQLite Controller Test";
			this.btnClickMe.UseVisualStyleBackColor = true;
			this.btnClickMe.Click += new System.EventHandler(this.btnSQLiteControllerTest_Click);
			// 
			// tbDataTableExtensionsTest
			// 
			this.tbDataTableExtensionsTest.Location = new System.Drawing.Point(12, 180);
			this.tbDataTableExtensionsTest.Multiline = true;
			this.tbDataTableExtensionsTest.Name = "tbDataTableExtensionsTest";
			this.tbDataTableExtensionsTest.Size = new System.Drawing.Size(974, 297);
			this.tbDataTableExtensionsTest.TabIndex = 5;
			// 
			// btnDataTableExtensionsTest
			// 
			this.btnDataTableExtensionsTest.Location = new System.Drawing.Point(12, 151);
			this.btnDataTableExtensionsTest.Name = "btnDataTableExtensionsTest";
			this.btnDataTableExtensionsTest.Size = new System.Drawing.Size(974, 23);
			this.btnDataTableExtensionsTest.TabIndex = 4;
			this.btnDataTableExtensionsTest.Text = "DataTable Extensions Test";
			this.btnDataTableExtensionsTest.UseVisualStyleBackColor = true;
			this.btnDataTableExtensionsTest.Click += new System.EventHandler(this.btnDataTableExtensionsTest_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(998, 489);
			this.Controls.Add(this.tbDataTableExtensionsTest);
			this.Controls.Add(this.btnDataTableExtensionsTest);
			this.Controls.Add(this.tbSQLiteControllerTest);
			this.Controls.Add(this.btnClickMe);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbSQLiteControllerTest;
		private System.Windows.Forms.Button btnClickMe;
		private System.Windows.Forms.TextBox tbDataTableExtensionsTest;
		private System.Windows.Forms.Button btnDataTableExtensionsTest;
	}
}

