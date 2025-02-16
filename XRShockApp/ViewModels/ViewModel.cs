using ShockCollar.Modules;
using ShockCollar.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShockCollar.ViewModels
{

    public class ViewModel :BaseViewModel 
    {

       static string ActiveButtonColor = "#FF6DEEFF";
        static string InactiveButtonColor = "#FFDDDDDD";
        public OSCConfigPage oscconfigWindow;
        public MainProgram mainProgram;

        public ViewModel()
        {
            _oSCCheckboxCheckedCommand = new DelegateCommand(OSCCheckboxChecked);
            _oSCRepeaterCheckboxCheckedCommand = new DelegateCommand(OSCRepeaterCheckboxChecked);
            _oSCLeashCheckedCommand = new DelegateCommand(OSCLeashChecked);
            _executeCommand = new DelegateCommand(ExecuteFunction);
            _refreshSerialCommand = new DelegateCommand(RefreshSerial);
            _changeModeCommand = new DelegateCommand(ChangeMode);
            _shockCommand = new DelegateCommand(ShockMode);
            _vibrateCommand= new DelegateCommand(VibrateMode);
            _toneCommand= new DelegateCommand(ToneMode);
            _openOSCSettingsCommand = new DelegateCommand(OpenOSCSettings);

            
      
        }


        public void RefreshSettings()
        {
            VibrateMode(null);
        }
        #region Commands

        private DelegateCommand _oSCCheckboxCheckedCommand;
        public DelegateCommand OSCCheckboxCheckedCommand => _oSCCheckboxCheckedCommand;
        private DelegateCommand _oSCLeashCheckedCommand;
        public DelegateCommand OSCLeashCheckedCommand => _oSCLeashCheckedCommand;
        private DelegateCommand _oSCRepeaterCheckboxCheckedCommand;
        public DelegateCommand OSCRepeaterCheckboxCheckedCommand => _oSCRepeaterCheckboxCheckedCommand;
        private DelegateCommand _executeCommand;
        public DelegateCommand ExecuteCommand => _executeCommand;
        private DelegateCommand _refreshSerialCommand;
        public DelegateCommand RefreshSerialCommand => _refreshSerialCommand;
        private DelegateCommand _changeModeCommand;
        public DelegateCommand ChangeModeCommand => _changeModeCommand;

    
        private DelegateCommand _vibrateCommand;
        public DelegateCommand VibrateCommand => _vibrateCommand;
        private DelegateCommand _shockCommand;
        public DelegateCommand ShockCommand => _shockCommand;
        private DelegateCommand _blinkCommand;
        public DelegateCommand BlinkCommand => _blinkCommand;
        private DelegateCommand _toneCommand;
        public DelegateCommand ToneCommand => _toneCommand;

        private DelegateCommand _openOSCSettingsCommand;
        public DelegateCommand OpenOSCSettingsCommand => _openOSCSettingsCommand; 


        private void RefreshSerial(object obj)
        {
          mainProgram.RefreshSerialConnections();
        }
        private void OpenOSCSettings(object obj)
        {
            oscconfigWindow= new OSCConfigPage();
            
          
        }
        public void OnWindowClose(object obj)
        {
            mainProgram.SaveUserSettings();
        }
        private void ChangeMode(object obj)
        {
            //Change Color
        }
        public void OSCCheckboxChecked(object obj)
        {
            mainProgram.ChangeOSCStatus(OSCActiveChecked);
        }
        private void ExecuteFunction(object obj)
        {
            //DildonicsClient client= new DildonicsClient();
           // DildonicsClient.RunExample().Wait();
            mainProgram.ExecuteAction();

          //  MainProgram.oscAPI.aaroStart();
        }
        private void VibrateMode(object obj)
        {
            MainProgram.collarMode = CollarMode.Vibrate;
            VibrateBackColor = ActiveButtonColor;
            ShockBackColor = InactiveButtonColor;
            LEDBackColor = InactiveButtonColor;
            SoundBackColor = InactiveButtonColor;
            PowerLevelSlider = MainProgram.GetCurrentPowerLevel();
            CooldownSlider = MainProgram.GetCurrentCooldown();
        }
        private void ShockMode(object obj)
        {
            MainProgram.collarMode=CollarMode.Shock;
            VibrateBackColor = InactiveButtonColor;
            ShockBackColor = ActiveButtonColor;
            LEDBackColor = InactiveButtonColor;
            SoundBackColor = InactiveButtonColor;
            PowerLevelSlider = MainProgram.GetCurrentPowerLevel();
            CooldownSlider = MainProgram.GetCurrentCooldown();
        }
        private void ToneMode(object obj)
        {
            MainProgram.collarMode = CollarMode.Tone;
            VibrateBackColor = InactiveButtonColor;
            ShockBackColor = InactiveButtonColor;
            LEDBackColor = InactiveButtonColor;
            SoundBackColor = ActiveButtonColor;
            PowerLevelSlider = MainProgram.GetCurrentPowerLevel();
            CooldownSlider = MainProgram.GetCurrentCooldown();
        }
       
        public void OSCLeashChecked(object obj)
        {
            if (OSCLeashActiveChecked)
            {
                mainProgram.oscAPI.LeashActive = true;
            }
            else
            {
                mainProgram.oscAPI.LeashActive = false;
            }
        }
        public void OSCRepeaterCheckboxChecked(object obj)
        {
            mainProgram.ChangeOSCRepeaterStatus(OSCRepeaterChecked);
        }
        #endregion

        private bool _powerSliderVisible = true;
        public bool PowerSliderVisible
        {
            get { return _powerSliderVisible; }
            set
            {
                if (_powerSliderVisible == value)
                    return;

                _powerSliderVisible = value;
                OnPropertyChanged(nameof(PowerSliderVisible));
            }
        }

        private bool _oSCActiveChecked = true;
        public bool OSCActiveChecked
        {
            get { return _oSCActiveChecked; }
            set
            {
                if (_oSCActiveChecked == value)
                    return;

                _oSCActiveChecked = value;
                OnPropertyChanged(nameof(OSCActiveChecked));
                OSCEnabled = value;
            }
        }

        private bool _oSCRepeaterChecked = false;
        public bool OSCRepeaterChecked
        {
            get { return _oSCRepeaterChecked; }
            set
            {
                if (_oSCRepeaterChecked == value)
                    return;

                _oSCRepeaterChecked = value;
                OnPropertyChanged(nameof(OSCRepeaterChecked));
            }
        }

        private bool _OSCLeashActiveChecked = false;
        public bool OSCLeashActiveChecked
        {
            get { return _OSCLeashActiveChecked; }
            set
            {
                if (_OSCLeashActiveChecked == value)
                    return;

                _OSCLeashActiveChecked = value;
                OnPropertyChanged(nameof(OSCLeashActiveChecked));
            }
        }

        private bool _OSCEnabled = true;
        public bool OSCEnabled
        {
            get { return _OSCEnabled; }
            set
            {
                if (_OSCEnabled == value)
                    return;

                _OSCEnabled = value;
                OnPropertyChanged(nameof(OSCEnabled));
       
            }
        }

        private List<string> _comPortList = new List<string>();
        public List<string> ComPortList
        {
            get { return _comPortList; }
            set
            {
                if (_comPortList == value)
                    return;

                _comPortList = value;
                OnPropertyChanged(nameof(ComPortList));
            }
        }

        private int _comPortSelected = 0;
        public int ComPortSelected
        {
            get { return _comPortSelected; }
            set
            {
                if (_comPortSelected == value)
                    return;

                _comPortSelected = value;
                OnPropertyChanged(nameof(ComPortSelected));
            }
        }

        private int _powerLevelSlider = 100;
        public int PowerLevelSlider
        {
            get { return _powerLevelSlider; }
            set
            {
                if (_powerLevelSlider == value)
                    return;

                _powerLevelSlider = value;
                MainProgram.ChangePowerlevel(value);
                OnPropertyChanged(nameof(PowerLevelSlider));
            }
        }

        private int _cooldownSlider = 2000;
        public int CooldownSlider
        {
            get { return _cooldownSlider; }
            set
            {
                if (_cooldownSlider == value)
                    return;

                _cooldownSlider = value;
                MainProgram.ChangeCooldown(value);
                OnPropertyChanged(nameof(CooldownSlider));
            }
        }

        private bool _keepAliveCheckbox = true;
        public bool KeepAliveCheckbox
        {
            get { return _keepAliveCheckbox; }
            set
            {
                if (_keepAliveCheckbox == value)
                    return;

                _keepAliveCheckbox = value;
                OnPropertyChanged(nameof(KeepAliveCheckbox));
            }
        }

        private string _buttonColor = ActiveButtonColor;
        public string ButtonColor
        {
            get => _buttonColor;
            set
            {
                if(_buttonColor == value)
                {
                    return;
                }
                _buttonColor = value;
                OnPropertyChanged(nameof(ButtonColor));
            }
        }
      
        private int _powerLevel = 0;
        public int PowerLevel
        {
            get { return _powerLevel; }
            set
            {
                if (_powerLevel == value)
                    return;

                _powerLevel = value;
                OnPropertyChanged(nameof(PowerLevel));
            }
        }

        private int _CooldownTime = 0;
        public int CooldownTime
        {
            get { return _CooldownTime; }
            set
            {
                if (_CooldownTime == value)
                    return;

                _CooldownTime = value;
                OnPropertyChanged(nameof(CooldownTime));
            }
        }

        private string _VibrateBackColor = ActiveButtonColor;
        public string VibrateBackColor
        {
            get { return _VibrateBackColor; }
            set
            {
                if (_VibrateBackColor == value)
                    return;

                _VibrateBackColor = value;
                OnPropertyChanged(nameof(VibrateBackColor));
            }
        }

        private string _ShockBackColor = InactiveButtonColor;
        public string ShockBackColor
        {
            get { return _ShockBackColor; }
            set
            {
                if (_ShockBackColor == value)
                    return;

                _ShockBackColor = value;
                OnPropertyChanged(nameof(ShockBackColor));
            }
        }

        private string _LEDBackColor = InactiveButtonColor;
        public string LEDBackColor
        {
            get { return _LEDBackColor; }
            set
            {
                if (_LEDBackColor == value)
                    return;

                _LEDBackColor = value;
                OnPropertyChanged(nameof(LEDBackColor));
            }
        }

        private string _SoundBackColor = InactiveButtonColor;
        public string SoundBackColor
        {
            get { return _SoundBackColor; }
            set
            {
                if (_SoundBackColor == value)
                    return;

                _SoundBackColor = value;
                OnPropertyChanged(nameof(SoundBackColor));
            }
        }


        private bool _showInTaskBar = true;
        public bool ShowInTaskbar
        {
            get { return _showInTaskBar; }
            set
            {
                if (_showInTaskBar == value)
                    return;

                _showInTaskBar = value;
                OnPropertyChanged(nameof(ShowInTaskbar));
            }
        }


        private WindowState _windowState =new WindowState();
        public WindowState WindowState
        {
            get { return _windowState; }
            set
            {
                if (_windowState == value)
                    return;
           
                _windowState = value;
                OnPropertyChanged(nameof(WindowState));
                //ShowInTaskbar = true;
                ShowInTaskbar = value != WindowState.Minimized;
            }
        }


        //private float _leashStrength = 1;
        //public float LeashStrength
        //{
        //    get { return _leashStrength; }
        //    set
        //    {
        //        if (_leashStrength == value)
        //            return;


        //        if (value > 1)
        //        {
        //            value = 1;
        //        }
        //        else if (value < 0)
        //        {
        //            value = 0;
        //        }


        //        _leashStrength = value;
        //        OnPropertyChanged(nameof(LeashStrength));
        //        mainProgram.oscAPI.leash.strength = value;
        //    }
        //}

        //private float _leashDeadzone = 0.1F;
        //public float LeashDeadzone
        //{
        //    get { return _leashDeadzone; }
        //    set
        //    {
        //        if (_leashDeadzone == value)
        //            return;
        //        if (value > 1)
        //        {
        //            value = 1;
        //        }
        //        else if (value < 0)
        //        {
        //            value = 0;
        //        }
        //        _leashDeadzone = value;
        //        OnPropertyChanged(nameof(LeashDeadzone));
        //        mainProgram.oscAPI.leash.deadzone = value;
        //    }
        //}





    }

}
