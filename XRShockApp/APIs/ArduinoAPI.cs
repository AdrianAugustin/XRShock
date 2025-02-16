using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net.Sockets;
using System.Text;

using System.Threading.Tasks;
using System.Timers;
using Microsoft.Win32;
using SharpOSC;


namespace ShockCollar
{
    public class ArduinoAPI
    {
        SerialPort _serialPort;
        public bool keepAlive = true;
        Timer KeepAliveTimer;
        Timer CooldownTimer;
        bool onCooldown = false;
        CollarMode lastmode;
        public ArduinoAPI(string comPort)
        {

            KeepAliveTimer = new Timer();
            KeepAliveTimer.Interval = 90000;
            KeepAliveTimer.AutoReset = true;
            KeepAliveTimer.Elapsed += OnTimedEvent;
            KeepAliveTimer.Start();

            CooldownTimer = new Timer();
            //CooldownTimer.Interval = 120000;
            CooldownTimer.AutoReset = false;
            CooldownTimer.Elapsed += OnCooldownEvent;
            // CooldownTimer.Start();

            try
            {


                _serialPort = new SerialPort();
                _serialPort.PortName = comPort;//Set your board COM
                _serialPort.BaudRate = 115200;
                _serialPort.DataBits = 8;
                _serialPort.Parity = Parity.None;
                _serialPort.Handshake = Handshake.None;
                _serialPort.Open();
                _serialPort.Write("\n");
            }
            catch (Exception ex)
            {

            }


        }


        private string CreateCommand(CollarMode Mode, int PowerLevel, int Channel)
        {

            string result = "";
            switch (Mode)
            {
                case CollarMode.Tone: result += "Tone"; break;
                case CollarMode.Vibrate: result += "Vibrate"; break;
                case CollarMode.Shock: result += "Shock"; break;
                case CollarMode.Blink: result += "Blink"; break;
                default: break;
            }
            int _powerlevel=0;
            if (PowerLevel > 99) { _powerlevel = 99; }
            else{ _powerlevel = PowerLevel; }
            string PowerLevelString = _powerlevel.ToString();
            if (_powerlevel < 10)
            {
                PowerLevelString = "0" + PowerLevelString;
            }
            result += ":";
            result += PowerLevelString + ",1"+"\n";
            
            return result;

        }

        public void ExecuteCommand(CollarMode Mode, int PowerLevel, int Channel, int cooldown)
        {

            if (lastmode != Mode)
            {
                lastmode = Mode;
                onCooldown = false;
                CooldownTimer.Stop();

            }
            if (!onCooldown)
            {
                onCooldown = true;

                CooldownTimer.Interval = cooldown + 1;
                CooldownTimer.Start();
                //  _serialPort.Write("0\n");

                if (_serialPort.IsOpen) {
                    _serialPort.Write(CreateCommand(Mode, PowerLevel, Channel)); }

            }

        }
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            sendKeepAliveMsg();
            //  Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
        }
        private void OnCooldownEvent(Object source, System.Timers.ElapsedEventArgs e)
        {

            onCooldown = false;
        }
        public void sendKeepAliveMsg()
        {
            if (_serialPort.IsOpen) {
                _serialPort.Write(CreateCommand(CollarMode.Blink, 10, 1));
            }
        }
        public void ClosePort() {
            CooldownTimer.Stop();
            _serialPort.Close();
        }

        public static List<string> DeviceDescribtionsList = new List<string> { "CH340", "Arduino" };
        
        public static List<string> AutodetectArduinoPort()
        {



            //DeviceDescribtionsList.Add("CH340");
            //DeviceDescribtionsList.Add("Arduino");
            List<string> result = new List<string>();


            using (ManagementClass i_Entity = new ManagementClass("Win32_PnPEntity"))
            {
                foreach (ManagementObject i_Inst in i_Entity.GetInstances())
                {
                    Object o_Guid = i_Inst.GetPropertyValue("ClassGuid");
                    if (o_Guid == null || o_Guid.ToString().ToUpper() != "{4D36E978-E325-11CE-BFC1-08002BE10318}")
                        continue; // Skip all devices except device class "PORTS"

                    String s_Caption = i_Inst.GetPropertyValue("Caption").ToString();
                    String s_Manufact = i_Inst.GetPropertyValue("Manufacturer").ToString();
                    String s_DeviceID = i_Inst.GetPropertyValue("PnpDeviceID").ToString();
                    String s_RegPath = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Enum\\" + s_DeviceID + "\\Device Parameters";
                    String s_PortName = Registry.GetValue(s_RegPath, "PortName", "").ToString();

                    int s32_Pos = s_Caption.IndexOf(" (COM");
                    if (s32_Pos > 0) // remove COM port from description
                        s_Caption = s_Caption.Substring(0, s32_Pos);
                    if (s_Caption.Contains("CH340") || s_Caption.Contains("Arduino"))
                    {
                        result.Add(s_PortName);
                    }

                    //Console.WriteLine("Description:  " + s_Caption);
                    //Console.WriteLine("Manufacturer: " + s_Manufact);
                    //Console.WriteLine("Device ID:    " + s_DeviceID);
                    //Console.WriteLine("-----------------------------------");
                }
            }
            return result;
        }


    } 

    
}
