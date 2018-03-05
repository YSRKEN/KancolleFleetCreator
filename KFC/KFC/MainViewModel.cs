using Eto.Drawing;
using Eto.Forms;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace KFC
{
	class MainViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// プロパティ
		public ReactiveProperty<Image> PreviewImage { get; } = new ReactiveProperty<Image>();
		public ReadOnlyReactiveProperty<bool> PreviewImageFlg1 { get; }
		public ReadOnlyReactiveProperty<bool> PreviewImageFlg2 { get; }

		// コマンド
		public ReactiveCommand LoadPiecePictureCommand { get; } = new ReactiveCommand();
		public ReactiveCommand CreateFormationPictureCommand { get; } = new ReactiveCommand();
		public ReactiveCommand DeleteDataAllCommand { get; } = new ReactiveCommand();

		// コンストラクタ
		public MainViewModel() {
			PreviewImageFlg1 = PreviewImage.Select(p => p != null && p.Width < p.Height).ToReadOnlyReactiveProperty();
			PreviewImageFlg2 = PreviewImage.Select(p => p != null && p.Width >= p.Height).ToReadOnlyReactiveProperty();
			//var bitmap = new Bitmap(@"横長画像.png");
			var bitmap = new Bitmap(@"縦長画像.png");
			PreviewImage.Value = bitmap;
			//
			LoadPiecePictureCommand.Subscribe(_ => { MessageBox.Show("画像追加"); });
			CreateFormationPictureCommand.Subscribe(_ => { MessageBox.Show("画像作成"); });
			DeleteDataAllCommand.Subscribe(_ => { MessageBox.Show("全消去"); });
		}
	}
}
