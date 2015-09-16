using System;

namespace Miscellaneous
{
    public interface IMaybe : IEquatable<IMaybe>
    {
        bool HasValue { get; }
    }

    public class Maybe<T> : IMaybe, IEquatable<Maybe<T>>
    {
        private readonly bool hasValue;
        private readonly T value;

        private Maybe() { this.hasValue = false; }
        private Maybe(T value)
        {
            this.value = value;
            this.hasValue = true;
        }

        public bool HasValue { get; private set; }
        public T Value { get { return this.value; } }

        #region Factory Methods

        public static Maybe<T> Some(T value)
        {
            return new Maybe<T>(value);
        }

        public static Maybe<T> None()
        {
            return new Maybe<T>();
        }

        #endregion

        #region Operators

        public static bool operator ==(Maybe<T> left, Maybe<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator ==(Maybe<T> left, IMaybe right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Maybe<T> left, Maybe<T> right)
        {
            return !left.Equals(right);
        }

        public static bool operator !=(Maybe<T> left, IMaybe right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region Equality

        public bool Equals(Maybe<T> other)
        {
            if (this.hasValue != other.hasValue)
                return false;

            return !this.hasValue || this.value.Equals(other.value);
        }

        public bool Equals(IMaybe other)
        {
            if (other is Maybe<T>)
                return this.Equals((Maybe<T>)other);

            return other != null && !this.HasValue && !other.HasValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is Maybe<T>)
                return this.Equals((Maybe<T>)obj);

            return this.Equals(obj as IMaybe);
        }

        public override int GetHashCode()
        {
            if (!this.hasValue)
                return 0;

            return ReferenceEquals(this.value, null) ? -1 : this.value.GetHashCode();
        }

        #endregion

        #region Pattern Matching

        public void Match(Action<T> some, Action none) 
        {
            if (some == null) throw new ArgumentNullException("some");
            if (none == null) throw new ArgumentNullException("none");
            
            if (this.hasValue)
                some(this.value);
            else 
                none();
        }

        public K Match<K>(Func<T, K> some, Func<K> none)
        {
            if (some == null) throw new ArgumentNullException("some");
            if (none == null) throw new ArgumentNullException("none");

            return this.hasValue ? some(this.value) : none();
        }

        #endregion
    }
}
