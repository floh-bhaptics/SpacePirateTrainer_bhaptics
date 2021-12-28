using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MelonLoader;
using HarmonyLib;
//using UnityEngine;

using MyBhapticsTactsuit;

namespace SpacePirateTrainer_bhaptics
{
    public class SpacePirateTrainer_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr;

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");
        }
        
        [HarmonyPatch(typeof(Gun), "Shoot", new Type[] {  })]
        public class bhaptics_ShootGun
        {
            [HarmonyPostfix]
            public static void Postfix(Gun __instance)
            {
                string weaponType = "Gun";

                if (__instance.gunType == Gun.GunType.shotgun) { weaponType = "Shotgun"; }
                if (__instance.gunType == Gun.GunType.nade) { weaponType = "Grenade"; }
                if (__instance.gunType == Gun.GunType.missiles) { weaponType = "Grenade"; }

                bool isRightHand = (__instance.TargetInputSource.ToString().Contains("ight"));

                tactsuitVr.Recoil(weaponType, isRightHand);
            }
        }

        [HarmonyPatch(typeof(Shield), "Rumble", new Type[] { })]
        public class bhaptics_ShieldRumble
        {
            [HarmonyPostfix]
            public static void Postfix(Shield __instance)
            {
                bool isRightHand = (__instance.TargetInputSource.ToString().Contains("ight"));

                tactsuitVr.Recoil("Whip", isRightHand);
            }
        }

        [HarmonyPatch(typeof(MeleeDevice), "Start", new Type[] { })]
        public class bhaptics_MeleeRumble
        {
            [HarmonyPostfix]
            public static void Postfix(MeleeDevice __instance)
            {
                bool isRightHand = (__instance.TargetInputSource.ToString().Contains("ight"));

                tactsuitVr.Recoil("Whip", isRightHand);
            }
        }


        [HarmonyPatch(typeof(PlayerCode), "Hit", new Type[] { })]
        public class bhaptics_PlayerHit
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerCode __instance)
            {
                //tactsuitVr.LOG("Hit health: " + __instance.health.ToString());
                
                if (__instance.health.GetValue() == 1) { tactsuitVr.StartHeartBeat(); }
                else { tactsuitVr.StopHeartBeat(); }
                tactsuitVr.PlaybackHaptics("Impact");
                //bool isRightHand = (__instance.TargetInputSource.ToString().Contains("ight"));
            }
        }

        [HarmonyPatch(typeof(PlayerCode), "SetHealth", new Type[] { typeof(int) })]
        public class bhaptics_SetHealth
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerCode __instance, int h)
            {
                //tactsuitVr.LOG("Hit health: " + __instance.health.ToString());

                if (__instance.health.GetValue() == 1) { tactsuitVr.StartHeartBeat(); }
                else { tactsuitVr.StopHeartBeat(); }
                //tactsuitVr.LOG("Health: " + h.ToString());
                //bool isRightHand = (__instance.TargetInputSource.ToString().Contains("ight"));
            }
        }
        [HarmonyPatch(typeof(Health), "Update", new Type[] { })]
        public class bhaptics_HealthUpdate
        {
            [HarmonyPostfix]
            public static void Postfix(Health __instance)
            {
                //tactsuitVr.LOG("Healthupdate: " + __instance.CurrentHealthPercentage.ToString());
                if (__instance.CurrentHealthPercentage <= 0.35f) { tactsuitVr.StartHeartBeat(); }
                else { tactsuitVr.StopHeartBeat(); }
                //tactsuitVr.LOG("Health: " + __instance.health.ToString());
                //tactsuitVr.PlaybackHaptics("Impact");
                //bool isRightHand = (__instance.TargetInputSource.ToString().Contains("ight"));
            }
        }

    }
}
