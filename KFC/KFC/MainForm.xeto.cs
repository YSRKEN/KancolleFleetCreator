using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;
using System.Linq;

namespace KFC
{
	public delegate string GetFileName();
	public class MainForm : Form
	{
		public MainForm()
		{
			XamlReader.Load(this);
			this.DataContext = new MainViewModel(GetFileNameFromOpenDialog, GetFileNameFromSaveDialog);
			this.Menu.ApplicationMenu.Text = "ファイル(&F)";
			this.Menu.QuitItem.Text = "終了(&X)";
			this.Menu.HelpMenu.Text = "ヘルプ(&H)";
			this.Menu.AboutItem.Text = "バージョン情報(&V)";
		}

		protected void CreateFormationPicture(object sender, EventArgs e)
			=> (this.DataContext as MainViewModel).CreateFormationPictureCommand.Execute();
		protected void DeleteDataAll(object sender, EventArgs e)
			=> (this.DataContext as MainViewModel).DeleteDataAllCommand.Execute();
		protected void DeletePiecePicture1(object sender, EventArgs e)
			=> (this.DataContext as MainViewModel).DeletePiecePicture1Command.Execute();
		protected void DeletePiecePicture2(object sender, EventArgs e)
			=> (this.DataContext as MainViewModel).DeletePiecePicture2Command.Execute();
		protected void DeletePiecePicture3(object sender, EventArgs e)
			=> (this.DataContext as MainViewModel).DeletePiecePicture3Command.Execute();
		protected void DropFiles1(object sender, DragEventArgs e) {
			(this.DataContext as MainViewModel).DropPiecePicture(e.Data.Uris, Utility.PiecePictureType.Main);
		}
		protected void DropFiles2(object sender, DragEventArgs e) {
			(this.DataContext as MainViewModel).DropPiecePicture(e.Data.Uris, Utility.PiecePictureType.Support);
		}
		protected void DropFiles3(object sender, DragEventArgs e) {
			(this.DataContext as MainViewModel).DropPiecePicture(e.Data.Uris, Utility.PiecePictureType.Base);
		}
		protected void HandleAbout(object sender, EventArgs e) {
			var dialog = new AboutDialog() {
				Title = "バージョン情報",
				ProgramName = "艦これ編成まとめ隊", License = "MIT License",
				Website = new Uri("https://github.com/YSRKEN/KancolleFleetCreator"), WebsiteLabel = "GitHub",
			};
			dialog.ShowDialog(this);
		}
		protected void HandleQuit(object sender, EventArgs e)
			=> Application.Instance.Quit();

		private string GetFileNameFromOpenDialog() {
			using (var dialog = new OpenFileDialog()) {
				if (dialog.ShowDialog(this) != DialogResult.Ok)
					return "";
				return dialog.FileName;
			}
		}
		private string GetFileNameFromSaveDialog() {
			using (var dialog = new SaveFileDialog()) {
				if (dialog.ShowDialog(this) != DialogResult.Ok)
					return "";
				return dialog.FileName;
			}
		}
	}
}
