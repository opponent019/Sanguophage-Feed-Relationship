using Verse;
using System.Collections.Generic;

namespace SanguophageFeedRelationship
{
    class SFR_AutoFeeders : GameComponent
    {
        // so many thanks to feldoh10 for all their help with debugging !

        //HashSet<Pawn> autoFeeders;
        static HashSet<int> autoFeederPawns;

        public SFR_AutoFeeders(Game g):base() {
            autoFeederPawns = new HashSet<int>();
        }

        public void SetAutoFeed(Pawn pawn, bool included)
        {
            if (autoFeederPawns.Contains(pawn.thingIDNumber))
            {
                if (included)
                    return;
                else
                {
                    autoFeederPawns.Remove(pawn.thingIDNumber);
                    //Log.Message("Removed " + pawn.Name + ". List now contains: " + autoFeederPawns.Count.ToString()); 
                }
            }
            else {
                if (included)
                {
                    autoFeederPawns.Add(pawn.thingIDNumber);
                    //Log.Message("Added " + pawn.Name + ". List now contains: " + autoFeederPawns.Count.ToString()); 
                }
                else
                    return;
            }
        }

        public bool GetAutoFeed(int pawnID)
        {
            //Log.Message("- Sending to check: " + pawnID + ". There are " + autoFeederPawns.Count + " pawns in the list.");
            return autoFeederPawns.Contains(pawnID);
            //return autoFeeders.Contains(pawn);
        }
        
        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.PostLoadInit)  
            { 

                if (autoFeederPawns == null)
                    autoFeederPawns = new HashSet<int>();

            }


            //Log.Message("EXPOSE DATA List count: " + autoFeeders.Count.ToString() + "  " + Scribe.mode.ToString());
            Scribe_Collections.Look(ref autoFeederPawns, "autoFeederPawnsIDs", LookMode.Value);

        }

    }
}
