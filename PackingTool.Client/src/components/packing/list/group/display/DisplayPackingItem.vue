<template>
  <q-item
    dense
    :class="itemClass"
    :clickable="true"
    @click="(event: MouseEvent) => setSelectedItemID(item.id, event.ctrlKey)"
    @dblclick="setEditItemName(item.id)"
  >
    <q-btn
      v-if="isImportant"
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
import type { PackingItem } from "@/models/packing/list/PackingItem"
import { ExistenceStatus } from "@/models/packing/list/ExistenceStatus"

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
  setEditItemName: {
    type: Function,
    default: () => {},
  },
})

const itemClass = computed(() => {
  let output = "q-pa-xs label-font shadow-transition"
  if (props.item.status == ExistenceStatus.New) output += " new-item-label"
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
  font-family: "Segoe Print";
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
