import { ref, watch } from "vue"
import { defineStore, storeToRefs } from "pinia"
import { useClipboardStore } from "./clipboardStore"
import { useOperationStatusStore } from "./operationStatusStore"
import { useOpenedPackingListStore } from "./openedPackingListStore"
import type { PackingList } from "@/models/packing/list/PackingList"
import { PackingListManager } from "@/logic/PackingListManager"

export const useAllPackingListsStore = defineStore(
  "allPackingListsStore",
  () => {
    const { currentAction, previousAction } = storeToRefs(
      useOperationStatusStore()
    )
    const { copiedGroups, copiedItems } = storeToRefs(useClipboardStore())
    const { packingList } = storeToRefs(useOpenedPackingListStore())

    const allPackingLists = ref([] as PackingList[])
    const selectedListName = ref("")
    const allListsFetched = ref(false)
    const packingListsShown = ref(false)
    const rightDrawerShown = ref(false)

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

    const setPackingListsShown = (areShown: boolean) => {
      packingListsShown.value = areShown
    }

    watch(selectedListName, (currentListName) => {
      if (allListsFetched.value && currentListName) {
        packingListManager.SetupList(currentListName)
      }
      if (!currentListName) {
        rightDrawerShown.value = false
      }
    })

    watch(allListsFetched, (areListsFetched) => {
      if (areListsFetched && selectedListName.value) {
        packingListManager.SetupList(selectedListName.value)
      }
    })

    watch(packingListsShown, (areShown) => {
      if (!areShown) {
        rightDrawerShown.value = false
      }
    })

    return {
      allPackingLists,
      selectedListName,
      allListsFetched,
      packingListsShown,
      rightDrawerShown,
      packingListManager,
      setPackingListsShown
    }
  }
)
