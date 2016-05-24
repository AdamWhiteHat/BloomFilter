using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BloomFilterCore;
using BloomFilterCore.Serialization;

namespace TestBloomFilter
{
	public partial class MainForm : Form
	{
		private bool IsFilterOpen;
		private Random rand;
		private BloomFilter _filter;
		private BackgroundWorker addHashesWorker;
		private static string buttonText_AddHashes = "Add Hashes";
		private static string buttonText_Cancel = "Cancel";
		private static string buttonText_CreateFilter = "Create Filter";
		private static string buttonText_CloseFilter = "Close Filter";

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			IsFilterOpen = false;
			_filter = null;
			rand = new Random();
			int counter = rand.Next(100, 200);
			while (--counter > 0)
			{
				rand.Next();
			}

			addHashesWorker = new BackgroundWorker();
			addHashesWorker.WorkerSupportsCancellation = true;
			addHashesWorker.DoWork += addHashesWorker_DoWork;
			addHashesWorker.RunWorkerCompleted += addHashesWorker_RunWorkerCompleted;
		}

		private void btnCreateFilter_Click(object sender, EventArgs e)
		{
			if (IsFilterOpen == false)
			{
				IsFilterOpen = true;
				btnCreateFilter.Text = buttonText_CloseFilter;

				int maxElements = 0;
				int hashesPerToken = 0;
				int.TryParse(tbElements.Text.Replace(",", ""), out maxElements);
				int.TryParse(tbHashes.Text.Replace(",", ""), out hashesPerToken);

				_filter = new BloomFilter(maxElements, hashesPerToken);

				btnCreateFilter.BackColor = Color.MintCream;
				btnCreateFilter.Text = buttonText_CreateFilter;
				RefreshControls();
			}
			else
			{
				btnCreateFilter.Text = buttonText_CreateFilter;
				IsFilterOpen = false;
			}
		}

		private void btnOpenFilter_Click(object sender, EventArgs e)
		{
			string file = OpenFileDlg();
			if (!string.IsNullOrWhiteSpace(file))
			{
				_filter = BloomFilterSerializer.Load(file);
				RefreshControls();
			}
		}

		private void btnSaveFilter_Click(object sender, EventArgs e)
		{
			string file = SaveFileDlg();
			if (!string.IsNullOrWhiteSpace(file))
			{
				BloomFilterSerializer.Save(_filter, file);
			}
		}
		
		private void btnAddHashes_Click(object sender, EventArgs e)
		{
			if (addHashesWorker.IsBusy)
			{
				addHashesWorker.CancelAsync();
				return;
			}
			string filename = "";
			if (_filter == null || tbHashInput == null || string.IsNullOrWhiteSpace(tbHashInput.Text))
			{
				filename = OpenFileDlg();
				//return;
			}

			if (string.IsNullOrWhiteSpace(filename)) { return; }
			
			btnAddHashes.Text = buttonText_Cancel;
			filename = Path.GetFullPath(filename.Replace(",", ""));

			if (filename.Contains('.')) // Treat as a filename to file of tokens to add to filter
			{
				if (File.Exists(filename))
				{
					if (!addHashesWorker.IsBusy)
					{
						addHashesWorker.RunWorkerAsync(filename);
					}
				}
			}
			else if (!string.IsNullOrWhiteSpace(filename)) // Treat as a token to add to the filter
			{
				_filter.Add(filename);
			}
		}

		void addHashesWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			foreach (string line in File.ReadAllLines((string)e.Argument))
			{
				if (addHashesWorker.CancellationPending)
				{
					e.Cancel = true;
					return;
				}
				_filter.Add(line);
				RefreshLabel();
			}
		}

		void addHashesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{		
			//btnAddHashes.BackColor = Color.MintCream;	
			btnAddHashes.Text = buttonText_AddHashes;
			RefreshControls();
		}

		private void btnTest_Click(object sender, EventArgs e)
		{
			string input = tbTestInput.Text.Replace(",", "");
			if (!input.All(c => "0123456789".Contains(c)) || cbForceToken.Checked) // If a string (token)
			{
				bool result = _filter.Query(input);
				if (result)
				{
					tbTestInput.BackColor = Color.MintCream;// += " EXISTS";
				}
				else
				{
					tbTestInput.BackColor = Color.MistyRose;// += " DOES NOT EXIST";
				}
			}
			else // If a number
			{
				string filename = OpenFileDlg();
				string[] lines = File.ReadAllLines(filename);
				int counter = 0;
				int correct = 0;
				int incorrect = 0;
				int.TryParse(input, out counter);
				int half = counter / 2;
				while (--counter > 0)
				{
					string testNumber = "";
					bool correctValue = false;
					if (counter > half)
					{
						int number = 1;
						while (number % 2 != 0)
						{ number = rand.Next(); }

						correctValue = false;
						testNumber = number.ToString();
					}
					else
					{
						correctValue = true;
						testNumber = lines[rand.Next(0, lines.Length)];
					}

					bool queryResult = _filter.Query(testNumber);
					if (queryResult == correctValue) correct += 1;
					else incorrect += 1;
				}
				label8.Text = string.Format("Correct:   {0}", correct);
				label9.Text = string.Format("Incorrect: {0}", incorrect);
				btnTestHashing.BackColor = Color.MintCream;
			}
		}

		private void RefreshControls()
		{
			label1.Text = string.Format("{0} bits", _filter.SizeBits);
			label2.Text = string.Format("{0} B", _filter.SizeBytes);
			label3.Text = string.Format("{0} MB", _filter.SizeMB);
			label4.Text = string.Format("{0} KB", _filter.SizeKB);
			label5.Text = string.Format("{0} IndexBitSize", _filter.IndexBitSize);
			label6.Text = string.Format("{0} IndexByteSize", _filter.IndexByteSize);
			RefreshLabel();
			showFilterOutput();
		}

		private void RefreshLabel()
		{
			if (this.InvokeRequired) { this.Invoke(new MethodInvoker(() => RefreshLabel())); return; }
			else
			{
				label7.Text = string.Concat(_filter.ElementsHashed.ToString(), " ElementsHashed");
			}
		}

		private void showFilterOutput()
		{
			if (_filter == null) { return; }
			if (this.InvokeRequired) { this.Invoke(new MethodInvoker(() => showFilterOutput())); return; }
			else
			{
				tbOutput.Text = _filter.GetUtilization();
			}
		}

		private void btnTest_Click_1(object sender, EventArgs e)
		{
			RefreshControls();
		}

		private void btnTestAbc_Click(object sender, EventArgs e)
		{
			int wrong = 0;
			string testFile = OpenFileDlg();
			if (string.IsNullOrWhiteSpace(testFile) || !File.Exists(testFile))
			{
				return;
			}

			string[] lines = File.ReadAllLines(testFile);
			foreach (string line in lines)
			{
				if (_filter.Query(line))
				{
					wrong += 1;
				}
			}

			int total = lines.Length;
			double percent = (wrong * 100) / total;
			tbOutput.Text += Environment.NewLine + string.Format("{0} out of {1} wrong ({2:0.00}%)", wrong, total, percent);
		}
		
		private string OpenFileDlg()
		{
			using (OpenFileDialog openDlg = new OpenFileDialog())
			{
				if (openDlg.ShowDialog() == DialogResult.OK) 
				{
					return openDlg.FileName;
				}
			}
			return string.Empty;
		}

		private string SaveFileDlg()
		{
			using (SaveFileDialog saveDlg = new SaveFileDialog())
			{
				if (saveDlg.ShowDialog() == DialogResult.OK) 
				{
					return saveDlg.FileName;
				}
			}
			return string.Empty;
		}
	}
}
