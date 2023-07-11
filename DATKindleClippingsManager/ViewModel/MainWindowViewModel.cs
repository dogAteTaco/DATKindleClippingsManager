using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DATKindleClippingsManager.Controller;
using DATKindleClippingsManager.Model;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DATKindleClippingsManager.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
	{
        public ObservableCollection<KindleClipping> Clippings { get; set; }
		public ObservableCollection<KindleClipping> FilteredClippings { get; set; }	
		public ObservableCollection<Book> Books { get; set; }


		[ObservableProperty]
		public Book selectedBook;

		[ObservableProperty]
		private string clippingsFile;
		public MainWindowViewModel()
		{
			this.Clippings = new ObservableCollection<KindleClipping>();
			this.Books = new ObservableCollection<Book>();
			this.FilteredClippings = new ObservableCollection<KindleClipping>();
			this.SelectedBook = new Book();
			this.ClippingsFile = Properties.Settings.Default.ClippingsLocation;
			if (File.Exists(this.ClippingsFile))
				this.LoadClippings();
			else
				MessageBox.Show("The 'My Clippings.txt' file couldn't be found. Check that your Kindle is connected or it's on the right location.");
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
				foreach (var clipping in ClippingsReader.GetClippings())
				{
					Clippings.Add(clipping);
				}
				this.Books.Clear();
				foreach (var clipping in this.Clippings)
					if (this.Books.Where(c => c.Title.Equals(clipping.BookTitle)
						&& c.Author.Equals(clipping.BookAuthor)).Count() == 0)
						this.Books.Add(new Book(clipping.BookTitle, clipping.BookAuthor));
			}
			catch (DirectoryNotFoundException notFound)
			{
				//MessageBox.Show($"The MyClippings.txt file couldn't be found. Check that the directory is correct.\n\n{notFound.Message}");
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
			foreach(var clipping in this.Clippings.Where(c => c.BookTitle == this.SelectedBook.Title
				&& c.BookAuthor == this.SelectedBook.Author))
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
			File.Copy(this.ClippingsFile,backUpFolder+"\\My Clippings.txt");
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
	}
}
