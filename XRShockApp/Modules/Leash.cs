using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRShock.Modules
{
    internal class Leash
    {
        float horiz = 0;
        float vertic = 0;
        float leashx = 0;
        float leashz = 0;
        float pullvalue = 0;
        float thispullvalue = 0;
        bool isLeashgrabbed = false;
   
       public float deadzone = 0.1F;
        OSCAPI oscApi;
        public Leash(OSCAPI api)
        {
            this.oscApi = api;
        }
        public void CheckMsg(string address, List<object> value)
        {

            if (address.Contains("/avatar/parameters/Leash_"))
            {

                switch (address)
                {
                    case "/avatar/parameters/Leash_X+":
                        leashx = (float)(value[0]);
                        break;
                    case "/avatar/parameters/Leash_X-":
                        leashx = (-(float)value[0]);
                        break;
                    case "/avatar/parameters/Leash_Z+":
                        leashz = (float)value[0];
                        break;
                    case "/avatar/parameters/Leash_Z-":
                        leashz = (-(float)value[0]);
                        break;
                    case "/avatar/parameters/Leash_Distance":
                        break;
                    case "/avatar/parameters/Leash_IsGrabbed":
                        isLeashgrabbed = (bool)value[0];
                        break;
                    case "/avatar/parameters/Leash_Stretch":
                        pullvalue = (2 * (float)value[0]);
                        break;
                    default: break;
                }
                thispullvalue = pullvalue;
                //if (Math.Abs(thispullvalue * leashx) < deadzone && Math.Abs(thispullvalue * leashz) < deadzone || isLeashgrabbed == false)
                //{
                //    thispullvalue = 0;
                //    horiz = 0;
                //    vertic = 0;

                //}
                //else if (thispullvalue > 1)
                //{
                //    horiz = strength * leashx;
                //    vertic = strength*leashz;
                //    thispullvalue = 1;
                //}
               


                //if (horiz < -1)
                //{
                //    horiz = -1;
                //}
                //else if (horiz > 1)
                //{
                //    horiz = 1;
                //}

                //if (vertic < -1)
                //{
                //    vertic = -1;
                //}
                //else if (vertic > 1)
                //{
                //    vertic = 1;
                //}

                //oscApi.SendOSCMessage("/input/Vertical", vertic);
                //oscApi.SendOSCMessage("/input/Horizontal", horiz);
                if (Math.Abs(thispullvalue * leashx) < deadzone && Math.Abs(thispullvalue * leashz) < deadzone || isLeashgrabbed == false)
                {
                    thispullvalue = 0;
                }
                else if (thispullvalue > 1)
                {
                    thispullvalue = 1;
                }
                horiz = thispullvalue * leashx;
                vertic = thispullvalue * leashz ;
                //if (horiz < -1)
                //{
                //    horiz = -1;

                //}
                //else if (horiz > 1)
                //{
                //    horiz = 1;

                //}

                //if (vertic < -1)
                //{
                //    vertic = -1;
                //}
                //else if (vertic > 1)
                //{
                //    vertic = 1;
                //}
                oscApi.SendOSCMessage("/input/Vertical", vertic);
                oscApi.SendOSCMessage("/input/Horizontal", horiz);
            }
        }
    }
}
