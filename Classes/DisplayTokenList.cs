using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static ScientificCalculator.PrecessionRule;

namespace ScientificCalculator
{
    internal class DisplayTokenList : INotifyPropertyChanged, IEnumerable<DisplayToken>
    {
        #region Properties

        protected List<DisplayToken> mainList;

        private int _currentPosition = 0;
        public int CurrentPosition { get; }

        // check to see if this cacheing is handled by code optimisation
        private DisplayToken? currentTokenCache;


        public DisplayToken? CurrentToken {
            get
            {
                if (mainList.Count == 0)
                {
                    return null;
                }
                else if (currentTokenCache == null)
                {
                    currentTokenCache = mainList[_currentPosition];
                }

                return currentTokenCache;
            }
        }

        private bool? isOnContainerCache = null;

        /*public String Content
        {
            get
            {
                return ToString();
            }
        }*/

        #endregion


        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion


        #region Methods

        public DisplayTokenList()
        {
            mainList = new List<DisplayToken>();
        }

        /*
        public DisplayTokenList GetWorkingList(bool secondDeepest = false)
        {
            if (CurrentToken != null && IsOnContainer())
            {
                IContainerDisplayToken container = (IContainerDisplayToken)CurrentToken;
                DisplayTokenList innerList = container.InnerList;
                if (secondDeepest && innerList.IsOnContainer() == false)
                {
                    return innerList;
                }

                return innerList.GetWorkingList();
            }
            else
            {
                return this;
            }
        }*/

        private bool IsOnContainer()
        {
            if (currentTokenCache == null || isOnContainerCache == null)
            {
                isOnContainerCache = (CurrentToken is IContainerDisplayToken);
            }

            return (bool)isOnContainerCache;
        }

        private DisplayTokenList GetInnerList()
        {
            try
            {
                IContainerDisplayToken container = (IContainerDisplayToken)CurrentToken!;
                return container.InnerList;
            }
            catch (InvalidCastException)
            {
                throw;
            }
        }

        public void AddToken(DisplayToken token)
        {
            if (IsOnContainer())
            {
                DisplayTokenList innerList = GetInnerList();
                innerList.AddToken(token);
                OnPropertyChanged();
                return;
            }

            switch (token.Behaviour.PrecessionRule)
            {
                case None:
                    break;
                case AfterExpression:
                    if (CurrentToken == null || CurrentToken.Behaviour.IsExpression == false) return;
                    break;
                case AfterDigit:
                    if (CurrentToken is not DigitToken) return;
                    break;
            }

            if (mainList.Count > 0)
            {
                mainList.Insert(_currentPosition + 1, token);
            }
            else
            {
                mainList.Add(token);
            }

            NextToken();
            OnPropertyChanged();
        }


        public void RemoveCurrentToken()
        {
            if (IsOnContainer())
            {
                DisplayTokenList innerList = GetInnerList();
                innerList.RemoveCurrentToken();
                OnPropertyChanged();
                return;
            }

            if (CurrentToken != null)
            {
                mainList.RemoveAt(_currentPosition);
                PreviousToken();
                OnPropertyChanged();
            }
        }


        public void ClearTokens()
        {
            mainList.Clear();
            currentTokenCache = null;
            _currentPosition = 0;
            OnPropertyChanged();
        }


        /**
         * <summary>
         * Advances the token pointer forwards.
         * </summary>
         */
        public void NextToken()
        {
            MoveToken(1);
        }


        /**
         * <summary>
         * Advances the token pointer forwards.
         * </summary>
         */
        public void PreviousToken()
        {
            MoveToken(-1);
        }


        /**
         * <summary>
         * Internal function for moving token pointer.
         * Returns bool based on whether movement indicates that the pointer has escaped a container token
         * </summary>
         */
        protected bool MoveToken(int amount)
        {
            if (IsOnContainer())
            {
                DisplayTokenList innerList = GetInnerList();
                bool escaped = innerList.MoveToken(amount);

                if (escaped == false)
                {
                    return false;
                }
            }

            ClearCache();

            _currentPosition += amount;

            // Everything that runs after this point will be pointing to a different token, hence why IsOnContainer is run twice
            if (_currentPosition >= mainList.Count)
            {
                _currentPosition = 0;
                OnPropertyChanged();
                return true;
            }
            else if (_currentPosition < 0)
            {
                _currentPosition = mainList.Count - 1;
                OnPropertyChanged();
                return true;
            }
            else if (IsOnContainer())
            {
                IContainerDisplayToken container = (IContainerDisplayToken)CurrentToken!;
                if (amount >= 1)
                {
                    container.InnerList.SetCurrentTokenToStart();
                }
                else if (amount <= -1)
                {
                    container.InnerList.SetCurrentTokenToEnd();
                }
                OnPropertyChanged();
                return false;
            }
            else
            {
                OnPropertyChanged();
                return false;
            }
        }


        public void SetCurrentTokenToStart()
        {
            ClearCache();
            _currentPosition = 0;
        }


        public void SetCurrentTokenToEnd()
        {
            ClearCache();
            _currentPosition = mainList.Count - 1;
        }


        private void ClearCache()
        {
            currentTokenCache = null;
            isOnContainerCache = null;
        }


        public bool isCurrentPosition(int index)
        {
            return (index == _currentPosition);
        }


        /**
         * <summary>
         * Temporary solution for displaying the input display.
         * Future solution will either use LaTeX or a pixel art render engine
         * </summary>
         */
        public String ToString()
        {
            StringBuilder strOut = new StringBuilder();
            int index = 0;
            foreach (DisplayToken token in mainList)
            {
                if (token is IContainerDisplayToken)
                {
                    strOut.Append(token.DisplayValue);
                    IContainerDisplayToken container = (IContainerDisplayToken)token;
                    strOut.Append("[");
                    strOut.Append(container.InnerList.ToString());
                    strOut.Append("]");
                }
                else
                {
                    if (index == _currentPosition)
                    {
                        strOut.Append("<Underline>");
                        strOut.Append(token.DisplayValue);
                        strOut.Append("</Underline>");
                    }
                    else
                    {
                        strOut.Append(token.DisplayValue);
                    }
                }

                index++;
            }
            
            return strOut.ToString();
        }

        #endregion

        #region INotifyPropertyChanged
        protected virtual void OnPropertyChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Content"));
            }
        }
        #endregion

        #region IEnumerable
        public IEnumerator<DisplayToken> GetEnumerator()
        {
            return new DisplayTokenListEnum(mainList);
        }
        private IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }
        #endregion
    }

    internal class DisplayTokenListEnum : IEnumerator<DisplayToken>
    {
        private List<DisplayToken> mainList;
        private int currentPosition = -1;

        public DisplayToken Current
        {
            get
            {
                return mainList[currentPosition];
            }
        }
        object IEnumerator.Current
        {
            get { return Current; }
        }

        public DisplayTokenListEnum(List<DisplayToken> mainList)
        {
            this.mainList = mainList;
        }

        public bool MoveNext()
        {
            return (++currentPosition < mainList.Count);
        }

        public void Reset()
        {
            currentPosition = -1;
        }

        void IDisposable.Dispose() { }
    }
}
