using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.ObjectCreation;
using PlusLevelStudio;
using PlusLevelStudio.Editor.Tools;
using PlusStudioLevelLoader;
using PridePerception.core;
using PridePerception.util;
using UnityEngine;

namespace PridePerception.compat
{
    public static class LevelStudioSupport
    {
        public static void Register()
        {
            if (!LevelStudioPlugin.Instance) return;

            var assets = Plugin.assets;
            if (assets == null) return;

            var bezz = assets.Get<NPC>("npcs/Bezz");
            if (bezz == null) return;

            var posterBezzTex = assets.Get<Texture2D>("Images/PosterObject/bezzPriPoster");
            if (posterBezzTex != null)
            {
                var posterBezz = ObjectCreators.CreatePosterObject(posterBezzTex, new PosterTextData[0]);
                posterBezz.name = "bezz_rule";
                LevelLoaderPlugin.Instance.posterAliases["bezz_rule"] = posterBezz;
            }

            var poster0Tex = assets.Get<Texture2D>("Images/PosterObject/poster0");
            if (poster0Tex != null)
            {
                var poster0 = ObjectCreators.CreatePosterObject(poster0Tex, new PosterTextData[0]);
                poster0.name = "poster0";
                LevelLoaderPlugin.Instance.posterAliases["poster0"] = poster0;
            }

            LevelLoaderPlugin.Instance.npcAliases["Bezz"] = bezz;
            EditorInterface.AddNPCVisual("Bezz", bezz);

            var slotSprite = assets.Get<Sprite>("Images/PrideSlotIndividual");
            if (slotSprite == null)
                slotSprite = bezz.spriteRenderer[0].sprite;

            var npcIcon = assets.Get<Sprite>("Images/compat/LevelStudioCompat/npc_bezz");
            if (npcIcon == null)
                npcIcon = bezz.spriteRenderer[0].sprite;

            EditorInterfaceModes.AddModeCallback((mode, vanilla) =>
            {
                var npcTool = new NPCTool("Bezz", npcIcon);
                npcTool.frameOverride = slotSprite;
                EditorInterfaceModes.AddToolToCategory(mode, "npcs", npcTool, false);

                if (LevelLoaderPlugin.Instance.posterAliases.ContainsKey("bezz_rule"))
                {
                    var posterToolBezz = new PosterTool("bezz_rule");
                    posterToolBezz.frameOverride = slotSprite;
                    EditorInterfaceModes.AddToolToCategory(mode, "posters", posterToolBezz, false);
                }

                if (LevelLoaderPlugin.Instance.posterAliases.ContainsKey("poster0"))
                {
                    var posterTool0 = new PosterTool("poster0");
                    posterTool0.frameOverride = slotSprite;
                    EditorInterfaceModes.AddToolToCategory(mode, "posters", posterTool0, false);
                }
            });
        }
    }
}