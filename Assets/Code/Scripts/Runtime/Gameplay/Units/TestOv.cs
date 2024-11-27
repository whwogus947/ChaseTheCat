using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public class TestOv : MonoBehaviour
    {
        Dictionary<Zero, int> test = new();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Zero alphaC = new AlphaC();
            Zero betaC = new BetaC();
            test[(Zero)alphaC] = 1;
            test[(Zero)betaC] = 1;

            foreach (var item in test)
            {
                Debug.Log(item.Key + ", " + item.Value);
                Debug.Log(item.Key.GetType());
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

    public interface Zero
    {

    }

    public interface Alpha : Zero
    {

    }

    public class AlphaC : Alpha
    {

    }

    public interface Beta : Zero
    {

    }

    public class BetaC : Beta
    {

    }
}
