<template>
  <q-item
    dense
    :class="itemClass"
    :clickable="true"
    @click="(event: MouseEvent) => setSelectedItemID(item.id, event.ctrlKey)"
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
          @focusout="editingName = false"
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
    default: () => {},
  },
  itemClass: {
    type: String,
    required: true,
  },
})

const editingName = ref(false)
const modifiedName = ref("")

const isImportant = computed(() => {
  return props.item.attributes.indexOf("Important") !== -1
})

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
  if (props.item.attributes.indexOf("Important") === -1) {
    props.item.attributes.push("Important")
  } else {
    props.item.attributes.splice(
      props.item.attributes.findIndex(
        (attribute) => attribute === "Important"
      ),
      1
    )
  }
}
</script>

<style lang="scss" scoped>
.new-item-label {
  div {
    color: #c6ff00; //lime-13
    font-weight: bold;
  }
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
