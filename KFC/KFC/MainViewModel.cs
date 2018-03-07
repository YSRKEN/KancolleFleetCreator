using Eto.Drawing;
using Eto.Forms;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using static KFC.Utility;

namespace KFC
{
	class MainViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private Dictionary<string, Bitmap> piecePictureData = new Dictionary<string, Bitmap>();

		// プロパティ
		public ReactiveProperty<Image> PreviewImage { get; } = new ReactiveProperty<Image>();
		public ReadOnlyReactiveProperty<bool> PreviewImageFlg1 { get; }
		public ReadOnlyReactiveProperty<bool> PreviewImageFlg2 { get; }
		public ReactiveProperty<bool> ShowImageDataFlg1 { get; } = new ReactiveProperty<bool>(true);
		public ReactiveProperty<bool> ShowImageDataFlg2 { get; } = new ReactiveProperty<bool>(true);
		public ReactiveProperty<bool> ShowImageDataFlg3 { get; } = new ReactiveProperty<bool>(true);

		// コマンド
		public ReactiveCommand CreateFormationPictureCommand { get; } = new ReactiveCommand();
		public ReactiveCommand DeleteDataAllCommand { get; } = new ReactiveCommand();
		//
		public ReactiveCommand AddPiecePicture1Command { get; } = new ReactiveCommand();
		public ReactiveCommand AddPiecePicture2Command { get; } = new ReactiveCommand();
		public ReactiveCommand AddPiecePicture3Command { get; } = new ReactiveCommand();
		public ReactiveCommand DeletePiecePicture1Command { get; } = new ReactiveCommand();
		public ReactiveCommand DeletePiecePicture2Command { get; } = new ReactiveCommand();
		public ReactiveCommand DeletePiecePicture3Command { get; } = new ReactiveCommand();
		public ReactiveCommand MoveUpPiecePicture1Command { get; } = new ReactiveCommand();
		public ReactiveCommand MoveUpPiecePicture2Command { get; } = new ReactiveCommand();
		public ReactiveCommand MoveUpPiecePicture3Command { get; } = new ReactiveCommand();
		public ReactiveCommand MoveDownPiecePicture1Command { get; } = new ReactiveCommand();
		public ReactiveCommand MoveDownPiecePicture2Command { get; } = new ReactiveCommand();
		public ReactiveCommand MoveDownPiecePicture3Command { get; } = new ReactiveCommand();

		// コレクション
		public ReactiveCollection<string> PiecePictureList1 { get; } = new ReactiveCollection<string>();
		public ReactiveCollection<string> PiecePictureList2 { get; } = new ReactiveCollection<string>();
		public ReactiveCollection<string> PiecePictureList3 { get; } = new ReactiveCollection<string>();

		public void SetPreViewImage(Image image) {
			PreviewImage.Value = image;
		}
		// 画像ファイルを追加する
		// ただし種別による選別は施す(当てはまらない画像データはインポートしないように設定)
		private void AddPiecePicture(GetFileName funcGFN, PiecePictureType ppt) {
			string fileName = funcGFN();
			if (fileName == "")
				return;
			try {
				var image = new Bitmap(fileName);
				string key = System.IO.Path.GetFileNameWithoutExtension(fileName);
				if (piecePictureData.ContainsKey(key)) {
					for(int i = 1; ; ++i) {
						string key2 = $"{key}_{i}";
						if (!piecePictureData.ContainsKey(key2)) {
							key = key2;
							break;
						}
					}
				}
				piecePictureData[key] = image;
				switch (ppt) {
				case PiecePictureType.Main:
					PiecePictureList1.Add(key);
					break;
				case PiecePictureType.Base:
					PiecePictureList2.Add(key);
					break;
					break;
				case PiecePictureType.Support:
					PiecePictureList3.Add(key);
					break;
				}
			} catch(Exception e) {
				Console.WriteLine(e.Message);
			}
		}

		// コンストラクタ
		public MainViewModel(GetFileName funcGFN) {
			PreviewImageFlg1 = PreviewImage.Select(p => p != null && p.Width < p.Height).ToReadOnlyReactiveProperty();
			PreviewImageFlg2 = PreviewImage.Select(p => p != null && p.Width >= p.Height).ToReadOnlyReactiveProperty();
			//
			CreateFormationPictureCommand.Subscribe(_ => { MessageBox.Show("画像作成"); });
			DeleteDataAllCommand.Subscribe(_ => { MessageBox.Show("全消去"); });
			AddPiecePicture1Command.Subscribe(_ => { AddPiecePicture(funcGFN, PiecePictureType.Main); });
			AddPiecePicture2Command.Subscribe(_ => { AddPiecePicture(funcGFN, PiecePictureType.Base); });
			AddPiecePicture3Command.Subscribe(_ => { AddPiecePicture(funcGFN, PiecePictureType.Support); });
			DeletePiecePicture1Command.Subscribe(_ => { MessageBox.Show("画像削除"); });
			DeletePiecePicture2Command.Subscribe(_ => { MessageBox.Show("画像削除"); });
			DeletePiecePicture3Command.Subscribe(_ => { MessageBox.Show("画像削除"); });
			MoveUpPiecePicture1Command.Subscribe(_ => { MessageBox.Show("上移動"); });
			MoveUpPiecePicture2Command.Subscribe(_ => { MessageBox.Show("上移動"); });
			MoveUpPiecePicture3Command.Subscribe(_ => { MessageBox.Show("上移動"); });
			MoveDownPiecePicture1Command.Subscribe(_ => { MessageBox.Show("下移動"); });
			MoveDownPiecePicture2Command.Subscribe(_ => { MessageBox.Show("下移動"); });
			MoveDownPiecePicture3Command.Subscribe(_ => { MessageBox.Show("下移動"); });
		}
	}
}
