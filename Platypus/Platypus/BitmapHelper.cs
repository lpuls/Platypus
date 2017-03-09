using System;
using System.IO;
using System.Drawing;

namespace Platypus
{
	public class BitmapHelper
	{
		public static Bitmap ReadImage(string path)
		{
			FileStream fs = File.OpenRead(path);
			int filelength = 0;
			filelength = (int)fs.Length;
			Byte[] image = new Byte[filelength];
			fs.Read(image, 0, filelength);
			System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
			fs.Close();
			Bitmap bit = new Bitmap(result);
			return bit;
		}

		public static Bitmap ToImage(TextureGather gather)
		{
			int width = gather.width;
			int height = gather.height;

			Bitmap bitmap = new Bitmap(width + 1, height + 1);
			foreach (var item in gather.pixels)
			{
				Position position = gather.GetPixelPosition(item.Key);
				Color pixel = item.Value;
				bitmap.SetPixel(position.x, position.y, pixel);
			}
			return bitmap;
		}

		public static void SaveImage(string path, Bitmap image)
		{
			image.Save(path, System.Drawing.Imaging.ImageFormat.Png);
		}

	}
}
