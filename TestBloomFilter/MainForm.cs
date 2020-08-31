using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Numerics;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Collections.Generic;

using BloomFilterCore;
using BloomFilterCore.Serialization;

namespace UnitTestBloomFilter
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

			double squareRoot = Math.Sqrt(bitArray.Length);

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
			btnAddHashesFromFile.Enabled = IsFilterOpen;
			btnTestHashesFromFile.Enabled = IsFilterOpen;
			btnGenerateSequenceToFile.Enabled = IsFilterOpen;
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

				tbMaxElementsToHash.Text = filter.MaxElementsToHash.ToString();
				tbHashesPerElement.Text = filter.HashesPerElement.ToString();
				tbErrorProbability.Text = filter.ErrorProbability.ToString();

				SetLoadedStatus(true);
				RefreshControls();
			}
			else
			{
				filter = null;
				pictureBoxFilter.Image = null;

				tbMaxElementsToHash.Text = defaultMaxElements;
				tbHashesPerElement.Text = defaultHashesPerElement;
				tbErrorProbability.Text = defaultErrorProbability;

				SetLoadedStatus(false);
			}
		}

		#region Test

		private void btnGenerateSequenceToFile_Click(object sender, EventArgs e)
		{
			if (!IsFilterOpen)
			{
				return;
			}

			string filename = FormHelper.SaveFileDlg();
			if (!string.IsNullOrWhiteSpace(filename))
			{
				CryptoRandom rand = new CryptoRandom();
				int randomNumber = rand.Next(50000);

				BigInteger randomIncrementValue = PrimeHelper.GetPreviousPrime(randomNumber);

				ByteGenerator.SequenceGenerator sequence = new ByteGenerator.SequenceGenerator(4, 1, (int)randomIncrementValue);

				int counter = 0;
				int maxElements = filter.MaxElementsToHash / 2;
				List<string> elementsCollection = new List<string>();
				while (counter < maxElements)
				{
					string nextElement = sequence.GetNext();
					elementsCollection.Add(nextElement);
					counter++;
				}

				File.WriteAllLines(filename, elementsCollection);
			}
		}

		private void btnAddHashesFromFile_Click(object sender, EventArgs e)
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
					btnAddHashesFromFile.Text = buttonText_Cancel;
					addHashesWorker.RunWorkerAsync(filename);
				}
			}
		}

		private void btnTestHashesFromFile_Click(object sender, EventArgs e)
		{
			string filename = FormHelper.OpenFileDlg();
			if (!string.IsNullOrWhiteSpace(filename))
			{
				TimeSpan totalRuntimeTotal = TimeSpan.Zero;
				TimeSpan filterAddTimeTotal = TimeSpan.Zero;
				TimeSpan averageAddTime = TimeSpan.Zero;

				Stopwatch totalRuntimeTimer = Stopwatch.StartNew();
				Stopwatch filterAddTimer = Stopwatch.StartNew();

				totalRuntimeTimer.Restart();
				string[] lines = File.ReadAllLines(filename);

				int counter = 0;
				int max = lines.Length - 1;

				int membershipCorrect = 0;
				int membershipIncorrect = 0;

				bool filterContains = false;

				while (counter < max)
				{
					filterAddTimer.Restart();
					filterContains = filter.Contains(lines[counter]);
					filterAddTimeTotal = filterAddTimeTotal.Add(filterAddTimer.Elapsed);

					if (filterContains)
					{
						membershipCorrect += 1;
					}
					else
					{
						membershipIncorrect += 1;
					}

					counter++;
				}

				ByteGenerator.RandomGenerator randomGenerator = new ByteGenerator.RandomGenerator();

				int randomCounter = 0;
				int falsePositiveCorrect = 0;
				int falsePositiveIncorrect = 0;
				while (randomCounter < max)
				{
					string nextRandom = randomGenerator.GetNext();
					if (!lines.Contains(nextRandom))
					{
						filterAddTimer.Restart();
						filterContains = filter.Contains(nextRandom);
						filterAddTimeTotal = filterAddTimeTotal.Add(filterAddTimer.Elapsed);

						if (filterContains)
						{
							falsePositiveIncorrect += 1;
						}
						else
						{
							falsePositiveCorrect += 1;
						}

						randomCounter++;
					}
				}

				totalRuntimeTotal = totalRuntimeTimer.Elapsed;

				int totalCounter = counter + randomCounter;
				long ticks = filterAddTimeTotal.Ticks / totalCounter;
				averageAddTime = new TimeSpan(ticks);

				double membershipRate = (double)membershipCorrect / (double)membershipIncorrect;
				double falsePositiveRate = (double)falsePositiveCorrect / (double)falsePositiveIncorrect;

				if (double.IsInfinity(membershipRate))
				{
					membershipRate = 100.0d;
				}
				if (double.IsInfinity(falsePositiveRate))
				{
					falsePositiveRate = 100.0d;
				}

				string correctnessReport = string.Format("Membership Correct/Incorrect: {0}/{1} ({2:0.00}%); False Positives: {3}/{4} ({5:0.00}%)", membershipCorrect, membershipIncorrect, membershipRate, falsePositiveCorrect, falsePositiveIncorrect, falsePositiveRate);

				string timersReport =
					$"Total runtime: {FormHelper.FormatTimeSpan(totalRuntimeTotal)} (Contains elements: {FormHelper.FormatTimeSpan(filterAddTimeTotal)} ({FormHelper.FormatTimeSpan(averageAddTime)} avg.))";

				SetControlText(label5, correctnessReport);
				SetControlText(label6, timersReport);
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
					SetControlText(btnGenerate, buttonText_GenerateStop);
					generateHashesWorker.RunWorkerAsync();
				}
			}
		}

		#region BackgroundWorkers

		private void addHashesWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			TimeSpan readFileTimeTotal = TimeSpan.Zero;
			TimeSpan filterAddTimeTotal = TimeSpan.Zero;

			TimeSpan totalRuntimeTotal = TimeSpan.Zero;
			TimeSpan averageAddTime = TimeSpan.Zero;

			Stopwatch readFileTimer = Stopwatch.StartNew();
			Stopwatch filterAddTimer = Stopwatch.StartNew();
			Stopwatch totalRuntimeTimer = Stopwatch.StartNew();

			int counter = 0;
			readFileTimer.Restart();
			string[] lines = File.ReadAllLines((string)e.Argument);
			readFileTimeTotal = readFileTimer.Elapsed;
			totalRuntimeTimer.Restart();
			foreach (string line in lines)
			{
				if (addHashesWorker.CancellationPending)
				{
					e.Cancel = true;
					break;
				}

				filterAddTimer.Restart();
				filter.Add(line);
				filterAddTimeTotal = filterAddTimeTotal.Add(filterAddTimer.Elapsed);

				/*
				if (counter % 1000 == 0)
				{
					refreshControlsTimer.Restart();
					RefreshControls();
					refreshControlsTimeTotal = refreshControlsTimeTotal.Add(refreshControlsTimer.Elapsed);
				}
				*/

				counter++;
			}
			totalRuntimeTotal = totalRuntimeTimer.Elapsed;

			long ticks = filterAddTimeTotal.Ticks / counter;
			averageAddTime = new TimeSpan(ticks);

			string timersReport =
			$"Total runtime: {FormHelper.FormatTimeSpan(totalRuntimeTotal)} (File read: {FormHelper.FormatTimeSpan(readFileTimeTotal)}, Add elmnts: {FormHelper.FormatTimeSpan(filterAddTimeTotal)} ttl., {FormHelper.FormatTimeSpan(averageAddTime)} avg.)";

			e.Result = timersReport;
		}

		private void addHashesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			string timersReport = (string)e.Result;

			RefreshControls();
			SetControlText(btnAddHashesFromFile, buttonText_AddHashes);
			SetControlText(label6, timersReport);
		}


		void generateHashesWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			ByteGenerator.RandomGenerator sequenceGenerator = new ByteGenerator.RandomGenerator();

			int counter = 0;
			while (!generateHashesWorker.CancellationPending)
			{
				filter.Add(sequenceGenerator.GetNext());

				/*
				if (counter % 1000 == 0)
				{
					RefreshControls();
				}
				*/
				counter++;
			}

			e.Result = sequenceGenerator.Size;
		}

		void generateHashesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			RefreshControls();
			SetControlText(btnGenerate, buttonText_GenerateHash);
		}

		#endregion

		#endregion

		#region Open & Save

		private void btnOpenFilter_Click(object sender, EventArgs e)
		{
			string file = FormHelper.OpenFileDlg();
			if (!string.IsNullOrWhiteSpace(file))
			{
				filter = BloomFilterSerializer.Load(file, cbCompress.Checked);
				tbMaxElementsToHash.Text = filter.MaxElementsToHash.ToString();
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
				BloomFilterSerializer.Save(filter, file, cbCompress.Checked);
			}
		}

		#endregion

		#region Refresh Methods

		private void RefreshControls()
		{
			if (filter == null) { return; }
			if (this.InvokeRequired) { this.Invoke(new MethodInvoker(() => RefreshControls())); }
			else
			{
				label1.Text = string.Format("{0:n0} bits", filter.FilterSizeInBits);
				label2.Text = FormHelper.FormatFilesize(filter.FilterSizeInBits);
				label3.Text = string.Concat(filter.ElementsHashed.ToString(), " Elements hashed");
				label4.Text = filter.ToString();

				if (IsFilterOpen)
				{
					pictureBoxFilter.Image = ToBitmap(filter.FilterArray);
				}
			}
		}

		private void RefreshLabel()
		{
			if (this.InvokeRequired) { this.Invoke(new MethodInvoker(() => RefreshLabel())); }
			else
			{
				label3.Text = string.Concat(filter.ElementsHashed.ToString(), " Elements hashed");
			}
		}

		private void SetControlText(Control control, string text)
		{
			if (control.InvokeRequired) { control.Invoke(new MethodInvoker(() => SetControlText(control, text))); }
			else
			{
				control.Text = text;
			}
		}

		#endregion

		private void toolStripSaveImage_Click(object sender, EventArgs e)
		{
			if (pictureBoxFilter.Image == null)
			{
				return;
			}

			string filename = FormHelper.SaveFileDlg("Bitmap files (*.bmp)|*.bmp|All files (*.*)|*.*");
			if (!string.IsNullOrWhiteSpace(filename))
			{
				pictureBoxFilter.Image.Save(filename, ImageFormat.Bmp);
			}
		}

		private void contextMenuPictureBox_Opening(object sender, CancelEventArgs e)
		{
			if (pictureBoxFilter.Image == null)
			{
				e.Cancel = true;
			}
		}
	}
}
