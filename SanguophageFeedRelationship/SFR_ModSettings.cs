//inspired by https://gist.github.com/erdelf/84dce0c0a1f00b5836a9d729f845298a
using HarmonyLib;
using RimWorld; 
using System.Reflection;
using UnityEngine;
using Verse;

namespace SanguophageFeedRelationship
{
    [StaticConstructorOnStartup]
    class Main
    { 
        static Main()
        {
            var harmony = new Harmony("com.SanguophageFeedRelationship.rimworld.mod");
            harmony.PatchAll(Assembly.GetExecutingAssembly()); 
        } 
    }


    public class SFR_Mod : Mod
    {
        readonly SFR_ModSettings md = new SFR_ModSettings();
        /*
        private void PatchEarly()
        {
            ConstructorInfo original = AccessTools.Constructor(typeof(ITab_Pawn_Social));
            MethodInfo postfix = AccessTools.Method(typeof(Patch_FillTab), nameof(Patch_FillTab.Postfix));

            Harmony harmony = new Harmony("com.SanguophageFeedRelationship.rimworld.mod");
            harmony.Patch(original, postfix: new HarmonyMethod(postfix));
        }*/

        public SFR_Mod(ModContentPack content) : base(content)
        {
            base.GetSettings<SFR_ModSettings>();
        }

        public override string SettingsCategory()
        {
            return "SanguophageFeedRelationship";
        }

        private static Vector2 scrollPosition = Vector2.zero;
        public override void DoSettingsWindowContents(Rect inRect)
        {
            // Example: https://www.programmersought.com/article/99794631765/

            //Rect rect = new Rect(0, 0, 0.9f * inRect.width, 0.8f * inRect.height);
            Listing_Standard ls = new Listing_Standard();

            float linesOfSettings = 30f;
            Rect rect = new Rect(inRect.x, inRect.y, inRect.width, inRect.height);
            Rect rect2 = new Rect(0f, 0f, inRect.width - 30f, linesOfSettings * 24);
            Widgets.BeginScrollView(rect, ref scrollPosition, rect2, true);

            ls.Begin(rect2);
            string s;
            if (ls.ButtonText("Restore default settings")) { SFR_ModSettings.SetDefaults(); } //Button and monitor

            ls.GapLine(20f);  
            ls.Label("Traits: ");

            s = "Should this trait generate at all or not?";
            ls.CheckboxLabeled("Sanguophile trait generation: ", ref SFR_ModSettings.Sanguophile_generated, s);
            //s = SFR_ModSettings.Sanguophile_commonality.ToString();
            //ls.TextFieldNumericLabeled("Sanguophile trait commonality: ", ref SFR_ModSettings.Sanguophile_commonality, ref s);
            s = SFR_ModSettings.Sanguophile_baseMoodEffect.ToString();
            ls.TextFieldNumericLabeled("Sanguophile bite mood effect: ", ref SFR_ModSettings.Sanguophile_baseMoodEffect, ref s);
            s = SFR_ModSettings.Sanguophile_opinionEffect.ToString();
            ls.TextFieldNumericLabeled("Sanguophile bite opinion effect: ", ref SFR_ModSettings.Sanguophile_opinionEffect, ref s);
            s = SFR_ModSettings.Sanguophile_opinionBonus.ToString();
            ls.TextFieldNumericLabeled("Sanguophile base opinion bonus: ", ref SFR_ModSettings.Sanguophile_opinionBonus, ref s);

            ls.Gap(5f);

            s = "Should this trait generate at all or not?";
            ls.CheckboxLabeled("Sanguofriend trait generation: ", ref SFR_ModSettings.Sanguofriend_generated, s);
            //s = SFR_ModSettings.Sanguofriend_commonality.ToString();
            //ls.TextFieldNumericLabeled("Sanguofriend trait commonality: ", ref SFR_ModSettings.Sanguofriend_commonality, ref s);
            s = SFR_ModSettings.Sanguofriend_opinionEffect.ToString();
            ls.TextFieldNumericLabeled("Sanguofriend bite mood effect: ", ref SFR_ModSettings.Sanguofriend_opinionEffect, ref s);
            s = SFR_ModSettings.Sanguofriend_baseMoodEffect.ToString();
            ls.TextFieldNumericLabeled("Sanguofriend bite opinion effect: ", ref SFR_ModSettings.Sanguofriend_baseMoodEffect, ref s);

            ls.Gap(5f);

            s = "Should this trait generate at all or not?";
            ls.CheckboxLabeled("Sanguophobe trait generation: ", ref SFR_ModSettings.Sanguophobe_generated, s);
            //s = SFR_ModSettings.Sanguophobe_commonality.ToString();
            //ls.TextFieldNumericLabeled("Sanguophobe trait commonality: ", ref SFR_ModSettings.Sanguophobe_commonality, ref s);
            s = SFR_ModSettings.Sanguophobe_baseMoodEffect.ToString();
            ls.TextFieldNumericLabeled("Sanguophobe bite mood effect: (-)", ref SFR_ModSettings.Sanguophobe_baseMoodEffect, ref s);
            s = SFR_ModSettings.Sanguophobe_opinionEffect.ToString();
            ls.TextFieldNumericLabeled("Sanguophobe bite opinion effect: (-)", ref SFR_ModSettings.Sanguophobe_opinionEffect, ref s);
            s = SFR_ModSettings.Sanguophobe_opinionPenalty.ToString();
            ls.TextFieldNumericLabeled("Sanguophobe base opinion penalty: (-)", ref SFR_ModSettings.Sanguophobe_opinionPenalty, ref s);

            ls.GapLine(20f);
            ls.Label("Relationships: ");

            s = SFR_ModSettings.Lover_baseMoodEffect.ToString();
            ls.TextFieldNumericLabeled("Lover bite mood effect: ", ref SFR_ModSettings.Lover_baseMoodEffect, ref s);
             
            ls.Gap(5f);

            s = SFR_ModSettings.CloseFriend_baseMoodEffect.ToString();
            ls.TextFieldNumericLabeled("Close friend bite mood effect: ", ref SFR_ModSettings.CloseFriend_baseMoodEffect, ref s);
            s = SFR_ModSettings.CloseFriend_opinionEffect.ToString();
            ls.TextFieldNumericLabeled("Close friend bite opinion effect: ", ref SFR_ModSettings.CloseFriend_opinionEffect, ref s);
            s = SFR_ModSettings.CloseFriend_minimumOpinion.ToString();
            ls.TextFieldNumericLabeled("Close friend minimum opinion: ", ref SFR_ModSettings.CloseFriend_minimumOpinion, ref s); 

            ls.GapLine(20f);
            ls.Label("Other settings: ");

            s = SFR_ModSettings.HemogenNutrition.ToString();
            ls.TextFieldNumericLabeled("Nutrition gained from hemogen: ", ref SFR_ModSettings.HemogenNutrition, ref s);

            s = SFR_ModSettings.minimumSeverity.ToString();
            ls.TextFieldNumericLabeled("Hediff severity threshold for auto-biting victim: ", ref SFR_ModSettings.minimumSeverity, ref s);
            ls.Label("From 0 to 1. I like to play with 0.5, 0.6 doesn't trigger a bite with baseline pawns.");
            ls.Gap(2f);

            s = "If this is on then bloodfeeders will feed off any pawn that has the Auto-Feed toggle on regardless of social consequences.";
            ls.CheckboxLabeled("Allow auto-feed even with negative mood reaction: ", ref SFR_ModSettings.autoFeedFree, s);

            s = "If the mod is bugging your social tab you can turn off the UI toggle for now without disabling the whole mod.";
            ls.CheckboxLabeled("Turn off auto-feed toggle on Social tab: ", ref SFR_ModSettings.autoFeedOptionsOff, s);

            ls.End();
            Widgets.EndScrollView();
            base.DoSettingsWindowContents(rect2);

        }

        public override void WriteSettings()
        {
            base.WriteSettings();
            md.ApplyWorkSetting(); 
        }
    }
    [StaticConstructorOnStartup]
    public class SFR_ModSettings : ModSettings
    {
        // traits
        public static int Sanguophile_opinionBonus = 10;
        public static bool Sanguophile_generated = true;
        public static float Sanguophile_commonality = 0.15f;
        public static int Sanguophile_baseMoodEffect = 10;
        public static int Sanguophile_opinionEffect = 15;

        public static int Sanguofriend_baseMoodEffect = 1;
        public static bool Sanguofriend_generated = true;
        public static float Sanguofriend_commonality = 0.15f;
        public static int Sanguofriend_opinionEffect = 7;

        public static int Sanguophobe_opinionPenalty = 20;
        public static bool Sanguophobe_generated = true;
        public static int Sanguophobe_baseMoodEffect = 30;
        public static float Sanguophobe_commonality = 0.075f;
        public static int Sanguophobe_opinionEffect = 20;

        // relationship
        public static int Lover_baseMoodEffect = 5;
        public static int Lover_opinionEffect = 10;

        public static int CloseFriend_baseMoodEffect = 3;
        public static int CloseFriend_opinionEffect = 5;
        public static int CloseFriend_minimumOpinion = 85;

        // other settings
        public static float HemogenNutrition = 0.35f;
        public static bool autoFeedOptionsOff = false;
        public static float minimumSeverity = 0.4499f;
        public static bool autoFeedFree = false;


        public static void SetDefaults()
        {
            // traits
            Sanguophile_baseMoodEffect = 10;
            Sanguophile_generated = true;
            Sanguophile_commonality = 0.15f;
            Sanguophile_opinionBonus = 10;
            Sanguophile_opinionEffect = 15;

            Sanguofriend_baseMoodEffect = 1;
            Sanguofriend_generated = true;
            Sanguofriend_commonality = 0.15f;
            Sanguofriend_opinionEffect = 7;
             
            Sanguophobe_baseMoodEffect = 30;
            Sanguophobe_generated = true;
            Sanguophobe_commonality = 0.075f;
            Sanguophobe_opinionPenalty = 20;
            Sanguophobe_opinionEffect = 20;

            // relationship
            Lover_baseMoodEffect = 5;
            Lover_opinionEffect = 10;
            CloseFriend_baseMoodEffect = 3;
            CloseFriend_minimumOpinion = 85;
            CloseFriend_opinionEffect = 5;

            // other settings
            HemogenNutrition = 0.35f;
            autoFeedOptionsOff = false;
            minimumSeverity = 0.4499f; // 0.4499f
            autoFeedFree = false;
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look<int>(ref Sanguophile_baseMoodEffect, "SanguophageFeedRelationship.Sanguophile_baseMoodEffect", Sanguophile_baseMoodEffect, true);
            Scribe_Values.Look<bool>(ref Sanguophile_generated, "SanguophageFeedRelationship.Sanguophile_generated", Sanguophile_generated, true);
            //Scribe_Values.Look<float>(ref Sanguophile_commonality, "SanguophageFeedRelationship.Sanguophile_commonality", Sanguophile_commonality, true); 
            Scribe_Values.Look<int>(ref Sanguophile_opinionBonus, "SanguophageFeedRelationship.Sanguophile_opinionBonus", Sanguophile_opinionBonus, true);
            Scribe_Values.Look<int>(ref Sanguophile_opinionEffect, "SanguophageFeedRelationship.Sanguophile_opinionEffect", Sanguophile_opinionEffect, true);

            Scribe_Values.Look<int>(ref Sanguofriend_baseMoodEffect, "SanguophageFeedRelationship.Sanguofriend_baseMoodEffect", Sanguofriend_baseMoodEffect, true);
            Scribe_Values.Look<bool>(ref Sanguofriend_generated, "SanguophageFeedRelationship.Sanguofriend_generated", Sanguofriend_generated, true);
            //Scribe_Values.Look<float>(ref Sanguofriend_commonality, "SanguophageFeedRelationship.Sanguofriend_commonality", Sanguofriend_commonality, true);
            Scribe_Values.Look<int>(ref Sanguofriend_opinionEffect, "SanguophageFeedRelationship.Sanguofriend_opinionEffect", Sanguofriend_opinionEffect, true);

            Scribe_Values.Look<int>(ref Sanguophobe_baseMoodEffect, "SanguophageFeedRelationship.Sanguophobe_baseMoodEffect", Sanguophobe_baseMoodEffect, true);
            Scribe_Values.Look<bool>(ref Sanguophobe_generated, "SanguophageFeedRelationship.Sanguophobe_generated", Sanguophobe_generated, true);
            //Scribe_Values.Look<float>(ref Sanguophobe_commonality, "SanguophageFeedRelationship.Sanguophobe_commonality", Sanguophobe_commonality, true);
            Scribe_Values.Look<int>(ref Sanguophobe_opinionPenalty, "SanguophageFeedRelationship.Sanguophobe_opinionPenalty", Sanguophobe_opinionPenalty, true);
            Scribe_Values.Look<int>(ref Sanguophobe_opinionEffect, "SanguophageFeedRelationship.Sanguophobe_opinionEffect", Sanguophobe_opinionEffect, true);


            Scribe_Values.Look<int>(ref Lover_baseMoodEffect, "SanguophageFeedRelationship.Lover_baseMoodEffect", Lover_baseMoodEffect, true);
            Scribe_Values.Look<int>(ref Lover_opinionEffect, "SanguophageFeedRelationship.Lover_opinionEffect", Lover_opinionEffect, true);
            Scribe_Values.Look<int>(ref CloseFriend_baseMoodEffect, "SanguophageFeedRelationship.CloseFriend_baseMoodEffect", CloseFriend_baseMoodEffect, true);
            Scribe_Values.Look<int>(ref CloseFriend_minimumOpinion, "SanguophageFeedRelationship.CloseFriend_minimumOpinion", CloseFriend_minimumOpinion, true);
            Scribe_Values.Look<int>(ref CloseFriend_opinionEffect, "SanguophageFeedRelationship.CloseFriend_opinionEffect", CloseFriend_opinionEffect, true);
             
            Scribe_Values.Look<float>(ref HemogenNutrition, "SanguophageFeedRelationship.HemogenNutrition", HemogenNutrition, true);
            Scribe_Values.Look<bool>(ref autoFeedOptionsOff, "SanguophageFeedRelationship.autoFeedOptionsOff", autoFeedOptionsOff, true);
            Scribe_Values.Look<bool>(ref autoFeedFree, "SanguophageFeedRelationship.autoFeedFree", autoFeedFree, true);
            Scribe_Values.Look<float>(ref minimumSeverity, "SanguophageFeedRelationship.minimumSeverity", minimumSeverity, true);
        }


        public void ApplyWorkSetting()
        {
            if (SFR_DefOf.FedOn_Sanguophile != null)
            {
                SFR_DefOf.FedOn_Sanguophile.stages[0].baseMoodEffect = Sanguophile_baseMoodEffect;
                SFR_DefOf.SFR_Sanguophile.generated = Sanguophile_generated;
                SFR_DefOf.FedOn_SocialSanguophile.stages[0].baseOpinionOffset = Sanguophile_opinionEffect;
                SFR_DefOf.SanguophageFan.stages[0].baseOpinionOffset = Sanguophile_opinionBonus;

                SFR_DefOf.FedOn_Sanguofriend.stages[0].baseMoodEffect = Sanguofriend_baseMoodEffect;
                SFR_DefOf.SFR_Sanguofriend.generated = Sanguofriend_generated;
                SFR_DefOf.FedOn_SocialSanguofriend.stages[0].baseOpinionOffset = Sanguofriend_opinionEffect;

                SFR_DefOf.FedOn_Sanguophobe.stages[0].baseMoodEffect = -Sanguophobe_baseMoodEffect;
                SFR_DefOf.SFR_Sanguophobe.generated = Sanguophobe_generated;
                SFR_DefOf.FedOn_SocialSanguophobe.stages[0].baseOpinionOffset = -Sanguophobe_opinionEffect;
                SFR_DefOf.Sanguophobic.stages[0].baseOpinionOffset = -Sanguophobe_opinionPenalty;

                SFR_DefOf.FedOn_Lover.stages[0].baseMoodEffect = Lover_baseMoodEffect;
                SFR_DefOf.FedOn_SocialLover.stages[0].baseOpinionOffset = Lover_opinionEffect;

                SFR_DefOf.FedOn_CloseFriend.stages[0].baseMoodEffect = CloseFriend_baseMoodEffect;
                SFR_DefOf.FedOn_SocialCloseFriend.stages[0].baseOpinionOffset = CloseFriend_opinionEffect;
            }
        }
    }


    [DefOf]
    public static class SFR_DefOf
    {
        public static ThoughtDef FedOn_Sanguophile;
        public static ThoughtDef FedOn_SocialSanguophile;
        public static ThoughtDef SanguophageFan;

        public static ThoughtDef FedOn_Sanguofriend;
        public static ThoughtDef FedOn_SocialSanguofriend;

        public static ThoughtDef FedOn_Sanguophobe;
        public static ThoughtDef FedOn_SocialSanguophobe;
        public static ThoughtDef Sanguophobic;

        public static ThoughtDef FedOn_Lover;
        public static ThoughtDef FedOn_SocialLover;

        public static ThoughtDef FedOn_CloseFriend;
        public static ThoughtDef FedOn_SocialCloseFriend;

        public static TraitDef SFR_Sanguophile;
        public static TraitDef SFR_Sanguofriend;
        public static TraitDef SFR_Sanguophobe;

        public static JobDef ChillPawnBloodfeed;

        public static PreceptDef Bloodfeeders_Revered;


        //public static GeneDef VRE_SanguoFeeder = null; // Vanilla Races Expanded - Sanguophage

        static SFR_DefOf()
        {
            /*if (ModsConfig.IsActive("vanillaracesexpanded.sanguophage")) {
                Log.Message("vre is active");
                VRE_SanguoFeeder = DefDatabase<GeneDef>.GetNamed("VRE_SanguoFeeder"); }
            else
                Log.Message("vre is not active");*/

            DefOfHelper.EnsureInitializedInCtor(typeof(SFR_DefOf));
        }
    }
}
