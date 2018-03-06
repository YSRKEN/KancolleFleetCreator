using System;
using Eto.Forms;
using Eto.Drawing;
using System.Threading;
using System.Windows.Threading;

namespace KFC.Desktop
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			var app = new Application(Eto.Platform.Detect);
			if (app.Platform.IsWpf) {
				SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());
			}
			app.Run(new MainForm());
		}
	}
}