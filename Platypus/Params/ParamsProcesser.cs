using System;
using System.Collections.Generic;

namespace Platypus
{
    public class ParamsCallback
    {
        public string key = string.Empty;
        public string description = string.Empty;
        public Action<string> callBack = null; 
    }

    public class ParamsProcesser
    {
        private Dictionary<string, ParamsCallback> _paramsDict = new Dictionary<string, ParamsCallback>();

        public void register(string key, string description, Action<string> callBack)
        {
            ParamsCallback pcb = new ParamsCallback();
            pcb.key = "-" + key;
            pcb.description = description;
            pcb.callBack = callBack;
            _paramsDict[key] = pcb;
        }

        public void trigger(string[] args)
        {
            for (int i = 0; i < args.Length; i += 2)
            {
                string param = args[i];
                string value = i < args.Length ? args[i + 1] : "";
                ParamsCallback pcb = getParams(args[i]);
                if (null == pcb)
                    Console.Write(string.Format("Command {0} Invalid", param));
                pcb.callBack.Invoke(value);
            }
        }

        public ParamsCallback getParams(string key)
        {
            ParamsCallback pcb = null;
            _paramsDict.TryGetValue(key, out pcb);
            return pcb;
        }
    }
}
