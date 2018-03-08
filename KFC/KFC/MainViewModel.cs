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
		public ReadOnlyReactiveProperty<bool> PreviewImageFlg { get; }
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

		public void SetPreViewImage(Bitmap image) {
			// プレビューなので、表示サイズに収まるように適当にリサイズする
			PreviewImage.Value = (image != null ? image.WithSize(500, 500) : null);
		}
		public void DropPiecePicture(IEnumerable<Uri> uris, PiecePictureType ppt) {
			foreach(var uri in uris) {
				Console.WriteLine(uri.LocalPath);
				AddPiecePicture(uri.LocalPath, ppt, false);
			}
			RedrawViewImage();
		}
		public void RedrawViewImage() {
			SetPreViewImage(CreateFormationPicture());
		}
		// 改装画面かを判定する
		private bool IsRefitScene(Bitmap bitmap) {
			if (bitmap.GetPixel(300, 172) != Color.FromArgb(241, 191, 119))
				return false;
			if (bitmap.GetPixel(462, 454) != Color.FromArgb(255, 244, 243))
				return false;
			return true;
		}
		// 基地航空隊画面かを判定する
		private bool IsBaseScene(Bitmap bitmap) {
			if (bitmap.GetPixel(616, 223) != Color.FromArgb(125, 117, 107))
				return false;
			if (bitmap.GetPixel(698, 132) != Color.FromArgb(214, 210, 202))
				return false;
			return true;
		}
		// 画像ファイルを追加する
		// ただし種別による選別は施す(当てはまらない画像データはインポートしないように設定)
		private void AddPiecePicture(string fileName, PiecePictureType ppt, bool redrawFlg = true) {
			if (fileName == "")
				return;
			switch (ppt) {
			case PiecePictureType.Main:
				if (PiecePictureList1.Count >= 12)
					return;
				break;
			case PiecePictureType.Support:
				if (PiecePictureList2.Count >= 12)
					return;
				break;
			case PiecePictureType.Base:
				if (PiecePictureList3.Count >= 3)
					return;
				break;
			}
			try {
				var image = new Bitmap(fileName);
				//
				if (image.Width != 800 || image.Height != 480)
					return;
				if (ppt != PiecePictureType.Base && !IsRefitScene(image))
					return;
				if (ppt == PiecePictureType.Base && !IsBaseScene(image))
					return;
				//
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
				case PiecePictureType.Support:
					PiecePictureList2.Add(key);
					break;
				case PiecePictureType.Base:
					PiecePictureList3.Add(key);
					break;
				}
				if(redrawFlg)
					RedrawViewImage();
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
					RedrawViewImage();
				}
				break;
			case PiecePictureType.Support:
				if (PiecePictureIndex2.Value == 0)
					return;
				{
					int selectedIndex = PiecePictureIndex2.Value;
					string temp = PiecePictureList2[selectedIndex];
					PiecePictureList2[selectedIndex] = PiecePictureList2[selectedIndex - 1];
					PiecePictureList2[selectedIndex - 1] = temp;
					PiecePictureIndex2.Value = selectedIndex - 1;
					RedrawViewImage();
				}
				break;
			case PiecePictureType.Base:
				if (PiecePictureIndex3.Value == 0)
					return;
				{
					int selectedIndex = PiecePictureIndex3.Value;
					string temp = PiecePictureList3[selectedIndex];
					PiecePictureList3[selectedIndex] = PiecePictureList3[selectedIndex - 1];
					PiecePictureList3[selectedIndex - 1] = temp;
					PiecePictureIndex3.Value = selectedIndex - 1;
					RedrawViewImage();
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
					RedrawViewImage();
				}
				break;
			case PiecePictureType.Support:
				if (PiecePictureIndex2.Value >= PiecePictureList2.Count - 1 || PiecePictureIndex2.Value < 0)
					return;
				{
					int selectedIndex = PiecePictureIndex2.Value;
					string temp = PiecePictureList2[selectedIndex];
					PiecePictureList2[selectedIndex] = PiecePictureList2[selectedIndex + 1];
					PiecePictureList2[selectedIndex + 1] = temp;
					PiecePictureIndex2.Value = selectedIndex + 1;
					RedrawViewImage();
				}
				break;
			case PiecePictureType.Base:
				if (PiecePictureIndex3.Value >= PiecePictureList3.Count - 1 || PiecePictureIndex3.Value < 0)
					return;
				{
					int selectedIndex = PiecePictureIndex3.Value;
					string temp = PiecePictureList3[selectedIndex];
					PiecePictureList3[selectedIndex] = PiecePictureList3[selectedIndex + 1];
					PiecePictureList3[selectedIndex + 1] = temp;
					PiecePictureIndex3.Value = selectedIndex + 1;
					RedrawViewImage();
				}
				break;
			}
		}
		// リストの選択項目を削除する
		private void DeletePiecePicture(PiecePictureType ppt) {
			switch (ppt) {
			case PiecePictureType.Main:
				if (PiecePictureIndex1.Value < 0)
					return;
				{
					int selectedIndex = PiecePictureIndex1.Value;
					string temp = PiecePictureList1[selectedIndex];
					PiecePictureList1.RemoveAt(selectedIndex);
					piecePictureData.Remove(temp);
					PiecePictureIndex1.Value = Math.Min(selectedIndex, PiecePictureList1.Count - 1);
					RedrawViewImage();
				}
				break;
			case PiecePictureType.Support:
				if (PiecePictureIndex2.Value < 0)
					return;
				{
					int selectedIndex = PiecePictureIndex2.Value;
					string temp = PiecePictureList2[selectedIndex];
					PiecePictureList2.RemoveAt(selectedIndex);
					piecePictureData.Remove(temp);
					PiecePictureIndex2.Value = Math.Min(selectedIndex, PiecePictureList2.Count - 1);
					RedrawViewImage();
				}
				break;
			case PiecePictureType.Base:
				if (PiecePictureIndex3.Value < 0)
					return;
				{
					int selectedIndex = PiecePictureIndex3.Value;
					string temp = PiecePictureList3[selectedIndex];
					PiecePictureList3.RemoveAt(selectedIndex);
					piecePictureData.Remove(temp);
					PiecePictureIndex3.Value = Math.Min(selectedIndex, PiecePictureList3.Count - 1);
					RedrawViewImage();
				}
				break;
			}
		}
		// 画像を再構成する
		private Bitmap CreateFormationPicture() {
			bool imageFlg1 = (ShowImageDataFlg1.Value && PiecePictureList1.Count > 0);
			bool imageFlg2 = (ShowImageDataFlg2.Value && PiecePictureList2.Count > 0);
			bool imageFlg3 = (ShowImageDataFlg3.Value && PiecePictureList3.Count > 0);
			// すべての項目にチェックが付いていない or リスト未登録な際はnullを返す
			if(!imageFlg1 && !imageFlg2 && !imageFlg3) {
				return null;
			}
			// 自艦隊・支援艦隊・基地航空隊の画像を作成する
			var image1 = (imageFlg1 ? CreateFormationPicture1() : null);
			var image2 = (imageFlg2 ? CreateFormationPicture2() : null);
			var image3 = (imageFlg3 ? CreateFormationPicture3() : null);
			// 画像を合成して、まとめ画像を作成する
			var combineImage = image1;
			if (combineImage == null) {
				combineImage = image2;
			} else if(image2 != null) {
				combineImage = new Bitmap(image1.Width + image2.Width, Math.Max(image1.Height, image2.Height), PixelFormat.Format24bppRgb);
				using(var g = new Graphics(combineImage)) {
					g.DrawImage(image1, 0, 0);
					g.DrawImage(image2, image1.Width, 0);
				}
			}
			if (combineImage == null)
				combineImage = image3;
			return combineImage;
		}
		private Bitmap CreateFormationPicture1() {
			var blockRect = new Rectangle(330, 100, 455, 365);
			if (PiecePictureList1.Count >= 8) {
				// 連合艦隊と判断。第2艦隊は常に6隻になるよう調整
				int width = blockRect.Width * 4;
				int height = blockRect.Height * 3;
				var image = new Bitmap(width, height, PixelFormat.Format24bppRgb);
				for(int i = 0; i < PiecePictureList1.Count - 6; ++i) {
					int x = (i % 2) * blockRect.Width, y = (i / 2) * blockRect.Height;
					using(var g = new Graphics(image)) {
						var image2 = piecePictureData[PiecePictureList1[i]];
						g.DrawImage(image2.Clone(blockRect), x, y);
						g.DrawText(new Font(SystemFont.Bold, 48), Colors.White, 455 - 56 * 2 + x + 4, 0 + y + 4, $"1-{i + 1}");
						g.DrawText(new Font(SystemFont.Bold, 48), Colors.Red, 455 - 56 * 2 + x, 0 + y, $"1-{i + 1}");
					}
				}
				for (int i = PiecePictureList1.Count - 6; i < PiecePictureList1.Count; ++i) {
					int i2 = (i - (PiecePictureList1.Count - 6));
					int x = ((i2 % 2) + 2) * blockRect.Width, y = (i2 / 2) * blockRect.Height;
					using (var g = new Graphics(image)) {
						var image2 = piecePictureData[PiecePictureList1[i]];
						g.DrawImage(image2.Clone(blockRect), x, y);
						g.DrawText(new Font(SystemFont.Bold, 48), Colors.White, 455 - 56 * 2 + x + 4, 0 + y + 4, $"2-{i2 + 1}");
						g.DrawText(new Font(SystemFont.Bold, 48), Colors.Red, 455 - 56 * 2 + x, 0 + y, $"2-{i2 + 1}");
					}
				}
				return image;
			} else {
				// 通常艦隊/遊撃艦隊と判断
				int width = (PiecePictureList1.Count > 1 ? blockRect.Width * 2 : blockRect.Width);
				int height = (PiecePictureList1.Count > 1 ? blockRect.Height * ((PiecePictureList1.Count + 1) / 2) : blockRect.Height);
				var image = new Bitmap(width, height, PixelFormat.Format24bppRgb);
				for (int i = 0; i < PiecePictureList1.Count; ++i) {
					int x = (i % 2) * blockRect.Width, y = (i / 2) * blockRect.Height;
					using (var g = new Graphics(image)) {
						var image2 = piecePictureData[PiecePictureList1[i]];
						g.DrawImage(image2.Clone(blockRect), x, y);
						g.DrawText(new Font(SystemFont.Bold, 48), Colors.White, 455 - 48 + x + 4, 0 + y + 4, $"{i + 1}");
						g.DrawText(new Font(SystemFont.Bold, 48), Colors.Red, 455 - 48 + x, 0 + y, $"{i+1}");
					}
				}
				return image;
			}
		}
		private Bitmap CreateFormationPicture2() {
			var blockRect = new Rectangle(330, 100, 455, 365);
			if (PiecePictureList2.Count >= 7) {
				// 連合艦隊と判断。第2艦隊は常に6隻になるよう調整
				int width = blockRect.Width * 4;
				int height = blockRect.Height * 3;
				var image = new Bitmap(width, height, PixelFormat.Format24bppRgb);
				for (int i = 0; i < PiecePictureList2.Count - 6; ++i) {
					int x = (i % 2) * blockRect.Width, y = (i / 2) * blockRect.Height;
					using (var g = new Graphics(image)) {
						var image2 = piecePictureData[PiecePictureList2[i]];
						g.DrawImage(image2.Clone(blockRect), x, y);
						g.DrawText(new Font(SystemFont.Bold, 48), Colors.White, 455 - 56 * 2 + x + 4, 0 + y + 4, $"1-{i + 1}");
						g.DrawText(new Font(SystemFont.Bold, 48), Colors.Blue, 455 - 56 * 2 + x, 0 + y, $"1-{i + 1}");
					}
				}
				for (int i = PiecePictureList2.Count - 6; i < PiecePictureList2.Count; ++i) {
					int i2 = (i - (PiecePictureList2.Count - 6));
					int x = ((i2 % 2) + 2) * blockRect.Width, y = (i2 / 2) * blockRect.Height;
					using (var g = new Graphics(image)) {
						var image2 = piecePictureData[PiecePictureList2[i]];
						g.DrawImage(image2.Clone(blockRect), x, y);
						g.DrawText(new Font(SystemFont.Bold, 48), Colors.White, 455 - 56 * 2 + x + 4, 0 + y + 4, $"2-{i2 + 1}");
						g.DrawText(new Font(SystemFont.Bold, 48), Colors.Blue, 455 - 56 * 2 + x, 0 + y, $"2-{i2 + 1}");

					}
				}
				return image;
			} else {
				// 通常艦隊/遊撃艦隊と判断
				int width = (PiecePictureList2.Count > 1 ? blockRect.Width * 2 : blockRect.Width);
				int height = (PiecePictureList2.Count > 1 ? blockRect.Height * ((PiecePictureList2.Count + 1) / 2) : blockRect.Height);
				var image = new Bitmap(width, height, PixelFormat.Format24bppRgb);
				for (int i = 0; i < PiecePictureList2.Count; ++i) {
					int x = (i % 2) * blockRect.Width, y = (i / 2) * blockRect.Height;
					using (var g = new Graphics(image)) {
						var image2 = piecePictureData[PiecePictureList2[i]];
						g.DrawImage(image2.Clone(blockRect), x, y);
						g.DrawText(new Font(SystemFont.Bold, 48), Colors.White, 455 - 48 + x + 4, 0 + y + 4, $"{i + 1}");
						g.DrawText(new Font(SystemFont.Bold, 48), Colors.Blue, 455 - 48 + x, 0 + y, $"{i + 1}");
					}
				}
				return image;
			}
		}
		private Bitmap CreateFormationPicture3() {
			return null;
		}

		// コンストラクタ
		public MainViewModel(GetFileName funcGFN1, GetFileName funcGFN2) {
			//
			PreviewImageFlg = PreviewImage.Select(p => p != null).ToReadOnlyReactiveProperty();
			//
			ShowImageDataFlg1.Subscribe(_ => RedrawViewImage());
			ShowImageDataFlg2.Subscribe(_ => RedrawViewImage());
			ShowImageDataFlg3.Subscribe(_ => RedrawViewImage());
			//
			CreateFormationPictureCommand.Subscribe(_ => {
				var image = CreateFormationPicture();
				if (image == null)
					return;
				string filePath = funcGFN2();
				if (filePath != "") {
					if(System.IO.Path.GetExtension(filePath) != ".png") {
						filePath += ".png";
					}
					image.Save(filePath, ImageFormat.Png);
				}
			});
			DeleteDataAllCommand.Subscribe(_ => { MessageBox.Show("全消去"); });
			AddPiecePicture1Command.Subscribe(_ => { AddPiecePicture(funcGFN1(), PiecePictureType.Main); });
			AddPiecePicture2Command.Subscribe(_ => { AddPiecePicture(funcGFN1(), PiecePictureType.Support); });
			AddPiecePicture3Command.Subscribe(_ => { AddPiecePicture(funcGFN1(), PiecePictureType.Base); });
			DeletePiecePicture1Command.Subscribe(_ => { DeletePiecePicture(PiecePictureType.Main); });
			DeletePiecePicture2Command.Subscribe(_ => { DeletePiecePicture(PiecePictureType.Support); });
			DeletePiecePicture3Command.Subscribe(_ => { DeletePiecePicture(PiecePictureType.Base); });
			MoveUpPiecePicture1Command.Subscribe(_ => { MoveUpPiecePicture(PiecePictureType.Main); });
			MoveUpPiecePicture2Command.Subscribe(_ => { MoveUpPiecePicture(PiecePictureType.Support); });
			MoveUpPiecePicture3Command.Subscribe(_ => { MoveUpPiecePicture(PiecePictureType.Base); });
			MoveDownPiecePicture1Command.Subscribe(_ => { MoveDownPiecePicture(PiecePictureType.Main); });
			MoveDownPiecePicture2Command.Subscribe(_ => { MoveDownPiecePicture(PiecePictureType.Support); });
			MoveDownPiecePicture3Command.Subscribe(_ => { MoveDownPiecePicture(PiecePictureType.Base); });
		}
	}
}
