using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platypus
{
	class Program
	{
		private static string READ_PATH = "-i";
		private static string SAVE_PATH = "-o";
		private static string CUT_OFF_VALUE = "-a";

        private static string _savePath = "";
        private static List<string> _loadPath = new List<string>();

		static void Main(string[] args)
		{

            ParamsProcesser processer = new ParamsProcesser();
            processer.register(READ_PATH, "The load path of textures", getTexturePath);
            processer.register(SAVE_PATH, "The save path of textures", getSavePath);
            processer.register(CUT_OFF_VALUE, "The cut off of textures", getCutOff);

            processer.trigger(args);

            for (int i = 0; i < _loadPath.Count; i++)
            {
                Console.WriteLine(string.Format("Processing picture {0}", _loadPath[i]));
                Bitmap bitmap = BitmapHelper.ReadImage(_loadPath[i]);
                Slice.init(_savePath, bitmap);
                Slice.SliceTexture();
                Slice.clear();
            }

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

	}
}
