using System.Collections.Generic;

namespace Platypus.Package
{
    public class TextureData
    {
        public string name = "";
        public string imagePath = "";
        public List<TextureBlock> SubTexture = new List<Package.TextureBlock>();
    }

    public class TextureBlock
    {
        public int x = 0;
        public int y = 0;
        public int width = 0;
        public int height = 0;
        public string name = "";

    }
}
