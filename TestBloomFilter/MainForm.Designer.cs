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
			this.tbMaxNumberOfElements = new System.Windows.Forms.TextBox();
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
			this.btnTestHashes = new System.Windows.Forms.Button();
			this.labelTests = new System.Windows.Forms.Label();
			this.tbHashTestQuantity = new System.Windows.Forms.TextBox();
			this.tbOutput = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.tbHashesPerElement = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.btnOpenFilter = new System.Windows.Forms.Button();
			this.btnSaveFilter = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.tbErrorProbability = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tbMaxNumberOfElements
			// 
			this.tbMaxNumberOfElements.Location = new System.Drawing.Point(409, 48);
			this.tbMaxNumberOfElements.Name = "tbMaxNumberOfElements";
			this.tbMaxNumberOfElements.Size = new System.Drawing.Size(100, 20);
			this.tbMaxNumberOfElements.TabIndex = 0;
			this.tbMaxNumberOfElements.Text = "100,000";
			this.tbMaxNumberOfElements.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// labelElements
			// 
			this.labelElements.AutoSize = true;
			this.labelElements.Location = new System.Drawing.Point(308, 51);
			this.labelElements.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.labelElements.Name = "labelElements";
			this.labelElements.Size = new System.Drawing.Size(97, 13);
			this.labelElements.TabIndex = 1;
			this.labelElements.Text = "Max # of elements:";
			this.labelElements.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnCreateFilter
			// 
			this.btnCreateFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCreateFilter.Location = new System.Drawing.Point(516, 2);
			this.btnCreateFilter.Name = "btnCreateFilter";
			this.btnCreateFilter.Size = new System.Drawing.Size(100, 22);
			this.btnCreateFilter.TabIndex = 2;
			this.btnCreateFilter.Text = "Create Filter";
			this.btnCreateFilter.UseVisualStyleBackColor = true;
			this.btnCreateFilter.Click += new System.EventHandler(this.btnCreateOrCloseFilter_Click);
			// 
			// btnAddHashes
			// 
			this.btnAddHashes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddHashes.Location = new System.Drawing.Point(516, 78);
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
			// btnTestHashes
			// 
			this.btnTestHashes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTestHashes.Location = new System.Drawing.Point(516, 101);
			this.btnTestHashes.Name = "btnTestHashes";
			this.btnTestHashes.Size = new System.Drawing.Size(100, 23);
			this.btnTestHashes.TabIndex = 15;
			this.btnTestHashes.Text = "Test Hashes";
			this.btnTestHashes.UseVisualStyleBackColor = true;
			this.btnTestHashes.Click += new System.EventHandler(this.btnTestHashes_Click);
			// 
			// labelTests
			// 
			this.labelTests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTests.AutoSize = true;
			this.labelTests.Location = new System.Drawing.Point(351, 106);
			this.labelTests.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.labelTests.Name = "labelTests";
			this.labelTests.Size = new System.Drawing.Size(54, 13);
			this.labelTests.TabIndex = 19;
			this.labelTests.Text = "# of tests:";
			this.labelTests.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbHashTestQuantity
			// 
			this.tbHashTestQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbHashTestQuantity.Location = new System.Drawing.Point(409, 103);
			this.tbHashTestQuantity.Name = "tbHashTestQuantity";
			this.tbHashTestQuantity.Size = new System.Drawing.Size(101, 20);
			this.tbHashTestQuantity.TabIndex = 18;
			this.tbHashTestQuantity.Text = "100";
			this.tbHashTestQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbOutput
			// 
			this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbOutput.BackColor = System.Drawing.SystemColors.Control;
			this.tbOutput.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbOutput.Location = new System.Drawing.Point(7, 174);
			this.tbOutput.Multiline = true;
			this.tbOutput.Name = "tbOutput";
			this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbOutput.Size = new System.Drawing.Size(609, 259);
			this.tbOutput.TabIndex = 22;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(301, 7);
			this.label11.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(104, 13);
			this.label11.TabIndex = 25;
			this.label11.Text = "Hashes per element:";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbHashesPerElement
			// 
			this.tbHashesPerElement.Location = new System.Drawing.Point(409, 4);
			this.tbHashesPerElement.Name = "tbHashesPerElement";
			this.tbHashesPerElement.Size = new System.Drawing.Size(55, 20);
			this.tbHashesPerElement.TabIndex = 24;
			this.tbHashesPerElement.Text = "0";
			this.tbHashesPerElement.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
			// btnOpenFilter
			// 
			this.btnOpenFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpenFilter.Location = new System.Drawing.Point(516, 24);
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
			this.btnSaveFilter.Enabled = false;
			this.btnSaveFilter.Location = new System.Drawing.Point(516, 46);
			this.btnSaveFilter.Name = "btnSaveFilter";
			this.btnSaveFilter.Size = new System.Drawing.Size(100, 22);
			this.btnSaveFilter.TabIndex = 30;
			this.btnSaveFilter.Text = "   Save Filter...";
			this.btnSaveFilter.UseVisualStyleBackColor = true;
			this.btnSaveFilter.Click += new System.EventHandler(this.btnSaveFilter_Click);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(297, 28);
			this.label10.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(108, 13);
			this.label10.TabIndex = 32;
			this.label10.Text = "Probabilty of collision:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbErrorProbability
			// 
			this.tbErrorProbability.Location = new System.Drawing.Point(409, 26);
			this.tbErrorProbability.Name = "tbErrorProbability";
			this.tbErrorProbability.Size = new System.Drawing.Size(55, 20);
			this.tbErrorProbability.TabIndex = 31;
			this.tbErrorProbability.Text = "0.01";
			this.tbErrorProbability.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(623, 440);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.tbErrorProbability);
			this.Controls.Add(this.btnSaveFilter);
			this.Controls.Add(this.btnOpenFilter);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.tbHashesPerElement);
			this.Controls.Add(this.tbOutput);
			this.Controls.Add(this.labelTests);
			this.Controls.Add(this.tbHashTestQuantity);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.btnTestHashes);
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
			this.Controls.Add(this.tbMaxNumberOfElements);
			this.Name = "MainForm";
			this.Text = "Bloom Filter";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbMaxNumberOfElements;
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
		private System.Windows.Forms.Button btnTestHashes;
		private System.Windows.Forms.Label labelTests;
		private System.Windows.Forms.TextBox tbHashTestQuantity;
		private System.Windows.Forms.TextBox tbOutput;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox tbHashesPerElement;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button btnOpenFilter;
		private System.Windows.Forms.Button btnSaveFilter;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox tbErrorProbability;
	}
}

