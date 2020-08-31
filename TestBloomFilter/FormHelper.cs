using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnitTestBloomFilter
{
	public class FormHelper
	{
		#region File Dialogs

		public static string OpenFileDlg()
		{
			return OpenFileDlg(string.Empty);
		}

		public static string OpenFileDlg(string filter)
		{
			string result = string.Empty;
			using (OpenFileDialog openDlg = new OpenFileDialog())
			{
				if (!string.IsNullOrWhiteSpace(filter))
				{
					openDlg.Filter = filter;
				}
				if (openDlg.ShowDialog() == DialogResult.OK)
				{
					result = openDlg.FileName;
				}
			}
			return result;
		}

		public static string SaveFileDlg()
		{
			return SaveFileDlg(string.Empty);
		}

		public static string SaveFileDlg(string filter)
		{
			string result = string.Empty;
			using (SaveFileDialog saveDlg = new SaveFileDialog())
			{
				if (!string.IsNullOrWhiteSpace(filter))
				{
					saveDlg.Filter = filter;
				}
				if (saveDlg.ShowDialog() == DialogResult.OK)
				{

					result = saveDlg.FileName;
				}
			}
			return result;
		}

		#endregion

		public static string FormatFilesize(int sizeBits)
		{
			decimal size = sizeBits;
			string denomination = "bits";

			if (size > 8)
			{
				size = size / 8;
				denomination = "Bytes";
				if (size > 1024)
				{
					size = size / 1024;
					denomination = "KB";
					if (size > 1024)
					{
						size = size / 1024;
						denomination = "MB";
						if (size > 1024)
						{
							size = size / 1024;
							denomination = "GB";
						}
					}
				}
			}
			return string.Format("{0:#,##0.##} {1}", size, denomination);
		}

		public static string FormatTimeSpan(TimeSpan timeSpan)
		{
			if (timeSpan.TotalMilliseconds == 0) { return "0.0 ms"; }
			if (timeSpan.TotalMinutes < 1) { return timeSpan.ToString(@"ss\.FFFFFFF"); }
			List<string> parts = new List<string>();
			if (timeSpan.Days > 0) { parts.Add($"{timeSpan.Days}d"); }
			if (timeSpan.Hours > 0) { parts.Add($"{timeSpan.Hours}h"); }
			if (timeSpan.Minutes > 0) { parts.Add($"{timeSpan.Minutes}m"); }
			if (timeSpan.Seconds > 0) { parts.Add($"{timeSpan.Seconds}s"); }
			if (timeSpan.Milliseconds > 0) { parts.Add($"{timeSpan.ToString("FFFFFFF")}ms"); }
			else if (timeSpan.TotalMilliseconds < 1) { parts.Add($"{timeSpan.TotalMilliseconds}ms"); }
			return string.Join(" ", parts);
		}
	}
}
