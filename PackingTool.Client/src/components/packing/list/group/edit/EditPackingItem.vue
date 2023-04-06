<template>
  <q-item
    dense
    :class="itemClass"
    :clickable="true"
    @click="trySelectItem"
    @dblclick="editItemName(groupID, item.id)"
  >
    <q-item-section>
      <div v-if="!editingName" class="non-selectable new-item-label">
        {{ item.name }}
      </div>
      <div v-else class="column">
        <q-input
          v-model="modifiedName"
          outlined
          dense
          autofocus
          @keydown.enter.prevent="submitModifiedName"
          @keydown.esc.prevent="cancelEditingName"
          @focusout="submitModifiedName"
          class="item-name-input"
        ></q-input>
      </div>
    </q-item-section>

    <q-item-section v-if="!editingName" avatar>
      <div class="row q-gutter-xs q-ma-none q-pa-none">
        <div class="column">
          <q-input
            v-model="item.count"
            type="number"
            outlined
            dense
            class="item-count-input"
          ></q-input>
        </div>
        <div class="column">
          <q-btn
            flat
            dense
            icon="close"
            size="10px"
            @click="packingListManager.RemoveItem(item.id, groupID)"
          />
          <q-btn
            flat
            dense
            icon="priority_high"
            size="10px"
            :color="important ? 'red' : 'white'"
            @click="markImportance"
          />
        </div>
      </div>
    </q-item-section>
  </q-item>
</template>

<script setup lang="ts">
import { computed, ref, watch } from "vue"
import { storeToRefs } from "pinia"
import type { PackingItem } from "@/models/packing/list/PackingItem"
import { ExistenceStatus } from "@/models/packing/list/ExistenceStatus"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"

const { packingListManager } = useAllPackingListsStore()
const { setSelectedItemID, editItemName } = useOpenedPackingListStore()
const { selectedItemIDs, editingNameForItemID } = storeToRefs(
  useOpenedPackingListStore()
)

const props = defineProps({
  item: {
    type: Object as () => PackingItem,
    required: true,
  },
  groupID: {
    type: Number,
    required: true,
  },
})

const editingName = computed(() => editingNameForItemID.value == props.item.id)

const modifiedName = ref(editingName.value ? props.item.name : "")
const importantAttribute = "Important"

const selected = computed(
  () => selectedItemIDs.value.indexOf(props.item.id) !== -1
)

const important = computed(() => {
  return props.item.attributes.indexOf(importantAttribute) !== -1
})

const trySelectItem = (event: MouseEvent) => {
  if (!editingName.value) {
    setSelectedItemID(props.groupID, props.item.id, event.ctrlKey)
  }
}

const submitModifiedName = () => {
  props.item.name = modifiedName.value
  editingNameForItemID.value = 0
}

const cancelEditingName = () => {
  modifiedName.value = props.item.name
  editingNameForItemID.value = 0
}

const markImportance = () => {
  if (props.item.attributes.indexOf(importantAttribute) === -1) {
    props.item.attributes.push(importantAttribute)
  } else {
    props.item.attributes.splice(
      props.item.attributes.findIndex(
        (attribute) => attribute === importantAttribute
      ),
      1
    )
  }
}

const itemClass = computed(() => {
  let output = "q-pa-xs q-pb-none edit-item-font shadow-transition"
  if (props.item.status == ExistenceStatus.New) output += " new-edit-item-label"
  if (selected.value) output += " selected-edit-item"
  return output
})

watch(editingNameForItemID, (itemID) => {
  if (itemID == props.item.id) {
    modifiedName.value = props.item.name
  }
})
</script>

<style lang="scss" scoped>
.new-edit-item-label {
  div {
    color: #c6ff00; //lime-13
    font-weight: bold;
  }
}
.edit-item-font {
  font-family: "Segoe Print";
  font-weight: bold;
}
.selected-edit-item {
  box-shadow: $selected 0px 0px 0px 3px, $selected 0px 0px 0px 100px inset;
  color: $selected-text;
}
.item-count-input {
  :deep(.q-field__control) {
    padding: 0 4px 0 8px;
    height: 40px;
  }
  :deep(input) {
    padding-top: 10px;
    font-weight: bold;
  }
  width: 45px;
}
.item-name-input {
  :deep(.q-field__control) {
    padding: 0 4px 0 8px;
    height: 40px;
  }
  :deep(input) {
    padding-top: 6px;
  }
}
</style>
