using Exiled.API.Features;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace callrenforcements
{
    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "sky";
        public override string Name { get; } = "renforcements call";
        public override string Prefix { get; } = "callRenforcements";

        public static Plugin Instance;

        public override void OnEnabled()
        {
            Instance = this;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            base.OnDisabled();
        }
    }
}
