using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PackingTool.Core.Service.PackingList.Json.Item
{
    public class PackingItem
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public int Sort { get; set; }
        [Required]
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public PackingItemAttribute[] Attributes { get; set; } //TODO do not serialize null
        [Required]
        public bool Packed { get; set; }

        public PackingItem()
        {
            ID = 0;
            Name = string.Empty;
            Count = 0;
            Sort = 0;
            Attributes = Array.Empty<PackingItemAttribute>();
            Packed = false;
        }

        [JsonConstructor]
        private PackingItem(
            int id,
            string name,
            int count,
            int sort,
            PackingItemAttribute[] attributes,
            bool packed
        )
        {
            ID = id;
            Name = name;
            Count = count;
            Sort = sort;
            Attributes = attributes ?? Array.Empty<PackingItemAttribute>();
            Packed = packed;
        }

        public bool IsValid()
        {
            if (ID <= 0)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingItem)}: ID is missing."
                );
            }

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
