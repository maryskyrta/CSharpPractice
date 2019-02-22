using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CSharpPractice1
{
    internal class DateViewModel:INotifyPropertyChanged, ILoaderOwner
    {
        private static readonly string[] WesternSigns = { "Aquarius", "Pisces", "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Sagittarius", "Capricorn" };
        private static readonly string[] ChineseSigns = { "Monkey","Rooster", "Dog", "Pig","Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat" };

        #region Fields
        private Visibility _loaderVisibility = Visibility.Hidden;
        private bool _isControlEnabled = true;
        private int _age;
        private string _westernSign="";
        private string _chineseSign="";
        private DateTime _birthday = DateTime.Today;

        private RelayCommand<object> _calculateCommand;   

        private RelayCommand<object> _closeCommand;
        #endregion

        #region Properties

        public string Age
        {
            get
            {
                return $"Your age: {_age}";
            }
            
        }

        public string WesternSign
        {
            get
            {
                return $"Your western zodiac sign: {_westernSign}";
            }
            set
            {
                _westernSign = value;
                OnPropertyChanged();
            }
        }

        public string ChineseSign
        {
            get
            {
                return $"Your Chinese zodiac sign: {_chineseSign}";
            }
            set
            {
                _chineseSign = value;
                OnPropertyChanged();
            }
        }

        public DateTime Birthday
        {
            get
            {
                return _birthday;
            }
            set
            {
                _birthday = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<Object> CalculateCommand
        {
            get
            {
                return _calculateCommand ?? (_calculateCommand = new RelayCommand<object>(o =>
                           {
                               try
                               {
                                   Calculate();
                               }
                               catch(Exception)
                               {
                                   MessageBox.Show("Invalid date input. Enter your birth date correctly.");
                               }
                           }));
            }
        }

        public RelayCommand<Object> CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new RelayCommand<object>(o => Environment.Exit(0)));
            }
        }

        public Visibility LoaderVisibility
        {
            get { return _loaderVisibility; }
            set
            {
                _loaderVisibility = value;
                OnPropertyChanged();
            }
        }

        public bool IsControlEnabled
        {
            get { return _isControlEnabled; }
            set
            {
                _isControlEnabled = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Constructor

        internal DateViewModel()
        {
            LoaderManager.Instance.Initialize(this);
        }

        #endregion

        #region Helping methods

        /// <summary>
        /// Sets values for fields
        /// </summary>
        private void Calculate()
        {
            int age = DateTime.Today.Year- _birthday.Year;
            if (DateTime.Today.DayOfYear < _birthday.DayOfYear)
                age--;
            if (age < 0 || age > 135)
                throw new ArgumentException("Illegal Argument Exception");
            _age = age;
           SetSigns();
        }

        /// <summary>
        /// Async method for setting signs
        /// </summary>
       private async void SetSigns()
       {
           LoaderManager.Instance.ShowLoader();
           await Task.Run(() => Thread.Sleep(5000));
           LoaderManager.Instance.HideLoader();
            if (DateTime.Today.DayOfYear == _birthday.DayOfYear)
               MessageBox.Show("Happy Birthday!");
           OnPropertyChanged(nameof(Age));
            WesternSign = SetWesternSign();
           ChineseSign = SetChineseSign();
        }

       /// <summary>
       /// Returns Western sign
       /// </summary>
       /// <returns></returns>
        private string SetWesternSign()
        {
            int day = _birthday.Day;
            int month = _birthday.Month;
            if (month == 1)
                return day < 20 ? WesternSigns[WesternSigns.Length-1] : WesternSigns[month - 1];
            if (month == 2)
                return day < 19 ? WesternSigns[month - 2] : WesternSigns[month-1];
            if (month > 2 && month < 7)
                return day < 21 ? WesternSigns[month - 2] : WesternSigns[month-1];
            if(month>6&&month<11)
                return day < 23 ? WesternSigns[month - 2] : WesternSigns[month - 1];
            return day < 22 ? WesternSigns[month - 2] : WesternSigns[month - 1];
        }

       /// <summary>
       /// Returns Chinese sign
       /// </summary>
       /// <returns></returns>
        private string SetChineseSign()
        {
            return ChineseSigns[_birthday.Year%12];
        }

        #endregion



     
    }
}
