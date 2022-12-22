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
        [SerializeField] private float _strength;
        [SerializeField] private float _willpower;
        [SerializeField] private float _dexterity;
        [SerializeField] private float _focus;
        [SerializeField] private float _endurance;
        [SerializeField] private float _agility;

        public Defenses defenses;
        public Dictionary<AilmentTypes, float> ailmentResistances;
        public Dictionary<DamageTypes, float> damageBonuses;
        public Dictionary<RateTypes, float> rateBonuses;
        

        void Awake() {
            float evasion = (attributes.agi.val - 10f) / 2f;
            float block = (attributes.agi.val - 10f) / 2f;
            
            attributes = new Attributes(_strength, _willpower, _dexterity, _focus, _endurance, _agility);
            resources = new Resources(_maxHP, _maxESS, _maxAP);
            defenses = new Defenses(evasion, block);
            ailmentResistances = new Dictionary<AilmentTypes, float>();
            damageBonuses = new Dictionary<DamageTypes, float>();
            rateBonuses = new Dictionary<RateTypes, float>();
        }
    }

}
