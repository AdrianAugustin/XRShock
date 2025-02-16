using Buttplug;
using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;
using ShockCollar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ShockCollar.Modules
{
    internal class DildonicsClient
    {

        public DildonicsClient(MainProgram mainProgramm) {
           
        this.mainProgramm= mainProgramm;
           
        
        }

        MainProgram mainProgramm;
        internal static bool isReady=false;
        private DildonicsViewModel viewModel=null;
        internal  float Power=0.0F;
        internal static List<string> DeviceList = new List<string>();
        internal static ButtplugClient client;
        internal static int deviceChoice;
        internal static string deviceName;
        internal static void DeviceSelectionChanged()
        {
    
            device = client.Devices.First(dev => dev.Name == deviceName);
            
        }
        internal static ButtplugClientDevice device; 

        internal void doDildonicsAction(float power)
        {
            Task.Run(() => { ControlDevice( 1, power); });
        }
        internal async void Run()
        {
            client = new ButtplugClient("XRShock");
            Task.Run(() =>
            {

                ConnectToIntiface();

            });
        }


        internal static async Task ControlDevice( int mode,float strength)
        {

         
            if (!client.Devices.Any())
            {
               
                return;
            }

     
            if (mode == 1)
            {

                try
                {
                    await device.VibrateAsync(strength);
                    //await Task.Delay(1000);
                    //await device.VibrateAsync(0);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Problem vibrating: {e}");
                }
            }
            else if (mode == 2)
            {
                Console.WriteLine(
                    $"Running all oscillators of {device.Name} at 50% for 1s.");
                try
                {
                    await device.OscillateAsync(0.5);
                    await Task.Delay(1000);
                    await device.OscillateAsync(0);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Problem oscillating: {e}");
                }
            }
            else if (mode == 3)
            {
                Console.WriteLine($"Rotating {device.Name} at 50% for 1s.");
                try
                {
                    await device.RotateAsync(0.5, true);
                    await Task.Delay(1000);
                    await device.RotateAsync(0, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Problem rotating: {e}");
                }
            }
            else if (mode == 4)
            {
                Console.WriteLine(
                    $"Oscillating linear motors of {device.Name} from 20% to 80% over 3s");
                try
                {
                    await device.LinearAsync(1000, 0.2);
                    await Task.Delay(1100);
                    await device.LinearAsync(1000, 0.8);
                    await Task.Delay(1100);
                    await device.LinearAsync(1000, 0.2);
                    await Task.Delay(1100);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Problem moving linearly: {e}");
                }
            }
            else if (mode == 5)
            {
                Console.WriteLine(
                    $"Checking battery level of {device.Name}");
                try
                {
                    Console.WriteLine($"Battery Level: {await device.BatteryAsync()}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Problem getting battery level: {e}");
                }
            }
        }
        internal static async Task ConnectToIntiface()
        {
           client = new ButtplugClient("XRShock");

 
            void HandleDeviceAdded(object aObj, DeviceAddedEventArgs aArgs)
            {
               DeviceList.Add(aArgs.Device.Name);
                deviceName= aArgs.Device.Name;
                DeviceSelectionChanged();
            }

            client.DeviceAdded += HandleDeviceAdded;

            void HandleDeviceRemoved(object aObj, DeviceRemovedEventArgs aArgs)
            {
                try { DeviceList.Remove(aArgs.Device.Name); }
                catch { }
             }

             client.DeviceRemoved += HandleDeviceRemoved;

            await client.ConnectAsync(new ButtplugWebsocketConnector(new Uri("ws://127.0.0.1:12345")));
            // Now we can connect.
          //  await client.ConnectAsync(new ButtplugWebsocketConnector(new Uri("ws://127.0.0.1:12345")));
            isReady = true;
            if (client.Connected)
            {
                Task.Run(() => { StartScanForDevices(); });
            }
        }
        internal static async Task StartScanForDevices()
        {
            await client.StartScanningAsync();
        }
        internal static async Task StopScanForDevices()
        {
            await client.StopScanningAsync();
        }
    }
}
