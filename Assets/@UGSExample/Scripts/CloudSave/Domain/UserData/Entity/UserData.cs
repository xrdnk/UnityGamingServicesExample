using System;

namespace Denicode.UGSExample.CloudSave.Domain.Entity
{
    /// <summary>
    /// ユーザ情報
    /// </summary>
    [Serializable]
    public class UserData
    {
        public string Name;
        public uint Age;

        public UserData(string name, uint age)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Age = age;
        }

        public override string ToString()
        {
            return $"UserData [Name: {Name}, Age: {Age}]";
        }
    }
}