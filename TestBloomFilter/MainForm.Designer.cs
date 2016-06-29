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
			this.tbMaxElementsToHash = new System.Windows.Forms.TextBox();
			this.labelElements = new System.Windows.Forms.Label();
			this.btnCreateFilter = new System.Windows.Forms.Button();
			this.btnAddHashes = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.btnTestHashes = new System.Windows.Forms.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.tbHashesPerElement = new System.Windows.Forms.TextBox();
			this.btnOpenFilter = new System.Windows.Forms.Button();
			this.btnSaveFilter = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.tbErrorProbability = new System.Windows.Forms.TextBox();
			this.pictureBoxFilter = new System.Windows.Forms.PictureBox();
			this.label6 = new System.Windows.Forms.Label();
			this.btnGenerate = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilter)).BeginInit();
			this.SuspendLayout();
			// 
			// tbMaxElementsToHash
			// 
			this.tbMaxElementsToHash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbMaxElementsToHash.Location = new System.Drawing.Point(270, 48);
			this.tbMaxElementsToHash.Name = "tbMaxElementsToHash";
			this.tbMaxElementsToHash.Size = new System.Drawing.Size(100, 20);
			this.tbMaxElementsToHash.TabIndex = 0;
			this.tbMaxElementsToHash.Text = "10,000";
			this.tbMaxElementsToHash.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// labelElements
			// 
			this.labelElements.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelElements.AutoSize = true;
			this.labelElements.Location = new System.Drawing.Point(169, 51);
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
			this.btnCreateFilter.Location = new System.Drawing.Point(377, 2);
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
			this.btnAddHashes.Enabled = false;
			this.btnAddHashes.Location = new System.Drawing.Point(377, 71);
			this.btnAddHashes.Name = "btnAddHashes";
			this.btnAddHashes.Size = new System.Drawing.Size(100, 23);
			this.btnAddHashes.TabIndex = 3;
			this.btnAddHashes.Text = "Load Hashes...";
			this.btnAddHashes.UseVisualStyleBackColor = true;
			this.btnAddHashes.Click += new System.EventHandler(this.btnAddHashes_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(0, 13);
			this.label1.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(0, 13);
			this.label2.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(7, 65);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(0, 13);
			this.label4.TabIndex = 9;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(7, 46);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(0, 13);
			this.label3.TabIndex = 8;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(7, 84);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(0, 13);
			this.label5.TabIndex = 10;
			// 
			// btnTestHashes
			// 
			this.btnTestHashes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTestHashes.Enabled = false;
			this.btnTestHashes.Location = new System.Drawing.Point(270, 71);
			this.btnTestHashes.Name = "btnTestHashes";
			this.btnTestHashes.Size = new System.Drawing.Size(100, 23);
			this.btnTestHashes.TabIndex = 15;
			this.btnTestHashes.Text = "Load Test...";
			this.btnTestHashes.UseVisualStyleBackColor = true;
			this.btnTestHashes.Click += new System.EventHandler(this.btnTestHashes_Click);
			// 
			// label11
			// 
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(207, 7);
			this.label11.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(104, 13);
			this.label11.TabIndex = 25;
			this.label11.Text = "Hashes per element:";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbHashesPerElement
			// 
			this.tbHashesPerElement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbHashesPerElement.Location = new System.Drawing.Point(315, 4);
			this.tbHashesPerElement.Name = "tbHashesPerElement";
			this.tbHashesPerElement.ReadOnly = true;
			this.tbHashesPerElement.Size = new System.Drawing.Size(55, 20);
			this.tbHashesPerElement.TabIndex = 24;
			this.tbHashesPerElement.Text = "0";
			this.tbHashesPerElement.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// btnOpenFilter
			// 
			this.btnOpenFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpenFilter.Location = new System.Drawing.Point(377, 24);
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
			this.btnSaveFilter.Location = new System.Drawing.Point(377, 46);
			this.btnSaveFilter.Name = "btnSaveFilter";
			this.btnSaveFilter.Size = new System.Drawing.Size(100, 22);
			this.btnSaveFilter.TabIndex = 30;
			this.btnSaveFilter.Text = "   Save Filter...";
			this.btnSaveFilter.UseVisualStyleBackColor = true;
			this.btnSaveFilter.Click += new System.EventHandler(this.btnSaveFilter_Click);
			// 
			// label10
			// 
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(203, 28);
			this.label10.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(108, 13);
			this.label10.TabIndex = 32;
			this.label10.Text = "Probabilty of collision:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbErrorProbability
			// 
			this.tbErrorProbability.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbErrorProbability.Location = new System.Drawing.Point(315, 26);
			this.tbErrorProbability.Name = "tbErrorProbability";
			this.tbErrorProbability.Size = new System.Drawing.Size(55, 20);
			this.tbErrorProbability.TabIndex = 31;
			this.tbErrorProbability.Text = "0.1";
			this.tbErrorProbability.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// pictureBoxFilter
			// 
			this.pictureBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBoxFilter.Location = new System.Drawing.Point(6, 123);
			this.pictureBoxFilter.Name = "pictureBoxFilter";
			this.pictureBoxFilter.Size = new System.Drawing.Size(471, 331);
			this.pictureBoxFilter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBoxFilter.TabIndex = 33;
			this.pictureBoxFilter.TabStop = false;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(7, 103);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(0, 13);
			this.label6.TabIndex = 34;
			// 
			// btnGenerate
			// 
			this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGenerate.Location = new System.Drawing.Point(377, 94);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(100, 23);
			this.btnGenerate.TabIndex = 35;
			this.btnGenerate.Text = "Generate Hashes";
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(484, 461);
			this.Controls.Add(this.btnGenerate);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.pictureBoxFilter);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.tbErrorProbability);
			this.Controls.Add(this.btnSaveFilter);
			this.Controls.Add(this.btnOpenFilter);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.tbHashesPerElement);
			this.Controls.Add(this.btnTestHashes);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnAddHashes);
			this.Controls.Add(this.btnCreateFilter);
			this.Controls.Add(this.labelElements);
			this.Controls.Add(this.tbMaxElementsToHash);
			this.MinimumSize = new System.Drawing.Size(500, 500);
			this.Name = "MainForm";
			this.Text = "Bloom Filter";
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilter)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbMaxElementsToHash;
		private System.Windows.Forms.Label labelElements;
		private System.Windows.Forms.Button btnCreateFilter;
		private System.Windows.Forms.Button btnAddHashes;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnTestHashes;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox tbHashesPerElement;
		private System.Windows.Forms.Button btnOpenFilter;
		private System.Windows.Forms.Button btnSaveFilter;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox tbErrorProbability;
		private System.Windows.Forms.PictureBox pictureBoxFilter;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnGenerate;
	}
}

