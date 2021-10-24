using System;

namespace Denicode.UGSExample.CloudSave.Domain.Entity
{
    [Serializable]
    public struct User : IEquatable<User>
    {
        readonly string _id;
        string _name;

        public User(string id, string name)
        {
            _id = id ?? throw new ArgumentNullException(nameof(id));
            _name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Id => _id;
        public string Name => _name;

        public void SetName(string name) =>
            _name = name ?? throw new ArgumentNullException(nameof(name));

        public bool Equals(User other)
        {
            return Equals(_id, other._id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_id != null ? _id.GetHashCode() : 0) * 397) ^ (_name != null ? _name.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return $"UserData [Id: {_id} / Name: {_name}]";
        }
    }
}