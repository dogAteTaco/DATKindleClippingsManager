using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DATKindleClippingsManager.Model
{
    public partial class Book : ObservableObject
	{
		[ObservableProperty]
		public String title;
		[ObservableProperty]
		public String author;
		[ObservableProperty]
		public String coverLocation;
		public Book(string title, string author)
		{
			Title = title;
			Author = author;
			string location = Directory.GetCurrentDirectory() + "\\covers\\" + this.Title.Trim().Replace(":", "-") + " by " + this.Author;
			if (File.Exists(location + ".jpg"))
				location = location + ".jpg";
			else if (File.Exists(location + ".jpeg"))
				location = location + ".jpeg";
			else if (File.Exists(location + ".png"))
				location = location + ".png";
			else
				location = "";
			this.CoverLocation = location;

		}

		public Book()
		{
			this.Title = "";
			this.Author = "";
			this.CoverLocation = "";
		}
	}
}
