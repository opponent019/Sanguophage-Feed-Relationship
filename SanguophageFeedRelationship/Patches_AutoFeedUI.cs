using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace SanguophageFeedRelationship
{
    [HarmonyPatch(typeof(SocialCardUtility))]
    [HarmonyPatch("DrawSocialCard")]
    public class Patch_DrawSocialCard
    {   
        public Patch_DrawSocialCard() {
        }


        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Rect temp = new Rect(0f, 0f, 0f, 0f);
            MethodInfo m_AddAutoFeedToggle = SymbolExtensions.GetMethodInfo(() => AddAutoFeedToggle(temp, temp, null));

            foreach (CodeInstruction instruction in instructions)
            {
                yield return instruction;
                if (instruction.opcode.Equals(OpCodes.Mul))
                { 
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 4);
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_1); 
                    yield return new CodeInstruction(OpCodes.Call, m_AddAutoFeedToggle); 
                }
            }
        }

        public static void AddAutoFeedToggle (Rect rect5, Rect rect, Pawn pawn)
        {
            if (SFR_ModSettings.autoFeedOptionsOff || !pawn.IsColonist)
                return;

            // Vanilla Races Expanded - Sanguophage mod
            bool bloodfeederLegal = false;
            if (pawn.genes.HasActiveGene(GeneDefOf.Hemogenic)) {
                if (ModsConfig.IsActive("vanillaracesexpanded.sanguophage")) 
                    bloodfeederLegal = true; 
            } else
                bloodfeederLegal = true;
            

            if (ModsConfig.BiotechActive && !pawn.Dead && bloodfeederLegal && (pawn.IsColonist || pawn.IsPrisoner || pawn.IsSlaveOfColony))
            {
                Widgets.BeginGroup(rect);

                Text.Font = GameFont.Small;
                //Rect SFR_rect = new Rect(rect.width/2, rect5.yMin - 30f, rect.width, 22f);
                Rect SFR_rect = new Rect((rect.width / 2) + 10f, rect5.yMin - 30f, (rect.width / 2) - 10f, 30f);

                SFR_AutoFeeders gc = Current.Game.GetComponent<SFR_AutoFeeders>();
                bool boo = gc.GetAutoFeed(pawn.thingIDNumber);

                //Widgets.CheckboxLabeled(SFR_rect, "Auto feed bloodfeeders?", ref boo, false, null, null, true);
                Widgets.CheckboxLabeled(SFR_rect, "Auto feed bloodfeeders?", ref boo, false);
                gc.SetAutoFeed(pawn, boo);
                
                Widgets.EndGroup();
            } 
        }

    }
     


        /*
        [HarmonyPatch(typeof(ITab_Pawn_Social))]
        [HarmonyPatch(MethodType.Constructor)]
        public class Patch_FillTab
        {
            public static void Postfix(ref Vector2 ___size)
            {
                Log.Message("Sizeeeeee: " + ___size);
                ___size += new Vector2(0f, 380f);
                Log.Message("Size new: " + ___size);
            }

        }*/

}