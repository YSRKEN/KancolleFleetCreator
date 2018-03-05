using Eto.Drawing;
using Eto.Forms;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace KFC
{
	class MainViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// プロパティ
		public ReactiveProperty<Image> PreviewImage { get; } = new ReactiveProperty<Image>();
		public ReactiveProperty<bool> PreviewImageFlg { get; } = new ReactiveProperty<bool>(true);

		// コマンド
		public ReactiveCommand LoadPiecePictureCommand { get; } = new ReactiveCommand();
		public ReactiveCommand CreateFormationPictureCommand { get; } = new ReactiveCommand();
		public ReactiveCommand DeleteDataAllCommand { get; } = new ReactiveCommand();

		// コンストラクタ
		public MainViewModel() {
			//
			PreviewImage.Value = new Bitmap(@"sample.png");
			//
			LoadPiecePictureCommand.Subscribe(_ => { MessageBox.Show("画像追加"); });
			CreateFormationPictureCommand.Subscribe(_ => { MessageBox.Show("画像作成"); });
			DeleteDataAllCommand.Subscribe(_ => { MessageBox.Show("全消去"); });
		}
	}
}
