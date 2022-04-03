using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Extensions
{
    public static class UnityExtensions
    {
        public static GameObject FindFittingParent(this GameObject CurrentObject, Func<GameObject, bool> Predicate)
        {
            if (CurrentObject.transform.parent == null) {
                return null;
            }

            if (Predicate(CurrentObject.transform.parent.gameObject)) {
                return CurrentObject.transform.parent.gameObject;
            } else {
                return CurrentObject.transform.parent.gameObject.FindFittingParent(Predicate);
            }
        }
    }
}