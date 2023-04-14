import { computed, ref } from "vue"
import { defineStore, storeToRefs } from "pinia"
import { useOperationStatusStore } from "./operationStatusStore"
import { PackingList } from "@/models/packing/list/PackingList"
import { PackingSectionType } from "@/enums/PackingSectionType"

export const useOpenedPackingListStore = defineStore(
  "openedPackingListStore",
  () => {
    const { currentSectionFocus, currentGroupIDFocus } = storeToRefs(
      useOperationStatusStore()
    )

    const packingList = ref(PackingList.Undefined())
    const packing = ref(false)
    const selectedGroupIDs = ref([] as number[])
    const selectedItemIDs = ref([] as number[])

    const editingGroupIDs = ref([] as number[])
    const editingHeaderGroupIDs = ref([] as number[])
    const editingAttributesGroupIDs = ref([] as number[])

    const highlightImportantItems = ref(false)
    const highlightShoppingItems = ref(false)

    const importantItems = computed(() => {
      return packingList.value.content.groups.flatMap((group) =>
        group.items.filter((item) => item.attributes.includes("Important"))
      )
    })

    const shoppingItems = computed(() => {
      return packingList.value.content.groups.flatMap((group) =>
        group.items.filter(
          (item) =>
            item.attributes.includes("ToBuy") ||
            item.attributes.includes("Bought")
        )
      )
    })

    const setSelectedGroupID = (groupID: number, allowMultiple: boolean) => {
      currentSectionFocus.value = PackingSectionType.Grid
      selectedItemIDs.value.length = 0

      if (groupID == 0) {
        selectedGroupIDs.value.length = 0
        return
      }

      const selectedGroupsCount = selectedGroupIDs.value.length
      const containsGroup = selectedGroupIDs.value.indexOf(groupID) !== -1

      if (!allowMultiple) {
        currentGroupIDFocus.value = groupID
        selectedGroupIDs.value.length = 0
        if (!containsGroup || selectedGroupsCount > 1) {
          selectedGroupIDs.value.push(groupID)
        }
        return
      }

      if (!containsGroup) {
        selectedGroupIDs.value.push(groupID)
      } else {
        selectedGroupIDs.value.splice(
          selectedGroupIDs.value.indexOf(groupID),
          1
        )
      }
    }

    const setSelectedItemID = (
      groupID: number,
      itemID: number,
      allowMultiple: boolean
    ) => {
      currentSectionFocus.value = PackingSectionType.Items
      currentGroupIDFocus.value = groupID
      selectedGroupIDs.value.length = 0

      const selectedItemsCount = selectedItemIDs.value.length
      const containsItem = selectedItemIDs.value.indexOf(itemID) !== -1

      if (!allowMultiple) {
        selectedItemIDs.value.length = 0
        if (!containsItem || selectedItemsCount > 1) {
          selectedItemIDs.value.push(itemID)
        }
        return
      }

      if (!containsItem) {
        selectedItemIDs.value.push(itemID)
      } else {
        selectedItemIDs.value.splice(selectedItemIDs.value.indexOf(itemID), 1)
      }
    }

    const editItemName = (groupID: number, itemID: number) => {
      editGroup(groupID)
    }

    const editGroup = (groupID: number) => {
      setSelectedGroupID(0, false)
      selectedItemIDs.value.length = 0
      editingGroupIDs.value.push(groupID)
    }

    const editHeaderForGroup = (groupID: number) => {
      setSelectedGroupID(0, false)
      editingHeaderGroupIDs.value.push(groupID)
    }

    const editAttributesForGroup = (groupID: number) => {
      setSelectedGroupID(0, false)
      editingAttributesGroupIDs.value.push(groupID)
    }

    const finishEditGroup = (groupID: number) => {
      finishEditHeaderForGroup(groupID)
      finishEditAttributesForGroup(groupID)
      editingGroupIDs.value.splice(editingGroupIDs.value.indexOf(groupID), 1)
    }

    const finishEditHeaderForGroup = (groupID: number) => {
      const editingHeaderGroupID = editingHeaderGroupIDs.value.indexOf(groupID)

      if (editingHeaderGroupID !== -1) {
        editingHeaderGroupIDs.value.splice(editingHeaderGroupID, 1)
      }
    }

    const finishEditAttributesForGroup = (groupID: number) => {
      const editingAttributesGroupID =
        editingAttributesGroupIDs.value.indexOf(groupID)

      if (editingAttributesGroupID !== -1) {
        editingAttributesGroupIDs.value.splice(editingAttributesGroupID, 1)
      }
    }

    return {
      packingList,
      packing,
      selectedGroupIDs,
      selectedItemIDs,
      editingGroupIDs,
      editingHeaderGroupIDs,
      editingAttributesGroupIDs,
      highlightImportantItems,
      highlightShoppingItems,
      importantItems,
      shoppingItems,
      setSelectedGroupID,
      setSelectedItemID,
      editGroup,
      editItemName,
      editHeaderForGroup,
      editAttributesForGroup,
      finishEditGroup,
      finishEditHeaderForGroup,
      finishEditAttributesForGroup,
    }
  }
)
