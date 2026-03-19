/*
   - CardboardBox -
   段ボール箱クラス用のscript。(基本はいじらないで使う)
*/
/*
   [使用例]

   CardboardBox c = new CardboardBox(); //段ボール箱を作成.

   //段ボール箱の設定.
   c.point = 100; //段ボール箱のpoint(=荷物の合計point)は100.
*/
using System;

/// <summary>
/// 「段ボール箱」データ.
/// 段ボール箱1つ分のデータをまとめて管理するためのもの.
/// </summary>
[Serializable]
public class CardboardBox
{
    public int point; //この段ボール箱は何pointか.
}
