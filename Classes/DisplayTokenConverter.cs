using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScientificCalculator
{
    internal class DisplayTokenConverter
    {
        private static DisplayTokenConverter? _instance;

        public static DisplayTokenConverter Instance {
            get {
                if (_instance == null)
                {
                    _instance = new DisplayTokenConverter();
                }
                return _instance;
            }
        }

        private DisplayTokenConverter() { }

        public LogicTokenList Convert(DisplayTokenList listIn)
        {
            foreach (DisplayToken in listIn)
            {

            }
        }
    }
}
