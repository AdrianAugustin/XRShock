using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRShock.Structs
{
  public  class OSCListenerEntry
    {
        public string adress;
        public object value=true;
        public string type = "bool";
        public ActionType action;
        public int power;
        public int cooldown;

 
     
    }
}
