using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Standard.Infrastructure.Domains
{
    public abstract class EntityBase<TKey> : DomainBase, IEntity<TKey>, IDomainObject
    {
        [Key]
        public TKey Id
        {
            get
            {
                return JustDecompileGenerated_get_Id();
            }
            set
            {
                JustDecompileGenerated_set_Id(value);
            }
        }

        private TKey JustDecompileGenerated_Id_k__BackingField;

        public TKey JustDecompileGenerated_get_Id()
        {
            return this.JustDecompileGenerated_Id_k__BackingField;
        }

        protected void JustDecompileGenerated_set_Id(TKey value)
        {
            this.JustDecompileGenerated_Id_k__BackingField = value;
        }

        protected EntityBase()
        {
        }

        public override bool Equals(object obj)
        {
            return this == obj as EntityBase<TKey>;
        }

        public override int GetHashCode()
        {
            return (this.Id == null ? 0 : this.Id.GetHashCode());
        }

        public static bool operator ==(EntityBase<TKey> left, EntityBase<TKey> right)
        {
            bool flag;
            if ((left != null ? false : right == null))
            {
                flag = true;
            }
            else if ((left == null ? true : right == null))
            {
                flag = false;
            }
            else if (!Object.Equals(left.Id, null))
            {
                TKey id = left.Id;
                if (!id.Equals(default(TKey)))
                {
                    id = left.Id;
                    flag = id.Equals(right.Id);
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public static bool operator !=(EntityBase<TKey> left, EntityBase<TKey> right)
        {
            return !(left == right);
        }
    }

    public abstract class EntityBase : EntityBase<long>
    {
        protected EntityBase()
        {
        }
    }
}
