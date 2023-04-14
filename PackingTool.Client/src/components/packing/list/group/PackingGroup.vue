<template>
  <div :style="`width: ${$q.screen.gt.sm ? '250px' : '250px'}`">
    <DisplayPackingGroup
      v-if="!packing"
      :group="group"
      :cardClass="cardClass"
    />
    <PackPackingGroup v-else :group="group" :cardClass="cardClass" />
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref, watch } from "vue"
import { useQuasar } from "quasar"
import { storeToRefs } from "pinia"
import DisplayPackingGroup from "./displayOrEdit/DisplayPackingGroup.vue"
import PackPackingGroup from "./pack/PackPackingGroup.vue"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import { useOperationStatusStore } from "@/stores/operationStatusStore"
import { useClipboardStore } from "@/stores/clipboardStore"
import type { PackingGroup } from "@/models/packing/list/PackingGroup"
import { PackingListAction } from "@/enums/PackingListAction"
import { PackingSectionType } from "@/enums/PackingSectionType"
import { ExistenceStatus } from "@/models/packing/list/ExistenceStatus"

const { packingListManager } = useAllPackingListsStore()
const { packing, selectedGroupIDs, selectedItemIDs } = storeToRefs(
  useOpenedPackingListStore()
)
const { currentAction, currentSectionFocus, currentGroupIDFocus } = storeToRefs(
  useOperationStatusStore()
)
const { copiedItems } = storeToRefs(useClipboardStore())

const props = defineProps({
  group: {
    type: Object as () => PackingGroup,
    required: true,
  },
})

const $q = useQuasar()
const synchronizing = ref(false)

const selected = computed(
  () => selectedGroupIDs.value.indexOf(props.group.id) !== -1
)

const cardClass = computed(() => {
  let output = "shadow-transition header-font col-12"
  if (selected.value && props.group.status == ExistenceStatus.New) {
    output += " new-selected-group-color"
  } else if (selected.value) {
    output += " selected-group-color"
  } else if (props.group.status == ExistenceStatus.New) {
    output += " new-group-color"
  }
  return output
})

watch(currentAction, (action) => {
  if (action != PackingListAction.Fetching && synchronizing) {
    synchronizing.value = false
  }
})

const selectAllItems = (event: KeyboardEvent) => {
  if (
    currentGroupIDFocus.value == props.group.id &&
    currentSectionFocus.value == PackingSectionType.Items &&
    event.ctrlKey &&
    event.key.toLowerCase() === "a"
  ) {
    event.preventDefault()
    if (selectedItemIDs.value.length != props.group.items.length) {
      selectedItemIDs.value = props.group.items.map((item) => item.id)
    } else {
      selectedItemIDs.value.length = 0
    }
  }
}

const copyItems = (event: KeyboardEvent) => {
  if (
    currentGroupIDFocus.value == props.group.id &&
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
  if (currentGroupIDFocus.value != props.group.id) return

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
  if (currentGroupIDFocus.value != props.group.id) return

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
  if (currentGroupIDFocus.value != props.group.id) return

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
  if (
    currentGroupIDFocus.value != props.group.id ||
    selectedItemIDs.value.length != 1 ||
    event.altKey
  )
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
  if (
    currentGroupIDFocus.value != props.group.id ||
    selectedItemIDs.value.length != 1 ||
    !event.altKey
  )
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
  box-shadow: rgb(50, 200, 0) 0px 0px 0px 3px,
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
