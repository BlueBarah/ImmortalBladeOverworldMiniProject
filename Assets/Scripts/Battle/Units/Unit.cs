using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    [RequireComponent(typeof(UnitActions))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Unit : MonoBehaviour, IComparable
    {
        public Resources resources;
        [Header("Resources")]
        [SerializeField] public float _maxHP;
        [SerializeField] public float _maxESS;
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
        public AilmentList ailmentList;
        // public SpecialRuleList specialRules;

        public UnitBattleData battleData;

        // States
        public HP HP_state { get; set; }
        public TN TN_state { get; set; }

        // Resources (Automatically round and clamp values to prevent invalid data)
        private float _hp = 0;
        public float HP_current { 
            // A whole number between zero and _maxHP
            get {
                return _hp;
            } 
            set {
                _hp = Mathf.Clamp(Mathf.Round(value), 0f, resources.HP_max.val);
                if (battleData != null)
                    battleData.currHP = _hp;
                HP_state = calculateHPstate();
            } 
        }
        private float _tn = 1;
        public float TN_current {
            // A value between 0.5 (50%) and 1.5 (150%), rounded to two decimal places
            get {
                return _tn;
            }
            set {
                float roundedVal = Mathf.Round(value * 100) / 100;
                _tn = Mathf.Clamp(roundedVal, 0.5f, 1.5f);
                TN_state = calculateTNstate();
            }
        }
        private float _ap = 0;
        public float AP_current {
            // A whole number between zero and _maxAP
            get {
                return _ap;
            }
            set {
                _ap = Mathf.Clamp(Mathf.Round(value), 0f, resources.AP_max.val);
            }
        }
        private float _ess;
        public float ESS_current {
            // A whole number between zero and _maxESS
            get {
                return _ess;
            }
            set {
                _ess = Mathf.Clamp(Mathf.Round(value), 0f, resources.ESS_max.val);
            }
        }

        // Flags
        private bool flag_changeEvasion = false;
        private bool flag_changeBlock = false;
        public bool flag_init { get; private set; } = false;
        

        void Awake() {
            if (!flag_init) {
                Init();
            }
        }
        void Update() {
            // Update calculated values if necessary
            if (flag_changeEvasion) defenses.evasion.val = calculateEvasion();
            if (flag_changeBlock) defenses.block.val = calculateBlock();
            //specialRules.CheckSpecialRules(this);
        }
        public int CompareTo(object obj)
        {
            // Use the unit's agility when sorting units in a list
            Unit otherUnit = obj as Unit;
            return attributes.agi.val.CompareTo(otherUnit.attributes.agi.val);
        }
        public void Init() {
            //Grab the unit data values from persistant battle data
            if (battleData != null)
            {
                attributes = new Attributes(battleData);
                resources = new Resources(battleData);

                _hp = battleData.currHP;
                _level = battleData.level;
                _strength = battleData.strength;
                _willpower = battleData.willpower;
                _dexterity = battleData.dexterity;
                _focus = battleData.focus;
                _endurance = battleData.endurance;
                _agility = battleData.agility;
                _maxHP = battleData.maxHP;
                _maxESS = battleData.maxESS;
                _maxAP = battleData.maxAP;
            }
            else
            {
                attributes = new Attributes(_level, _strength, _willpower, _dexterity, _focus, _endurance, _agility);
                resources = new Resources(_maxHP, _maxESS, _maxAP);
            }

            defenses = new Defenses(calculateEvasion(), calculateBlock());
            ailmentList = new AilmentList(this);

            InitializeResources();
            flag_init = true;
        }

        public DamageTaken TakeDamage(DamageDealt in_damageData) {
            DamageTaken damageTaken = StaticUnitFunctions.CalculateDamage(in_damageData, damageResistances, defenses, _maxHP);
            HP_current -= damageTaken.damage;
            TN_current += damageTaken.TN_change;
            if (in_damageData.ailments.Count > 0) {
                foreach (SerializableAilmentEntry item in damageTaken.ailments) {
                    ailmentList.AddAilment(item, false);
                }
                ailmentList.CheckAilments();
            }
            return damageTaken;
        }
        public void StartTurn() {
            Debug.Log($"START TURN: {name}");
            ailmentList.CheckAilments();
            // Apply any ailments that are meant to be applied at the start of the turn
            foreach (AilmentListItem item in ailmentList.ailments) {
                if (item.majorActive && item.ailment.major.applyAtStartOfTurn) item.ailment.major.ApplyEffect(this);
                else if (item.minorActive && item.ailment.minor.applyAtStartOfTurn) item.ailment.minor.ApplyEffect(this);
            }
        }
        public void EndTurn() {
            ailmentList.ManageAilments();
        }

        private void InitializeResources() {
            HP_current = resources.HP_max.val;
            ESS_current = resources.ESS_max.val;
            AP_current = resources.AP_max.val;
            TN_current = 1f;
        }
        private float calculateEvasion() {
            return (attributes.agi.val - 10f) / 2f;
        }
        private float calculateBlock() {
            return (attributes.agi.val - 10f) / 2f;
        }
        private HP calculateHPstate() {
            HP returnState;
            int HP_percent = Mathf.RoundToInt((HP_current / _maxHP) * 100);

            if (HP_percent >= (int)HP.Full) returnState = HP.Full;
            else if (HP_percent >= (int)HP.High) returnState = HP.High;
            else if (HP_percent >= (int)HP.Low) returnState = HP.Low;
            else if (HP_percent >= (int)HP.VeryLow) returnState = HP.VeryLow;
            else returnState = HP.Incapacitated;

            return returnState;
        }
        private TN calculateTNstate() {
            TN returnState;
            int TN_wholeNumber = Mathf.RoundToInt(TN_current * 100);

            if (TN_wholeNumber >= (int)TN.VeryHigh) returnState = TN.VeryHigh;
            else if (TN_wholeNumber >= (int)TN.High) returnState = TN.High;
            else if (TN_wholeNumber >= (int)TN.Normal) returnState = TN.Normal;
            else if (TN_wholeNumber >= (int)TN.Low) returnState = TN.Low;
            else returnState = TN.VeryLow;

            return returnState;
        }
    }

}
