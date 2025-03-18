using UnityEngine;

namespace Com2usGameDev
{
    public interface ISalesItem
    {
        int Price { get; set; }
        string SalesName {get; set;}
        Sprite Profile { get; }
    }
}
