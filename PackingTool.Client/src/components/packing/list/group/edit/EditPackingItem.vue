<template>
  <q-item
    dense
    :class="itemClass"
    :clickable="true"
    @click="trySelectItem"
    @dblclick="editName"
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
          @keydown.esc.prevent="editingName = false"
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
            :color="isImportant ? 'red' : 'white'"
            @click="markImportance"
          />
        </div>
      </div>
    </q-item-section>
  </q-item>
</template>

<script setup lang="ts">
import { computed, ref } from "vue"
import type { PackingItem } from "@/models/packing/list/PackingItem"
import { usePackingListStore } from "@/stores/packingListStore"
import { ExistenceStatus } from "@/models/packing/list/ExistenceStatus"

const { packingListManager } = usePackingListStore()

const props = defineProps({
  item: {
    type: Object as () => PackingItem,
    required: true,
  },
  groupID: {
    type: Number,
    required: true,
  },
  selected: {
    type: Boolean,
    default: false,
  },
  setSelectedItemID: {
    type: Function,
    required: true,
  },
  autofocusAndEditName: {
    type: Boolean,
    default: false,
  },
})

const editingName = ref(props.autofocusAndEditName)
const modifiedName = ref(props.autofocusAndEditName ? props.item.name : "")
const importantAttribute = "Important"

const isImportant = computed(() => {
  return props.item.attributes.indexOf(importantAttribute) !== -1
})

const trySelectItem = (event: MouseEvent) => {
  if (!editingName.value) {
    props.setSelectedItemID(props.item.id, event.ctrlKey)
  }
}

const editName = () => {
  editingName.value = true
  modifiedName.value = props.item.name
  props.setSelectedItemID(0, false)
}

const submitModifiedName = () => {
  props.item.name = modifiedName.value
  editingName.value = false
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
  if (props.selected) output += " selected-edit-item"
  return output
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
