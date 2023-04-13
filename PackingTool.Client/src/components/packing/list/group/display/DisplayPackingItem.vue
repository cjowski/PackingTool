<template>
  <q-item
    dense
    :class="itemClass"
    :clickable="true"
    @click="(event: MouseEvent) => setSelectedItemID(groupID, item.id, event.ctrlKey)"
    @dblclick="editItemName(groupID, item.id)"
  >
    <q-btn
      v-if="isImportant && highlightImportantItems"
      icon="priority_high"
      color="red"
      round
      :ripple="false"
      size="8px"
      class="item-attribute-mark no-pointer-events"
    ></q-btn>
    <q-btn
      v-if="isToBuy && highlightShoppingItems"
      icon="shopping_cart"
      color="primary"
      round
      :ripple="false"
      size="8px"
      class="item-attribute-mark no-pointer-events"
    ></q-btn>
    <q-btn
      v-if="isBought && highlightShoppingItems"
      icon="done"
      color="green"
      round
      :ripple="false"
      size="8px"
      class="item-attribute-mark no-pointer-events"
    ></q-btn>

    <q-item-section>
      <div class="row q-gutter-xs non-selectable">
        {{ item.name }}
      </div>
    </q-item-section>

    <q-item-section v-if="item.count > 1" side>
      <q-badge rounded outline :color="selected ? 'blue-grey-10' : 'indigo-2'" :label="item.count" />
    </q-item-section>
  </q-item>
</template>

<script setup lang="ts">
import { computed } from "vue"
import { storeToRefs } from "pinia"
import type { PackingItem } from "@/models/packing/list/PackingItem"
import { ExistenceStatus } from "@/models/packing/list/ExistenceStatus"
import { useOperationStatusStore } from "@/stores/operationStatusStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"

const { currentGroupIDFocus } = storeToRefs(useOperationStatusStore())
const { setSelectedItemID, editItemName } = useOpenedPackingListStore()
const { selectedItemIDs, highlightImportantItems, highlightShoppingItems } =
  storeToRefs(useOpenedPackingListStore())

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

const selected = computed(
  () =>
    selectedItemIDs.value.indexOf(props.item.id) !== -1 &&
    currentGroupIDFocus.value == props.groupID
)

const itemClass = computed(() => {
  let output = "q-pa-xs label-font shadow-transition"
  if (props.item.status == ExistenceStatus.New) output += " new-item-label"
  if (selected.value) output += " selected-item"
  return output
})

const isImportant = computed(
  () => props.item.attributes.indexOf("Important") !== -1
)
const isToBuy = computed(() => props.item.attributes.indexOf("ToBuy") !== -1)
const isBought = computed(() => props.item.attributes.indexOf("Bought") !== -1)
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
.item-attribute-mark {
  position: absolute;
  left: -30px;
  top: 3px;
}
.selected-item {
  box-shadow: $selected 0px 0px 0px 3px, $selected 0px 0px 0px 100px inset;
  color: $selected-text;
}
</style>
