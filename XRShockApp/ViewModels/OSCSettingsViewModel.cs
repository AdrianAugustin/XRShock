using ShockCollar.Structs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShockCollar.ViewModels
{
    public class OSCSettingsViewModel : BaseViewModel
    {
        public OSCSettingsViewModel()
        {
            _saveAndCloseCommand = new DelegateCommand(SaveAndClose);
        }
        private DelegateCommand _saveAndCloseCommand;
        public DelegateCommand SaveAndCloseCommand => _saveAndCloseCommand;

        private void SaveAndClose(object obj)
        {

        }


        private ObservableCollection<OSCListenerEntry> _oscListenerEntryList =new ObservableCollection<OSCListenerEntry>();
        public ObservableCollection<OSCListenerEntry> OscListenerEntryList
        {
            get { return _oscListenerEntryList; }
            set
            {
                if (_oscListenerEntryList == value)
                    return;

                _oscListenerEntryList = value;
                OnPropertyChanged(nameof(OscListenerEntryList));
            }
        }

        private int _ListenerPort = 0;
        public int ListenerPort
        {
            get { return _ListenerPort; }
            set
            {
                if (_ListenerPort == value)
                    return;

                _ListenerPort = value;
                OnPropertyChanged(nameof(ListenerPort));
            }
        }

        private int _SenderPort = 0;
        public int SenderPort 
        {
            get { return _SenderPort ; }
            set
            {
                if (_SenderPort  == value)
                    return;

                _SenderPort  = value;
                OnPropertyChanged(nameof(SenderPort ));
            }
        }


    }
}
