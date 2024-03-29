﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PackingTool.Core.Service.PackingList.Json
{
    public class PackingListContent
    {
        [Required]
        public int GridColumnCount { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Notes { get; set; }
        [Required]
        public PackingGroup[] Groups { get; set; }
        [Required]
        public PackingTask[] Tasks { get; set; }

        public PackingListContent()
        {
            GridColumnCount = 0;
            Notes = string.Empty;
            Groups = Array.Empty<PackingGroup>();
            Tasks = Array.Empty<PackingTask>();
        }

        [JsonConstructor]
        private PackingListContent(

            int gridColumnCount,
            string notes,
            PackingGroup[] groups,
            PackingTask[] tasks
        )
        {
            GridColumnCount = gridColumnCount;
            Notes = notes ?? string.Empty;
            Groups = groups;
            Tasks = tasks ?? Array.Empty<PackingTask>();
        }

        public static PackingListContent FromJson(
            string jsonList
        )
        {
            var list = JsonConvert.DeserializeObject<PackingListContent>(jsonList);

            if (list == null)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingListContent)}: unable to deserialize JSON."
                );
            }

            list.IsValid();
            list.PrettifySorts();
            return list;
        }

        private bool IsValid()
        {
            if (GridColumnCount < 1)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingListContent)}: invalid GridColumnCount: {GridColumnCount}."
                );
            }

            if (Groups == null)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingListContent)}: has null Groups."
                );
            }

            return Groups.All(g => g.IsValid());
        }

        private void PrettifySorts()
        {
            Groups.ToList().Sort((x, y) => x.Sort.CompareTo(y.Sort));

            var groupSort = 1;
            foreach (var group in Groups)
            {
                group.Sort = groupSort++;
                group.Items.ToList().Sort((x, y) => x.Sort.CompareTo(y.Sort));

                var itemSort = 1;
                foreach (var item in group.Items)
                {
                    item.Sort = itemSort++;
                }
            }
        }
    }
}
