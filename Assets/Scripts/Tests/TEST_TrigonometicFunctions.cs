using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Voidless
{
public class TEST_TrigonometicFunctions : MonoBehaviour
{

        [SerializeField] private bool DoVMath = true;
    // Start is called before the first frame update
    void Start()
    {
            if(DoVMath)
            {

                // DoVMathSin();
                DoTaylorSin();

            }else if(!DoVMath)
            {
                
                DoMathFSin();
            }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

        void DoVMathSin()
        {
            for (float i = 0; i <= 180f; i += 0.25f)
            {
                float vSin = VMath.SinInterpolation(i); 
            }

        }

        void DoMathFSin()
        {
            for (float i = 0; i <= 180f; i += 0.25f)
            {
                float mSin = Mathf.Sin(i * Mathf.Deg2Rad);
          
            }
        }

        void DoTaylorSin()
        {
            for (float i = 0; i <= 180f; i += 0.25f)
            {
                float taylorSin = VMath.SinTaylorRads(i);
                
            }
        }

    void CheckSinDifference()
    {
            for (float i = 0; i <= 180f; i += 0.25f)
            {
                float vSin = VMath.SinInterpolation(i);
                float mSin = Mathf.Sin(i * Mathf.Deg2Rad);
                float dif = Mathf.Abs(mSin - vSin);
            }
           
    }

    void CheckCosDifference()
    {
            for (float i = 0; i <= 180f; i += 0.25f)
            {
                float vCos = VMath.CosInterpolation(i);
                float mCos = Mathf.Cos(i * Mathf.Deg2Rad);
                float dif = Mathf.Abs(mCos - vCos);
            }
    }

    void CheckTanDifference()
    {
        for (float i = 0; i <= 180f; i += 0.25f)
        {
            float vTan = VMath.TanInterpolation(i);
            float mTan = Mathf.Tan(i * Mathf.Deg2Rad);
            float dif = Mathf.Abs(mTan - vTan);
        }
    }
    }

}
