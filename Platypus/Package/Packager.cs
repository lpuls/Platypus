using System;
using System.Collections.Generic;
using System.Drawing;

namespace Platypus.Package
{
    public class Packager
    {
        private static Color INVALID = Color.FromArgb(0, 0, 0, 0);
        private static Bitmap _texture = null;  // new Bitmap(_width, _height);
        private static Graphics _graphics = null;
        private static List<Position> _vectors = new List<Position>();
        private static List<TextureBlock> _blocks = new List<TextureBlock>();
        private static Dictionary<string, Bitmap> _images = new Dictionary<string, Bitmap>();

        public static void init(List<Bitmap> images, List<string> names, int width, int height)
        {
            _texture = new Bitmap(width, height);
            _graphics = Graphics.FromImage(_texture);
            // _graphics.Clear(INVALID);
            _blocks.Clear();

            for (int i = 0; i < images.Count; i++)
                _images[names[i]] = images[i];

            sortTexture();

            // 输入当前最大的4个顶点, 按先左上，后右下的排序方式
            _vectors.Clear();
            _vectors.Add(new Position(0, 0));
        }

        private static void sortTexture()
        {
            // for (int i = 0; i < _images.Count; i++)
            foreach (var item in _images)
            {
                Bitmap image = item.Value;
                TextureBlock block = new TextureBlock();
                block.width = image.Width;
                block.height = image.Height;
                block.name = item.Key;
                _blocks.Add(block);
            }
            _blocks.Sort((TextureBlock a, TextureBlock b) => 
            {
                return a.width * a.height > b.width * a.height ? -1 : 1;
            });
        }

        public static void package()
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                TextureBlock block = _blocks[i];
                for (int j = 0; j < _vectors.Count; j++)
                {
                    Position position = _vectors[j];
                    if (!suitableBlocks(position, block.width, block.height))
                        continue;

                    drawTexture(position.x, position.y, block.name);
                    Console.WriteLine(block.name);
                    // BitmapHelper.SaveImage(string.Format("{0}/{1}.png", @"G:\Resources", "Texture"), _texture);

                    // 修改新的节点
                    removeVector(position.x, position.y);
                    int newX = position.x + block.width;
                    int newY = position.y + block.height;
                    // 检查右上角
                    if (newX < _texture.Width && position.y < _texture.Height)
                    {
                        removeInvalidVector(newX, position.y, block.width, block.height);
                        pushVector(newX, position.y);
                    }
                    // 检查左下角
                    if (position.x < _texture.Width && newY < _texture.Height)
                    {
                        removeInvalidVector(position.x, newY, block.width, block.height);
                        pushVector(position.x, newY);
                    }
                    // 检查右下角
                    if (newX < _texture.Width && newY < _texture.Height)
                    {
                        removeInvalidVector(newX, newY, block.width, block.height);
                        pushVector(newX, newY);
                    }
                    break;
                }
            }
        }

        public static void pushVector(int x, int y)
        {
            for (int i = 0; i < _vectors.Count; i++)
            {
                // TODO 这里也有BUG
                Position vector = _vectors[i];
                if (x < vector.x && y < vector.y)
                {
                    _vectors.Insert(i, new Position(x, y));
                    return;
                }
            }
            _vectors.Add(new Position(x, y));
        }

        public static void removeInvalidVector(int x, int y, int width, int height)
        {
            for (int i = 0; i < _vectors.Count; i++)
            {
                Position vector = _vectors[i];
                if (x - vector.x < width && y - vector.y < height)
                {
                    _vectors.Remove(vector);
                    i--;
                }
            }
        }

        public static void removeVector(int x, int y)
        {
            for (int i = 0; i < _vectors.Count; i++)
            {
                Position vector = _vectors[i];
                if (vector.x == x && vector.y == y)
                {
                    _vectors.Remove(vector);
                    i--;
                }
            }
        }

        private static bool suitableBlocks(Position position, int width, int height)
        {
            if ((_texture.Width - position.x < width) || (_texture.Height - position.y < height))
                return false;

            for (int i = position.x; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color color = _texture.GetPixel(i, j);
                    if (!color.Equals(INVALID))
                        return false;
                }
            }
            return true;
        }

        private static void drawTexture(int x, int y, string imageName)
        {
            Bitmap image = null;
            if (_images.TryGetValue(imageName, out image))
            {
                _graphics.DrawImage(image, new PointF(x, y));
            }
        }

        public static void saveTexture(string path, string textureName = "Texture")
        {
            /*
            TextureData textureData = new TextureData();
            textureData.name = string.Format("{0}.png", textureName);
            textureData.imagePath = textureData.name;
            textureData.SubTexture = _blocks;
            string json = JsonHelper.toJson<TextureData>(textureData);
            JsonHelper.saveFile(string.Format("{0}/{1}.json", path, textureName), json);
            */

            /*
            Bitmap texture = new Bitmap(_width, _height);
            Graphics g = Graphics.FromImage(texture);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            for (int i = 0; i < _blocks.Count; i++)
            {
                TextureBlock block = _blocks[i];
                Console.WriteLine("Name: {0}, X: {1}, Y: {2}, Whidth: {3}, Height: {4}", block.name, block.x, block.y, block.width, block.height);
                Bitmap image = null;
                if (_images.TryGetValue(block.name, out image))
                {
                    g.DrawImage(image, new PointF(block.x, block.y));
                    // BitmapHelper.SaveImage(string.Format("{0}/{1}.png", path, textureName), texture);
                    // Console.Write("");
                }
            }
            */
            BitmapHelper.SaveImage(string.Format("{0}/{1}.png", path, textureName), _texture);

        }

    }
}
