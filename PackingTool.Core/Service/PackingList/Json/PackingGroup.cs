using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PackingTool.Core.Service.PackingList.Json
{
    public class PackingGroup
    {
        [Required]
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Item.PackingItemType Type { get; set; }
        public int Sort { get; set; }
        [Required]
        public Item.PackingItem[] Items { get; set; }

        [JsonConstructor]
        private PackingGroup(
            string name,
            Item.PackingItemType type,
            int sort,
            Item.PackingItem[] items
        )
        {
            Name = name;
            Type = type;
            Sort = sort;
            Items = items;
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidDataException(
                    $"{nameof(PackingGroup)}: name is missing."
                );
            }

            if (Type == Item.PackingItemType.Undefined)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingGroup)}: '{Name}' Type is undefined."
                );
            }

            if (Sort < 1)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingGroup)}: '{Name}' invalid Sort: {Sort}."
                );
            }

            if (Items == null)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingGroup)}: '{Name}' has null Items."
                );
            }

            return Items.All(i => i.IsValid());
        }
    }
}
