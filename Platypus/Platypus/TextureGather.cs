using System.Drawing;
using System.Collections.Generic;

namespace Platypus
{
	public class TextureGather
	{
		private int _id = 0;
		private int _top = int.MaxValue;
		private int _bottom = int.MinValue;
		private int _left = int.MaxValue;
		private int _right = int.MinValue;
		private Dictionary<Position, Color> _pixels = new Dictionary<Position, Color>();

		public int id
		{
			get { return _id; }
			set { _id = value; }
		}

		public int width
		{
			get { return _right - _left; }
		}

		public int height
		{
			get { return _bottom - _top; }
		}

		public Dictionary<Position, Color> pixels
		{
			get { return _pixels; }
		}

		public void AddPixel(Position position, Color pixle)
		{
			_pixels[position] = pixle;

			if (_top > position.y)
				_top = position.y;
			if (_bottom < position.y)
				_bottom = position.y;

			if (_right < position.x)
				_right = position.x;
			if (_left > position.x)
				_left = position.x;
		}

		public Position GetPixelPosition(Position position)
		{
			Position location = new Position();
			location.x = position.x - _left;
			location.y = position.y - _top;
			return location;
		}

	}
}
