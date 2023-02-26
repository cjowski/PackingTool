<template>
  <EditPackingItem
    v-if="edit"
    :item="item"
    :groupID="groupID"
    :selected="selected"
    :setSelectedItemID="setSelectedItemID"
    :itemClass="itemClass"
  />
  <PackPackingItem v-else-if="packing" :item="item" :itemClass="itemClass" />
  <q-item
    v-else
    dense
    :class="itemClass"
    :clickable="true"
    @click="(event: MouseEvent) => setSelectedItemID(item.id, event.ctrlKey)"
  >
    <q-btn
      v-if="isImportant && !packing && !importantSection && !edit"
      icon="priority_high"
      color="red"
      round
      :ripple="false"
      size="6px"
      class="important-mark no-pointer-events"
    ></q-btn>

    <q-item-section>
      <div class="row q-gutter-xs non-selectable">
        <div class="col-11 q-ma-none">
          <span>{{ item.name }}</span>
        </div>
        <div class="col-1 q-ma-none text-left">
          <span :style="item.count == 1 ? 'visibility: hidden' : ''">
            {{ item.count }}
          </span>
        </div>
      </div>
    </q-item-section>
  </q-item>
</template>

<script setup lang="ts">
import { computed } from "vue"
import PackPackingItem from "./PackPackingItem.vue"
import EditPackingItem from "./EditPackingItem.vue"
import type { PackingItem } from "@/models/packing/list/PackingItem"

const props = defineProps({
  item: {
    type: Object as () => PackingItem,
    required: true,
  },
  groupID: {
    type: Number,
    required: true,
  },
  edit: {
    type: Boolean,
    required: true,
  },
  packing: {
    type: Boolean,
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
  importantSection: {
    type: Boolean,
    default: false,
  },
})

const itemClass = computed(() => {
  let output = "q-pa-xs label-font shadow-transition"
  if (props.edit) output += " q-pb-none"
  if (props.item.isNew) output += " new-item-label"
  if (props.selected) output += " selected-item"
  return output
})

const isImportant = computed(() => {
  return props.item.attributes.indexOf("Important") !== -1
})
</script>

<style lang="scss" scoped>
.new-item-label {
  div {
    color: #c6ff00; //lime-13
    font-weight: bold;
  }
}
.label-font {
  // font-family: 'Comic Sans MS';
  // font-family: 'MV Boli';
  font-family: "Segoe Print";
  // font-family: 'Segoe Script';
}
.important-mark {
  position: absolute;
  left: -26px;
  top: 6px;
}
.selected-item {
  box-shadow: $selected 0px 0px 0px 3px, $selected 0px 0px 0px 100px inset;
  color: $selected-text;
}
</style>
