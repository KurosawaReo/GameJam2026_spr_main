/*
   - Luggage -
   荷物クラス用のscript。(基本はいじらないで使う)
*/
/*
   [使用例]

   Luggage l = new Luggage();   //荷物を作成.

   //荷物の設定.
   l.type  = LuggageType.Chair; //この荷物は椅子.
   l.point = 10;                //pointは10.
*/
using System;

/// <summary>
/// 荷物の種類は何があるか.
/// </summary>
public enum LuggageType
{
    BalanceBall,
    Bicycle,
    Bed,
    Cactus,
    Chair,     //椅子.
    ChairBaby,
    Desk,      //机.
    Dryer,
    ElectricFan,
    FireExtinguisher,
    Game,
    Hat,
    Marlin,
    Pants,
    Plants,
    Shirt,
    Tv,
    Umbrella,
    VacuumCleaner,
}

/// <summary>
/// 「荷物」データ.
/// 荷物1つ分のデータをまとめて管理するためのもの.
/// </summary>
[Serializable]
public class Luggage
{
    public LuggageType type; //この荷物は何か.
    public int point;        //この荷物は何pointか.
}
