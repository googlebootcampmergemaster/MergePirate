using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitScriptableObject", menuName = "ScriptableObjects/UnitScriptableObject", order = 1)]
public class UnitSO : ScriptableObject 
{   
    public string unitName;
    public int unitID;
    public int unitLevel;
    public int unitCost;
    public int unitHealth;
    public int unitAttack;
    public int unitDefense;
    public int unitSpeed;
    public int unitRange;
    public int unitAttackSpeed;
    public int unitAttackRange;
    public int unitAttackType;
}
