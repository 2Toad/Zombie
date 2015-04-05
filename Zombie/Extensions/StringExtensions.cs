using System;
using System.Text;

namespace Zombie.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a new string in which all the occurrences of <paramref name="oldValue"/> are replaced 
        /// with <paramref name="newValue"/>.
        /// </summary>
        /// <param name="input">The string to search.</param>
        /// <param name="oldValue">The value to find.</param>
        /// <param name="newValue">The replacement value.</param>
        /// <param name="comparison">Specifies the culture, case, and sort rules to use when searching for <paramref name="oldValue"/>.</param>
        /// <returns>A new string.</returns>
        public static string Replace(this string input, string oldValue, string newValue, StringComparison comparison)
        {
            var output = new StringBuilder();
            var previousIndex = 0;

            var index = input.IndexOf(oldValue, comparison);
            while (index > 0)
            {
                output.Append(input.Substring(previousIndex, index - previousIndex));
                output.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = input.IndexOf(oldValue, index, comparison);
            }

            output.Append(input.Substring(previousIndex));
            return output.ToString();
        }
    }
}
