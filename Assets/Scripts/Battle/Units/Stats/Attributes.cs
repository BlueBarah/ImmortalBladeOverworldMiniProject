
namespace Battle {
    public class Attributes{
        public Stat str;
        public Stat will;
        public Stat dex;
        public Stat foc;
        public Stat end;
        public Stat agi;
        public Attributes(float in_str, float in_will, float in_dex, float in_foc, float in_end, float in_agi) {
            str = new Stat(in_str);
            will = new Stat(in_will);
            dex = new Stat(in_dex);
            foc = new Stat(in_foc);
            end = new Stat(in_end);
            agi = new Stat(in_agi);
        }
    }

}
