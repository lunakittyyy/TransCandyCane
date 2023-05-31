using BepInEx;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using System.Threading.Tasks;
using System.Linq;
using Utilla;
using System;

namespace TransCandyCane
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    public class Plugin : BaseUnityPlugin
    {
        private static Texture2D TransCandyCaneTex;
        private Harmony Harmony;
        
        // TODO: make this not use Utilla for the fix
        // perhaps we need to make Awake run asyncronously for the game to not lock up?
        void Awake()
        {
            Utilla.Events.GameInitialized += GameInitialized;
        }

        public void GameInitialized(object sender, EventArgs e)
        {
            var str = Assembly.GetExecutingAssembly().GetManifestResourceStream("TransCandyCane.Resources.transcandycanetex");
            var bundle = AssetBundle.LoadFromStream(str);
            TransCandyCaneTex = bundle.LoadAsset("transcandycane") as Texture2D;

            Harmony = new Harmony(PluginInfo.GUID);
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [HarmonyPatch(typeof(VRRig), "Start", MethodType.Normal)]
        public class Patch
        {
            public static async void Postfix(VRRig __instance)
            {
                await Task.Delay(500);

                var holdables = __instance.GetComponent<BodyDockPositions>().allObjects;
                var candycane = holdables.First(a => a.name == "CANDY CANE");
                candycane.GetComponent<Renderer>().material.mainTexture = TransCandyCaneTex;
            }
        }
    }
}