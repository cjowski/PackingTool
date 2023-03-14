import { computed, ref, watch } from "vue"
import { defineStore, storeToRefs } from "pinia"
import { useClipboardStore } from "./clipboardStore"
import { useOperationStatusStore } from "./operationStatusStore"
import { PackingList } from "@/models/packing/list/PackingList"
import { PackingListManager } from "@/logic/PackingListManager"

export const usePackingListStore = defineStore("packingList", () => {
  const { currentAction, previousAction } = storeToRefs(useOperationStatusStore())
  const { copiedGroups, copiedItems } = storeToRefs(useClipboardStore())

  const allPackingLists = ref([] as PackingList[])
  const packingList = ref(PackingList.Undefined())
  const selectedListName = ref("")
  const allListsFetched = ref(false)

  const packingListManager = new PackingListManager(
    allPackingLists,
    packingList,
    selectedListName,
    allListsFetched,
    currentAction,
    previousAction,
    copiedGroups,
    copiedItems
  )

  const importantItems = computed(() => {
    return packingList.value.content.groups.flatMap((group) =>
      group.items.filter((item) =>
        item.attributes.includes("Important")
      )
    )
  })

  watch(selectedListName, (currentListName) => {
    if (allListsFetched.value && currentListName) {
      packingListManager.SetupList(currentListName)
    }
  })

  watch(allListsFetched, (areListsFetched) => {
    if (areListsFetched && selectedListName.value) {
      packingListManager.SetupList(selectedListName.value)
    }
  })

  return {
    allPackingLists,
    packingList,
    selectedListName,
    allListsFetched,
    importantItems,
    packingListManager,
  }
})
