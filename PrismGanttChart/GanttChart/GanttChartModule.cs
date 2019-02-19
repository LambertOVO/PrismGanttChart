using GanttChart.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GanttChart
{
    public class GanttChartModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(GanttChartView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}