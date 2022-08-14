using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Numerics;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Collections.Generic;

using BloomFilterCore;
using BloomFilterCore.Utilities;
using BloomFilterCore.Serializers;

namespace UnitTestBloomFilter
{
	public partial class MainForm : Form
	{
		private CryptoRandom rand;
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
		//private static string defaultMaxElements;
		//private static string defaultHashesPerElement;
		//private static string defaultErrorProbability;

		public MainForm()
		{
			InitializeComponent();

			rand = new CryptoRandom();

			//defaultMaxElements = tbMaxElementsToHash.Text;
			//defaultHashesPerElement = tbHashesPerElement.Text;
			//defaultErrorProbability = tbErrorProbability.Text;

			addHashesWorker = new BackgroundWorker();
			addHashesWorker.WorkerSupportsCancellation = true;
			addHashesWorker.DoWork += addHashesWorker_DoWork;
			addHashesWorker.RunWorkerCompleted += addHashesWorker_RunWorkerCompleted;

			generateHashesWorker = new BackgroundWorker();
			generateHashesWorker.WorkerSupportsCancellation = true;
			generateHashesWorker.DoWork += generateHashesWorker_DoWork;
			generateHashesWorker.RunWorkerCompleted += generateHashesWorker_RunWorkerCompleted;

			panelWorkingAnimation.Visible = false;
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

			string filename = FormHelper.SaveFileDlg(FormHelper.FiletypeFilters.SequenceFiles);
			if (!string.IsNullOrWhiteSpace(filename))
			{
				int randomNumber = rand.Next(50000);

				BigInteger randomIncrementValue = PrimeHelper.GetPreviousPrime(randomNumber);

				ByteGenerator.SequenceGenerator randomSequence = new ByteGenerator.SequenceGenerator(8, randomNumber, (int)randomIncrementValue);

				int counter = 0;
				int maxElements = filter.MaxElementsToHash / 2;
				List<string> elementsCollection = new List<string>();
				while (counter < maxElements)
				{
					string nextElement = randomSequence.GetNext();
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

				string filename = FormHelper.OpenFileDlg(FormHelper.FiletypeFilters.SequenceFiles);
				if (!string.IsNullOrWhiteSpace(filename))
				{
					btnAddHashesFromFile.Text = buttonText_Cancel;

					addHashesWorker.RunWorkerAsync(filename);
				}
			}
		}

		private void btnTestHashesFromFile_Click(object sender, EventArgs e)
		{
			string filename = FormHelper.OpenFileDlg(FormHelper.FiletypeFilters.SequenceFiles);
			if (string.IsNullOrWhiteSpace(filename))
			{
				return;
			}

			ShowWorkingAnimation();

			var testHashingElements = Task.Factory.StartNew(() =>
			{
				TimeSpan filterAddTimeTotal = TimeSpan.Zero;

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
					filterContains = filter.ContainsElement(lines[counter]);
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

				int randomNumber = rand.Next(50000);

				ByteGenerator.SequenceGenerator randomGenerator = new ByteGenerator.SequenceGenerator(8, counter, randomNumber);

				int randomCounter = 0;
				int falsePositiveCorrect = 0;
				int falsePositiveIncorrect = 0;
				while (randomCounter < max)
				{
					string nextRandom = randomGenerator.GetNext();
					if (!lines.Contains(nextRandom))
					{
						filterAddTimer.Restart();
						filterContains = filter.ContainsElement(nextRandom);
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

				TimeSpan totalRuntimeTotal = totalRuntimeTimer.Elapsed;

				int totalCounter = counter + randomCounter;
				long ticks = filterAddTimeTotal.Ticks / totalCounter;
				TimeSpan averageAddTime = new TimeSpan(ticks);

				double membershipRate = (double)membershipCorrect / (double)membershipIncorrect;
				double falsePositiveRate = (double)falsePositiveIncorrect / (double)falsePositiveCorrect;

				if (double.IsInfinity(membershipRate))
				{
					membershipRate = 100.0d;
				}
				if (double.IsInfinity(falsePositiveRate))
				{
					falsePositiveRate = 100.0d;
				}

				string correctnessReport = string.Format("Membership Correct/Incorrect: {0}/{1} ({2:0.00}%); False Positives: {3}/{4} ({5:0.00}%)", membershipCorrect, membershipIncorrect, membershipRate, falsePositiveIncorrect, falsePositiveCorrect, falsePositiveRate);

				string timersReport =
					$"Total runtime: {FormHelper.FormatTimeSpan(totalRuntimeTotal)} (Contains elements: {FormHelper.FormatTimeSpan(filterAddTimeTotal)} ({FormHelper.FormatTimeSpan(averageAddTime)} avg.))";

				SetControlText(label5, correctnessReport);
				SetControlText(label6, timersReport);
			});

			var hideLoadingAnimation = testHashingElements.ContinueWith((task) => HideWorkingAnimation());
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
			if (!IsFilterOpen)
			{
				return;
			}
			if (filter == null || generateHashesWorker == null)
			{
				throw new ArgumentNullException();
			}

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

		#region BackgroundWorkers

		private void addHashesWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			ShowWorkingAnimation();

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
				filter.AddElement(line);
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
			ShowWorkingAnimation();

			ByteGenerator.RandomGenerator sequenceGenerator = new ByteGenerator.RandomGenerator();

			int counter = 0;
			while (!generateHashesWorker.CancellationPending)
			{
				filter.AddElement(sequenceGenerator.GetNext());

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

		#region Open & Save Bloom Filter

		private void btnOpenFilter_Click(object sender, EventArgs e)
		{
			string file = FormHelper.OpenFileDlg(FormHelper.FiletypeFilters.BloomFiles);
			if (!string.IsNullOrWhiteSpace(file))
			{
				filter = BinarySerializer.Load(file, cbCompress.Checked);
				tbMaxElementsToHash.Text = filter.MaxElementsToHash.ToString();
				tbHashesPerElement.Text = filter.HashesPerElement.ToString();
				tbErrorProbability.Text = filter.ErrorProbability.ToString();
				SetLoadedStatus(true);
				RefreshControls();
			}
		}

		private void btnSaveFilter_Click(object sender, EventArgs e)
		{
			string file = FormHelper.SaveFileDlg(FormHelper.FiletypeFilters.BloomFiles);
			if (!string.IsNullOrWhiteSpace(file))
			{
				BinarySerializer.Save(filter, file, cbCompress.Checked);
			}
		}

		#endregion

		#region Refresh Methods

		private void ShowWorkingAnimation()
		{
			SetControlVisibility(panelWorkingAnimation, true);
			SetControlVisibility(pictureBoxFilter, false);
		}

		private void HideWorkingAnimation()
		{
			SetControlVisibility(pictureBoxFilter, true);
			SetControlVisibility(panelWorkingAnimation, false);
		}

		private void RefreshControls()
		{
			if (filter == null) { return; }

			if (IsFilterOpen)
			{
				var paintBitmap = Task.Factory.StartNew(() => (Image)ToBitmap(filter.FilterArray));
				var setPictureBox = paintBitmap.ContinueWith((image) => { SetPictureBoxImage(pictureBoxFilter, image.Result); HideWorkingAnimation(); });
			}

			SetControlText(label1, string.Format("{0:n0} bits", filter.FilterSizeInBits));
			SetControlText(label2, FormHelper.FormatFilesize(filter.FilterSizeInBits));
			SetControlText(label3, string.Concat(filter.ElementsHashed.ToString(), " Elements hashed"));
			SetControlText(label4, filter.ToString());
		}

		private void SetPictureBoxImage(PictureBox control, Image image)
		{
			if (this.InvokeRequired) { this.Invoke(new MethodInvoker(() => SetPictureBoxImage(control, image))); }
			else
			{
				control.Image = image;
			}
		}

		private void SetControlText(Control control, string text)
		{
			if (this.InvokeRequired) { this.Invoke(new MethodInvoker(() => SetControlText(control, text))); }
			else
			{
				control.Text = text;
			}
		}

		private void SetControlVisibility(Control control, bool visible)
		{
			if (this.InvokeRequired) { this.Invoke(new MethodInvoker(() => SetControlVisibility(control, visible))); }
			else
			{
				control.Visible = visible;
			}
		}

		#endregion

		private void toolStripSaveImage_Click(object sender, EventArgs e)
		{
			if (pictureBoxFilter.Image == null)
			{
				return;
			}

			string filename = FormHelper.SaveFileDlg(FormHelper.FiletypeFilters.BitmapFiles);
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
