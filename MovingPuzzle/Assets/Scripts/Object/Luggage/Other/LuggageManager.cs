using UnityEngine;
using System.Collections.Generic;

public class LuggageManager : MonoBehaviour
{
    #region ===== ‰×•¨ƒŠƒXƒg =====
    public List<Luggage> luggageList = new List<Luggage>();
    #endregion

    #region ===== ‰Šú‰» =====
    void Start()
    {
        luggageList.Add(new Luggage { type = LuggageType.Chair, point = 10 });
        luggageList.Add(new Luggage { type = LuggageType.Desk, point = 20 });
    }
    #endregion
   
}