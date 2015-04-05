using System;
using System.Threading.Tasks;
using Zombie.Extensions;

namespace Zombie.Email
{
    /// <summary>
    /// A lightweight template engine.
    /// </summary>
    public class Handlebars
    {
        /// <summary>
        /// The name of the handlebar expression.
        /// This property is case-insensitive.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The value of the handlebar expression.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Parses the specified <paramref name="template"/>, replacing handlebar expressions with
        /// the specified <paramref name="handlebars"/>.
        /// </summary>
        /// <param name="template">The template to parse.</param>
        /// <param name="handlebars">Handlebar expressions for the template.</param>
        /// <returns>The parsed template.</returns>
        public static async Task<string> Parse(string template, params Handlebars[] handlebars)
        {
            await Task.Run(() => {
                foreach (var handlebar in handlebars)
                    template = template.Replace(handlebar.Name, handlebar.Value, StringComparison.OrdinalIgnoreCase);
            });
            return template;
        }

        /// <summary>
        /// Creates a new handlebar expression.
        /// </summary>
        /// <param name="name">The name of the expression.</param>
        /// <param name="value">The value of the expression.</param>
        /// <returns>A handlebar expression.</returns>
        public static Handlebars New(string name, string value)
        {
            return new Handlebars { 
                Name = "{{" + name.ToUpper() + "}}",
                Value = value 
            };
        }
    }
}
