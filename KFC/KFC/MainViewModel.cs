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
		public ReactiveProperty<int> PiecePictureIndex1 { get; } = new ReactiveProperty<int>(-1);
		public ReactiveProperty<int> PiecePictureIndex2 { get; } = new ReactiveProperty<int>(-1);
		public ReactiveProperty<int> PiecePictureIndex3 { get; } = new ReactiveProperty<int>(-1);

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
				piecePictureData[fileName] = image;
				switch (ppt) {
				case PiecePictureType.Main:
					PiecePictureList1.Add(key);
					break;
				case PiecePictureType.Base:
					PiecePictureList2.Add(key);
					break;
				case PiecePictureType.Support:
					PiecePictureList3.Add(key);
					break;
				}
			} catch(Exception e) {
				Console.WriteLine(e.Message);
			}
		}
		// リストの選択項目を上に移動させる
		private void MoveUpPiecePicture(PiecePictureType ppt) {
			switch (ppt) {
			case PiecePictureType.Main:
				if (PiecePictureIndex1.Value <= 0)
					return;
				{
					int selectedIndex = PiecePictureIndex1.Value;
					string temp = PiecePictureList1[selectedIndex];
					PiecePictureList1[selectedIndex] = PiecePictureList1[selectedIndex - 1];
					PiecePictureList1[selectedIndex - 1] = temp;
					PiecePictureIndex1.Value = selectedIndex - 1;
				}
				break;
			case PiecePictureType.Base:
				if (PiecePictureIndex2.Value == 0)
					return;
				{
					int selectedIndex = PiecePictureIndex2.Value;
					string temp = PiecePictureList2[selectedIndex];
					PiecePictureList2[selectedIndex] = PiecePictureList2[selectedIndex - 1];
					PiecePictureList2[selectedIndex - 1] = temp;
					PiecePictureIndex2.Value = selectedIndex - 1;
				}
				break;
			case PiecePictureType.Support:
				if (PiecePictureIndex3.Value == 0)
					return;
				{
					int selectedIndex = PiecePictureIndex3.Value;
					string temp = PiecePictureList3[selectedIndex];
					PiecePictureList3[selectedIndex] = PiecePictureList3[selectedIndex - 1];
					PiecePictureList3[selectedIndex - 1] = temp;
					PiecePictureIndex3.Value = selectedIndex - 1;
				}
				break;
			}
		}
		// リストの選択項目を下に移動させる
		private void MoveDownPiecePicture(PiecePictureType ppt) {
			switch (ppt) {
			case PiecePictureType.Main:
				if (PiecePictureIndex1.Value >= PiecePictureList1.Count - 1 || PiecePictureIndex1.Value < 0)
					return;
				{
					int selectedIndex = PiecePictureIndex1.Value;
					string temp = PiecePictureList1[selectedIndex];
					PiecePictureList1[selectedIndex] = PiecePictureList1[selectedIndex + 1];
					PiecePictureList1[selectedIndex + 1] = temp;
					PiecePictureIndex1.Value = selectedIndex + 1;
				}
				break;
			case PiecePictureType.Base:
				if (PiecePictureIndex2.Value >= PiecePictureList2.Count - 1 || PiecePictureIndex2.Value < 0)
					return;
				{
					int selectedIndex = PiecePictureIndex2.Value;
					string temp = PiecePictureList2[selectedIndex];
					PiecePictureList2[selectedIndex] = PiecePictureList2[selectedIndex + 1];
					PiecePictureList2[selectedIndex + 1] = temp;
					PiecePictureIndex2.Value = selectedIndex + 1;
				}
				break;
			case PiecePictureType.Support:
				if (PiecePictureIndex3.Value >= PiecePictureList3.Count - 1 || PiecePictureIndex3.Value < 0)
					return;
				{
					int selectedIndex = PiecePictureIndex3.Value;
					string temp = PiecePictureList3[selectedIndex];
					PiecePictureList3[selectedIndex] = PiecePictureList3[selectedIndex + 1];
					PiecePictureList3[selectedIndex + 1] = temp;
					PiecePictureIndex3.Value = selectedIndex + 1;
				}
				break;
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
			MoveUpPiecePicture1Command.Subscribe(_ => { MoveUpPiecePicture(PiecePictureType.Main); });
			MoveUpPiecePicture2Command.Subscribe(_ => { MoveUpPiecePicture(PiecePictureType.Base); });
			MoveUpPiecePicture3Command.Subscribe(_ => { MoveUpPiecePicture(PiecePictureType.Support); });
			MoveDownPiecePicture1Command.Subscribe(_ => { MoveDownPiecePicture(PiecePictureType.Main); });
			MoveDownPiecePicture2Command.Subscribe(_ => { MoveDownPiecePicture(PiecePictureType.Base); });
			MoveDownPiecePicture3Command.Subscribe(_ => { MoveDownPiecePicture(PiecePictureType.Support); });
		}
	}
}
