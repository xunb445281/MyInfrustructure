using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Standard.Infrastructure.Domains
{
    public class ValueObject : DomainBase
    {
        public ValueObject()
        {
        }

        public virtual ValueObject Clone()
        {
            return (ValueObject)base.MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            return this == (ValueObject)obj;
        }

        public override int GetHashCode()
        {
            PropertyInfo[] properties = base.GetType().GetProperties();
            int num = (
                from pro in properties
                select pro.GetValue(this) into value
                where (object)value != (object)null
                select value).Aggregate<object, int>(0, (int current, object value) => current ^ value.GetHashCode());
            return num;
        }

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            bool flag;
            if ((left != null ? true : right != null))
            {
                PropertyInfo[] properties = left.GetType().GetProperties();
                flag = properties.All<PropertyInfo>((PropertyInfo pro) => (object)pro.GetValue(left) == (object)pro.GetValue(right));
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right);
        }
    }
}
