using XRShock.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XRShock
{
    public class MainProgram
    {
        public ViewModels.ViewModel VModel;
        public ArduinoAPI arduinoAPI;
        public OSCAPI oscAPI;
        public PiShockAPI piShockAPI;
        public static CollarMode collarMode = CollarMode.Vibrate;
        //internal DildonicsClient dildonicsClient;

        public MainProgram(ViewModels.ViewModel viewModel)
        {
            VModel = viewModel;
            oscAPI = new OSCAPI(this);

            RefreshSerialConnections();
            LoadUserSettings();
            piShockAPI = new PiShockAPI(PiShockName, PiShockApiKey, PiShockDeviceId, "SerialTest");
            ChangeOSCStatus(VModel.OSCEnabled);

            //dildonicsClient = new DildonicsClient(this);
            //try
            //{
            //    dildonicsClient.Run();

            //}
            //catch (Exception ex)
            //{

            //}
        }
        public void SaveUserSettings()
        {
            Properties.Settings.Default.LastShockCooldown = ShockCooldown;
            Properties.Settings.Default.LastShockIntensity = ShockPower;
            Properties.Settings.Default.LastVibrationCooldown = VibrateCooldown;
            Properties.Settings.Default.LastVibrationIntensity = VibratePower;
            Properties.Settings.Default.OSCActive = VModel.OSCActiveChecked;
            Properties.Settings.Default.OSCRepeaterActive = VModel.OSCRepeaterChecked;

            Properties.Settings.Default.OSCLeashActive = VModel.OSCLeashActiveChecked;

            Properties.Settings.Default.PiShockName = PiShockName;
            Properties.Settings.Default.PiShockApiKey = PiShockApiKey;
            Properties.Settings.Default.PiShockDeviceId = PiShockDeviceId;



            Properties.Settings.Default.Save();
        }
        public void LoadUserSettings()
        {
            ShockCooldown = Properties.Settings.Default.LastShockCooldown;
            ShockPower = Properties.Settings.Default.LastShockIntensity;
            VibrateCooldown = Properties.Settings.Default.LastVibrationCooldown;
            VibratePower = Properties.Settings.Default.LastVibrationIntensity;
            VModel.OSCActiveChecked = Properties.Settings.Default.OSCActive;


            //   VModel.OSCCheckboxChecked(null);
            VModel.OSCRepeaterChecked = Properties.Settings.Default.OSCRepeaterActive;
            oscAPI.Repeating = VModel.OSCRepeaterChecked;
            //    VModel.OSCRepeaterCheckboxChecked(null);
            VModel.OSCLeashActiveChecked = Properties.Settings.Default.OSCLeashActive;
            oscAPI.LeashActive = VModel.OSCLeashActiveChecked;
            //    VModel.OSCLeashChecked(null);
            PiShockDeviceId = Properties.Settings.Default.PiShockDeviceId;
            PiShockName = Properties.Settings.Default.PiShockName;
            PiShockApiKey = Properties.Settings.Default.PiShockApiKey;




            VModel.RefreshSettings();
        }
        public void RefreshSerialConnections()
        {

            Task.Run(() =>
            {
                if (arduinoAPI != null)
                {
                    arduinoAPI.ClosePort();
                    arduinoAPI = null;
                }
                VModel.ComPortList = ArduinoAPI.AutodetectArduinoPort();
                if (VModel.ComPortList.Count > 0)
                {
                    VModel.ComPortSelected = 0;
                    arduinoAPI = new ArduinoAPI(VModel.ComPortList[VModel.ComPortSelected]);
                }
            });
        }

        internal void ChangeOSCRepeaterStatus(bool oSCRepeaterChecked)
        {
            if (oSCRepeaterChecked)
            {
                oscAPI.Repeating = true;
            }
            else
            {
                oscAPI.Repeating = false;
            }
        }

        public static int VibrateCooldown = 200;
        public static int VibratePower = 100;
        public static int ShockCooldown = 3000;
        public static int ShockPower = 10;
        public static int ToneCooldown = 0;
        public static int TonePower = 100;
        public static string PiShockName = "x";
        public static string PiShockApiKey = "x";
        public static string PiShockDeviceId = "x";


        public static int GetCurrentPowerLevel()
        {
            switch (collarMode)
            {
                case CollarMode.Vibrate: return VibratePower;
                case CollarMode.Shock: return ShockPower;
                case CollarMode.Tone: return TonePower;
                default: return 0;
            }
        }
        public static int GetCurrentCooldown()
        {
            switch (collarMode)
            {
                case CollarMode.Vibrate: return VibrateCooldown;
                case CollarMode.Shock: return ShockCooldown;
                case CollarMode.Tone: return ToneCooldown;
                default: return 0;
            }
        }
        public static void ChangePowerlevel(int newValue)
        {
            switch (collarMode)
            {
                case CollarMode.Vibrate: VibratePower = newValue; break;
                case CollarMode.Shock: ShockPower = newValue; break;
                case CollarMode.Tone: TonePower = newValue; break;
                default: break;
            }
        }
        public static void ChangeCooldown(int newValue)
        {
            switch (collarMode)
            {
                case CollarMode.Vibrate: VibrateCooldown = newValue; break;
                case CollarMode.Shock: ShockCooldown = newValue; break;
                case CollarMode.Tone: ToneCooldown = newValue; break;
                default: break;
            }
        }
        public static void ChangeCollarMode()
        {

        }
        public void ChangeOSCStatus(bool isActive)
        {
            if (oscAPI == null) { return; }
            if (isActive)
            {
                oscAPI.Start();
            }
            else
            {
                oscAPI.Stop();
            }
        }
        public void ExecuteAction()
        {

            oscAPI.aaroStart();

            switch (collarMode)
            {
                case CollarMode.Tone:
                    //piShockAPI.BeepAsync(1);
                    if (arduinoAPI != null)
                    {
                        arduinoAPI.ExecuteCommand(collarMode, TonePower, 1, MainProgram.ToneCooldown);
                    }
                    break;
                case CollarMode.Vibrate:
                    //piShockAPI.VibrateAsync(1, VibratePower);
                    if (arduinoAPI != null)
                    {
                        arduinoAPI.ExecuteCommand(collarMode, VibratePower, 1, MainProgram.VibrateCooldown);
                    }
                    break;
                case CollarMode.Shock:
                   // piShockAPI.ShockAsync(1, ShockPower);
                    if (arduinoAPI != null)
                    {
                        arduinoAPI.ExecuteCommand(collarMode, ShockPower, 1, MainProgram.ShockCooldown);
                    }
                    break;
                default: break;
            }
        }

    }
}
