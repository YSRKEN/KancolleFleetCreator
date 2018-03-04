using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;

namespace KFC
{	
	public class MainForm : Form
	{	
		public MainForm()
		{
			XamlReader.Load(this);
			this.Menu.ApplicationMenu.Text = "ファイル(&F)";
			this.Menu.QuitItem.Text = "終了(&X)";
			this.Menu.HelpMenu.Text = "ヘルプ(&H)";
			this.Menu.AboutItem.Text = "バージョン情報(&V)";
		}

		protected void HandleClickMe(object sender, EventArgs e)
		{
			MessageBox.Show("I was clicked!");
		}

		protected void HandleAbout(object sender, EventArgs e)
		{
			new AboutDialog().ShowDialog(this);
		}

		protected void HandleQuit(object sender, EventArgs e)
		{
			Application.Instance.Quit();
		}
	}
}
