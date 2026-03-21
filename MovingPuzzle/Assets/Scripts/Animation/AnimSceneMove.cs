using UnityEngine;
using KR.Unity.Animation;

/// <summary>
/// ASM = AnimSceneMove
/// inかoutか.
/// </summary>
public enum ASMType
{
    In,
    Out
}

/// <summary>
/// シーン遷移する時のアニメーション.
/// </summary>
public class AnimSceneMove : AnimationKR
{
    [Header("- object -")]
    [SerializeField] GameObject square;

    [Header("- value -")]
    [SerializeField] ASMType type;    //アニメーション種類.
    [SerializeField] float   maxTime; //アニメーション時間.
    [SerializeField] float   moveLen; //移動量.

    bool active; //有効かどうか.

    void Start()
    {
        //1秒で移動.
        InitAnim(maxTime, t => t * t * (3f - 2f * t));

        switch (type)
        {
            case ASMType.In:
                active = false;
                break;
            case ASMType.Out:
                active = true;
                break;
        }
    }

    void Update()
    {
        //有効な間のみ.
        if (active)
        {
            UpdateAnim();
            MovePos();
            //アニメーションが終わったら.
            if (IsFinished())
            {
                active = false; //無効に.
            }            
        }
    }

    /// <summary>
    /// アニメーション実行.
    /// </summary>
    public void AnimExe()
    {
        active = true;
    }

    /// <summary>
    /// 座標移動.
    /// </summary>
    private void MovePos()
    {
        //座標 & サイズ.
        switch (type) 
        { 
            case ASMType.In:
                square.transform.position   = new Vector2(moveLen * (-1.0f + GetTimer()), 0);
                square.transform.localScale = new Vector2(moveLen * 2 * GetTimer(), 10);
                break;

            case ASMType.Out:
                square.transform.position   = new Vector2(0 + moveLen * GetTimer(), 0);
                square.transform.localScale = new Vector2(moveLen * 2 * (-1.0f + GetTimer()), 10);
                break;
        }
    }
}
