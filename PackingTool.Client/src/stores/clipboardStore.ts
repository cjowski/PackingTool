import { ref } from "vue"
import { defineStore } from "pinia"
import type { PackingGroup } from "@/models/packing/list/PackingGroup"
import type { PackingItem } from "@/models/packing/list/PackingItem"

export const useClipboardStore = defineStore("clipboard", () => {
  const copiedGroups = ref([] as PackingGroup[])
  const copiedItems = ref([] as PackingItem[])

  return { copiedGroups, copiedItems }
})
