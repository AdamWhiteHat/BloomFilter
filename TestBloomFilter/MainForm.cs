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
		private bool IsFilterOpen = false;
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

		private int CalculateFilterSize(int hashElementCeiling, double probabilityFloor)
		{
			double top = hashElementCeiling * Math.Log(probabilityFloor);
			double bottom = Math.Pow(Math.Log(2),2);

			top = Math.Abs(top);
			bottom = Math.Abs(bottom);

			double dividend = top / bottom;
			double result = Math.Round(dividend);

			return (int)result;
		}

		private int CalculateHashesPerElement(int sizeOfArray, int quantityHashElements)
		{
			double rhs = sizeOfArray / quantityHashElements;
			double hashesPerElement =  rhs * Math.Log(2);
			
			double result = Math.Round(Math.Abs(hashesPerElement));			
			return (int)result;
		}

		private void btnCreateOrCloseFilter_Click(object sender, EventArgs e)
		{
			if (IsFilterOpen == false)
			{
				int sizeOfArray = 0;
				int hashElementQuantity = 0;
				double errorProbabilityFloor = 0;
				int hashesPerElement = 0;
				
				//int.TryParse(tbHashesPerElement.Text.Replace(",", ""), out hashesPerToken);
				double.TryParse(tbErrorProbability.Text.Replace(",", ""), out errorProbabilityFloor);
				int.TryParse(tbMaxNumberOfElements.Text.Replace(",", ""), out hashElementQuantity);

				sizeOfArray = CalculateFilterSize(hashElementQuantity, errorProbabilityFloor);
				hashesPerElement = CalculateHashesPerElement(sizeOfArray, hashElementQuantity);

				tbHashesPerElement.Text = hashesPerElement.ToString();

				_filter = new BloomFilter(hashElementQuantity, hashesPerElement);

				SetLoadedStatus(true);
				RefreshControls();
			}
			else
			{
				_filter = null;
				SetLoadedStatus(false);
			}
		}

		private void SetLoadedStatus(bool isLoaded)
		{
			IsFilterOpen = isLoaded;
			if (IsFilterOpen)
			{
				btnCreateFilter.Text = buttonText_CloseFilter;				
			}
			else
			{
				btnCreateFilter.Text = buttonText_CreateFilter;
				label1.Text = label2.Text = label3.Text = label4.Text = label5.Text = "";
				label6.Text = label7.Text = label8.Text = label9.Text = "";
				tbOutput.Clear();
			}

			btnOpenFilter.Enabled = !IsFilterOpen;
			btnSaveFilter.Enabled = IsFilterOpen;
			btnAddHashes.Enabled = IsFilterOpen;
			btnTestHashes.Enabled = IsFilterOpen;
		}

		private void btnOpenFilter_Click(object sender, EventArgs e)
		{
			string file = OpenFileDlg();
			if (!string.IsNullOrWhiteSpace(file) && File.Exists(file))
			{
				_filter = BloomFilterSerializer.Load(file);
				tbMaxNumberOfElements.Text = _filter.MaxElements.ToString();
				tbHashesPerElement.Text = _filter.HashesPerToken.ToString();
				SetLoadedStatus(true);				
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
			if (_filter == null || addHashesWorker == null) { throw new ArgumentNullException(); }
			if (addHashesWorker.IsBusy)
			{
				addHashesWorker.CancelAsync();
				return;
			}

			string filename = OpenFileDlg();

			if (string.IsNullOrWhiteSpace(filename)) { return; }

			filename = Path.GetFullPath(filename.Replace(",", ""));

			if (File.Exists(filename))
			{
				btnAddHashes.Text = buttonText_Cancel;
				addHashesWorker.RunWorkerAsync(filename);
			}
		}

		void addHashesWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			foreach (string line in File.ReadAllLines((string)e.Argument))
			{
				if (addHashesWorker.CancellationPending)
				{
					e.Cancel = true;
					break;
				}
				_filter.Add(line);
				RefreshLabel();
			}
		}

		void addHashesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			btnAddHashes.Text = buttonText_AddHashes;
			RefreshControls();
		}

		private void btnTestHashes_Click(object sender, EventArgs e)
		{
			string quantity = tbHashTestQuantity.Text.Replace(",", "");

			string filename = OpenFileDlg();
			if (!string.IsNullOrWhiteSpace(filename))
			{
				string[] lines = File.ReadAllLines(filename);
				int counter = 0;
				int correct = 0;
				int incorrect = 0;
				int.TryParse(quantity, out counter);
				int half = counter / 2;
				while (--counter > 0)
				{
					string testNumber = "";
					bool correctValue = false;
					if (counter > half)
					{
						int number = 1;
						while (number % 2 != 0)
						{
							number = rand.Next();
						}
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
			//tbOutput.AppendText(string.Concat(Environment.NewLine, Environment.NewLine, _filter.AsString()));			
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
