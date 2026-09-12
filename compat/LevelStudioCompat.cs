using MTM101BaldAPI;
using MTM101BaldAPI.ObjectCreation;
using PlusLevelStudio;
using PlusLevelStudio.Editor.Tools;
using PlusStudioLevelLoader;
using PridePerception.core;
using UnityEngine;

namespace PridePerception.compat
{
    public static class LevelStudioSupport
    {
        public static void Register()
        {
            if (!LevelStudioPlugin.Instance) return;
            if (Plugin.assets == null) return;

            NPC bezz = null;
            try { bezz = Plugin.assets.Get<NPC>("npcs/Bezz"); } catch { }
            if (bezz == null) return;

            if (bezz.Poster != null)
            {
                bezz.Poster.name = "bezz_rule";
                LevelLoaderPlugin.Instance.posterAliases["bezz_rule"] = bezz.Poster;
            }

            Texture2D poster0Tex = null;
            try { poster0Tex = Plugin.assets.Get<Texture2D>("Images/PosterObject/poster0"); } catch { }
            if (poster0Tex != null)
            {
                var poster0 = ObjectCreators.CreatePosterObject(poster0Tex, new PosterTextData[0]);
                poster0.name = "poster0";
                LevelLoaderPlugin.Instance.posterAliases["poster0"] = poster0;
            }

            LevelLoaderPlugin.Instance.npcAliases["Bezz"] = bezz;
            EditorInterface.AddNPCVisual("Bezz", bezz);

            Sprite slotSprite = null;
            try { slotSprite = Plugin.assets.Get<Sprite>("Images/PrideSlotIndividual"); } catch { }

            Sprite npcIcon = null;
            try { npcIcon = Plugin.assets.Get<Sprite>("Images/compat/LevelStudioCompat/npc_bezz"); } catch { }
            if (npcIcon == null && bezz.spriteRenderer.Length > 0)
                npcIcon = bezz.spriteRenderer[0].sprite;

            EditorInterfaceModes.AddModeCallback((mode, vanilla) =>
            {
                var npcTool = new NPCTool("Bezz", npcIcon);
                if (slotSprite != null) npcTool.frameOverride = slotSprite;
                EditorInterfaceModes.AddToolToCategory(mode, "npcs", npcTool, false);

                if (LevelLoaderPlugin.Instance.posterAliases.ContainsKey("bezz_rule"))
                {
                    var tool = new PosterTool("bezz_rule");
                    if (slotSprite != null) tool.frameOverride = slotSprite;
                    EditorInterfaceModes.AddToolToCategory(mode, "posters", tool, false);
                }

                if (LevelLoaderPlugin.Instance.posterAliases.ContainsKey("poster0"))
                {
                    var tool = new PosterTool("poster0");
                    if (slotSprite != null) tool.frameOverride = slotSprite;
                    EditorInterfaceModes.AddToolToCategory(mode, "posters", tool, false);
                }
            });
        }
    }
}