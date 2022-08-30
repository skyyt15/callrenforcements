using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace callrenforcements
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public string FalseCall { get; set; } = "appel échoué.";

        public string TrueCallMTF { get; set; } = "ton appel à été recus. l'unité va arriver dans approximativement dans 30 secondes";
        public string TrueCallCI { get; set; } = "ton appel à été recus. l'unité va arriver dans approximativement dans 2 minutes";
        public bool LockDownEZForCI { get; set; } = true;
        public int Cooldown { get; set; } = 120;
        public string NotRole { get; set; } = "you are not a mobile task force or Chaos Insurgency.";
        public string NotIntercomRoom { get; set; } = "you are not in intercom room.";
        public string NotRadioinCurrentItem { get; set; } = "you aren't have a radio in your current item.";
        public string CooldownProgress { get; set; } = "<color=red>Command is on cooldown. please try later.</color>";
    }
}
