using System;
using System.Collections.Generic;
using System.Drawing;

namespace Platypus.Package
{
    public class Packager
    {
        private static int _area = 0;
        private static int _curArea = 0;
        private static int _width = 0;
        private static int _height = 0;
        private static List<Bitmap> _images = new List<Bitmap>();
        private static List<Position> _vectors = new List<Position>();
        private static List<TextureBlock> _blocks = new List<TextureBlock>();

        public static void init(List<Bitmap> images, int width, int height)
        {
            _images = images;
            _width = width;
            _height = height;
            _area = width * height;
            _curArea = width * height;
            _blocks.Clear();
            sortTexture();
            for (int i = 0; i < _blocks.Count; i++)
                Console.WriteLine(string.Format("Width: {0}, Height: {1}", _blocks[i].width, _blocks[i].height));

            // 输入当前最大的4个顶点, 按先左上，后右下的排序方式
            _vectors.Clear();
            _vectors.Add(new Position(0, 0));
        }

        private static void sortTexture()
        {
            for (int i = 0; i < _images.Count; i++)
            {
                Bitmap image = _images[i];
                TextureBlock block = new TextureBlock();
                block.width = image.Width;
                block.height = image.Height;
                block.image = i;
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
                int area = block.width * block.height;
                if (area > _curArea)
                {
                    Console.WriteLine("There is not enough area");
                    return;
                }

                // 选取出前左上的点
                if (_vectors.Count <= 0)
                {
                    Console.WriteLine("Unknow Error");
                    return;
                }

                // 检查每个顶点，直到找到合适的
                for (int j = 0; j < _vectors.Count; j++)
                {
                    Position position = _vectors[j];
                    int spaceX = _width - position.x;
                    int spaceY = _height - position.y;
                    if (spaceX >= 0 && spaceY >= 0)
                    {
                        removeVector(position.x, position.y);
                        block.x = position.x;
                        block.y = position.y;
                        int nextX = position.x + block.width;
                        int nextY = position.y + block.height;
                        if (nextX < _width && position.y < _height)
                            pushVector(nextX, position.y);
                        else if (position.x < _width && nextY < _height)
                            pushVector(position.x, nextY);
                        else if (nextX < _width && nextY < _height)
                            pushVector(nextX, nextY);
                        break;
                    }
                    
                }
            }
        }

        private static void pushVector(int x, int y)
        {
            for (int i = 0; i < _vectors.Count; i++)
            {
                Position vector = _vectors[i];
                if (x < vector.x)
                {
                    _vectors.Insert(i, new Position(x, y));
                    return;
                }
                else if (y < vector.y)
                {
                    _vectors.Insert(i, new Position(x, y));
                    return;
                }
            }
            _vectors.Add(new Position(x, y));
        }

        private static void removeVector(int x, int y)
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

        public static void saveTexture(string path, string textureName = "Texture")
        {
            Bitmap texture = new Bitmap(_width, _height);
            Graphics g = Graphics.FromImage(texture);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            for (int i = 0; i < _blocks.Count; i++)
            {
                TextureBlock block = _blocks[i];
                if (block.image >= 0 && block.image < _images.Count)
                {
                    Bitmap blockTexture = _images[block.image];
                    g.DrawImage(blockTexture, new PointF(block.x, block.y));
                }
            }
            BitmapHelper.SaveImage(string.Format("{0}/{1}.png", path, textureName), texture);

        }

    }
}
