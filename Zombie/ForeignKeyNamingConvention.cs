using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Zombie
{
    /// <summary>
    /// Provides a convention for fixing the independent association (IA) foreign key column names.
    /// </summary>
    public class ForeignKeyNamingConvention : IStoreModelConvention<AssociationType>
    {
        public void Apply(AssociationType association, DbModel model)
        {
            if (!association.IsForeignKey) return;

            // Rename FK columns
            var constraint = association.Constraint;
            if (DoPropertiesHaveDefaultNames(constraint.FromProperties, constraint.ToRole.Name, constraint.ToProperties)) NormalizeForeignKeyProperties(constraint.FromProperties);
            if (DoPropertiesHaveDefaultNames(constraint.ToProperties, constraint.FromRole.Name, constraint.FromProperties)) NormalizeForeignKeyProperties(constraint.ToProperties);
        }

        private static bool DoPropertiesHaveDefaultNames(IReadOnlyCollection<EdmProperty> properties, string roleName, IReadOnlyList<EdmProperty> otherEndProperties)
        {
            if (properties.Count != otherEndProperties.Count) return false;
            return !properties.Where((t, i) => !t.Name.EndsWith("_" + otherEndProperties[i].Name)).Any();
        }

        private static void NormalizeForeignKeyProperties(IEnumerable<EdmProperty> properties)
        {
            foreach (var property in properties)
            {
                var defaultPropertyName = property.Name;
                var ichUnderscore = defaultPropertyName.IndexOf('_');
                if (ichUnderscore <= 0) continue;

                var navigationPropertyName = defaultPropertyName.Substring(0, ichUnderscore);
                var targetKey = defaultPropertyName.Substring(ichUnderscore + 1);

                property.Name = targetKey.StartsWith(navigationPropertyName)
                    ? targetKey
                    : navigationPropertyName + targetKey;
            }
        }
    }
}