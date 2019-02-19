using GanttChart.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GanttChart.Views
{
    /// <summary>
    /// GanttChartView.xaml の相互作用ロジック
    /// </summary>
    public partial class GanttChartView : UserControl
    {
        public GanttChartView()
        {
            InitializeComponent();
        }

        private async void DataGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.DataContext is GanttChartViewModel vm)
            {
                if (sender is DataGrid dg)
                {
                    // MEMO : 描画が間に合わないので少し待つ
                    // スペックにより調整が必要かも
                    await Task.Delay(500);
                    if (!double.IsNaN(dg.Columns[dg.FrozenColumnCount].ActualWidth))
                    {
                        // MEMO : スクロールバーの端のサイズ分固定値でマイナス
                        vm.TextColumnWidth = dg.Columns[0].ActualWidth + dg.Columns[1].ActualWidth - 17.0;
                        vm.TimeColumnWidth = dg.Columns[2].ActualWidth;
                        vm.Items.ForEach(i => i.TimeColumnWidth = dg.Columns[dg.FrozenColumnCount].ActualWidth);
                    }
                }
            }
        }
    }
}
