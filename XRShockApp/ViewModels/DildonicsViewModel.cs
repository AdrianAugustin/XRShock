using ShockCollar.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShockCollar.ViewModels
{
    internal class DildonicsViewModel : BaseViewModel
    {
        public DildonicsViewModel(DildonicsClient client)
        {
         this.client = client;

        }
      private DildonicsClient client;
        private bool _buttplugIOClientActive = false;
        public bool ButtplugIOClientActive
        {
            get { return _buttplugIOClientActive; }
            set
            {
                if (_buttplugIOClientActive == value)
                    return;

                _buttplugIOClientActive = value;
                OnPropertyChanged(nameof(ButtplugIOClientActive));
            }
        }

        private List<string> _deviceList = new List<string>();
        public List<string> DeviceList
        {
            get { return _deviceList; }
            set
            {
                if (_deviceList == value)
                    return;

                _deviceList = value;
                OnPropertyChanged(nameof(DeviceList));
            }
        }

        private int _deviceSelected = 0;
        public int DeviceSelected
        {
            get { return _deviceSelected; }
            set
            {
                if (_deviceSelected == value)
                    return;

                _deviceSelected = value;
                OnPropertyChanged(nameof(DeviceSelected));
            }
        }



    }
}
