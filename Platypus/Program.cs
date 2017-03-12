using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platypus
{
    enum FunctionType
    {
        PACKAGE,
        SLICE
    }

	class Program
	{
		private static string READ_PATH = "-i";
		private static string SAVE_PATH = "-o";
		private static string CUT_OFF_VALUE = "-a";
        private static string FUCTION_TYPE = "-t";
        private static string SLICE_TEXTURE = "s";
        private static string PACKAGE_TEXTURE = "p";

        private static string _savePath = "";
        private static FunctionType _type = FunctionType.SLICE;
        private static List<string> _loadPath = new List<string>();

		static void Main(string[] args)
		{
            List<Bitmap> images = new List<Bitmap>();
            List<string> names = new List<string>();
            getTexturePath(@"G:\Resources\Example");
            for (int i = 0; i < _loadPath.Count; i++)
            {
                Bitmap bitmap = BitmapHelper.ReadImage(_loadPath[i]);
                string name = System.IO.Path.GetFileNameWithoutExtension(_loadPath[i]);
                images.Add(bitmap);
                names.Add(name);
            }
            Package.Packager.init(images, names, 1024, 1024);
            Package.Packager.package();
            Package.Packager.saveTexture(@"G:\Resources", "Texture");

            /*
            ParamsProcesser processer = new ParamsProcesser();
            processer.register(READ_PATH, "The load path of textures", getTexturePath);
            processer.register(SAVE_PATH, "The save path of textures", getSavePath);
            processer.register(CUT_OFF_VALUE, "The cut off of textures", getCutOff);
            processer.register(FUCTION_TYPE, "How to deal with textures s(slice) or p(package)", setType);

            processer.trigger(args);

            switch (_type)
            {
            case FunctionType.PACKAGE:
                List<Bitmap> images = new List<Bitmap>();
                List<string> names = new List<string>();
                getTexturePath(@"G:\Resources");
                for (int i = 0; i < _loadPath.Count; i++)
                {
                    Console.WriteLine(string.Format("Processing picture {0}", _loadPath[i]));
                    Bitmap bitmap = BitmapHelper.ReadImage(_loadPath[i]);
                    images.Add(bitmap);
                }
                Package.Packager.init(images, names 1024, 1024);
                Package.Packager.package();
                break;
            case FunctionType.SLICE:
                for (int i = 0; i < _loadPath.Count; i++)
                {
                    Console.WriteLine(string.Format("Processing picture {0}", _loadPath[i]));
                    Bitmap bitmap = BitmapHelper.ReadImage(_loadPath[i]);
                    Slice.init(_savePath, bitmap);
                    Slice.SliceTexture();
                    Slice.clear();
                }
                break;
            default:
                break;
            }*/

            Console.WriteLine("Please Press Any Key");
            Console.ReadKey();
        }

        private static void getSavePath(string path)
        {
            _savePath = path;   
        }

        private static void getTexturePath(string path)
        {
            string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
            string[] ImageType = imgtype.Split('|');

            for (int i = 0; i < ImageType.Length; i++)
            {
                string[] dirs = Directory.GetFiles(path, ImageType[i]);
                int j = 0;
                foreach (string dir in dirs)
                {
                    _loadPath.Add(dir);
                }
            }
        }

        private static void getCutOff(string value)
        {
            int cutOff = 0;
            if (!int.TryParse(value, out cutOff))
                Console.WriteLine(string.Format("{0} is invalid value"));
            Slice.cutOff = cutOff;
        }

        private static void setType(string value)
        {
            if (value.Equals(SLICE_TEXTURE))
                _type = FunctionType.SLICE;
            else if (value.Equals(PACKAGE_TEXTURE))
                _type = FunctionType.PACKAGE;
            else
                Console.WriteLine("Unkown Type");
        }

	}
}
