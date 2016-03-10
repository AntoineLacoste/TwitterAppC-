using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterUniversalApp.ViewModels
{
    class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<ConnectionPageViewModel>();
            SimpleIoc.Default.Register<TimeLinePageViewModel>();
        }

        public ConnectionPageViewModel Connection
        {
            get { return ServiceLocator.Current.GetInstance<ConnectionPageViewModel>(); }
        }
        public TimeLinePageViewModel TimeLine
        {
            get { return ServiceLocator.Current.GetInstance<TimeLinePageViewModel>(); }
        }
    }
}
