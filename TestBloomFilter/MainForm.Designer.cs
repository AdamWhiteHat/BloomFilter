﻿namespace UnitTestBloomFilter
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
			this.components = new System.ComponentModel.Container();
			this.tbMaxElementsToHash = new System.Windows.Forms.TextBox();
			this.labelElements = new System.Windows.Forms.Label();
			this.btnCreateFilter = new System.Windows.Forms.Button();
			this.btnAddHashesFromFile = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnTestHashesFromFile = new System.Windows.Forms.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.tbHashesPerElement = new System.Windows.Forms.TextBox();
			this.btnOpenFilter = new System.Windows.Forms.Button();
			this.btnSaveFilter = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.tbErrorProbability = new System.Windows.Forms.TextBox();
			this.pictureBoxFilter = new System.Windows.Forms.PictureBox();
			this.contextMenuPictureBox = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripSaveImage = new System.Windows.Forms.ToolStripMenuItem();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.btnGenerateSequenceToFile = new System.Windows.Forms.Button();
			this.cbCompress = new System.Windows.Forms.CheckBox();
			this.progressBarBusy = new System.Windows.Forms.ProgressBar();
			this.panelWorkingAnimation = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilter)).BeginInit();
			this.contextMenuPictureBox.SuspendLayout();
			this.panelWorkingAnimation.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbMaxElementsToHash
			// 
			this.tbMaxElementsToHash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbMaxElementsToHash.Location = new System.Drawing.Point(470, 48);
			this.tbMaxElementsToHash.Name = "tbMaxElementsToHash";
			this.tbMaxElementsToHash.Size = new System.Drawing.Size(100, 20);
			this.tbMaxElementsToHash.TabIndex = 0;
			this.tbMaxElementsToHash.Text = "10000";
			this.tbMaxElementsToHash.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// labelElements
			// 
			this.labelElements.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelElements.AutoSize = true;
			this.labelElements.Location = new System.Drawing.Point(370, 51);
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
			this.btnCreateFilter.Location = new System.Drawing.Point(577, 2);
			this.btnCreateFilter.Name = "btnCreateFilter";
			this.btnCreateFilter.Size = new System.Drawing.Size(100, 22);
			this.btnCreateFilter.TabIndex = 2;
			this.btnCreateFilter.Text = "Create Filter";
			this.btnCreateFilter.UseVisualStyleBackColor = true;
			this.btnCreateFilter.Click += new System.EventHandler(this.btnCreateOrCloseFilter_Click);
			// 
			// btnAddHashesFromFile
			// 
			this.btnAddHashesFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddHashesFromFile.Enabled = false;
			this.btnAddHashesFromFile.Location = new System.Drawing.Point(210, 32);
			this.btnAddHashesFromFile.Name = "btnAddHashesFromFile";
			this.btnAddHashesFromFile.Size = new System.Drawing.Size(157, 23);
			this.btnAddHashesFromFile.TabIndex = 3;
			this.btnAddHashesFromFile.Text = "Add Hashes From File...";
			this.btnAddHashesFromFile.UseVisualStyleBackColor = true;
			this.btnAddHashesFromFile.Click += new System.EventHandler(this.btnAddHashesFromFile_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(0, 13);
			this.label1.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(0, 13);
			this.label2.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 44);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(0, 13);
			this.label3.TabIndex = 8;
			// 
			// btnTestHashesFromFile
			// 
			this.btnTestHashesFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTestHashesFromFile.Enabled = false;
			this.btnTestHashesFromFile.Location = new System.Drawing.Point(210, 57);
			this.btnTestHashesFromFile.Name = "btnTestHashesFromFile";
			this.btnTestHashesFromFile.Size = new System.Drawing.Size(157, 23);
			this.btnTestHashesFromFile.TabIndex = 15;
			this.btnTestHashesFromFile.Text = "Test Filter From File...";
			this.btnTestHashesFromFile.UseVisualStyleBackColor = true;
			this.btnTestHashesFromFile.Click += new System.EventHandler(this.btnTestHashesFromFile_Click);
			// 
			// label11
			// 
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(407, 7);
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
			this.tbHashesPerElement.Location = new System.Drawing.Point(515, 4);
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
			this.btnOpenFilter.Location = new System.Drawing.Point(577, 24);
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
			this.btnSaveFilter.Location = new System.Drawing.Point(577, 46);
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
			this.label10.Location = new System.Drawing.Point(403, 28);
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
			this.tbErrorProbability.Location = new System.Drawing.Point(515, 26);
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
			this.pictureBoxFilter.ContextMenuStrip = this.contextMenuPictureBox;
			this.pictureBoxFilter.Location = new System.Drawing.Point(6, 122);
			this.pictureBoxFilter.Name = "pictureBoxFilter";
			this.pictureBoxFilter.Size = new System.Drawing.Size(671, 332);
			this.pictureBoxFilter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBoxFilter.TabIndex = 33;
			this.pictureBoxFilter.TabStop = false;
			// 
			// contextMenuPictureBox
			// 
			this.contextMenuPictureBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSaveImage});
			this.contextMenuPictureBox.Name = "contextMenuPictureBox";
			this.contextMenuPictureBox.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.contextMenuPictureBox.ShowImageMargin = false;
			this.contextMenuPictureBox.Size = new System.Drawing.Size(119, 26);
			this.contextMenuPictureBox.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuPictureBox_Opening);
			// 
			// toolStripSaveImage
			// 
			this.toolStripSaveImage.Name = "toolStripSaveImage";
			this.toolStripSaveImage.Size = new System.Drawing.Size(118, 22);
			this.toolStripSaveImage.Text = "Save Image...";
			this.toolStripSaveImage.Click += new System.EventHandler(this.toolStripSaveImage_Click);
			// 
			// btnGenerate
			// 
			this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGenerate.Location = new System.Drawing.Point(577, 93);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(100, 23);
			this.btnGenerate.TabIndex = 35;
			this.btnGenerate.Text = "Generate Hashes";
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// btnGenerateSequenceToFile
			// 
			this.btnGenerateSequenceToFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGenerateSequenceToFile.Enabled = false;
			this.btnGenerateSequenceToFile.Location = new System.Drawing.Point(210, 7);
			this.btnGenerateSequenceToFile.Name = "btnGenerateSequenceToFile";
			this.btnGenerateSequenceToFile.Size = new System.Drawing.Size(157, 23);
			this.btnGenerateSequenceToFile.TabIndex = 36;
			this.btnGenerateSequenceToFile.Text = "Generate Sequence To File...";
			this.btnGenerateSequenceToFile.UseVisualStyleBackColor = true;
			this.btnGenerateSequenceToFile.Click += new System.EventHandler(this.btnGenerateSequenceToFile_Click);
			// 
			// cbCompress
			// 
			this.cbCompress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbCompress.AutoSize = true;
			this.cbCompress.Location = new System.Drawing.Point(579, 70);
			this.cbCompress.Name = "cbCompress";
			this.cbCompress.Size = new System.Drawing.Size(104, 17);
			this.cbCompress.TabIndex = 37;
			this.cbCompress.Text = "File compression";
			this.cbCompress.UseVisualStyleBackColor = true;
			// 
			// progressBarBusy
			// 
			this.progressBarBusy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBarBusy.Location = new System.Drawing.Point(21, 25);
			this.progressBarBusy.Name = "progressBarBusy";
			this.progressBarBusy.Size = new System.Drawing.Size(337, 23);
			this.progressBarBusy.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progressBarBusy.TabIndex = 38;
			this.progressBarBusy.Value = 50;
			// 
			// panelWorkingAnimation
			// 
			this.panelWorkingAnimation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelWorkingAnimation.Controls.Add(this.label7);
			this.panelWorkingAnimation.Controls.Add(this.progressBarBusy);
			this.panelWorkingAnimation.Location = new System.Drawing.Point(153, 161);
			this.panelWorkingAnimation.Name = "panelWorkingAnimation";
			this.panelWorkingAnimation.Size = new System.Drawing.Size(378, 55);
			this.panelWorkingAnimation.TabIndex = 39;
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(154, 6);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(70, 15);
			this.label7.TabIndex = 39;
			this.label7.Text = "Working . . .";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Location = new System.Drawing.Point(6, 2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(201, 80);
			this.panel1.TabIndex = 40;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 63);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(0, 13);
			this.label5.TabIndex = 41;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(9, 105);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(0, 13);
			this.label6.TabIndex = 42;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(9, 86);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(0, 13);
			this.label4.TabIndex = 10;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(684, 462);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panelWorkingAnimation);
			this.Controls.Add(this.cbCompress);
			this.Controls.Add(this.btnGenerateSequenceToFile);
			this.Controls.Add(this.btnGenerate);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.tbErrorProbability);
			this.Controls.Add(this.btnSaveFilter);
			this.Controls.Add(this.btnOpenFilter);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.tbHashesPerElement);
			this.Controls.Add(this.btnTestHashesFromFile);
			this.Controls.Add(this.btnAddHashesFromFile);
			this.Controls.Add(this.btnCreateFilter);
			this.Controls.Add(this.labelElements);
			this.Controls.Add(this.tbMaxElementsToHash);
			this.Controls.Add(this.pictureBoxFilter);
			this.MinimumSize = new System.Drawing.Size(700, 500);
			this.Name = "MainForm";
			this.Text = "Bloom Filter";
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilter)).EndInit();
			this.contextMenuPictureBox.ResumeLayout(false);
			this.panelWorkingAnimation.ResumeLayout(false);
			this.panelWorkingAnimation.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbMaxElementsToHash;
		private System.Windows.Forms.Label labelElements;
		private System.Windows.Forms.Button btnCreateFilter;
		private System.Windows.Forms.Button btnAddHashesFromFile;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnTestHashesFromFile;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox tbHashesPerElement;
		private System.Windows.Forms.Button btnOpenFilter;
		private System.Windows.Forms.Button btnSaveFilter;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox tbErrorProbability;
		private System.Windows.Forms.PictureBox pictureBoxFilter;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.Button btnGenerateSequenceToFile;
		private System.Windows.Forms.CheckBox cbCompress;
		private System.Windows.Forms.ContextMenuStrip contextMenuPictureBox;
		private System.Windows.Forms.ToolStripMenuItem toolStripSaveImage;
		private System.Windows.Forms.ProgressBar progressBarBusy;
		private System.Windows.Forms.Panel panelWorkingAnimation;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label4;
	}
}

