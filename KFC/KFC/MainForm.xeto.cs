using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;

namespace KFC
{
	public delegate string GetFileName();
	public class MainForm : Form
	{
		public MainForm()
		{
			XamlReader.Load(this);
			this.DataContext = new MainViewModel(GetFileNameFromDialog);
			this.Menu.ApplicationMenu.Text = "�t�@�C��(&F)";
			this.Menu.QuitItem.Text = "�I��(&X)";
			this.Menu.HelpMenu.Text = "�w���v(&H)";
			this.Menu.AboutItem.Text = "�o�[�W�������(&V)";
		}

		protected void LoadPiecePicture(object sender, EventArgs e) {
			using(var dialog = new OpenFileDialog()) {
				if (dialog.ShowDialog(this) != DialogResult.Ok)
					return;
				var image = new Bitmap(dialog.FileName);
				(this.DataContext as MainViewModel).SetPreViewImage(image);
			}
		}
		protected void CreateFormationPicture(object sender, EventArgs e)
			=> (this.DataContext as MainViewModel).CreateFormationPictureCommand.Execute();
		protected void DeleteDataAll(object sender, EventArgs e)
			=> (this.DataContext as MainViewModel).DeleteDataAllCommand.Execute();
		protected void HandleAbout(object sender, EventArgs e) {
			var dialog = new AboutDialog() {
				Title = "�o�[�W�������",
				ProgramName = "�͂���Ґ��܂Ƃߑ�", License = "MIT License",
				Website = new Uri("https://github.com/YSRKEN/"), WebsiteLabel = "GitHub"
			};
			dialog.ShowDialog(this);
		}
		protected void HandleQuit(object sender, EventArgs e)
			=> Application.Instance.Quit();

		private string GetFileNameFromDialog() {
			using (var dialog = new OpenFileDialog()) {
				if (dialog.ShowDialog(this) != DialogResult.Ok)
					return "";
				return dialog.FileName;
			}
		}
	}
}
