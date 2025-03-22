using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScientficCalculator
{
    internal class CalculatorMain
    {
        private DisplayTokenList mainDisplayTokenList;

        public string Display
        {
            get
            {
                return mainDisplayTokenList.ToString();
            }
        }

        public CalculatorMain()
        {
            mainDisplayTokenList = new DisplayTokenList();
        }
    }
}
