using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATKindleClippingsManager.Model
{
	public class KindleClipping
	{
		public String BookTitle { get; set; }
		public String BookAuthor { get; set; }
		public int PageLocation { get; set; }
		public int StartingLocation { get; set; }
		public int EndingLocation { get; set; }
		public DateTime DateAdded { get; set; }
		public String TextHighlighted { get; set; }
		public String Note { get; set; }
		public TypeOfClipping Type { get; set; }
		public enum TypeOfClipping
		{
			Highlight,
			Note
		}
	}
}
