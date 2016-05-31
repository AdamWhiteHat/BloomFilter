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
		private BloomFilter filter;
		private BackgroundWorker addHashesWorker;

		// Static readonly strings 
		private static readonly string buttonText_AddHashes = "Add Hashes";
		private static readonly string buttonText_Cancel = "Cancel";
		private static readonly string buttonText_CreateFilter = "Create Filter";
		private static readonly string buttonText_CloseFilter = "Close Filter";

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			filter = null;
			IsFilterOpen = false;
			
			addHashesWorker = new BackgroundWorker();
			addHashesWorker.WorkerSupportsCancellation = true;
			addHashesWorker.DoWork += addHashesWorker_DoWork;
			addHashesWorker.RunWorkerCompleted += addHashesWorker_RunWorkerCompleted;
		}
		
		private void btnCreateOrCloseFilter_Click(object sender, EventArgs e)
		{
			if (IsFilterOpen == false)
			{
				int maxElementsToHash = 0;
				double errorProbabilityFloor = 0;

				double.TryParse(tbErrorProbability.Text.Replace(",", ""), out errorProbabilityFloor);
				int.TryParse(tbMaxElementsToHash.Text.Replace(",", ""), out maxElementsToHash);

				filter = new BloomFilter(maxElementsToHash, errorProbabilityFloor);

				tbMaxElementsToHash.Text = filter.MaxElements.ToString();
				tbHashesPerElement.Text = filter.HashesPerElement.ToString();

				SetLoadedStatus(true);
				RefreshControls();
			}
			else
			{
				filter = null;
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
				filter = BloomFilterSerializer.Load(file);
				tbMaxElementsToHash.Text = filter.MaxElements.ToString();
				tbHashesPerElement.Text = filter.HashesPerElement.ToString();
				SetLoadedStatus(true);
				RefreshControls();
			}
		}

		private void btnSaveFilter_Click(object sender, EventArgs e)
		{
			string file = SaveFileDlg();
			if (!string.IsNullOrWhiteSpace(file))
			{
				BloomFilterSerializer.Save(filter, file);
			}
		}

		private void btnAddHashes_Click(object sender, EventArgs e)
		{
			if (filter == null || addHashesWorker == null) { throw new ArgumentNullException(); }
			if (addHashesWorker.IsBusy)
			{
				addHashesWorker.CancelAsync();
				return;
			}

			string filename = OpenFileDlg();

			if (string.IsNullOrWhiteSpace(filename)) { return; }

			filename = Path.GetFullPath(filename);

			if (File.Exists(filename))
			{
				btnAddHashes.Text = buttonText_Cancel;
				addHashesWorker.RunWorkerAsync(filename);
			}
		}

		private void addHashesWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string[] lines = File.ReadAllLines((string)e.Argument);
			foreach (string line in lines)
			{
				if (addHashesWorker.CancellationPending)
				{
					e.Cancel = true;
					break;
				}
				filter.Add(line);
				RefreshLabel();
			}
		}

		private void addHashesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			btnAddHashes.Text = buttonText_AddHashes;
			RefreshControls();
		}

		private void btnTestHashes_Click(object sender, EventArgs e)
		{
			string filename = OpenFileDlg();
			if (!string.IsNullOrWhiteSpace(filename))
			{
				string[] lines = File.ReadAllLines(filename);
								
				int counter = 0;
				int max = lines.Length - 1;

				int correct = 0;
				int incorrect = 0;

				while (counter++ < max)
				{
					if (filter.Query(lines[counter]))
					{
						correct += 1;
					}
					else
					{
						incorrect += 1;
					}
				}
				label8.Text = string.Format("Correct:   {0}", correct);
				label9.Text = string.Format("Incorrect: {0}", incorrect);
			}
		}

		private void RefreshControls()
		{
			if (filter == null) { return; }
			if (this.InvokeRequired) { this.Invoke(new MethodInvoker(() => RefreshControls())); return; }
			else
			{
				label1.Text = string.Format("{0} bits", filter.SizeBits);
				label2.Text = string.Format("{0} B", filter.SizeBytes);
				label3.Text = string.Format("{0} KB", filter.SizeKB);
				label4.Text = string.Format("{0} MB", filter.SizeMB);
				label5.Text = string.Format("{0} IndexBitSize", filter.IndexBitSize);
				label6.Text = string.Format("{0} IndexByteSize", filter.IndexByteSize);
				RefreshLabel();
				tbOutput.Text = filter.GetUtilization();
				//tbOutput.AppendText(string.Concat(Environment.NewLine, Environment.NewLine, _filter.AsString()));
			}
		}

		private void RefreshLabel()
		{
			if (this.InvokeRequired) { this.Invoke(new MethodInvoker(() => RefreshLabel())); return; }
			else
			{
				label7.Text = string.Concat(filter.ElementsHashed.ToString(), " ElementsHashed");
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
