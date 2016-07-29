using System;
using System.Collections;
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
using BloomFilterCore.Hashes;
using BloomFilterCore.Serialization;

namespace TestBloomFilter
{
	public partial class MainForm : Form
	{
		private bool IsFilterOpen = false;
		private BloomFilter filter = null;
		private BackgroundWorker addHashesWorker;
		private BackgroundWorker generateHashesWorker;

		// Static readonly strings 
		private static readonly string buttonText_AddHashes = "Add Hashes";
		private static readonly string buttonText_Cancel = "Cancel";
		private static readonly string buttonText_CreateFilter = "Create Filter";
		private static readonly string buttonText_CloseFilter = "Close Filter";
		private static readonly string buttonText_GenerateHash = "Generate Hash";
		private static readonly string buttonText_GenerateStop = "Stop Generating";
		private static string defaultMaxElements;
		private static string defaultHashesPerElement;
		private static string defaultErrorProbability;

		public MainForm()
		{
			InitializeComponent();

			defaultMaxElements = tbMaxElementsToHash.Text;
			defaultHashesPerElement = tbHashesPerElement.Text;
			defaultErrorProbability = tbErrorProbability.Text;

			addHashesWorker = new BackgroundWorker();
			addHashesWorker.WorkerSupportsCancellation = true;
			addHashesWorker.DoWork += addHashesWorker_DoWork;
			addHashesWorker.RunWorkerCompleted += addHashesWorker_RunWorkerCompleted;

			generateHashesWorker = new BackgroundWorker();
			generateHashesWorker.WorkerSupportsCancellation = true;
			generateHashesWorker.DoWork += generateHashesWorker_DoWork;
			generateHashesWorker.RunWorkerCompleted += generateHashesWorker_RunWorkerCompleted;
		}

		private Bitmap ToBitmap(BitArray bitArray)
		{
			Bitmap result;

			if (bitArray == null)
			{
				throw new ArgumentNullException("bitArray");
			}

			double squareRoot = Math.Sqrt(bitArray.Count);

			if (squareRoot % 1 != 0)
			{
				throw new ArgumentException("bitArray.Count must be a perfect square number");
			}

			int width = (int)squareRoot;
			int height = (int)squareRoot;

			result = new Bitmap(width, height);

			int x = 0;
			int y = 0;
			foreach (bool bit in bitArray)
			{
				result.SetPixel(x, y, bit ? Color.White : Color.Black);

				x += 1;
				if (x == width)
				{
					x = 0;
					y += 1;
				}
			}

			return result;
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
				label1.Text = label2.Text = label3.Text =
					label4.Text = label5.Text = label6.Text = "";
			}

			btnOpenFilter.Enabled = !IsFilterOpen;
			btnSaveFilter.Enabled = IsFilterOpen;
			btnAddHashes.Enabled = IsFilterOpen;
			btnTestHashes.Enabled = IsFilterOpen;
		}

		private void btnCreateOrCloseFilter_Click(object sender, EventArgs e)
		{
			if (!IsFilterOpen)
			{
				int maxElementsToHash = 0;
				double errorProbabilityFloor = 0;

				double.TryParse(tbErrorProbability.Text.Replace(",", ""), out errorProbabilityFloor);
				int.TryParse(tbMaxElementsToHash.Text.Replace(",", ""), out maxElementsToHash);

				filter = new BloomFilter(maxElementsToHash, errorProbabilityFloor);

				tbMaxElementsToHash.Text = filter.MaxElements.ToString();
				tbHashesPerElement.Text = filter.HashesPerElement.ToString();
				tbErrorProbability.Text = filter.ErrorProbability.ToString();

				SetLoadedStatus(true);
				RefreshControls();
			}
			else
			{
				filter = null;
				Bitmap blank = new Bitmap(1, 1);
				blank.SetPixel(0,0, Color.Black);
				this.pictureBoxFilter.Image = blank;

				tbMaxElementsToHash.Text= defaultMaxElements;
				tbHashesPerElement.Text = defaultHashesPerElement;
				tbErrorProbability.Text = defaultErrorProbability;

				SetLoadedStatus(false);
			}
		}

		private void btnTestHashes_Click(object sender, EventArgs e)
		{
			string filename = FormHelper.OpenFileDlg();
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
				label6.Text = string.Format("Correct/Incorrect: {0}/{1}", correct, incorrect);
			}
		}

		#region Add Hashes

		private void btnAddHashes_Click(object sender, EventArgs e)
		{
			if (IsFilterOpen)
			{
				if (filter == null || addHashesWorker == null) { throw new ArgumentNullException(); }
				if (addHashesWorker.IsBusy)
				{
					addHashesWorker.CancelAsync();
					return;
				}

				string filename = FormHelper.OpenFileDlg();
				if (!string.IsNullOrWhiteSpace(filename))
				{
					btnAddHashes.Text = buttonText_Cancel;
					addHashesWorker.RunWorkerAsync(filename);
				}
			}
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
			if (IsFilterOpen)
			{
				if (filter == null || generateHashesWorker == null) { throw new ArgumentNullException(); }
				if (generateHashesWorker.IsBusy)
				{
					generateHashesWorker.CancelAsync();
				}
				else
				{
					btnGenerate.Text = buttonText_GenerateStop;
					generateHashesWorker.RunWorkerAsync();
				}
			}
		}

		#region BackgroundWorkers

		private void addHashesWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			int counter = 0;
			string[] lines = File.ReadAllLines((string)e.Argument);
			foreach (string line in lines)
			{
				if (addHashesWorker.CancellationPending)
				{
					e.Cancel = true;
					break;
				}
				filter.Add(line);

				if (counter++ > 1000)
				{
					counter = 0;
					RefreshControls();
				}
			}
		}

		private void addHashesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			btnAddHashes.Text = buttonText_AddHashes;
			RefreshControls();
		}


		void generateHashesWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			//ByteGenerator.SequenceGenerator sequenceGenerator = new ByteGenerator.SequenceGenerator(9, 0, 1000000);
			ByteGenerator.RandomGenerator sequenceGenerator = new ByteGenerator.RandomGenerator();

			int counter = 0;
			while (!generateHashesWorker.CancellationPending)
			{
				filter.Add(sequenceGenerator.GetNext());
				if (counter++ > 1000)
				{
					counter = 0;
					RefreshControls();
				}
			}

			e.Result = sequenceGenerator.Size;			
		}

		void generateHashesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			btnGenerate.Text = buttonText_GenerateHash;
			RefreshControls();
			//MessageBox.Show(((int)e.Result).ToString());
		}

		#endregion

		#endregion

		#region Open & Save

		private void btnOpenFilter_Click(object sender, EventArgs e)
		{
			string file = FormHelper.OpenFileDlg();
			if (!string.IsNullOrWhiteSpace(file))
			{
				filter = BloomFilterSerializer.Load(file);
				tbMaxElementsToHash.Text = filter.MaxElements.ToString();
				tbHashesPerElement.Text = filter.HashesPerElement.ToString();
				tbErrorProbability.Text = filter.ErrorProbability.ToString();				
				SetLoadedStatus(true);
				RefreshControls();
			}
		}

		private void btnSaveFilter_Click(object sender, EventArgs e)
		{
			string file = FormHelper.SaveFileDlg();
			if (!string.IsNullOrWhiteSpace(file))
			{
				BloomFilterSerializer.Save(filter, file);
			}
		}

		#endregion

		#region Refresh Methods

		private void RefreshControls()
		{
			if (filter == null) { return; }
			if (this.InvokeRequired) { this.Invoke(new MethodInvoker(() => RefreshControls())); return; }
			else
			{
				label1.Text = string.Format("{0:#,###,###,###} bits", filter.SizeBits);
				label2.Text = FormHelper.FormatFilesize(filter.SizeBits);
				label3.Text = string.Format("IndexSize: {0:0.##} Bits / {1} Bytes", filter.IndexBitSize, filter.IndexByteSize);
				label4.Text = string.Concat(filter.ElementsHashed.ToString(), " ElementsHashed");
				label5.Text = filter.GetUtilization();
				
				if (IsFilterOpen)
				{
					pictureBoxFilter.Image = ToBitmap(filter.FilterArray);
				}
			}
		}

		private void RefreshLabel()
		{
			if (this.InvokeRequired) { this.Invoke(new MethodInvoker(() => RefreshLabel())); return; }
			else
			{
				label4.Text = string.Concat(filter.ElementsHashed.ToString(), " ElementsHashed");
			}
		}
		
		#endregion

	}
}
