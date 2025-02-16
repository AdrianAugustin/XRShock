using System;
using System.IO.Ports;
using System.Threading;
using Xunit;

namespace XRShockTests
{
    public class ArduinoApiTests : IDisposable
    {
        private SerialPort _serialPort;
        private const string PortName = "COM3"; // Change this to match your Arduino port
        private const int BaudRate = 9600;

        public ArduinoApiTests()
        {
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
        }

        private string SendCommand(string command)
        {
            _serialPort.DiscardInBuffer();
            _serialPort.WriteLine(command);
            Thread.Sleep(500); // Give Arduino time to process
            return _serialPort.ReadLine()?.Trim();
        }

        [Fact]
        public void Test_GetDeviceID()
        {
            string response = SendCommand("DevID");
            Assert.StartsWith("DeviceID=", response);
        }

        [Fact]
        public void Test_SetDeviceID()
        {
            string newId = "1234";
            string response = SendCommand($"SetDevID:{newId}");
            Assert.Equal($"NewDeviceID={newId}", response);
        }

        [Fact]
        public void Test_GetDeviceVersion()
        {
            string response = SendCommand("Version");
            Assert.StartsWith("DeviceVersion=", response);
        }

        [Fact]
        public void Test_SetDeviceVersion()
        {
            string newVersion = "TestVer1.0";
            string response = SendCommand($"SetVersion:{newVersion}");
            Assert.Equal($"NewDeviceVersion={newVersion}", response);
        }

        [Fact]
        public void Test_VibrateCommand()
        {
            string response = SendCommand("Vibrate:100,1");
            Assert.NotNull(response);
        }
    }

}