using System;

using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DATKindleClippingsManager.View
{
    /// <summary>
    /// Interaction logic for DetailedErrorView.xaml
    /// </summary>
    public partial class DetailedErrorView : Window
    {
		public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(DetailedErrorView));
		public static readonly DependencyProperty DetailsProperty = DependencyProperty.Register("Details", typeof(string), typeof(DetailedErrorView));
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(DetailedErrorView));

		public string Message
		{
			get { return (string)GetValue(MessageProperty); }
			set { SetValue(MessageProperty, value); }
		}

		public string Details
		{
			get { return (string)GetValue(DetailsProperty); }
			set { SetValue(DetailsProperty, value); }
		}

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}
		public DetailedErrorView(string message, string title, string details)
		{
			InitializeComponent();
			Message = message;
			Details = details;
			Title = title;
			DataContext = this;
		}

		private void Expands(object sender, RoutedEventArgs e)
		{
			this.Height = 400;	
		}

		private void Collaps(object sender, RoutedEventArgs e)
		{
			this.Height = 170;
		}
	}
}
