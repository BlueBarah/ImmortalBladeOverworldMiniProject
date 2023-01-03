namespace Battle {
    public class Defenses
    {
        public Stat evasion;
        public Stat block;
        public Stat intercept = new Stat(0f);
        public Stat counter = new Stat(0f);
        public Defenses(float in_evasion, float in_block) {
            evasion = new Stat(in_evasion);
            block = new Stat(in_block);
        }
    }

}
