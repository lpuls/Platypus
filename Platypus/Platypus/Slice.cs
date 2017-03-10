using System;
using System.Collections.Generic;
using System.Drawing;

namespace Platypus
{
	public class Slice
	{
		private static Bitmap _image = null;  // 要分割的图片
		private static int _cutOff = 0;  // 透明适伐值，低于该值则为算透时
		private static bool[,] _stand = null;  // 标记位，用来标记哪些像素己经被使用过了
		private static List<Position> _pixelQueue = new List<Position>();  // 用来记录待检查的像素点
		private static TextureGather _gather = null;
		private static string _savePath = string.Empty;
        private static int _processValue = 0;

		public static int cutOff
		{
			set { _cutOff = value; }
		}

        public static int processValue
        {
            get { return _processValue; }
        }

        public static int MaxPixel
        {
            get
            {
                if (null != _image)
                    return _image.Width * _image.Height;
                return 0;
            }
        }

		public static void init(string savePath, Bitmap image)
		{
			_image = image;
			_savePath = savePath;
			SetStand(_image.Width, _image.Height);
		}

        public static void clear()
        {
            _image = null;
            _gather = null;
            _processValue = 0;
            _pixelQueue.Clear();
            _stand = null;
        }

		private static void SetStand(int width, int height)
		{
			_stand = new bool[width, height];
			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
					_stand[i, j] = false;
		}

		// 划分图片
		public static void SliceTexture()
		{
			int gatherCount = 0;
			bool isLucency = true;
			for (int i = 0; i < _image.Width; i++)
			{
				for (int j = 0; j < _image.Height; j++)
				{
                    _processValue++;
                    // Console.WriteLine(_processValue);
                    Color pixel = _image.GetPixel(i, j);
					// 根据找到的第一个非透时像素点开始划分像素值
					if (isLucency) 
					{
						if (_cutOff < pixel.A && !_stand[i, j])
						{
							// 开始对该像素进行划分
							PushToQueue(i, j);
							SliceTextureImpl(gatherCount);

							// 将得到的像素值分保存起来
                            if (gatherCount == 79)
                                Console.WriteLine("pause");
							Bitmap image = BitmapHelper.ToImage(_gather);
							BitmapHelper.SaveImage(string.Format("{0}/Texture_{1}.png", _savePath, _gather.id), image);
							_gather = null;

							gatherCount++;
							isLucency = false;
						}
					}
					else  // 划分像素集后，找到第一个透明像素点，并使状态恢复到划分前
					{
						if (_cutOff > pixel.A)
							isLucency = true;
					}
					_stand[i, j] = true;
				}
			}
		}

		// 将像素点压入队列准备进行划分，压入时会进行是否有效点的检查
		private static void PushToQueue(int x, int y)
		{
			if ((x < 0 || x >= _image.Width) || (y < 0 || y >= _image.Height))
				return;

			if (_stand[x, y])
				return;

			Color color = _image.GetPixel(x, y);
			if (_cutOff >= color.A)
				return;

			_stand[x, y] = true;
			_pixelQueue.Add(new Position(x, y));
		}

		// 获取像素集集合
		private static TextureGather GetGather(int id)
		{
			if (_gather == null)
			{
				_gather = new TextureGather();
				_gather.id = id;
			}
			return _gather;
		}

		// 划分像素值，并将八方向的像素放入队列准备划分
		private static void SetGather(int x, int y, int id)
		{
			Color color = _image.GetPixel(x, y);
			TextureGather textureGather = GetGather(id);
			textureGather.AddPixel(new Position(x, y), color);

			PushToQueue(x + 1, y);
			PushToQueue(x - 1, y);
			PushToQueue(x, y + 1);
			PushToQueue(x, y - 1);
			PushToQueue(x + 1, y + 1);
			PushToQueue(x - 1, y - 1);
			PushToQueue(x + 1, y - 1);
			PushToQueue(x - 1, y + 1);
		}

		// 通过广度遍历进行划分像素信
		private static void SliceTextureImpl(int id)
		{
			while (0 != _pixelQueue.Count)
			{
				Position position = _pixelQueue[0];
				_pixelQueue.RemoveAt(0);
				SetGather(position.x, position.y, id);
			}
		}


	}
}
