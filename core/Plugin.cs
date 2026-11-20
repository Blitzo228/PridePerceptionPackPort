using BepInEx;
using BepInEx.Bootstrap;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.ObjectCreation;
using MTM101BaldAPI.Registers;
using MTM101BaldAPI.UI;
using PridePerception.compat;
using PridePerception.npcs;
using PridePerception.util;
using TMPro;
using UnityEngine;

namespace PridePerception.core
{
    [BepInDependency("mtm101.rulerp.bbplus.baldidevapi")]
    [BepInDependency("mtm101.rulerp.baldiplus.levelstudio", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("alexbw145.baldiplus.pinedebug", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin("blitzo.baldiplus.prideperception", "Pride Perception Pack", "1.3.0.2")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin current;
        public static AssetManager assets;

        public void Awake()
        {
            current = this;
            Harmony harmony = new("blitzo.baldiplus.prideperception");
            harmony.PatchAllConditionals();
            assets = new();
            LoadingEvents.RegisterOnAssetsLoaded(Info, runCallbacks, LoadingEventOrder.Pre);
            GeneratorManagement.Register(this, GenerationModType.Addend, sceneGenerated);
        }

        public void runCallbacks()
        {
            try { registerAssets(); } catch { }
            try { updateAssets(); } catch { }
            try { registerNPCs(); } catch { }
            try { registerCompatibilities(); } catch { }
        }

        public void registerAssets()
        {
            try
            {
                assets.Add<Sprite>("Images/compat/LevelStudioCompat/npc_bezz",
                    AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(ModPaths.GetPath("compat/LevelStudioCompat/npc_bezz.png")), 10.0f));

                assets.Add<Sprite>("Images/PrideSlotIndividual",
                    AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(ModPaths.GetPath("compat/LevelStudioCompat/PrideSlotIndividual.png")), 10.0f));

                assets.Add<Sprite>("Images/npcs/Bezz/bezzIdle",
                    AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(ModPaths.GetPath("npcs/Bezz/bezzIdle.png")), 30.0f));

                for (int i = 0; i < 12; i++)
                    assets.Add<Sprite>("Images/npcs/Bezz/bezzWalk" + i,
                        AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(ModPaths.GetPath("npcs/Bezz/bezzWalk" + i + ".png")), 30.0f));

                for (int i = 0; i < 2; i++)
                    assets.Add<Sprite>("Images/npcs/Bezz/bezzHappyTalk" + i,
                        AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(ModPaths.GetPath("npcs/Bezz/bezzHappyTalk" + i + ".png")), 30.0f));

                for (int i = 0; i < 2; i++)
                    assets.Add<Sprite>("Images/npcs/Bezz/bezzUpsetTalk" + i,
                        AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(ModPaths.GetPath("npcs/Bezz/bezzUpsetTalk" + i + ".png")), 30.0f));

                for (int i = 0; i < 5; i++)
                {
                    assets.Add<Sprite>("Images/npcs/Bezz/Flag/" + i + "0",
                        AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(ModPaths.GetPath("npcs/Bezz/Flag/" + i + "0.png")), 30.0f));
                    assets.Add<Sprite>("Images/npcs/Bezz/Flag/" + i + "1",
                        AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(ModPaths.GetPath("npcs/Bezz/Flag/" + i + "1.png")), 30.0f));
                    assets.Add<Sprite>("Images/npcs/Bezz/Flag/3dflag" + i,
                        AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(ModPaths.GetPath("npcs/Bezz/Flag/3dflag" + i + ".png")), 10.0f));
                }

                assets.Add<Sprite>("Images/npcs/Bezz/Flag/mapMarker",
                    AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(ModPaths.GetPath("npcs/Bezz/Flag/mapMarker.png")), 15.0f));
                assets.Add<Sprite>("Images/npcs/Bezz/Flag/mapIcon",
                    AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(ModPaths.GetPath("npcs/Bezz/Flag/mapIcon.png")), 16.0f));

                assets.Add<Sprite>("Images/patches/Minigame_CampfirePatches/3dflagpride",
                    AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(ModPaths.GetPath("patches/Minigame_CampfirePatches/3dflagpride.png")), 10.0f));

                assets.Add<Texture2D>("Images/patches/Minigame_CampfirePatches/CampfireFrenzy_TutPic_04Replacement",
                    AssetLoader.TextureFromFile(ModPaths.GetPath("patches/Minigame_CampfirePatches/CampfireFrenzy_TutPic_04Replacement.png")));

                assets.Add<Sprite>("Images/patches/Minigame_PicnicPatches/brownie",
                    AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(ModPaths.GetPath("patches/Minigame_PicnicPatches/brownie.png")), 50.0f));

                assets.Add<Texture2D>("Images/PosterObject/poster0",
                    AssetLoader.TextureFromFile(ModPaths.GetPath("PosterObject/poster0.png")));

                assets.Add<Texture2D>("Images/PosterObject/bezzPriPoster",
                    AssetLoader.TextureFromFile(ModPaths.GetPath("npcs/Bezz/bezzPriPoster.png")));

                AssetLoader.LocalizationFromMod(this);

                assets.Add<string>("Midis/npcs/Bezz/FlagCatch0", AssetLoader.MidiFromMod("Midis/npcs/Bezz/FlagCatch0", this, "Midis", "npcs", "Bezz", "FlagCatch0.mid"));
                assets.Add<string>("Midis/npcs/Bezz/FlagCatch1", AssetLoader.MidiFromMod("Midis/npcs/Bezz/FlagCatch1", this, "Midis", "npcs", "Bezz", "FlagCatch1.mid"));
                assets.Add<string>("Midis/npcs/Bezz/FlagCatchEnd0", AssetLoader.MidiFromMod("Midis/npcs/Bezz/FlagCatchEnd0", this, "Midis", "npcs", "Bezz", "FlagCatchEnd0.mid"));
                assets.Add<string>("Midis/npcs/Bezz/FlagCatchEnd1", AssetLoader.MidiFromMod("Midis/npcs/Bezz/FlagCatchEnd1", this, "Midis", "npcs", "Bezz", "FlagCatchEnd1.mid"));

                assets.Add<SoundObject>("Sounds/npcs/Bezz/bezzTarget0", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "Sounds", "npcs", "Bezz", "bezzTarget0.wav"), "bezzTarget0", SoundType.Voice, new Color(1f, 0.5f, 0f)));
                assets.Add<SoundObject>("Sounds/npcs/Bezz/bezzTarget1", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "Sounds", "npcs", "Bezz", "bezzTarget1.wav"), "bezzTarget1", SoundType.Voice, new Color(1f, 0.5f, 0f)));
                assets.Add<SoundObject>("Sounds/npcs/Bezz/bezzPrompt0", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "Sounds", "npcs", "Bezz", "bezzPrompt0.wav"), "bezzPrompt0", SoundType.Voice, new Color(1f, 0.5f, 0f)));
                assets.Add<SoundObject>("Sounds/npcs/Bezz/bezzPrompt1", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "Sounds", "npcs", "Bezz", "bezzPrompt1.wav"), "bezzPrompt1", SoundType.Voice, new Color(1f, 0.5f, 0f)));
                assets.Add<SoundObject>("Sounds/npcs/Bezz/bezzActivityWin0", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "Sounds", "npcs", "Bezz", "bezzActivityWin0.wav"), "bezzActivityWin0", SoundType.Voice, new Color(1f, 0.5f, 0f)));
                assets.Add<SoundObject>("Sounds/npcs/Bezz/bezzActivityWin1", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "Sounds", "npcs", "Bezz", "bezzActivityWin1.wav"), "bezzActivityWin1", SoundType.Voice, new Color(1f, 0.5f, 0f)));
                assets.Add<SoundObject>("Sounds/npcs/Bezz/bezzActivityWin1Alt", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "Sounds", "npcs", "Bezz", "bezzActivityWin1Alt.wav"), "bezzActivityWin1Alt", SoundType.Voice, new Color(1f, 0.5f, 0f)));
                assets.Add<SoundObject>("Sounds/npcs/Bezz/bezzActivityLoss0", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "Sounds", "npcs", "Bezz", "bezzActivityLoss0.wav"), "bezzActivityLoss0", SoundType.Voice, new Color(1f, 0.5f, 0f)));
                assets.Add<SoundObject>("Sounds/npcs/Bezz/bezzActivityLoss1", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "Sounds", "npcs", "Bezz", "bezzActivityLoss1.wav"), "bezzActivityLoss1", SoundType.Voice, new Color(1f, 0.5f, 0f)));
                assets.Add<SoundObject>("Sounds/npcs/Bezz/bezzActivityLoss2", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "Sounds", "npcs", "Bezz", "bezzActivityLoss2.wav"), "bezzActivityLoss2", SoundType.Voice, new Color(1f, 0.5f, 0f)));
            }
            catch { }
        }

        public void updateAssets()
        {
            Texture2D tex = null;
            try { tex = assets.Get<Texture2D>("Images/patches/Minigame_CampfirePatches/CampfireFrenzy_TutPic_04Replacement"); } catch { }
            if (tex != null)
                AssetLoader.ReplaceTexture("CampfireFrenzy_TutPic_04", AssetLoader.AttemptConvertTo(tex, TextureFormat.RGB24));
        }

        public void registerNPCs()
        {
            NPCBuilder<Bezz> bezzBuilder = new(Info);
            bezzBuilder.SetName("Bezz");
            bezzBuilder.SetEnum("npcs/Bezz");
            bezzBuilder.SetPoster(AssetLoader.TextureFromFile(ModPaths.GetPath("npcs/Bezz/bezzPriPoster.png")), "bezzPriPosterTitle", "bezzPriPosterDesc");
            bezzBuilder.AddLooker();
            bezzBuilder.AddTrigger();
            bezzBuilder.AddSpawnableRoomCategories(RoomCategory.Hall);
            Bezz bezz = bezzBuilder.Build();

            Sprite idle = null;
            try { idle = assets.Get<Sprite>("Images/npcs/Bezz/bezzIdle"); } catch { }
            if (idle != null)
                bezz.spriteRenderer[0].sprite = idle;

            bezz.spriteRenderer[0].transform.localPosition += Vector3.up;

            assets.Add<NPC>("npcs/Bezz", bezz);
        }

        public void registerCompatibilities()
        {
            if (Chainloader.PluginInfos.ContainsKey("mtm101.rulerp.baldiplus.levelstudio"))
            {
                try { LevelStudioSupport.Register(); } catch { }
            }
        }

        public void sceneGenerated(string sceneTitle, int sceneIndex, SceneObject sceneObject)
        {
            try
            {
                if (sceneObject == null) return;
                if (!sceneTitle.StartsWith("F")) return;

                sceneObject.MarkAsNeverUnload();
                CustomLevelObject[] levelObjects = sceneObject.GetCustomLevelObjects();

                NPC bezzNpc = null;
                try { bezzNpc = assets.Get<NPC>("npcs/Bezz"); } catch { }

                Texture2D poster0 = null;
                try { poster0 = assets.Get<Texture2D>("Images/PosterObject/poster0"); } catch { }

                for (int i = 0; i < levelObjects.Length; i++)
                {
                    CustomLevelObject levelObject = levelObjects[i];
                    if (levelObject == null) continue;

                    if (sceneIndex > 0 && bezzNpc != null)
                    {
                        int weight = 100 + (sceneIndex + 1) * 10;
                        sceneObject.potentialNPCs.Add(new() { selection = bezzNpc, weight = weight });
                    }

                    if (poster0 != null)
                        levelObject.posters = levelObject.posters.AddToArray(new WeightedPosterObject() { selection = ObjectCreators.CreatePosterObject(poster0, new PosterTextData[0]), weight = 25 });
                }
            }
            catch { }
        }
    }
}