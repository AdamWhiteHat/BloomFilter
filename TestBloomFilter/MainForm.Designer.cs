namespace TestBloomFilter
{
	partial class MainForm
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
			this.tbElements = new System.Windows.Forms.TextBox();
			this.labelElements = new System.Windows.Forms.Label();
			this.btnCreateFilter = new System.Windows.Forms.Button();
			this.btnAddHashes = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.btnTestHashing = new System.Windows.Forms.Button();
			this.labelTests = new System.Windows.Forms.Label();
			this.tbTestInput = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.tbHashInput = new System.Windows.Forms.TextBox();
			this.tbOutput = new System.Windows.Forms.TextBox();
			this.btnTest1 = new System.Windows.Forms.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.tbHashes = new System.Windows.Forms.TextBox();
			this.cbForceToken = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.btnTestAbc = new System.Windows.Forms.Button();
			this.btnOpenFilter = new System.Windows.Forms.Button();
			this.btnSaveFilter = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbElements
			// 
			this.tbElements.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbElements.Location = new System.Drawing.Point(396, 7);
			this.tbElements.Name = "tbElements";
			this.tbElements.Size = new System.Drawing.Size(109, 20);
			this.tbElements.TabIndex = 0;
			this.tbElements.Text = "100,000";
			this.tbElements.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// labelElements
			// 
			this.labelElements.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelElements.AutoSize = true;
			this.labelElements.Location = new System.Drawing.Point(293, 10);
			this.labelElements.Name = "labelElements";
			this.labelElements.Size = new System.Drawing.Size(97, 13);
			this.labelElements.TabIndex = 1;
			this.labelElements.Text = "Max # of elements:";
			this.labelElements.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// btnCreateFilter
			// 
			this.btnCreateFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCreateFilter.Location = new System.Drawing.Point(511, 5);
			this.btnCreateFilter.Name = "btnCreateFilter";
			this.btnCreateFilter.Size = new System.Drawing.Size(100, 22);
			this.btnCreateFilter.TabIndex = 2;
			this.btnCreateFilter.Text = "Create Filter";
			this.btnCreateFilter.UseVisualStyleBackColor = true;
			this.btnCreateFilter.Click += new System.EventHandler(this.btnCreateFilter_Click);
			// 
			// btnAddHashes
			// 
			this.btnAddHashes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddHashes.Location = new System.Drawing.Point(511, 82);
			this.btnAddHashes.Name = "btnAddHashes";
			this.btnAddHashes.Size = new System.Drawing.Size(100, 23);
			this.btnAddHashes.TabIndex = 3;
			this.btnAddHashes.Text = "Add Hashes";
			this.btnAddHashes.UseVisualStyleBackColor = true;
			this.btnAddHashes.Click += new System.EventHandler(this.btnAddHashes_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(0, 13);
			this.label1.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 26);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(0, 13);
			this.label2.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(7, 63);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(0, 13);
			this.label4.TabIndex = 9;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(7, 45);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(0, 13);
			this.label3.TabIndex = 8;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(7, 82);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(0, 13);
			this.label5.TabIndex = 10;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(7, 101);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(0, 13);
			this.label6.TabIndex = 11;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(7, 120);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(0, 13);
			this.label7.TabIndex = 13;
			// 
			// btnTestHashing
			// 
			this.btnTestHashing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTestHashing.Location = new System.Drawing.Point(511, 131);
			this.btnTestHashing.Name = "btnTestHashing";
			this.btnTestHashing.Size = new System.Drawing.Size(100, 23);
			this.btnTestHashing.TabIndex = 15;
			this.btnTestHashing.Text = "Test Hashing";
			this.btnTestHashing.UseVisualStyleBackColor = true;
			this.btnTestHashing.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// labelTests
			// 
			this.labelTests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTests.Location = new System.Drawing.Point(319, 156);
			this.labelTests.Name = "labelTests";
			this.labelTests.Size = new System.Drawing.Size(186, 15);
			this.labelTests.TabIndex = 19;
			this.labelTests.Text = "( Text to text OR # of random Tests )";
			this.labelTests.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tbTestInput
			// 
			this.tbTestInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbTestInput.Location = new System.Drawing.Point(322, 133);
			this.tbTestInput.Name = "tbTestInput";
			this.tbTestInput.Size = new System.Drawing.Size(183, 20);
			this.tbTestInput.TabIndex = 18;
			this.tbTestInput.Text = "100";
			this.tbTestInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label10
			// 
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label10.Location = new System.Drawing.Point(322, 107);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(183, 15);
			this.label10.TabIndex = 21;
			this.label10.Text = "( Text to hash OR a filename )";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tbHashInput
			// 
			this.tbHashInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbHashInput.Location = new System.Drawing.Point(322, 84);
			this.tbHashInput.Name = "tbHashInput";
			this.tbHashInput.Size = new System.Drawing.Size(183, 20);
			this.tbHashInput.TabIndex = 20;
			this.tbHashInput.Text = "primes_68906.txt";
			this.tbHashInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbOutput
			// 
			this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbOutput.BackColor = System.Drawing.SystemColors.Control;
			this.tbOutput.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbOutput.Location = new System.Drawing.Point(10, 214);
			this.tbOutput.Multiline = true;
			this.tbOutput.Name = "tbOutput";
			this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbOutput.Size = new System.Drawing.Size(601, 214);
			this.tbOutput.TabIndex = 22;
			// 
			// btnTest1
			// 
			this.btnTest1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTest1.Location = new System.Drawing.Point(374, 185);
			this.btnTest1.Name = "btnTest1";
			this.btnTest1.Size = new System.Drawing.Size(75, 23);
			this.btnTest1.TabIndex = 23;
			this.btnTest1.Text = "Refresh Ctrls";
			this.btnTest1.UseVisualStyleBackColor = true;
			this.btnTest1.Click += new System.EventHandler(this.btnTest_Click_1);
			// 
			// label11
			// 
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(286, 32);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(104, 13);
			this.label11.TabIndex = 25;
			this.label11.Text = "Hashes per element:";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tbHashes
			// 
			this.tbHashes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbHashes.Location = new System.Drawing.Point(396, 29);
			this.tbHashes.Name = "tbHashes";
			this.tbHashes.Size = new System.Drawing.Size(55, 20);
			this.tbHashes.TabIndex = 24;
			this.tbHashes.Text = "8";
			this.tbHashes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// cbForceToken
			// 
			this.cbForceToken.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbForceToken.AutoSize = true;
			this.cbForceToken.Location = new System.Drawing.Point(528, 154);
			this.cbForceToken.Name = "cbForceToken";
			this.cbForceToken.Size = new System.Drawing.Size(83, 17);
			this.cbForceToken.TabIndex = 26;
			this.cbForceToken.Text = "Force token";
			this.cbForceToken.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(7, 139);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(0, 13);
			this.label8.TabIndex = 16;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(7, 158);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(0, 13);
			this.label9.TabIndex = 17;
			// 
			// btnTestAbc
			// 
			this.btnTestAbc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTestAbc.Location = new System.Drawing.Point(455, 185);
			this.btnTestAbc.Name = "btnTestAbc";
			this.btnTestAbc.Size = new System.Drawing.Size(75, 23);
			this.btnTestAbc.TabIndex = 27;
			this.btnTestAbc.Text = "Test ABC";
			this.btnTestAbc.UseVisualStyleBackColor = true;
			this.btnTestAbc.Click += new System.EventHandler(this.btnTestAbc_Click);
			// 
			// btnOpenFilter
			// 
			this.btnOpenFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpenFilter.Location = new System.Drawing.Point(511, 27);
			this.btnOpenFilter.Name = "btnOpenFilter";
			this.btnOpenFilter.Size = new System.Drawing.Size(100, 22);
			this.btnOpenFilter.TabIndex = 29;
			this.btnOpenFilter.Text = "   Open Filter...";
			this.btnOpenFilter.UseVisualStyleBackColor = true;
			this.btnOpenFilter.Click += new System.EventHandler(this.btnOpenFilter_Click);
			// 
			// btnSaveFilter
			// 
			this.btnSaveFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveFilter.Location = new System.Drawing.Point(511, 49);
			this.btnSaveFilter.Name = "btnSaveFilter";
			this.btnSaveFilter.Size = new System.Drawing.Size(100, 22);
			this.btnSaveFilter.TabIndex = 30;
			this.btnSaveFilter.Text = "   Save Filter...";
			this.btnSaveFilter.UseVisualStyleBackColor = true;
			this.btnSaveFilter.Click += new System.EventHandler(this.btnSaveFilter_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(623, 440);
			this.Controls.Add(this.btnSaveFilter);
			this.Controls.Add(this.btnOpenFilter);
			this.Controls.Add(this.btnTestAbc);
			this.Controls.Add(this.cbForceToken);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.tbHashes);
			this.Controls.Add(this.btnTest1);
			this.Controls.Add(this.tbOutput);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.tbHashInput);
			this.Controls.Add(this.labelTests);
			this.Controls.Add(this.tbTestInput);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.btnTestHashing);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnAddHashes);
			this.Controls.Add(this.btnCreateFilter);
			this.Controls.Add(this.labelElements);
			this.Controls.Add(this.tbElements);
			this.Name = "MainForm";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbElements;
		private System.Windows.Forms.Label labelElements;
		private System.Windows.Forms.Button btnCreateFilter;
		private System.Windows.Forms.Button btnAddHashes;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button btnTestHashing;
		private System.Windows.Forms.Label labelTests;
		private System.Windows.Forms.TextBox tbTestInput;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox tbHashInput;
		private System.Windows.Forms.TextBox tbOutput;
		private System.Windows.Forms.Button btnTest1;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox tbHashes;
		private System.Windows.Forms.CheckBox cbForceToken;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button btnTestAbc;
		private System.Windows.Forms.Button btnOpenFilter;
		private System.Windows.Forms.Button btnSaveFilter;
	}
}

