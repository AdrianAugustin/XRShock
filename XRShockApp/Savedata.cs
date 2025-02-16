using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShockCollar
{
    internal class Savedata
    {
        CollarMode CurrentMode;
        int ShockPower;
        int ShockCooldown;
        int BlinkCooldown;
        int VibrationPower;
        int VibrationCooldown;
        int ToneCooldown;
        bool KeepAlive;
        bool OSCActive;
        int senderPort;
        int receiverPort;
        Dictionary<int, string> repeaterPorts;
    }
}
