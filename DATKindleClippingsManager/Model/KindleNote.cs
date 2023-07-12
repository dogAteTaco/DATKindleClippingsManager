using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATKindleClippingsManager.Model
{
    public class KindleNote : KindleClipping
    {
		public int Location { get; set; }

		public KindleNote() : base() {
			
		}
	}
}
