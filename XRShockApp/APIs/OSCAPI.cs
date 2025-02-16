using XRShock.ViewModels;
using SharpOSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using XRShock.Modules;

namespace XRShock
{
    public class OSCAPI
    {
        bool OSCReady = true;
        bool OSCRunning=false;
        int sendPort = 9000;
        int listenerport = 9001;
        List<int> repeaterPorts = new List<int>();
        List<UDPSender> repeaterSender;
        public bool LeashActive = true;
        MainProgram mainProgram;
        UDPListener listener;
        Timer timer;
        SharpOSC.UDPSender sender;
        internal Leash leash;
        string address1 = "/avatar/parameters/RemotePressed";
        string address2 = "/avatar/parameters/thefunny";

        public bool Repeating = true;

        public OSCAPI(MainProgram mainProgram)
        {
            this.mainProgram = mainProgram;
            leash = new Leash(this);
        }

        public void Stop()
        {
            if (listener != null && sender != null && OSCReady)
            {
                try
                {
                    OSCReady = false;
                    sender.Close();
                    foreach(UDPSender repeater in repeaterSender)
                    {
                        repeater.Close();
                       
                    }
                    repeaterSender = new List<UDPSender>();
                    listener.Close();
                    listener = null;
                    OSCReady = true;
                OSCRunning= false;
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void Start()
        {
            if (listener == null && OSCReady)
            {

                OSCReady = false;
                repeaterPorts.Add(9011);
                repeaterPorts.Add(9012);
                repeaterPorts.Add(9013);
                repeaterPorts.Add(9014);
                repeaterSender = new List<UDPSender>();
                sender = new SharpOSC.UDPSender("127.0.0.1", sendPort);
                
                foreach (int port in repeaterPorts)
                {
                    repeaterSender.Add(new SharpOSC.UDPSender("127.0.0.1", port));
                }
               
                timer = new Timer();
                timer.Interval = 700;
                timer.AutoReset = false;
                timer.Elapsed += OnTimedEvent;




                HandleOscPacket callback = delegate (OscPacket packet)
                {
                    var messageReceived = (OscMessage)packet;
                    if (messageReceived != null)
                    {


                        if (LeashActive)
                        {
                            leash.CheckMsg(messageReceived.Address, messageReceived.Arguments);
                        }
                        if (messageReceived.Address == address1 || messageReceived.Address == address2)
                        {
                            if ((bool)messageReceived.Arguments[0])
                            {
                                aaroActive();

                                mainProgram.ExecuteAction();
                            }
                        }
                        else if (messageReceived.Address == "/avatar/parameters/XRShock")
                        {
                            if ((bool)messageReceived.Arguments[0])
                            {
                                mainProgram.ExecuteAction();
                            }
                        }
                        else if (messageReceived.Address == "/avatar/parameters/XRVibrate")
                        {
                            if ((bool)messageReceived.Arguments[0])
                            {
                                mainProgram.piShockAPI.VibrateAsync(1, 100);
                                if (mainProgram.arduinoAPI != null)
                                {
                                    
                                    mainProgram.arduinoAPI.ExecuteCommand(CollarMode.Vibrate, 100, 1, 0);
                                }
                            }
                        }
                        else if (Repeating)
                        {
                            foreach (UDPSender sender in repeaterSender)
                            {
                                sender.Send(messageReceived);
                            }
                        }
                    }

                };

                listener = new UDPListener(listenerport, callback);
                OSCReady = true;
                OSCRunning= true;
            }
        }
        public void SendOSCMessage(string address, object value)
        {
            if (OSCRunning)
            {
                var msg = new SharpOSC.OscMessage(address, value);
                sender.Send(msg);
            }
        }
        #region aaro
        public void aaroStart()
        {
            if (OSCRunning)
            {
                var msg1 = new SharpOSC.OscMessage("/avatar/parameters/allowaaromenu", true);
                sender.Send(msg1);

                var msg2 = new SharpOSC.OscMessage("/avatar/parameters/bruhmoment", 1);
                sender.Send(msg2);
            }
        }
        public void aaroStop()
        {
            if (OSCRunning)
            {
                var msg = new SharpOSC.OscMessage("/avatar/parameters/watchmode", 0);
                sender.Send(msg);
            }
        }
        public void aaroActive()
        {
                if (OSCRunning)
                {
                    timer.Start();
                    var msg = new SharpOSC.OscMessage("/avatar/parameters/thefunny_active", true);
                    sender.Send(msg);
                }

        }
        public void aaroInActive()
        {
            if (OSCRunning)
            {
                var msg = new SharpOSC.OscMessage("/avatar/parameters/thefunny_active", false);
                sender.Send(msg);
            }
        }
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
                if (OSCRunning)
                {
                    aaroInActive();
                }
        }
        #endregion
    }
}
