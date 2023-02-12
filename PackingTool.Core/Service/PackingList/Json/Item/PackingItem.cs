using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PackingTool.Core.Service.PackingList.Json.Item
{
    public class PackingItem
    {
        [Required]
        public string Name { get; set; }
        public int Count { get; set; }
        public int Sort { get; set; }
        [Required]
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public PackingItemAttribute[] Attributes { get; set; } //TODO do not serialize null
        public bool Packed { get; set; }

        [JsonConstructor]
        private PackingItem(
            string name,
            int count,
            int sort,
            PackingItemAttribute[] attributes,
            bool packed
        )
        {
            Name = name;
            Count = count;
            Sort = sort;
            Attributes = attributes ?? Array.Empty<PackingItemAttribute>();
            Packed = packed;
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidDataException(
                    $"{nameof(PackingItem)}: name is missing."
                );
            }

            if (Count < 1)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingItem)}: '{Name}' invalid Count: {Count}."
                );
            }

            if (Sort < 1)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingItem)}: '{Name}' invalid Sort: {Sort}."
                );
            }

            if (Attributes == null)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingItem)}: '{Name}' has null Attributes."
                );
            }

            if (Attributes.Contains(PackingItemAttribute.Undefined))
            {
                throw new InvalidDataException(
                    $"{nameof(PackingItem)}: '{Name}' has undefined Attribute."
                );
            }

            return true;
        }
    }
}
