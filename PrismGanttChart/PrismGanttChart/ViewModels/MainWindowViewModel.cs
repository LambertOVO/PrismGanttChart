using Prism.Mvvm;

namespace PrismGanttChart.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "WPF GanttChart";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
