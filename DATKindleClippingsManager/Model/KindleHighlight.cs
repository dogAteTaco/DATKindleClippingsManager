using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATKindleClippingsManager.Model
{
    public class KindleHighlight : KindleClipping
    {
		public int StartingLocation { get; set; }
		public int EndingLocation { get; set; }
		public KindleNote Note { get; set; }

		public KindleHighlight() : base()
		{
			Note = new KindleNote();
			
		}
	}
}
