using CommandSystem;
using Exiled.API.Features;
using InventorySystem.Items.Radio;
using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Exiled.API.Enums;

namespace callrenforcements
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class call_for_MTF : ParentCommand
    {
        public override string Command { get; } = "call";
        public override string[] Aliases { get; } = new string[] {"helpme","chm"};

        public override string Description { get; } = "call your squad";

        public int Cooldown = 0;
        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {   
            Player ev = Player.Get(((CommandSender)sender).SenderId);
            if (ev == null)
            {
                response = "<color=red>a error was occured. please try later.</color>";
                return false;
            }
            if (Cooldown > 0)
            {
                response = $"\n{Plugin.Instance.Config.CooldownProgress}\n ({Cooldown})";
                return false;
            }
            if (ev.Role.Team == Team.MTF)
            {
                if (ev.Role.Type == RoleType.FacilityGuard)
                {
                    response = Plugin.Instance.Config.NotRole;
                    return false;
                }
                if (ev.CurrentItem != null)
                {
                    if (ev.CurrentItem.Type == ItemType.Radio)
                    {
                        System.Random random = new System.Random();
                        int mle = random.Next(0, 100);
                        if (mle < 70)
                        {
                            Timing.RunCoroutine(RespawnMTF());
                            Timing.RunCoroutine(CoolDoown());
                            response = Plugin.Instance.Config.TrueCallMTF;
                            return true;
                        }
                        else
                        {
                            response = Plugin.Instance.Config.FalseCall;
                            Timing.RunCoroutine(CoolDoown());
                            return false;
                        }
                    }
                    response = Plugin.Instance.Config.NotRadioinCurrentItem;
                    return false;
                }
                response = Plugin.Instance.Config.NotRadioinCurrentItem;
                return false;
            }
            else if (ev.Role.Team == Team.CHI)
            {
                if (ev.CurrentRoom.Type == RoomType.EzIntercom)
                {
                    System.Random random = new System.Random();
                    int mle = random.Next(0, 100);
                    if (mle < 70)
                    {
                        Timing.RunCoroutine(RespawnCI());
                        Timing.RunCoroutine(CoolDoown());
                        Cassie.Message("pitch_0.2 .g4 pitch_0.7 . pitch_0.2 .g4 pitch_0.7 . pitch_0.2 .g4 pitch_0.7 . pitch_0.9 alerte alerte . .g3 .g3 .g2 pitch_1 an Unauthorized access of surface detected in intercom .g2 .g2 .g3 room . gate NATO_A . gate beta and checkpoint entrance heavy zone lockdown in progress for your security .", true,true,false);
                        Timing.RunCoroutine(LockDown());
                        response = Plugin.Instance.Config.TrueCallCI;
                        return true;
                    }
                    else
                    {
                        response = Plugin.Instance.Config.FalseCall;
                        Timing.RunCoroutine(CoolDoown());
                        return false;
                    }
                }
                response = Plugin.Instance.Config.NotIntercomRoom;
                return false;
            }
            response = response = Plugin.Instance.Config.NotRole;
            return false;
        }
        public IEnumerator<float> RespawnMTF()
        {
            yield return Timing.WaitForSeconds(new System.Random().Next(20, 45));
            Respawn.ForceWave(Respawning.SpawnableTeamType.NineTailedFox, true);
        }
        public IEnumerator<float> RespawnCI()
        {
            yield return Timing.WaitForSeconds(new System.Random().Next(110,140));
            Respawn.ForceWave(Respawning.SpawnableTeamType.ChaosInsurgency, true);
        }
        public IEnumerator<float> CoolDoown()
        {
            Cooldown = Plugin.Instance.Config.Cooldown;
            for (int i = 0; i < Plugin.Instance.Config.Cooldown; i+=1)
            {
                Cooldown-=1;
                yield return Timing.WaitForSeconds(1f);
            }
        }
        public IEnumerator<float> LockDown()
        {
            foreach(Door door in Door.List)
            {
                if (door.Type == DoorType.CheckpointEntrance)
                {
                    door.ChangeLock(DoorLockType.Isolation);
                    door.IsOpen = false;
                }

                if (door.Type == DoorType.GateA)
                {
                    door.ChangeLock(DoorLockType.Isolation);
                    door.IsOpen = false;
                }

                if (door.Type == DoorType.GateB)
                {
                    door.ChangeLock(DoorLockType.Isolation);
                    door.IsOpen = false;
                }
            }
            yield return Timing.WaitForSeconds(60f);
            foreach (Door door in Door.List)
            {
                if (door.Type == DoorType.CheckpointEntrance)
                    door.ChangeLock(DoorLockType.None);

                if (door.Type == DoorType.GateA)
                    door.ChangeLock(DoorLockType.None);

                if (door.Type == DoorType.GateB)
                    door.ChangeLock(DoorLockType.None);
            }
        }
    }
}
