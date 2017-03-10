using System.Collections.Generic;
using System.Drawing;

namespace Platypus.Package
{
    public class Packager
    {
        private int _width = 1024;
        private int _height = 1024;
        private List<Bitmap> _images = new List<Bitmap>();

        public void init(List<Bitmap> images)
        {
            _images = images;
        }

    }
}
