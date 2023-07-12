using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DATKindleClippingsManager.Controller;
using DATKindleClippingsManager.Model;
using DATKindleClippingsManager.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DATKindleClippingsManager.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
	{
        public ObservableCollection<KindleHighlight> Clippings { get; set; }
		public ObservableCollection<KindleHighlight> FilteredClippings { get; set; }	
		public ObservableCollection<Book> Books { get; set; }

		ClippingsHandler handler = new ClippingsHandler();
		
		[ObservableProperty]
		private Book selectedBook;

		[ObservableProperty]
		private string clippingsFile;

		[ObservableProperty]
		private Boolean changes;
		public MainWindowViewModel()
		{
			this.Clippings = new ObservableCollection<KindleHighlight>();
			this.Books = new ObservableCollection<Book>();
			this.FilteredClippings = new ObservableCollection<KindleHighlight>();
			this.SelectedBook = new Book();
			this.ClippingsFile = Properties.Settings.Default.ClippingsLocation;
			if (File.Exists(this.ClippingsFile))
				this.LoadClippings();
			else
				MessageBox.Show("The 'My Clippings.txt' file couldn't be found. Check that your Kindle is connected or it's on the right location.");

			this.Changes = false;
		}

		[RelayCommand]
		void SelectClippingsFile()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "My Clippings files (*.txt)|My Clippings.txt";
			openFileDialog.FilterIndex = 2;
			openFileDialog.RestoreDirectory = true;

			if (openFileDialog.ShowDialog() == true)
			{
				Properties.Settings.Default.ClippingsLocation = openFileDialog.FileName;
				this.ClippingsFile = Properties.Settings.Default.ClippingsLocation;
				Properties.Settings.Default.Save();
				this.LoadClippings();
			}
		}
		[RelayCommand]
		public void LoadClippings()
		{
			
			try
			{
				if (Properties.Settings.Default.ClippingsLocation.Equals(""))
					MessageBox.Show($"The MyClippings.txt file couldn't be found. Check that the directory is correct.");
				Clippings.Clear();
				
				handler.GetClippings();
				foreach (var clipping in handler.highlights)
				{
					Clippings.Add(clipping);
				}
				this.Books.Clear();
				foreach (var clipping in this.Clippings)
					if (this.Books.Where(c => c.Title.Equals(clipping.Title)
						&& c.Author.Equals(clipping.Author)).Count() == 0)
						this.Books.Add(new Book(clipping.Title, clipping.Author));
			}
			catch (DirectoryNotFoundException notFound)
			{
				this.ShowError("The MyClippings.txt file couldn't be found. Check that the directory is correct.","Couldn't load Clippings", notFound.Message);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		[RelayCommand]
		void BookSelected()
		{
			this.FilteredClippings.Clear();
			foreach(var clipping in this.Clippings.Where(c => c.Title == this.SelectedBook.Title
				&& c.Author == this.SelectedBook.Author))
				this.FilteredClippings.Add(clipping);
		}

		[RelayCommand]
		void PasteCover()
		{
			if (Clipboard.ContainsImage())
			{
				var image = Clipboard.GetImage();
				if (image != null)
				{
					string fileName = Directory.GetCurrentDirectory() + "\\" + $"covers\\{this.SelectedBook.Title.Replace(":", "-")}by {this.SelectedBook.Author}.png";
					try
					{
						using (FileStream stream = new FileStream(fileName, FileMode.Create))
						{
							PngBitmapEncoder encoder = new PngBitmapEncoder();
							encoder.Frames.Add(BitmapFrame.Create(image));
							encoder.Save(stream);
						}
						this.SelectedBook.CoverLocation = fileName;
					}
					catch (IOException ioEx)
					{
						MessageBox.Show("The cover couldn't be replaced. Try deleting the cover manually in the cover folder in the root folder.\n\n" + ioEx.Message);
					}
				}
			}
			else
				MessageBox.Show("Couldn't find an Image in the Clipboard");
		}

		[RelayCommand]
		void SelectCover()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
			dialog.Title = "Select an image file";

			if (dialog.ShowDialog() == true)
			{
				string cover = $"covers\\{this.SelectedBook.Title.Replace(":", "-")}by {this.SelectedBook.Author}{Path.GetExtension(dialog.FileName)}";
				File.Copy(dialog.FileName,cover,true);
				this.SelectedBook.CoverLocation = Directory.GetCurrentDirectory()+"\\"+cover;
			}
		}

		[RelayCommand]
		void BackUpClippings()
		{
			string backUpFolder = $"backup\\{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}";
			Directory.CreateDirectory(backUpFolder);
			try
			{
				File.Copy(this.ClippingsFile, backUpFolder + "\\My Clippings.txt");
				if (File.Exists(backUpFolder + "\\My Clippings.txt"))
					MessageBox.Show("The back up has been complete succesfully.");
			}
			catch (DirectoryNotFoundException dirEx)
			{
				this.ShowError("The back up couldn't be completed. Check that your Kindle is connected.","",dirEx.ToString());
			}
		}

		[RelayCommand]
		void OpenBackUps()
		{
			string folderPath = Directory.GetCurrentDirectory() + "\\" + $"backup";
			if (Directory.Exists(folderPath))
				Process.Start("explorer.exe", folderPath);
			else
				MessageBox.Show("No Back Ups have been made.");
		}

		[RelayCommand]
		void SaveClippings()
		{
			MessageBoxResult result = MessageBox.Show("Are you sure you want to overwrite your Clippings? (A back up will be made automatically)",
				"Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
			if (result == MessageBoxResult.Yes)
			{
				this.BackUpClippings();
				List<KindleHighlight> highlights = new List<KindleHighlight>();
				List<KindleNote> notes = new List<KindleNote>();
				List<KindleBookmark> bookmarks = handler.bookmarks;
				foreach (var highlight in this.Clippings)
				{
					highlights.Add(highlight);
					if (highlight.Note != null)
						notes.Add(highlight.Note);
				}
				try
				{
					ClippingsHandler.SaveClippings(this.ClippingsFile, highlights, notes, bookmarks);
					MessageBox.Show("The Clippings file has been saved into your Kindle device.", "Saved Clippings", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				catch (DirectoryNotFoundException dirEx)
				{
					this.ShowError("Couldn't save your Clippings into your device. Check that your Kindle device is connected and that the path is correct.", "Couldn't save Clippings", dirEx.ToString());
				}
				catch (IOException IOE)
				{
					this.ShowError("Couldn't save your Clippings into your device. Check that your Kindle device is connected and that the path is correct.", "Couldn't save Clippings", IOE.ToString());
				}
			}
		}

		void ShowError(String message, String title, String details)
		{
			DetailedErrorView detailedErrorView = new DetailedErrorView(message,title,details);
			detailedErrorView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			detailedErrorView.ShowDialog();
		}
	}
}
