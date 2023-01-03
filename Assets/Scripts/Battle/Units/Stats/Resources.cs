namespace Battle {
    public class Resources
    {
        public Stat HP_max;
        public Stat ESS_max;
        public Stat AP_max;
        public Resources(float in_hp, float in_ess, float in_ap) {
            HP_max = new Stat(in_hp);
            ESS_max = new Stat(in_ess);
            AP_max = new Stat(in_ap);

        }
    }

}
