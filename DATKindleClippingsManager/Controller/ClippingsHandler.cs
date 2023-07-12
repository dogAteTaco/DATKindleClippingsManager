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
using System.Windows;

namespace DATKindleClippingsManager.Controller
{
	public class ClippingsHandler
	{
		public List<KindleHighlight> highlights = new List<KindleHighlight>();
		public List<KindleBookmark> bookmarks = new List<KindleBookmark>();
		public void GetClippings()
		{
			highlights = new List<KindleHighlight>();
			bookmarks = new List<KindleBookmark>();

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
			rawClippings = rawClippings.ToArray();
			
			foreach (String raw in rawClippings)
			{
				string rawClipping = raw;
				var highlight = new KindleHighlight();
				var note = new KindleNote();
				var bookmark = new KindleBookmark();
				string pageText;
				string locationString;
				string titleString;
				string dateString;
				string format = "dddd, MMMM d, yyyy h:mm:ss tt";
				int startIndex;
				int endIndex;
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
							startIndex = rawClipping.IndexOf("- Your Highlight on page ") + 25;
							pageText = rawClipping.Substring(startIndex, rawClipping.IndexOf(" | ") - startIndex);
							highlight.Page = Convert.ToInt32(pageText);

							startIndex = rawClipping.IndexOf(" | Location ") + 12;
							locationString = rawClipping.Substring(startIndex, rawClipping.IndexOf(" | Added on") - startIndex);
							highlight.StartingLocation = Convert.ToInt32(locationString.Substring(0, locationString.IndexOf("-")));
							startIndex = locationString.IndexOf('-') + 1;
							highlight.EndingLocation = Convert.ToInt32(locationString.Substring(startIndex, locationString.Length - startIndex));

							highlight.Page = Convert.ToInt32(pageText);
							titleString = rawClipping.Substring(0, rawClipping.IndexOf("\r\n- Your "));
							endIndex = titleString.LastIndexOf("(");
							highlight.Title = titleString.Substring(0, endIndex);
							highlight.Author = titleString.Substring(endIndex + 1, titleString.Length - endIndex - 2);

							dateString = rawClipping.Substring(rawClipping.IndexOf("| Added on ") + 11);
							dateString = dateString.Substring(0, dateString.IndexOf("\r"));

							highlight.DateAdded = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);

							highlight.Text = rawClipping.Substring(rawClipping.IndexOf("\r\n") + 2);
							highlight.Text = highlight.Text.Substring(highlight.Text.IndexOf("\r\n\r\n") + 4);
							highlight.Text = highlight.Text.Substring(0, highlight.Text.LastIndexOf("\r\n"));
						}
						else if (rawClipping.Contains("- Your Note on page "))
						{
							//For Notes
							startIndex = rawClipping.IndexOf("- Your Note on page ") + 20;
							pageText = rawClipping.Substring(startIndex, rawClipping.IndexOf(" | ") - startIndex);
							note.Page = Convert.ToInt32(pageText);

							startIndex = rawClipping.IndexOf(" | Location ") + 12;
							locationString = rawClipping.Substring(startIndex, rawClipping.IndexOf(" | Added on") - startIndex);
							note.Location = Convert.ToInt32(locationString);

							
							titleString = rawClipping.Substring(0, rawClipping.IndexOf("\r\n- Your "));
							endIndex = titleString.LastIndexOf("(");
							note.Title = titleString.Substring(0, endIndex);
							note.Author = titleString.Substring(endIndex + 1, titleString.Length - endIndex - 2);

							dateString = rawClipping.Substring(rawClipping.IndexOf("| Added on ") + 11);
							dateString = dateString.Substring(0, dateString.IndexOf("\r"));
							note.DateAdded = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);

							note.Text = rawClipping.Substring(rawClipping.IndexOf("\r\n") + 2);
							note.Text = note.Text.Substring(note.Text.IndexOf("\r\n\r\n") + 4);
							note.Text = note.Text.Substring(0, note.Text.LastIndexOf("\r\n"));
						}
						else
						{
							startIndex = rawClipping.IndexOf("- Your Bookmark on page ") + 24;
							pageText = rawClipping.Substring(startIndex, rawClipping.IndexOf(" | ") - startIndex);
							bookmark.Page = Convert.ToInt32(pageText);

							startIndex = rawClipping.IndexOf(" | Location ") + 12;
							locationString = rawClipping.Substring(startIndex, rawClipping.IndexOf(" | Added on") - startIndex);
							bookmark.Location = Convert.ToInt32(locationString);

							
							titleString = rawClipping.Substring(0, rawClipping.IndexOf("\r\n- Your "));
							endIndex = titleString.LastIndexOf("(");
							bookmark.Title = titleString.Substring(0, endIndex);
							bookmark.Author = titleString.Substring(endIndex + 1, titleString.Length - endIndex - 2);

							dateString = rawClipping.Substring(rawClipping.IndexOf("| Added on ") + 11);
							dateString = dateString.Substring(0, dateString.IndexOf("\r"));
							bookmark.DateAdded = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);

							this.bookmarks.Add(bookmark);
						}
						//If the Clipping is a Note it adds it to the Highlighted text it belongs to instead of adding a new Clipping to the list
						if (note.Title!=null)
						{
							var clipHighligh = highlights.Where(c => c.Title==note.Title&&(c.StartingLocation <= note.Location && c.EndingLocation >= note.Location)).First();
							clipHighligh.Note = note;
						}
						else if(bookmark.Title==null)
							this.highlights.Add(highlight);
					}
					catch (ArgumentOutOfRangeException ex)
					{
						Console.WriteLine(rawClipping+':'+ex.ToString());
					}
				}
			}
		}

		public static void SaveClippings(string fileToReplace, List<KindleHighlight> highlights,List<KindleNote> notes, List<KindleBookmark> bookmarks)
		{
			string tempString = "";
			string filename = "My Clippings.txt";
			File.WriteAllText(filename,string.Empty);
			foreach (var highlight in highlights)
			{
				tempString = "";
				tempString += $"{highlight.Title}({highlight.Author})\r\n";
				tempString += $"- Your Highlight on page {highlight.Page} | Location {highlight.StartingLocation}-{highlight.EndingLocation} | Added on {highlight.DateAdded.ToString("dddd, MMMM d, yyyy h:mm:ss tt")}\r\n\r\n";
				tempString += $"{highlight.Text}\r\n==========\r\n";
				using (StreamWriter streamWriter = new StreamWriter(filename, true, Encoding.UTF8))
				{
					streamWriter.Write(tempString);
				}

				if (highlight.Note != null)
				{
					if (highlight.Note.Text.Trim() != "")
					{
						if (highlight.Note.Title == null)
						{
							highlight.Note.Location = highlight.EndingLocation;
							highlight.Note.DateAdded = DateTime.Now;
							highlight.Note.Page = highlight.Page;
							highlight.Note.Title = highlight.Title;
							highlight.Note.Author = highlight.Author;
						}
						tempString = "";
						tempString += $"{highlight.Note.Title}({highlight.Note.Author})\r\n";
						tempString += $"- Your Note on page {highlight.Note.Page} | Location {highlight.Note.Location} | Added on {highlight.Note.DateAdded.ToString("dddd, MMMM d, yyyy h:mm:ss tt")}\r\n\r\n";
						tempString += $"{highlight.Note.Text}\r\n==========\r\n";
						using (StreamWriter streamWriter = new StreamWriter(filename, true, Encoding.UTF8))
						{
							streamWriter.Write(tempString);
						}
					}	
				}
			}

			foreach (var bookmark in bookmarks)
			{
				tempString = "";
				tempString += $"{bookmark.Title}({bookmark.Author})\r\n";
				tempString += $"- Your Bookmark on page {bookmark.Page} | Location {bookmark.Location} | Added on {bookmark.DateAdded.ToString("dddd, MMMM d, yyyy h:mm:ss tt")}\r\n\r\n";
				tempString += $"\r\n==========\r\n";
				using (StreamWriter streamWriter = new StreamWriter(filename, true, Encoding.UTF8))
				{
					streamWriter.Write(tempString);
				}
			}
			try
			{
				File.Move(filename, fileToReplace, true);
			}
			catch (DirectoryNotFoundException dirEx)
			{
				throw dirEx;
			}
			catch (IOException IOE)
			{
				throw IOE;
			}
		}

	}
}
