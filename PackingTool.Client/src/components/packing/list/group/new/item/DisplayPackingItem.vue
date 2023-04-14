<template>
  <q-item
    dense
    :class="itemClass"
    :clickable="true"
    @click="(event: MouseEvent) => setSelectedItemID(groupID, item.id, event.ctrlKey)"
    @dblclick="setEditing()"
  >
    <q-btn
      v-if="!editing && isImportant && highlightImportantItems"
      icon="priority_high"
      color="red"
      round
      :ripple="false"
      size="8px"
      class="item-attribute-mark no-pointer-events"
    ></q-btn>
    <q-btn
      v-if="!editing && isToBuy && highlightShoppingItems"
      icon="shopping_cart"
      color="primary"
      round
      :ripple="false"
      size="8px"
      class="item-attribute-mark no-pointer-events"
    ></q-btn>
    <q-btn
      v-if="!editing && isBought && highlightShoppingItems"
      icon="done"
      color="green"
      round
      :ripple="false"
      size="8px"
      class="item-attribute-mark no-pointer-events"
    ></q-btn>

    <q-item-section
      v-if="editing && !editingAttributes"
      side
      style="padding: 0px; padding-right: 8px"
      @click.stop=""
    >
      <q-btn
        flat
        dense
        icon="edit"
        :color="selected ? 'blue-grey-10' : ''"
        @click="setEditing()"
      />
    </q-item-section>

    <q-item-section>
      <div class="row q-gutter-xs non-selectable">
        {{ item.name }}
      </div>
    </q-item-section>

    <q-item-section v-if="item.count > 1 && !editingAttributes" side>
      <q-badge
        rounded
        outline
        :color="selected ? 'blue-grey-10' : 'indigo-2'"
        :label="item.count"
      />
    </q-item-section>

    <q-item-section
      v-if="editing && !editingAttributes"
      side
      style="padding-left: 4px"
      @click.stop=""
    >
      <q-btn
        flat
        dense
        icon="close"
        :color="selected ? 'blue-grey-10' : ''"
        @click="packingListManager.RemoveItem(item.id, groupID)"
      />
    </q-item-section>

    <q-item-section v-if="editingAttributes" side @click.stop="">
      <q-btn
        round
        dense
        size="sm"
        icon="shopping_cart"
        :color="isToBuy || isBought ? 'blue' : 'grey'"
        @click="markForShopping"
      />
    </q-item-section>

    <q-item-section
      v-if="editingAttributes"
      side
      @click.stop=""
      style="padding-left: 8px"
    >
      <q-btn
        round
        dense
        size="sm"
        icon="priority_high"
        :color="isImportant ? 'red-5' : 'grey'"
        @click="markImportance"
      />
    </q-item-section>
  </q-item>
</template>

<script setup lang="ts">
import { computed } from "vue"
import { storeToRefs } from "pinia"
import type { PackingItem } from "@/models/packing/list/PackingItem"
import { ExistenceStatus } from "@/models/packing/list/ExistenceStatus"
import { useOperationStatusStore } from "@/stores/operationStatusStore"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"

const { currentGroupIDFocus } = storeToRefs(useOperationStatusStore())
const { packingListManager } = useAllPackingListsStore()
const { setSelectedItemID } = useOpenedPackingListStore()
const {
  selectedItemIDs,
  editingGroupIDs,
  editingAttributesGroupIDs,
  highlightImportantItems,
  highlightShoppingItems,
} = storeToRefs(useOpenedPackingListStore())

const props = defineProps({
  item: {
    type: Object as () => PackingItem,
    required: true,
  },
  groupID: {
    type: Number,
    required: true,
  },
  setEditing: {
    type: Function,
    required: true,
  },
})

const selected = computed(
  () =>
    selectedItemIDs.value.indexOf(props.item.id) !== -1 &&
    currentGroupIDFocus.value == props.groupID
)

const editing = computed(
  () => editingGroupIDs.value.indexOf(props.groupID) !== -1
)

const editingAttributes = computed(
  () => editingAttributesGroupIDs.value.indexOf(props.groupID) !== -1
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

const markForShopping = () => {
  if (
    props.item.attributes.indexOf("ToBuy") === -1 &&
    props.item.attributes.indexOf("Bought") === -1
  ) {
    props.item.attributes.push("ToBuy")
  } else if (props.item.attributes.indexOf("ToBuy") !== -1) {
    props.item.RemoveAttribute("ToBuy")
  } else if (props.item.attributes.indexOf("Bought") !== -1) {
    props.item.RemoveAttribute("Bought")
  }
}

const markImportance = () => {
  if (props.item.attributes.indexOf("Important") === -1) {
    props.item.attributes.push("Important")
  } else {
    props.item.RemoveAttribute("Important")
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
