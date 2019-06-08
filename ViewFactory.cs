using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    abstract class ViewFactory
    {
      public abstract List<Week> CreateWeeks(List<Day> days);
      public abstract List<Day> CreateDays();
      public abstract List<Day> LoadDays(DateTime date);
      public abstract List<Month> CreateMonths(List<Day> days);
      public abstract int DaysLeftInYear();
    }
    class NormalViewFactory : ViewFactory
    {
        private  int _daysLeftInYear;
        //adds days in year to weeks list
        public override List<Week> CreateWeeks(List<Day> _days)
        {
            List<Week> _weeks = new List<Week>();
            int _weeksLeftInYear, _daysInFirstWeek, _weekDay;
            _weekDay = 0;
            _weeksLeftInYear = ((int)_daysLeftInYear / 7);
            _daysInFirstWeek = (_daysLeftInYear - (_weeksLeftInYear * 7));
            //create first week
            if (_daysInFirstWeek != 0) { _weekDay = AddFirstWeek(_days, _weeks, _daysInFirstWeek, _weekDay); }
            //create rest of weeks
            for (int i = 1; i < _weeksLeftInYear; i++)
            {
                _weeks.Add(new Week());
                for (int j = 0; j < 7; j++)
                {
                    _weeks.ElementAt(i).Days.Add(_days.ElementAt(_weekDay));
                    _weekDay++;
                }
            }
            return _weeks;
        }

        private static int AddFirstWeek(List<Day> _days, List<Week> _weeks, int _daysInFirstWeek, int _weekDay)
        {
            _weeks.Add(new Week());
            for (int j = 0; j < _daysInFirstWeek; j++)
            {
                _weeks.ElementAt(0).Days.Add(_days.ElementAt(_weekDay));
                _weekDay++;
            }

            return _weekDay;
        }

        //adds days in year to months list
        public override List<Month> CreateMonths(List<Day> _days)
        {
            int _monthsLeftInYear,_monthDay,_daysInFMonth,_daysLeftInFirstMonth;
            List <Month>_months= new List<Month>();
            DateTime _month = DateTime.Now;
            _monthsLeftInYear = 12 - _month.Month;
            _daysInFMonth = DateTime.DaysInMonth(_month.Year, _month.Month);
            _month = DateTime.Now;
            _daysLeftInFirstMonth = _daysInFMonth - _month.Day+1;
            _monthDay = 0;
            //Days left in first month done first
            _months.Add(new Month(_month.ToString("MMM")));
            for (int n = 0; n < _daysLeftInFirstMonth; n++)
            {
                _months.ElementAt(0).Days.Add(_days.ElementAt(_monthDay));
                _monthDay++;
            }
            _month = _month.AddMonths(1);
            //rest of days added to months
            for (int l = 1; l < _monthsLeftInYear; l++)
            {
                _months.Add(new Month(_month.ToString("MMM")));
                int daysInMonth = DateTime.DaysInMonth(_month.Year, _month.Month);   //local var daysInMonth used to find n days in current month 
                for (int m = 0; m < daysInMonth; m++)
                {
                    _months.ElementAt(l).Days.Add(_days.ElementAt(_monthDay + daysInMonth));
                    _monthDay++;
                }
                _month = _month.AddMonths(1);
            }
            return _months;
        }
        //create days left in year and add to planner
        public override List<Day> CreateDays()
        {
            
            DateTime _date = DateTime.Now;
            return LoadDays(_date);
        }
        public override int DaysLeftInYear() {
            return _daysLeftInYear; 
        }
        public override List<Day> LoadDays(DateTime date) {

            List<Day> _days = new List<Day>();
            
            int _daysInYear;
            _daysInYear = DateTime.IsLeapYear(date.Year) ? 366 : 365; //https://stackoverflow.com/questions/7128399/c-sharp-how-many-days-left-in-year-from-datetime-now
            _daysLeftInYear = _daysInYear - date.DayOfYear;
            for (int i = 0; i < _daysLeftInYear; i++)
            {
                _days.Add(new Day(date));
                date = date.AddDays(1);
            }
            return _days;
        }
    }
}
