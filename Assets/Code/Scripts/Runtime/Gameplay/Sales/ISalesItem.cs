using UnityEngine;

namespace Com2usGameDev
{
    public interface ISalesItem : IDescription
    {
        int Price { get; set; }
        string SalesName {get; set;}
        Sprite Profile { get; }
    }
}
