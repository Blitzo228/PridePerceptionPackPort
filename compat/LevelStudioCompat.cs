using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.UI;
using PlusLevelStudio;
using PlusLevelStudio.Editor.Tools;
using PlusStudioLevelLoader;
using PridePerception.core;
using PridePerception.util;
using TMPro;
using UnityEngine;

namespace PridePerception.compat {
    public static class LevelStudioSupport {
        public static void Register() {
            NPC bezzPrefab = Plugin.assets.Get<NPC>("npcs/Bezz");
            EditorInterface.AddNPCVisual("Bezz", bezzPrefab);

            if (LevelLoaderPlugin.Instance != null && !LevelLoaderPlugin.Instance.npcAliases.ContainsKey("Bezz")) {
                LevelLoaderPlugin.Instance.npcAliases.Add("Bezz", bezzPrefab);
            }

            PosterObject BezzPoster = ScriptableObject.CreateInstance<PosterObject>();
            BezzPoster.name = "bezzPriPoster";
            BezzPoster.baseTexture = AssetLoader.TextureFromFile(ModPaths.GetPath("npcs/Bezz/bezzPriPoster.png"));

            BezzPoster.textData = new PosterTextData[] {
                new PosterTextData() {
                    textKey = "bezzPriPosterDesc",
                    position = new IntVector2(144, 98),
                    size = new IntVector2(96, 128),
                    fontSize = 12,
                    font = BaldiFonts.ComicSans12.FontAsset(),
                    color = Color.black,
                    alignment = TextAlignmentOptions.Center,
                    style = FontStyles.Normal
                },
                new PosterTextData() {
                    textKey = "bezzPriPosterTitle",
                    position = new IntVector2(48, 48),
                    size = new IntVector2(160, 32),
                    fontSize = 18,
                    font = BaldiFonts.ComicSans18.FontAsset(),
                    color = Color.black,
                    alignment = TextAlignmentOptions.Center,
                    style = FontStyles.Bold
                }
            };

            if (LevelLoaderPlugin.Instance != null && !LevelLoaderPlugin.Instance.posterAliases.ContainsKey("bezz_rule")) {
                LevelLoaderPlugin.Instance.posterAliases.Add("bezz_rule", BezzPoster);
            }

            EditorInterfaceModes.AddModeCallback((mode, isVanilla) => {
                Texture2D iconTex = AssetLoader.TextureFromFile(ModPaths.GetPath("compat/LevelStudioCompat/npc_bezz.png"));
                Sprite icon = AssetLoader.SpriteFromTexture2D(iconTex, 100f);
                EditorInterfaceModes.AddToolToCategory(mode, "npcs", new BezzTool(icon));

                if (mode.availableTools.ContainsKey("posters")) {
                    EditorInterfaceModes.AddToolToCategory(mode, "posters", new BezzPosterTool());
                }
            });
        }
    }

    public class BezzTool : NPCTool {
        public BezzTool(Sprite sprite) : base("Bezz", sprite) { }
        public override string titleKey => "Ed_Tool_Npc_Bezz";
        public override string descKey => "Ed_Tool_Npc_Bezz_Desc";
    }

    public class BezzPosterTool : PosterTool {
        public BezzPosterTool() : base("bezz_rule") { }
        public override string titleKey => "Bezz's Office Poster";
        public override string descKey => titleKey;
    }
}