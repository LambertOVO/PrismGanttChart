using Prism.Mvvm;
using System;
using System.Windows;
using System.Windows.Media;

namespace GanttChart.Models
{
    class ChartItem : BindableBase
    {
        #region Constant Fields

        private const int TimeLength = 24;

        #endregion

        #region Properties

        public int DrawStartHour { get; set; }

        public string Caption { get; set; }

        public int Level { get; set; }

        public DateTime DrawStartTime { get; set; }

        public DateTime DrawEndTime { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string WorkTime =>
            this.StartTime.ToShortTimeString() + " - " +
            this.EndTime.ToShortTimeString();

        public Color Color { get; set; }

        public bool[] Times { get; private set; }

        public string Details =>
            this.Caption + Environment.NewLine +
            this.StartTime.ToString("yyyy/MM/dd HH:mm") + " ～ " +
            this.EndTime.ToString("yyyy/MM/dd HH:mm");

        public HorizontalAlignment[] HAlignment { get; private set; }

        public double[] TimeWidth { get; private set; }

        private double _timeColumnWidth;

        public double TimeColumnWidth
        {
            get { return _timeColumnWidth; }
            set
            {
                if (SetProperty(ref _timeColumnWidth, value))
                {
                    this.SetWidthAndAlignment();
                }
            }
        }

        #endregion

        #region Methods

        public void SetWidthAndAlignment()
        {
            if (this.TimeColumnWidth == 0)
            {
                return;
            }

            var startpos = this.DrawStartTime.Hour >= this.DrawStartHour
                ? this.DrawStartTime.Hour - this.DrawStartHour
                : (this.DrawStartTime.Hour + TimeLength) - this.DrawStartHour;
            var endpos = this.DrawEndTime.Hour >= this.DrawStartHour
                ? this.DrawEndTime.Hour - this.DrawStartHour
                : (this.DrawEndTime.Hour + TimeLength) - this.DrawStartHour;
            if (this.DrawEndTime.Minute == 0)
            {
                endpos--;   // 終了位置が0分の場合は-1
            }

            var tw = new double[TimeLength];
            var ha = new HorizontalAlignment[TimeLength];
            for (int i = 0; i < TimeLength; i++)
            {
                if (i == startpos)
                {
                    if (startpos == endpos)
                    {
                        // 開始終了が同じ時間帯
                        var diff = this.DrawEndTime.Minute - this.DrawStartTime.Minute < 0
                            ? this.DrawEndTime.Minute - this.DrawStartTime.Minute + 60  // 終了分＜開始分の場合60加算
                            : this.DrawEndTime.Minute - this.DrawStartTime.Minute;
                        tw[i] = this.TimeColumnWidth * ((double)diff / 60);
                    }
                    else
                    {
                        tw[i] = this.TimeColumnWidth * ((60 - (double)this.DrawStartTime.Minute) / 60);
                    }
                }
                else if (i == endpos && this.DrawEndTime.Minute > 0)
                {
                    tw[i] = this.TimeColumnWidth * ((double)this.DrawEndTime.Minute / 60);
                }
                else if (i < startpos || i > endpos)
                {
                    tw[i] = 0.0;
                }
                else
                {
                    tw[i] = this.TimeColumnWidth;
                }

                if (i == startpos && startpos == endpos)
                {
                    // 開始終了が同じ時間帯
                    if (this.DrawStartTime.Minute == 0)
                    {
                        ha[i] = HorizontalAlignment.Left;
                    }
                    else if (this.DrawEndTime.Minute == 0)
                    {
                        ha[i] = HorizontalAlignment.Right;
                    }
                    else
                    {
                        ha[i] = HorizontalAlignment.Center;
                    }
                }
                else if (i == startpos)
                {
                    ha[i] = HorizontalAlignment.Right;
                }
                else if (i == endpos)
                {
                    ha[i] = HorizontalAlignment.Left;
                }
                else if (i < startpos || i > endpos)
                {
                    ha[i] = HorizontalAlignment.Center;
                }
                else
                {
                    ha[i] = HorizontalAlignment.Stretch;
                }
            }

            this.TimeWidth = tw;
            this.HAlignment = ha;
        }

        public void SetTimes()
        {
            var times = new bool[TimeLength];
            var startpos = this.DrawStartTime.Hour >= this.DrawStartHour
                ? this.DrawStartTime.Hour - this.DrawStartHour
                : (this.DrawStartTime.Hour + TimeLength) - this.DrawStartHour;
            var endpos = this.DrawEndTime.Hour >= this.DrawStartHour
                ? this.DrawEndTime.Hour - this.DrawStartHour
                : (this.DrawEndTime.Hour + TimeLength) - this.DrawStartHour;
            if (this.DrawEndTime.Minute == 0)
            {
                endpos--;   // 終了位置が0分の場合は-1
            }
            for (int i = 0; i < times.Length; i++)
            {
                if (i >= startpos && i <= endpos)
                {
                    times[i] = true;
                }
            }

            this.Times = times;
        }

        #endregion
    }
}
