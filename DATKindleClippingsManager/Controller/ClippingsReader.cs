using DATKindleClippingsManager.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DATKindleClippingsManager.Controller
{
	public class ClippingsReader
	{
		public static List<KindleClipping> GetClippings()
		{
			var clippings = new List<KindleClipping>();
			String completeText;
			try
			{
				completeText = File.ReadAllText(Properties.Settings.Default.ClippingsLocation);
			}
			catch (DirectoryNotFoundException notFound)
			{
				throw notFound;
			}
			
			String[] rawClippings = completeText.Split("==========");
			rawClippings = rawClippings.Where(s => !s.Contains("- Your Bookmark on page ")).ToArray();
			
			foreach (String raw in rawClippings)
			{
				string rawClipping = raw;
				var clipping = new KindleClipping();
				string pageText;
				string locationString;
				int startIndex;
				if (!rawClipping.Equals("\r\n"))
				{
					if(rawClipping.StartsWith("\r\n"))
						rawClipping = rawClipping.Substring(2);
					try
					{
						//Extracts the page number
						if (rawClipping.Contains("- Your Highlight on page "))
						{
							//For Highlights
							clipping.Type = KindleClipping.TypeOfClipping.Highlight;
							startIndex = rawClipping.IndexOf("- Your Highlight on page ") + 25;
							pageText = rawClipping.Substring(startIndex, rawClipping.IndexOf(" | ") - startIndex);
							
							startIndex = rawClipping.IndexOf(" | Location ")+12;
							locationString = rawClipping.Substring(startIndex, rawClipping.IndexOf(" | Added on") - startIndex);
							clipping.StartingLocation = Convert.ToInt32(locationString.Substring(0,locationString.IndexOf("-")));
							startIndex = locationString.IndexOf('-')+1;
							clipping.EndingLocation = Convert.ToInt32(locationString.Substring(startIndex,locationString.Length - startIndex));
						}
						else
						{
							//For Notes
							clipping.Type = KindleClipping.TypeOfClipping.Note;
							startIndex = rawClipping.IndexOf("- Your Note on page ") + 20;
							pageText = rawClipping.Substring(startIndex, rawClipping.IndexOf(" | ") - startIndex);
							
							startIndex = rawClipping.IndexOf(" | Location ")+12;
							locationString = rawClipping.Substring(startIndex, rawClipping.IndexOf(" | Added on") - startIndex);
							clipping.StartingLocation = Convert.ToInt32(locationString);
						}
						clipping.PageLocation = Convert.ToInt32(pageText);
						string titleString = rawClipping.Substring(0,rawClipping.IndexOf("\r\n- Your "));
						int endIndex = titleString.LastIndexOf("(");
						clipping.BookTitle = titleString.Substring(0,endIndex);
						clipping.BookAuthor = titleString.Substring(endIndex+1,titleString.Length-endIndex-2);

						string dateString = rawClipping.Substring(rawClipping.IndexOf("| Added on ")+11);
						dateString = dateString.Substring(0, dateString.IndexOf("\r"));
						string format = "dddd, MMMM d, yyyy h:mm:ss tt";
						clipping.DateAdded = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);

						clipping.TextHighlighted = rawClipping.Substring(rawClipping.IndexOf("\r\n")+2);
						clipping.TextHighlighted = clipping.TextHighlighted.Substring(clipping.TextHighlighted.IndexOf("\r\n\r\n") + 4);
						clipping.TextHighlighted = clipping.TextHighlighted.Substring(0, clipping.TextHighlighted.LastIndexOf("\r\n"));
						//If the Clipping is a Note it adds it to the Highlighted text it belongs to instead of adding a new Clipping to the list
						if (clipping.Type == KindleClipping.TypeOfClipping.Note)
						{
							var clipHighligh = clippings.Where(c => c.BookTitle==clipping.BookTitle&&(c.StartingLocation <= clipping.StartingLocation && c.EndingLocation >= clipping.StartingLocation)).First();
							clipHighligh.Note = clipping.TextHighlighted;
						}
						else
							clippings.Add(clipping);
					}
					catch (ArgumentOutOfRangeException ex)
					{
						Console.WriteLine(rawClipping);
					}
				}

			}
			return clippings;
		}

	}
}
