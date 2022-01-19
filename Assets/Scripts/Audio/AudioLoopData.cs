using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flamingo
{
[Serializable]
public struct AudioLoopData
{
	public int sourceIndex; 	/// <summary>Source's Index.</summary>	
	public int soundIndex; 		/// <summary>Sound's Index.</summary>
	public bool loop; 			/// <summary>Loop?.</summary>

	/// <summary>AudioLoopData's Constructor.</summary>
	/// <param name="_sourceIndex">Source's Index.</param>
	/// <param name="_soundIndex">Sound's Index.</param>
	/// <param name="_loop">Loop? true by default.</param>
	public AudioLoopData(int _sourceIndex, int _soundIndex, bool _loop = true)
	{
		sourceIndex = _sourceIndex;
		soundIndex = _soundIndex;
		loop = _loop;
	}	
}
}