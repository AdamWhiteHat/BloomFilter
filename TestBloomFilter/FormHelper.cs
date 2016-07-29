using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestBloomFilter
{
	public class FormHelper
	{
		#region File Dialogs

		public static string OpenFileDlg()
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

		public static string SaveFileDlg()
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



	}
}
