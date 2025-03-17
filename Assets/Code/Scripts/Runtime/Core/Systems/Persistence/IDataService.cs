using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public interface IDataService
    {
        void Save(FlotData data, bool overwrite = true);
        FlotData Load(string name);
        void Delete(string name);
        void DeleteAll();
        IEnumerable<string> ListSaves();
        List<string> GetFileNames();
    }
}
