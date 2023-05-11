using BepInEx;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using System.Threading.Tasks;
using System.Linq;

namespace TransCandyCane
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private static Texture2D TransCandyCaneTex;
        private Harmony Harmony;

        public void Awake()
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