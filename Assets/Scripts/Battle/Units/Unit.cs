using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public class Unit : MonoBehaviour
    {
        public Resources resources;
        [Header("Resources")]
        [SerializeField] private float _maxHP;
        [SerializeField] private float _maxESS;
        [SerializeField] private float _maxAP;
        
        public Attributes attributes;
        [Space(10)][Header("Attributes")]
        [SerializeField] private float _level;
        [SerializeField] private float _strength;
        [SerializeField] private float _willpower;
        [SerializeField] private float _dexterity;
        [SerializeField] private float _focus;
        [SerializeField] private float _endurance;
        [SerializeField] private float _agility;

        public Defenses defenses;
        public BonusList<AilmentTypes> ailmentResistances = new BonusList<AilmentTypes>();
        public BonusList<DamageTypes> damageResistances = new BonusList<DamageTypes>();
        public BonusList<DamageTypes> damageBonuses = new BonusList<DamageTypes>();
        public BonusList<RateTypes> rateBonuses = new BonusList<RateTypes>();

        [Space(10)][Header("Actions")]
        [SerializeField] public List<Attack> Attacks;

        // States
        public HP HP_state { get; set; }
        public TN TN_state { get; set; }

        // Resources (Automatically round and clamp values to prevent invalid data)
        public float HP_current { 
            // A whole number between zero and _maxHP
            get {
                return HP_current;
            } 
            set {
                HP_current = Mathf.Clamp(Mathf.Round(value), 0f, _maxHP);
                calculateHPstate();
            } 
        }
        public float TN_current {
            // A value between 0.5 (50%) and 1.5 (150%), rounded to two decimal places
            get {
                return TN_current;
            }
            set {
                float roundedVal = Mathf.Round(value * 100) / 100;
                TN_current = Mathf.Clamp(roundedVal, 0.5f, 1.5f);
                calculateTNstate();
            }
        }
        public float AP_current {
            // A whole number between zero and _maxAP
            get {
                return AP_current;
            }
            set {
                AP_current = Mathf.Clamp(Mathf.Round(value), 0f, _maxAP);
            }
        }
        public float ESS_current {
            // A whole number between zero and _maxESS
            get {
                return ESS_current;
            }
            set {
                ESS_current = Mathf.Clamp(Mathf.Round(value), 0f, _maxESS);
            }
        }

        // Flags
        private bool flag_changeEvasion = false;
        private bool flag_changeBlock = false;
        private bool flag_changeHP = false;
        private bool flag_changeTN = false;
        

        void Awake() {
            attributes = new Attributes(_level, _strength, _willpower, _dexterity, _focus, _endurance, _agility);
            resources = new Resources(_maxHP, _maxESS, _maxAP);
            defenses = new Defenses(calculateEvasion(), calculateBlock());
        }
        void Update() {
            // Update calculated values if necessary
            if (flag_changeEvasion) defenses.evasion.val = calculateEvasion();
            if (flag_changeBlock) defenses.block.val = calculateBlock();
        }

        public void TakeDamage(DamageDealt in_damageData) {
            DamageTaken damageTaken = StaticUnitFunctions.CalculateDamage(in_damageData, damageResistances, defenses, _maxHP);
        }

        private float calculateEvasion() {
            return (attributes.agi.val - 10f) / 2f;
        }
        private float calculateBlock() {
            return (attributes.agi.val - 10f) / 2f;
        }
        private HP calculateHPstate() {
            HP returnState;
            float HP_percent = Mathf.Round((HP_current / _maxHP) * 100);

            if (HP_percent >= (int)HP.Full) returnState = HP.Full;
            else if (HP_percent >= (int)HP.High) returnState = HP.High;
            else if (HP_percent >= (int)HP.Low) returnState = HP.Low;
            else if (HP_percent >= (int)HP.VeryLow) returnState = HP.VeryLow;
            else returnState = HP.Incapacitated;

            return returnState;
        }
        private TN calculateTNstate() {
            TN returnState;
            float TN_wholeNumber = Mathf.Round(TN_current * 100);

            if (TN_wholeNumber >= (int)TN.VeryHigh) returnState = TN.VeryHigh;
            else if (TN_wholeNumber >= (int)TN.High) returnState = TN.High;
            else if (TN_wholeNumber >= (int)TN.Normal) returnState = TN.Normal;
            else if (TN_wholeNumber >= (int)TN.Low) returnState = TN.Low;
            else returnState = TN.VeryLow;

            return returnState;
        }
    }

}
