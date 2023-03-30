<template>
  <div>
    <div class="row q-col-gutter-md no-wrap">
      <div
        v-for="gridColumn in grid.GetColumns()"
        class="q-col-gutter-y-md"
        :style="`width: ${columnsWidth}`"
      >
        <div v-for="node in gridColumn" class="row">
          <PackingGroup
            :group="packingListManager.GetGroup(node.element.id)"
            :isSelected="selectedGroupIDs.indexOf(node.element.id) !== -1"
            :packing="packing"
            :setSelectedGroupID="setSelectedGroupID"
            style="width: 100%"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref, watch } from "vue"
import { useQuasar } from "quasar"
import { storeToRefs } from "pinia"
import { usePackingListStore } from "@/stores/packingListStore"
import { useOperationStatusStore } from "@/stores/operationStatusStore"
import { useClipboardStore } from "@/stores/clipboardStore"
import { Grid } from "@/models/packing/grid/Grid"
import { NeighborDirection } from "@/models/packing/grid/NeighborDirection"
import PackingGroup from "@/components/packing/list/group/PackingGroup.vue"
import { PackingSectionType } from "@/enums/PackingSectionType"

const { packingListManager } = usePackingListStore()
const { packingList } = storeToRefs(usePackingListStore())
const { currentSectionFocus, currentGroupIDFocus } = storeToRefs(
  useOperationStatusStore()
)
const { copiedGroups } = storeToRefs(useClipboardStore())

defineProps({
  packing: {
    type: Boolean,
    required: true,
  },
})

const $q = useQuasar()
const selectedGroupIDs = ref([] as number[])

const grid = computed(() => {
  return Grid.FromGridElements(
    packingList.value.content.groups,
    packingList.value.content.gridColumnCount
  )
})

const columnsWidth = computed(() => {
  return `${(1 / packingList.value.content.gridColumnCount) * 100}%`
})

const setSelectedGroupID = (groupID: number, allowMultiple: boolean) => {
  currentSectionFocus.value = PackingSectionType.Grid

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
    selectedGroupIDs.value.splice(selectedGroupIDs.value.indexOf(groupID), 1)
  }
}

watch(packingList, (updatedList) => {
  selectedGroupIDs.value = [] as number[]
  const groupsLength = updatedList.content.groups.length
  if (groupsLength < updatedList.content.gridColumnCount) {
    updatedList.content.gridColumnCount = groupsLength
  }
})

const selectAllGroups = (event: KeyboardEvent) => {
  if (
    currentSectionFocus.value == PackingSectionType.Grid &&
    event.ctrlKey &&
    event.key.toLowerCase() === "a"
  ) {
    event.preventDefault()
    selectedGroupIDs.value.length = 0
    packingList.value.content.groups.forEach((group) => {
      selectedGroupIDs.value.push(group.id)
    })
  }
}

const copySelectedGroups = (event: KeyboardEvent) => {
  if (
    currentSectionFocus.value == PackingSectionType.Grid &&
    selectedGroupIDs.value.length > 0 &&
    event.ctrlKey &&
    event.key.toLowerCase() === "c"
  ) {
    event.preventDefault()
    packingListManager.CopyGroups(selectedGroupIDs.value, false)

    $q.notify({
      message: "Copied group(s)",
      color: "purple",
      icon: "done",
    })
  }
}

const cutSelectedGroups = (event: KeyboardEvent) => {
  if (
    currentSectionFocus.value == PackingSectionType.Grid &&
    selectedGroupIDs.value.length > 0 &&
    event.ctrlKey &&
    event.key.toLowerCase() === "x"
  ) {
    event.preventDefault()
    packingListManager.CopyGroups(selectedGroupIDs.value, true)

    $q.notify({
      message: "Copied group(s)",
      color: "purple",
      icon: "done",
    })
  }
}

const pasteGroups = (event: KeyboardEvent) => {
  if (
    copiedGroups.value.length &&
    event.ctrlKey &&
    event.key.toLowerCase() === "v"
  ) {
    event.preventDefault()
    copiedGroups.value.forEach((copiedGroup) => {
      const addedGroup = packingListManager.AddGroup(
        copiedGroup.name,
        copiedGroup.type
      )
      addedGroup.Synchronize(copiedGroup)
    })
  }
}

const removeGroups = (event: KeyboardEvent) => {
  if (
    currentSectionFocus.value == PackingSectionType.Grid &&
    selectedGroupIDs.value.length > 0 &&
    event.key.toLowerCase() === "delete"
  ) {
    event.preventDefault()
    selectedGroupIDs.value.forEach((groupID) =>
      packingListManager.RemoveGroup(groupID)
    )
  }
}

const selectNextNode = (event: KeyboardEvent) => {
  if (event.altKey || selectedGroupIDs.value.length != 1) {
    return
  }

  let direction = null as NeighborDirection | null
  switch (event.key) {
    case "ArrowUp":
      event.preventDefault()
      direction = NeighborDirection.Top
      break
    case "ArrowDown":
      event.preventDefault()
      direction = NeighborDirection.Bottom
      break
    case "ArrowLeft":
      event.preventDefault()
      direction = NeighborDirection.Left
      break
    case "ArrowRight":
      event.preventDefault()
      direction = NeighborDirection.Right
      break
    default:
      return
  }

  const node = grid.value.GetNode(selectedGroupIDs.value[0])
  if (node.HasNeighbor(direction)) {
    selectedGroupIDs.value[0] = node.GetNeighbor(direction).element.id
  }
}

const moveNode = (event: KeyboardEvent) => {
  if (!event.altKey || selectedGroupIDs.value.length != 1) {
    return
  }

  const selectedGroupID = selectedGroupIDs.value[0]
  if (event.key === "ArrowUp") {
    event.preventDefault()
    grid.value.TryMoveTop(grid.value.GetNode(selectedGroupID))
  } else if (event.key === "ArrowDown") {
    event.preventDefault()
    grid.value.TryMoveBottom(grid.value.GetNode(selectedGroupID))
  } else if (event.key === "ArrowLeft") {
    event.preventDefault()
    grid.value.TryMoveLeft(grid.value.GetNode(selectedGroupID))
  } else if (event.key === "ArrowRight") {
    event.preventDefault()
    grid.value.TryMoveRight(grid.value.GetNode(selectedGroupID))
  }
}

onMounted(() => {
  window.addEventListener("keydown", selectAllGroups)
  window.addEventListener("keydown", copySelectedGroups)
  window.addEventListener("keydown", cutSelectedGroups)
  window.addEventListener("keydown", pasteGroups)
  window.addEventListener("keydown", removeGroups)
  window.addEventListener("keydown", selectNextNode)
  window.addEventListener("keydown", moveNode)
})

onUnmounted(() => {
  window.removeEventListener("keydown", selectAllGroups)
  window.removeEventListener("keydown", copySelectedGroups)
  window.removeEventListener("keydown", cutSelectedGroups)
  window.removeEventListener("keydown", pasteGroups)
  window.removeEventListener("keydown", removeGroups)
  window.removeEventListener("keydown", selectNextNode)
  window.removeEventListener("keydown", moveNode)
})
</script>
