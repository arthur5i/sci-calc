using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScientficCalculator
{
    internal class DisplayTokenList
    {
        private List<DisplayToken> mainList;

        public DisplayTokenList()
        {
            mainList = new List<DisplayToken>();
        }

        public void AddToken(DisplayToken token)
        {
            mainList.Add(token);
        }

        public double GiveOutputValue()
        {
            // This function will trigger all of the calculations and give a resulting float value
            return 1;
        }

        public override string ToString()
        {
            StringBuilder strOut = new StringBuilder();
            foreach (DisplayToken token in mainList)
            {
                strOut.Append(token.DisplayValue);
            }
            return strOut.ToString();
        }
    }
}
