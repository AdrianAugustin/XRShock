namespace XRShockTests
{
    using System;
    using System.IO.Ports;
    using System.Security.Policy;
    using System.Threading;
    using Xunit;

    public class ArduinoApiTests : IDisposable
    {
        private SerialPort _serialPort;
        private string PortName="";
        private const int BaudRate = 115200;

        public ArduinoApiTests()
        {
            PortName = XRShock.ArduinoAPI.AutodetectArduinoPort()[0];
            _serialPort = new SerialPort(PortName, BaudRate)
            {
                ReadTimeout = 2000,
                WriteTimeout = 2000
            };
            _serialPort.Open();
            Thread.Sleep(2000); // Allow time for the Arduino to reset
        }

        public void Dispose()
        {
            _serialPort.Close();
            Thread.Sleep(2000);
        }

        private string SendCommand(string command)
        {
            _serialPort.DiscardInBuffer();
            _serialPort.WriteLine(command);
            Thread.Sleep(500); // Give Arduino time to process
            return _serialPort.ReadLine()?.Trim();
        }

        [Fact]
        public void Test_GetSetDeviceID()
        {
            string response=SendCommand("DevID");
            string oldId = response.Split('=')[1];
            Assert.Equal("15465", oldId);
            string newId = "12345";
            response = SendCommand($"SetDevID:{newId}");
            Assert.Equal($"NewDeviceID={newId}", response);
            response = SendCommand("DevID");
            Assert.StartsWith($"DeviceID={newId}", response);
            response = SendCommand($"SetDevID:{oldId}");
            Assert.Equal($"NewDeviceID={oldId}", response);
        }


        [Fact]
        public void Test_GetSetDeviceVersion()
        {
            string response = SendCommand("Version");
            string oldVersion=response.Split('=')[1];
            Assert.Equal($"DeviceVersion={oldVersion}", response);

            string newVersion = "TestVer1.2.3";
            response = SendCommand($"SetVersion:{newVersion}");
            Assert.Equal($"NewDeviceVersion={newVersion}", response);

            response = SendCommand("Version");
            Assert.Equal($"DeviceVersion={newVersion}", response);

            response = SendCommand($"SetVersion:{oldVersion}");
            Assert.Equal($"NewDeviceVersion={oldVersion}", response);

            response = SendCommand("Version");
            Assert.Equal($"DeviceVersion={oldVersion}", response);
        }

        [Fact]
        public void Test_GetSetRepeats()
        {
            string response = SendCommand("Reps");
            string oldVersion = response.Split('=')[1];
            Assert.Equal($"Repeats={oldVersion}", response);

            string newVersion = "3";
            response = SendCommand($"SetReps:{newVersion}");
            Assert.Equal($"NewRepeats={newVersion}", response);

            response = SendCommand("Reps");
            Assert.Equal($"Repeats={newVersion}", response);

            response = SendCommand($"SetReps:{oldVersion}");
            Assert.Equal($"NewRepeats={oldVersion}", response);

            response = SendCommand("Reps");
            Assert.Equal($"Repeats={oldVersion}", response);
        }

        [Fact]
        public void Test_SetEEPROMDefaults()
        {
            string response;

            string defaultVersion = "3.0";
            response = SendCommand($"SetVersion:{defaultVersion}");
            Assert.Equal($"NewDeviceVersion={defaultVersion}", response);

            string defaultReps = "5";
            response = SendCommand($"SetReps:{defaultReps}");
            Assert.Equal($"NewRepeats={defaultReps}", response);

            string defaultDevID = "15465";
            response = SendCommand($"SetDevID:{defaultDevID}");
            Assert.Equal($"NewDeviceID={defaultDevID}", response);

        }

        [Fact]
        public void Test_VibrateCommand()
        {
            string response = SendCommand("Vibrate:100,1");
            Assert.Equal("Vibrating", response);
        }
        [Fact]
        public void Test_ShockCommand()
        {
            string response = SendCommand("Shock:100,1");
            Assert.Equal("Shocking", response);
        }
        [Fact]
        public void Test_BeepCommand()
        {
            string response = SendCommand("Tone:100,1");
            Assert.Equal("Beeping", response);
        }
    }

}