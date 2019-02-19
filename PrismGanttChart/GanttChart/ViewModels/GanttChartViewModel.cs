using GanttChart.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace GanttChart.ViewModels
{
    class GanttChartViewModel : BindableBase
    {
        #region Fields

        private readonly int _drawStartHour;

        private readonly Color[] _colorList;

        #endregion

        #region Constructors

        public GanttChartViewModel()
        {
            this._drawStartHour = 8;
            this._colorList = new Color[]
                {
                    Colors.Blue,
                    Colors.BlueViolet,
                    Colors.Green,
                    Colors.SlateBlue,
                };

            this.Draw();
        }

        #endregion

        #region Properties

        private ObservableCollection<int> _headers;

        public ObservableCollection<int> Headers
        {
            get { return _headers; }
            set { SetProperty(ref _headers, value); }
        }

        private bool _isExpand;

        public bool IsExpand
        {
            get { return _isExpand; }
            set
            {
                if (SetProperty(ref _isExpand, value))
                {
                    CollectionViewSource.GetDefaultView(Items).Filter = p =>
                    {
                        bool result = true;
                        if (p is ChartItem item && this.IsExpand)
                        {
                            result = item.Level == 0;
                        }
                        return result;
                    };
                }
            }
        }

        private double _textColumnWidth;

        public double TextColumnWidth
        {
            get { return _textColumnWidth; }
            set { SetProperty(ref _textColumnWidth, value); }
        }

        private double _timeColumnWidth;

        public double TimeColumnWidth
        {
            get { return _timeColumnWidth; }
            set
            {
                if (SetProperty(ref _timeColumnWidth, value))
                {
                    this.Draw();
                }
            }
        }

        private List<ChartItem> _items = new List<ChartItem>();

        public List<ChartItem> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        #endregion

        #region Methods

        protected void Draw()
        {
            var headers = Enumerable.Range(this._drawStartHour, 24).ToArray();
            for (int i = 0; i < headers.Length; i++)
            {
                headers[i] = headers[i] >= 24 ? headers[i] - 24 : headers[i];
            }

            this.Headers = new ObservableCollection<int>(headers);

            var ci = 0;
            var target = new DateTime(2018, 12, 1, this._drawStartHour, 0, 0);
            var items = new List<ChartItem>()
            {
                new ChartItem()
                {
                    DrawStartHour = this._drawStartHour,
                    Caption = "○○店",
                    StartTime = new DateTime(2018, 12, 1, 9, 30, 0),
                    EndTime = new DateTime(2018, 12, 1, 18, 45, 0),
                    Color = this._colorList[ci],
                    Level = 0,
                },
                new ChartItem()
                {
                    DrawStartHour = this._drawStartHour,
                    Caption = "スタッフ-1",
                    StartTime = new DateTime(2018, 12, 1, 9, 30, 0),
                    EndTime = new DateTime(2018, 12, 1, 12, 0, 0),
                    Color = this._colorList[ci],
                    Level = 1,
                },
                new ChartItem()
                {
                    DrawStartHour = this._drawStartHour,
                    Caption = "スタッフ-2",
                    StartTime = new DateTime(2018, 12, 1, 12, 0, 0),
                    EndTime = new DateTime(2018, 12, 1, 15, 0, 0),
                    Color = this._colorList[ci],
                    Level = 1,
                },
                new ChartItem()
                {
                    DrawStartHour = this._drawStartHour,
                    Caption = "スタッフ-3",
                    StartTime = new DateTime(2018, 12, 1, 13, 30, 0),
                    EndTime = new DateTime(2018, 12, 1, 17, 30, 0),
                    Color = this._colorList[ci],
                    Level = 1,
                },
            };

            ci++;
            items.Add(new ChartItem()
            {
                DrawStartHour = this._drawStartHour,
                Caption = "緊急呼出",
                StartTime = new DateTime(2018, 12, 1, 15, 30, 0),
                EndTime = new DateTime(2018, 12, 1, 22, 10, 0),
                Color = this._colorList[ci],
                Level = 0,
            });

            ci++;
            items.Add(new ChartItem()
            {
                DrawStartHour = this._drawStartHour,
                Caption = "開始終了が同じ時間帯はごめん",
                StartTime = new DateTime(2018, 12, 1, 9, 10, 0),
                EndTime = new DateTime(2018, 12, 1, 9, 25, 0),
                Color = this._colorList[ci],
                Level = 0,
            });

            ci++;
            items.Add(new ChartItem()
            {
                DrawStartHour = this._drawStartHour,
                Caption = "24時間シフト",
                StartTime = new DateTime(2018, 12, 1, 6, 00, 0),
                EndTime = new DateTime(2018, 12, 2, 6, 0, 0),
                Color = this._colorList[ci],
                Level = 0,
            });

            foreach (var i in Enumerable.Range(1, 12))
            {
                items.Add(
                    new ChartItem()
                    {
                        DrawStartHour = this._drawStartHour,
                        Caption = "スタッフ" + i.ToString("00"),
                        StartTime = new DateTime(2018, 12, 1, i + this._drawStartHour, 60 - i * 3, 0),
                        EndTime = new DateTime(2018, 12, 1, i + this._drawStartHour + 2, i * 4, 0),
                        Level = 1,
                        Color = this._colorList[ci],
                    });
            }

            foreach (var item in items)
            {
                item.DrawStartTime = item.StartTime < target ? target : item.StartTime;
                item.DrawEndTime = item.EndTime > target.AddHours(24) ? target.AddHours(24) : item.EndTime;
                item.SetWidthAndAlignment();
                item.SetTimes();
            }

            this.Items?.Clear();
            this.Items = items;

        }

        #endregion
    }
}
