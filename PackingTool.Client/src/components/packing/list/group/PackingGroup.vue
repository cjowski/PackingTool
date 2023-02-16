<template>
  <EditPackingGroup
    v-if="editing"
    :group="group"
    :isSelected="isSelected"
    :cardClass="cardClass"
    :setSelectedGroupID="setSelectedGroupID"
    :setSelectedItemID="setSelectedItemID"
    :selectedItemIDs="selectedItemIDs"
    :finishEdit="finishEdit"
  />
  <q-card v-else :class="cardClass">
    <q-card-section
      horizontal
      @click="select($event)"
      @dblclick="edit"
      class="cursor-pointer"
    >
      <q-item class="col">
        <q-item-section side>
          <q-icon :name="getIconByItemType(group.Type)" />
        </q-item-section>

        <q-item-section>
          <q-item-label
            class="text-capitalize text-body1 text-bold non-selectable"
          >
            {{ group.Name }}
          </q-item-label>
        </q-item-section>
      </q-item>

      <q-card-actions @click.stop="">
        <div class="col">
          <div class="row justify-end">
            <q-btn icon="more_vert" flat dense color="blue-grey-2" padding="xs">
              <q-menu
                v-model="showMoreButtons"
                anchor="top right"
                transition-show="jump-down"
                transition-hide="jump-up"
                class="transparent no-shadow"
                :offset="[-10, 25]"
              >
                <q-list dense>
                  <q-item>
                    <q-btn
                      icon="edit"
                      @click="edit"
                      round
                      color="green"
                      size="md"
                      padding="sm"
                    />
                  </q-item>
                  <q-item v-if="!group.IsNew">
                    <q-btn
                      icon="sync"
                      @click="synchronize"
                      round
                      color="orange"
                      size="md"
                      padding="sm"
                    />
                  </q-item>
                  <q-item>
                    <q-btn
                      icon="delete"
                      @click="remove"
                      round
                      color="red-5"
                      size="md"
                      padding="sm"
                    />
                  </q-item>
                </q-list>
              </q-menu>
            </q-btn>
          </div>
        </div>
      </q-card-actions>
    </q-card-section>

    <q-separator dark />

    <q-item
      v-if="packAllItemsCheckboxVisible"
      dense
      clickable
      @click="packAllItems(!allItemsPacked)"
      style="padding-left: 20px"
      class="cursor-pointer"
    >
      <q-item-section side>
        <q-checkbox dense v-model="allItemsPacked" color="primary" keep-color />
      </q-item-section>
    </q-item>

    <q-separator v-if="packAllItemsCheckboxVisible" dark />

    <q-card-section>
      <PackingItem
        v-for="item in group.Items"
        :item="item"
        :groupID="group.ID"
        :edit="false"
        :packing="packing"
        :selected="selectedItemIDs.includes(item.ID)"
        :setSelectedItemID="setSelectedItemID"
      />
    </q-card-section>

    <q-inner-loading :showing="synchronizing">
      <q-spinner-ball size="100px" color="orange" />
    </q-inner-loading>
  </q-card>
</template>

<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref, watch } from "vue"
import { QList, useQuasar } from "quasar"
import { storeToRefs } from "pinia"
import EditPackingGroup from "./EditPackingGroup.vue"
import PackingItem from "./item/PackingItem.vue"
import { usePackingListStore } from "@/stores/packingListStore"
import { useOperationStatusStore } from "@/stores/operationStatusStore"
import { useClipboardStore } from "@/stores/clipboardStore"
import type { PackingGroup } from "@/models/packing/group/PackingGroup"
import getIconByItemType from "@/methods/getIconForItemType"
import { PackingListAction } from "@/enums/PackingListAction"
import { PackingSectionType } from "@/enums/PackingSectionType"

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
const showMoreButtons = ref(false)
const selectedItemIDs = ref([] as number[])

const packAllItemsCheckboxVisible = computed(
  () => props.packing && !editing.value && props.group.Items.length > 1
)

const allItemsPacked = computed({
  get() {
    return props.group.Items.every((item) => item.Packed)
  },
  set(value: boolean) {
    packAllItems(value)
  },
})

const packAllItems = (value: boolean) => {
  props.group.Items.forEach((item) => (item.Packed = value))
}

const select = (event: MouseEvent) => {
  props.setSelectedGroupID(!editing.value ? props.group.ID : 0, event.ctrlKey)
  selectedItemIDs.value.length = 0
}

const synchronize = async () => {
  showMoreButtons.value = false
  synchronizing.value = true
  await packingListManager.SynchronizeGroup(props.group.ID)
}

const edit = () => {
  editing.value = true
  props.setSelectedGroupID(0, false) //todo clear
  selectedItemIDs.value.length = 0
}

const finishEdit = () => {
  showMoreButtons.value = false
  editing.value = false
  selectedItemIDs.value.length = 0
}

const remove = () => {
  showMoreButtons.value = false
  packingListManager.RemoveGroup(props.group.ID)
}

const setSelectedItemID = (itemID: number, allowMultiple: boolean) => {
  props.setSelectedGroupID(0, false)
  currentSectionFocus.value = PackingSectionType.Items
  currentGroupIDFocus.value = props.group.ID

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
  let output = "shadow-transition header-font"
  if (props.isSelected && props.group.IsNew) {
    output += " new-selected-group-color"
  } else if (props.isSelected) {
    output += " selected-group-color"
  } else if (props.group.IsNew) {
    output += " new-group-color"
  }
  return output
})

watch(currentGroupIDFocus, (groupIDFocus) => {
  if (groupIDFocus != props.group.ID) {
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
      selectedItemIDs.value.length = 0
    }
  }
)

const selectAllItems = (event: KeyboardEvent) => {
  if (
    currentGroupIDFocus.value == props.group.ID &&
    currentSectionFocus.value == PackingSectionType.Items &&
    event.ctrlKey &&
    event.key.toLowerCase() === "a"
  ) {
    event.preventDefault()
    selectedItemIDs.value = props.group.Items.map((item) => item.ID)
  }
}

const copySelectedItems = (event: KeyboardEvent) => {
  if (
    currentSectionFocus.value == PackingSectionType.Items &&
    selectedItemIDs.value.length > 0 &&
    event.ctrlKey &&
    event.key.toLowerCase() === "c"
  ) {
    event.preventDefault()
    packingListManager.CopyItems(props.group.ID, selectedItemIDs.value, false)

    $q.notify({
      message: "Copied item(s)",
      color: "purple",
      icon: "done",
    })
  }
}

const cutSelectedItems = (event: KeyboardEvent) => {
  if (
    currentSectionFocus.value == PackingSectionType.Items &&
    selectedItemIDs.value.length > 0 &&
    event.ctrlKey &&
    event.key.toLowerCase() === "x"
  ) {
    event.preventDefault()
    packingListManager.CopyItems(props.group.ID, selectedItemIDs.value, true)

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
        copiedItem.Name,
        copiedItem.Count,
        copiedItem.Attributes,
        props.group.ID
      )
    })
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
        props.group.ID,
        selectedItemID
      )
      if (previousItem.ID !== undefined) {
        selectedItemIDs.value[0] = previousItem.ID
      }
      break
    case "ArrowDown":
      event.preventDefault()
      const nextItem = packingListManager.GetNextItem(
        props.group.ID,
        selectedItemID
      )
      if (nextItem.ID !== undefined) {
        selectedItemIDs.value[0] = nextItem.ID
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
      packingListManager.SortItemUp(props.group.ID, selectedItemID)
      break
    case "ArrowDown":
      event.preventDefault()
      packingListManager.SortItemDown(props.group.ID, selectedItemID)
      break
    default:
      return
  }
}

onMounted(() => {
  window.addEventListener("keydown", selectAllItems)
  window.addEventListener("keydown", copySelectedItems)
  window.addEventListener("keydown", cutSelectedItems)
  window.addEventListener("keydown", pasteItems)
  window.addEventListener("keydown", changeItemSelection)
  window.addEventListener("keydown", changeItemSort)
})

onUnmounted(() => {
  window.removeEventListener("keydown", selectAllItems)
  window.removeEventListener("keydown", copySelectedItems)
  window.removeEventListener("keydown", cutSelectedItems)
  window.removeEventListener("keydown", pasteItems)
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
