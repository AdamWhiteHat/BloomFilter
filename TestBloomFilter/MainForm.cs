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

namespace TestBloomFilter
{
	public partial class MainForm : Form
	{
		private Random rand;
		private BloomFilter _filter;
		private string filename;
		private BackgroundWorker addHashesWorker;
		private static string buttonText_AddHashes	= "Add Hashes";
		private static string buttonText_Cancel		= "Cancel";

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			_filter = null;
			rand = new Random();
			int counter = rand.Next(100, 200);
			while (--counter > 0)
			{
				rand.Next();
			}

			addHashesWorker = new BackgroundWorker();
			addHashesWorker.DoWork += addHashesWorker_DoWork;
			addHashesWorker.RunWorkerCompleted += addHashesWorker_RunWorkerCompleted;
			addHashesWorker.WorkerSupportsCancellation = true;
		}

		private void btnCreateFilter_Click(object sender, EventArgs e)
		{
			int factor = 0;
			int maxElements = 0;
			int hashesPerToken = 0;
			int.TryParse(tbElements.Text.Replace(",",""), out maxElements);
			int.TryParse(tbFactor.Text.Replace(",", ""), out factor);
			int.TryParse(tbHashes.Text.Replace(",", ""), out hashesPerToken);

			_filter = new BloomFilter(maxElements, hashesPerToken, factor);

			label1.Text = string.Format("{0} bits", _filter.SizeBits);
			label2.Text = string.Format("{0} B", _filter.SizeBytes);
			label3.Text = string.Format("{0} MB", _filter.SizeMB);
			label4.Text = string.Format("{0} KB", _filter.SizeKB);
			label5.Text = string.Format("{0} IndexBitSize", _filter.IndexBitSize);
			label6.Text = string.Format("{0} IndexByteSize", _filter.IndexByteSize);

			RefreshLabel();
			btnCreateFilter.BackColor = Color.MintCream;
		}

		private void RefreshLabel()
		{
			if (this.InvokeRequired) { this.Invoke(new MethodInvoker(() => RefreshLabel())); return; }
			else
			{
				label7.Text = string.Format("{0} ElementsHashed", _filter.ElementsHashed);
				showFilterOutput();
			}
		}

		private void btnAddHashes_Click(object sender, EventArgs e)
		{
			string input = tbHashInput == null?"":tbHashInput.Text.Replace(",","");
			if (_filter == null || string.IsNullOrWhiteSpace(input))
			{
				return;
			}

			if (addHashesWorker.IsBusy)
			{
				addHashesWorker.CancelAsync();
				btnAddHashes.Text = buttonText_AddHashes;
				return;
			}
			else
			{
				btnAddHashes.Text = buttonText_Cancel;
			}

			if (input.Contains('.')) // Treat as a filename to file of tokens to add to filter
			{
				filename = Path.GetFullPath(input);
				if (File.Exists(filename))
				{
					if (!addHashesWorker.IsBusy)
					{
						addHashesWorker.RunWorkerAsync(filename);
					}
				}
			}
			else if (!string.IsNullOrWhiteSpace(input)) // Treat as a token to add to the filter
			{
				_filter.Add(input);
			}
		}

		void addHashesWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			List<string> lines = File.ReadAllLines((string)e.Argument).ToList();
			while (lines.Count > 0)
			{
				if (e.Cancel)
				{	break; }
				string line = lines[0];
				lines.Remove(line);
				_filter.Add(line);
				RefreshLabel();
			}
		}

		void addHashesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{			
			RefreshLabel();
			btnAddHashes.Text = buttonText_AddHashes;
			btnAddHashes.BackColor = Color.MintCream;
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
			showFilterOutput();	//tbOutput.Text += Environment.NewLine + _filter.AsString();
		}

		private void btnTestAbc_Click(object sender, EventArgs e)
		{			
			int wrong = 0;
			string testFile = OpenFileDlg(); // string fn = new string(filename.Where(c => !"0123456789".Contains(c)).ToArray()); testFile = Path.ChangeExtension(new string(Enumerable.Repeat('z', Path.GetFileNameWithoutExtension(fn).Length).ToArray()), ".txt");			
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
			int percent = (wrong * 100) / total;
			tbOutput.Text += Environment.NewLine + string.Format("{0} out of {1} wrong ({2}%)", wrong, total, percent);
		}

		private void btnOpenFilter_Click(object sender, EventArgs e)
		{
			string file = OpenFileDlg();
			if (!string.IsNullOrWhiteSpace(file))
			{
				_filter = BloomFilter.DeserializeFilter(file);
			}
		}

		private void btnSaveFilter_Click(object sender, EventArgs e)
		{
			string file = SaveFileDlg();
			if (!string.IsNullOrWhiteSpace(file))
			{
				_filter.SerializeFilter(file);
			}
		}

		private string OpenFileDlg()
		{
			using (OpenFileDialog openDlg = new OpenFileDialog())			
				if (openDlg.ShowDialog() == DialogResult.OK)				
					return openDlg.FileName;
			return string.Empty;
		}

		private string SaveFileDlg()
		{
			using (SaveFileDialog saveDlg = new SaveFileDialog())
				if (saveDlg.ShowDialog() == DialogResult.OK)
					return saveDlg.FileName;
			return string.Empty;
		}
	}
}
