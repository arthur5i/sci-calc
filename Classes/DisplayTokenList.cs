using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using static ScientificCalculator.PrecessionRule;

namespace ScientificCalculator
{
    internal class DisplayTokenList : INotifyPropertyChanged
    {
        private List<DisplayToken> mainList { get; set; }
        private int _currentToken = 0;
        // check to see if this cacheing is handled by code optimisation
        private DisplayToken? _currentTokenCache;
        public DisplayToken? CurrentToken {
            get
            {
                if (mainList.Count == 0)
                {
                    return null;
                }
                else if (_currentTokenCache != null)
                {
                    return _currentTokenCache;
                }
                else
                {
                    DisplayToken currentTokenObj = mainList[_currentToken];
                    if (currentTokenObj is IContainerToken)
                    {
                        IContainerToken containerToken = (IContainerToken)currentTokenObj;
                        DisplayToken tokenOut = containerToken.InnerList.CurrentToken;
                        _currentTokenCache = tokenOut;
                        return tokenOut;
                    }
                    else
                    {
                        _currentTokenCache = currentTokenObj;
                        return currentTokenObj;
                    }
                }
            }
        }
        public string Content
        {
            get
            {
                return ToString();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public DisplayTokenList()
        {
            mainList = new List<DisplayToken>();
        }

        public void AddToken(DisplayToken token)
        {
            switch (token.PrecessionRule)
            {
                case None:
                    break;
                case AfterExpression:
                    if (CurrentToken == null || CurrentToken.IsExpression == false) return;
                    break;
                case AfterDigit:
                    if (CurrentToken is not DigitToken) return;
                    break;
            }

            mainList.Add(token);
            SetCurrentTokenToEnd();
            OnPropertyChanged();
        }

        public double GiveOutputValue()
        {
            // This function will trigger all of the calculations and give a resulting float value
            return 1;
        }

        protected virtual void OnPropertyChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Content"));
            }
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

        public void NextToken()
        {
            _currentTokenCache = null;
            _currentToken++;
            if (_currentToken >= mainList.Count)
            {
                _currentToken = 0;
            }
        }

        public void PreviousToken()
        {
            _currentTokenCache = null;
            _currentToken--;
            if (_currentToken < 0)
            {
                _currentToken = mainList.Count - 1;
            }
        }

        // If the current token is a container, make sure this goes to the end of the container
        public void SetCurrentTokenToEnd()
        {
            _currentTokenCache = null;
            _currentToken = mainList.Count - 1;
        }
    }
}
