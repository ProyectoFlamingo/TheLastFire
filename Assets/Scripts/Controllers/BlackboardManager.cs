using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class BlackboardManager : Singleton<BlackboardManager>
{
	private Blackboard<string, HashSet<Projectile>> _projectilesBlackboards;
}
}