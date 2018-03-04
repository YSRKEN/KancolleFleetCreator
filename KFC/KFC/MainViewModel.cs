using Eto.Forms;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Text;

namespace KFC
{
	class MainViewModel
	{
		// コマンド
		public ReactiveCommand LoadPiecePictureCommand { get; } = new ReactiveCommand();
		public ReactiveCommand CreateFormationPictureCommand { get; } = new ReactiveCommand();
		public ReactiveCommand DeleteDataAllCommand { get; } = new ReactiveCommand();

		// コンストラクタ
		public MainViewModel() {
			LoadPiecePictureCommand.Subscribe(_ => { MessageBox.Show("画像追加"); });
			CreateFormationPictureCommand.Subscribe(_ => { MessageBox.Show("画像作成"); });
			DeleteDataAllCommand.Subscribe(_ => { MessageBox.Show("全消去"); });
		}
	}
}
