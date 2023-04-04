<template>
  <DisplayPackingGroup
    v-if="displaying"
    :group="group"
    :isSelected="isSelected"
    :selectedItemIDs="selectedItemIDs"
    :setSelectedGroupID="selectGroup"
    :setSelectedItemID="setSelectedItemID"
    :setEditing="editGroup"
    :setEditItemName="editItemName"
    :cardClass="cardClass"
  />
  <EditPackingGroup
    v-if="editing"
    :group="group"
    :isSelected="isSelected"
    :cardClass="cardClass"
    :setSelectedGroupID="selectGroup"
    :setSelectedItemID="setSelectedItemID"
    :selectedItemIDs="selectedItemIDs"
    :finishEdit="finishEdit"
    :editNameForItemID="editNameForItemID"
  />
  <PackPackingGroup v-else-if="packing" :group="group" :cardClass="cardClass" />
</template>

<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref, watch } from "vue"
import { useQuasar } from "quasar"
import { storeToRefs } from "pinia"
import DisplayPackingGroup from "./display/DisplayPackingGroup.vue"
import EditPackingGroup from "./edit/EditPackingGroup.vue"
import PackPackingGroup from "./pack/PackPackingGroup.vue"
import { usePackingListStore } from "@/stores/packingListStore"
import { useOperationStatusStore } from "@/stores/operationStatusStore"
import { useClipboardStore } from "@/stores/clipboardStore"
import type { PackingGroup } from "@/models/packing/list/PackingGroup"
import { PackingListAction } from "@/enums/PackingListAction"
import { PackingSectionType } from "@/enums/PackingSectionType"
import { ExistenceStatus } from "@/models/packing/list/ExistenceStatus"

const { packingListManager } = usePackingListStore()
const { currentAction, currentSectionFocus, currentGroupIDFocus } = storeToRefs(
  useOperationStatusStore()
)
const { copiedItems } = storeToRefs(useClipboardStore())

const props = defineProps({
  group: {
    type: Object as () => PackingGroup,
    required: true,
  },
  isSelected: {
    type: Boolean,
    required: true,
  },
  packing: {
    type: Boolean,
    required: true,
  },
  setSelectedGroupID: {
    type: Function,
    required: true,
  },
})

const $q = useQuasar()
const synchronizing = ref(false)
const editing = ref(false)
const selectedItemIDs = ref([] as number[])
const editNameForItemID = ref(0)

const displaying = computed(() => !editing.value && !props.packing)

const selectGroup = (allowMultiple: boolean) => {
  props.setSelectedGroupID(props.group.id, allowMultiple)
  selectedItemIDs.value.length = 0
}

const editGroup = () => {
  editing.value = true
  props.setSelectedGroupID(0, false) //todo clear
  selectedItemIDs.value.length = 0
  editNameForItemID.value = 0
}

const editItemName = (itemID: number) => {
  editing.value = true
  editNameForItemID.value = itemID
}

const finishEdit = () => {
  editing.value = false
  editNameForItemID.value = 0
}

const setSelectedItemID = (itemID: number, allowMultiple: boolean) => {
  props.setSelectedGroupID(0, false)
  currentSectionFocus.value = PackingSectionType.Items
  currentGroupIDFocus.value = props.group.id

  if (itemID == 0 || (props.packing && !editing.value)) {
    selectedItemIDs.value.length = 0
    return
  }

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

const cardClass = computed(() => {
  let output = "shadow-transition header-font col-12"
  if (props.isSelected && props.group.status == ExistenceStatus.New) {
    output += " new-selected-group-color"
  } else if (props.isSelected) {
    output += " selected-group-color"
  } else if (props.group.status == ExistenceStatus.New) {
    output += " new-group-color"
  }
  return output
})

watch(currentGroupIDFocus, (groupIDFocus) => {
  if (groupIDFocus != props.group.id) {
    selectedItemIDs.value.length = 0
  }
})

watch(currentAction, (action) => {
  if (action != PackingListAction.Fetching && synchronizing) {
    synchronizing.value = false
  }
})

watch(
  () => props.packing,
  (isPacking) => {
    if (isPacking) {
      editing.value = false
      selectedItemIDs.value.length = 0
    }
  }
)

const selectAllItems = (event: KeyboardEvent) => {
  if (
    currentGroupIDFocus.value == props.group.id &&
    currentSectionFocus.value == PackingSectionType.Items &&
    event.ctrlKey &&
    event.key.toLowerCase() === "a"
  ) {
    event.preventDefault()
    selectedItemIDs.value = props.group.items.map((item) => item.id)
  }
}

const copyItems = (event: KeyboardEvent) => {
  if (
    currentSectionFocus.value == PackingSectionType.Items &&
    selectedItemIDs.value.length > 0 &&
    event.ctrlKey &&
    event.key.toLowerCase() === "c"
  ) {
    event.preventDefault()
    packingListManager.CopyItems(props.group.id, selectedItemIDs.value, false)

    $q.notify({
      message: "Copied item(s)",
      color: "purple",
      icon: "done",
    })
  }
}

const cutItems = (event: KeyboardEvent) => {
  if (
    currentSectionFocus.value == PackingSectionType.Items &&
    selectedItemIDs.value.length > 0 &&
    event.ctrlKey &&
    event.key.toLowerCase() === "x"
  ) {
    event.preventDefault()
    packingListManager.CopyItems(props.group.id, selectedItemIDs.value, true)

    $q.notify({
      message: "Copied item(s)",
      color: "purple",
      icon: "done",
    })
  }
}

const pasteItems = (event: KeyboardEvent) => {
  if (!props.isSelected) return

  if (
    copiedItems.value.length &&
    event.ctrlKey &&
    event.key.toLowerCase() === "v"
  ) {
    event.preventDefault()
    copiedItems.value.forEach((copiedItem) => {
      packingListManager.AddItem(
        copiedItem.name,
        copiedItem.count,
        copiedItem.attributes,
        props.group.id
      )
    })
  }
}

const deleteItems = (event: KeyboardEvent) => {
  if (
    currentSectionFocus.value == PackingSectionType.Items &&
    selectedItemIDs.value.length > 0 &&
    event.key.toLowerCase() === "delete"
  ) {
    event.preventDefault()
    selectedItemIDs.value.forEach((itemID) =>
      packingListManager.RemoveItem(itemID, props.group.id)
    )
  }
}

const changeItemSelection = (event: KeyboardEvent) => {
  if (selectedItemIDs.value.length != 1 || (editing.value && event.altKey))
    return

  const selectedItemID = selectedItemIDs.value[0]

  switch (event.key) {
    case "ArrowUp":
      event.preventDefault()
      const previousItem = packingListManager.GetPreviousItem(
        props.group.id,
        selectedItemID
      )
      if (previousItem.id !== undefined) {
        selectedItemIDs.value[0] = previousItem.id
      }
      break
    case "ArrowDown":
      event.preventDefault()
      const nextItem = packingListManager.GetNextItem(
        props.group.id,
        selectedItemID
      )
      if (nextItem.id !== undefined) {
        selectedItemIDs.value[0] = nextItem.id
      }
      break
    default:
      return
  }
}

const changeItemSort = (event: KeyboardEvent) => {
  if (!editing.value || selectedItemIDs.value.length != 1 || !event.altKey)
    return

  const selectedItemID = selectedItemIDs.value[0]

  switch (event.key) {
    case "ArrowUp":
      event.preventDefault()
      packingListManager.SortItemUp(props.group.id, selectedItemID)
      break
    case "ArrowDown":
      event.preventDefault()
      packingListManager.SortItemDown(props.group.id, selectedItemID)
      break
    default:
      return
  }
}

onMounted(() => {
  window.addEventListener("keydown", selectAllItems)
  window.addEventListener("keydown", copyItems)
  window.addEventListener("keydown", cutItems)
  window.addEventListener("keydown", pasteItems)
  window.addEventListener("keydown", deleteItems)
  window.addEventListener("keydown", changeItemSelection)
  window.addEventListener("keydown", changeItemSort)
})

onUnmounted(() => {
  window.removeEventListener("keydown", selectAllItems)
  window.removeEventListener("keydown", copyItems)
  window.removeEventListener("keydown", cutItems)
  window.removeEventListener("keydown", pasteItems)
  window.removeEventListener("keydown", deleteItems)
  window.removeEventListener("keydown", changeItemSelection)
  window.removeEventListener("keydown", changeItemSort)
})
</script>

<style scoped lang="scss">
.selected-group-color {
  box-shadow: $selected 0px 0px 0px 3px;
}
.new-group-color {
  box-shadow: rgb(102, 255, 0) 0px 0px 0px 3px,
    rgba(198, 255, 0, 0.25) 0px 30px 60px -12px inset,
    rgba(0, 110, 10, 0.3) 0px 18px 36px -18px inset;
}
.new-selected-group-color {
  box-shadow: $selected 0px 0px 0px 3px,
    rgba(198, 255, 0, 0.25) 0px 30px 60px -12px inset,
    rgba(0, 110, 10, 0.3) 0px 18px 36px -18px inset;
}
.header-font {
  font-family: "Ink Free";
}
</style>
