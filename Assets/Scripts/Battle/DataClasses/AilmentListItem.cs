using System;

namespace Battle {
    public class AilmentListItem: IEquatable<Ailment>
    {
        public Ailment ailment;
        public float buildup;
        public bool minorActive;
        public bool majorActive;
        public AilmentListItem(Ailment in_ailment, float in_buildup) {
            ailment = in_ailment;
            buildup = in_buildup;
        }

        public bool Equals(Ailment in_ailemnt) {
            // Just in case the input value is null
            if (in_ailemnt == null) return false;

            // Compare the ailment types of this ailment and the input ailment
            if (in_ailemnt == ailment) return true;
            else return false;
        }
    }
}

