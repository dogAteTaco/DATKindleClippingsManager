using DATKindleClippingsManager.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DATKindleClippingsManager
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		MainWindowViewModel model = new MainWindowViewModel();
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = model;
			this.WaitForUSB();
		}

		private void WaitForUSB()
		{
			var watcher = new ManagementEventWatcher();
			var query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
			watcher.EventArrived += (sender, eventArgs) =>
			{

				Application.Current.Dispatcher.Invoke(() =>
				{
					if (!Properties.Settings.Default.ClippingsLocation.Equals(""))
					{
						Thread.Sleep(400);
						DriveInfo[] allDrives = DriveInfo.GetDrives();
						string drive = Properties.Settings.Default.ClippingsLocation.Substring(0, Properties.Settings.Default.ClippingsLocation.IndexOf(":"));
						foreach (DriveInfo d in allDrives)
						{
							if (d.IsReady && d.Name == $"{drive}:\\")
							{
								model.LoadClippings();
							}
						}
					}
				});
					
			};
			watcher.Query = query;
			watcher.Start();
		}

		private void NoteTextChanged(object sender, TextChangedEventArgs e)
		{
			model.Changes = true;
		}
	}
}
