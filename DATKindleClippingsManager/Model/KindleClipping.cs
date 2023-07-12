using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATKindleClippingsManager.Model
{
	public class KindleClipping
	{
		public String Title { get; set; }
		public String Author { get; set; }
		public int Page { get; set; }
		public DateTime DateAdded { get; set; }
		public String Text { get; set; }

		public KindleClipping()
		{
			this.Text = "";
		}
	}
}
