using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etohum.NextStep.Data.Model
{
    public class EntityBase<T> : IComparable<EntityBase<T>> where T: IComparable
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        protected EntityBase()
        {
        }

        protected EntityBase(T key) => Id = key;

        /// <summary>
        /// primary Identifier value for the class.
        /// </summary>
        [Key]
        public virtual T Id { get; set; }

        private DateTime? _creationDate;

        public virtual DateTime? CreationDate
        {
            get => _creationDate;
            internal set => _creationDate = value ?? DateTime.Now;
        }

        private DateTime? _modifiedDate;

        public virtual DateTime? ModifiedDate
        {
            get => _modifiedDate;
            set => _modifiedDate = value ?? DateTime.Now;
        }

        [StringLength(50, ErrorMessage = @"Too many characters. length exceeded")]
        public virtual string CreatedBy { get; set; }

        [StringLength(50, ErrorMessage = @"Too many characters. length exceeded")]
        public virtual string ModifiedBy { get; set; }

        #region Equality Tests
        /// <summary>
        /// Compare this object with another object of the Type <see cref="EntityBase<T>"/> for equality (comparison is based on Id)
        /// </summary>
        /// <param name="other"></param>
        /// <returns>returns 0 in case of equality, if current object is greater than other return 1 other wise -1</returns>
        public int CompareTo(EntityBase<T> other)
        {
            return this.Id.CompareTo(other.Id);
        }

        /// <summary>
        /// Determines whether the specified entity is equal to the
        /// current instance.
        /// </summary>
        /// <param name="entity">An <see cref="System.Object"/> that
        /// will be compared to the current instance.</param>
        /// <returns>True if the passed in entity is equal to the
        /// current instance.</returns>
        public override bool Equals(object entity)
        {
            return entity != null
                && (entity as EntityBase<T>) != null && this == (EntityBase<T>)entity;
        }

        /// <summary>
        /// Operator overload for determining equality.
        /// </summary>
        /// <param name="base1">The first instance of an
        /// <see cref="EntityBase"/>.</param>
        /// <param name="base2">The second instance of an
        /// <see cref="EntityBase"/>.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(EntityBase<T> base1,
            EntityBase<T> base2)
        {
            // check for both null (cast to object or recursive loop)
            return (object) base1 == null && (object) base2 == null ||
                   (object) base1 != null && (object) base2 != null && base1.Id.Equals(base2.Id);

            // check for either of them == to null
        }

        /// <summary>
        /// Operator overload for determining inequality.
        /// </summary>
        /// <param name="base1">The first instance of an
        /// <see cref="EntityBase"/>.</param>
        /// <param name="base2">The second instance of an
        /// <see cref="EntityBase"/>.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(EntityBase<T> base1,
            EntityBase<T> base2)
        {
            return (!(base1 == base2));
        }

        /// <summary>
        /// Operator overload for determining Greater than.
        /// only supports these types: long, int, short, float, double, byte, guId, string
        /// </summary>
        /// <param name="base1">The first instance of an
        /// <see cref="EntityBase"/>.</param>
        /// <param name="base2">The second instance of an
        /// <see cref="EntityBase"/>.</param>
        /// <returns>True if is greater.</returns>
        public static bool operator >(EntityBase<T> base1,
            EntityBase<T> base2)
        {
            return base1.Id.CompareTo(base2.Id) > 0;
        }

        /// <summary>
        /// Operator overload for determining Smaller than.
        /// only supports these types: long, int, short, float, double, byte, guId, string
        /// </summary>
        /// <param name="_this">The first instance of an
        /// <see cref="EntityBase"/>.</param>
        /// <param name="_that">The second instance of an
        /// <see cref="EntityBase"/>.</param>
        /// <returns>True if is smaller.</returns>
        public static bool operator <(EntityBase<T> _this,
            EntityBase<T> _that)
        {
            return (!(_this > _that));
        }

        /// <summary>
        /// Serves as a hash function for this type.
        /// </summary>
        /// <returns>A hash code for the current Id
        /// property.</returns>
        public override int GetHashCode()
        {
            try
            {
                return Id.GetHashCode();
            }
            catch (Exception ex)
            {
                throw new Exception("-- GetHashCode() --", ex);
            }
        }

        #endregion Equality Tests
    }
}