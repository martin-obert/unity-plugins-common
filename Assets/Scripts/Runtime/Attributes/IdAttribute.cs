using System;
using UnityEngine;

namespace Obert.Common.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IdAttribute : PropertyAttribute
    {
        public bool IsRequired { get; set; }
    }
}