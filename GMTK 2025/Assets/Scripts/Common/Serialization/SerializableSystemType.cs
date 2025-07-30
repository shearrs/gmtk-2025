using System;
using UnityEngine;

namespace Shears
{
    [Serializable]
    public struct SerializableSystemType
    {
        public static readonly SerializableSystemType Empty = new();
        
        [SerializeField] private string name;
        [SerializeField] private string assemblyQualifiedName;
        [SerializeField] private string assemblyName;
        [SerializeField] private string prettyName;
        private Type systemType;

        public readonly string Name => name;
        public readonly string AssemblyQualifiedName => assemblyQualifiedName;
        public readonly string AssemblyName => assemblyName;
        public readonly string PrettyName => prettyName;
        public Type SystemType
        {
            get
            {
                if (systemType == null)
                    GetSystemType();

                return systemType;
            }
        }

        private void GetSystemType()
        {
            systemType = Type.GetType(assemblyQualifiedName);
        }

        public SerializableSystemType(Type type)
        {
            systemType = type;
            name = type.Name;
            assemblyQualifiedName = type.AssemblyQualifiedName;
            assemblyName = type.Assembly.FullName;
            prettyName = StringUtil.PascalSpace(name);
        }

        public SerializableSystemType(string assemblyQualifiedName)
        {
            Type type = Type.GetType(assemblyQualifiedName) ?? throw new Exception($"Invalid type {assemblyQualifiedName}!");

            systemType = type;
            name = type.Name;
            this.assemblyQualifiedName = assemblyQualifiedName;
            assemblyName = type.Assembly.FullName;
            prettyName = StringUtil.PascalSpace(name);
        }

        #region Operators
        public override bool Equals(object obj)
        {
            if (obj is not SerializableSystemType type)
                return false;

            return Equals(type);
        }

        public bool Equals(SerializableSystemType type) 
        {
            return SystemType == type.SystemType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, AssemblyQualifiedName, AssemblyName, SystemType);
        }

        public override string ToString()
        {
            return name;
        }

        public static bool operator==(SerializableSystemType a, SerializableSystemType b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a == null || b == null)
                return false;

            return a.Equals(b);
        }

        public static bool operator!=(SerializableSystemType a, SerializableSystemType b)
        {
            return !(a == b);
        }

        public static implicit operator Type(SerializableSystemType t)
        {
            return t.SystemType;
        }

        public static implicit operator SerializableSystemType(Type t)
        {
            return new(t);
        }
        #endregion
    }
}
