<template>
  <q-item
    dense
    :class="itemClass"
    :clickable="true"
    @click="trySelectItem"
    @dblclick="editItemName(groupID, item.id)"
  >
    <q-btn
      icon="priority_high"
      :color="important ? 'red' : 'grey'"
      round
      dense
      size="10px"
      class="edit-item-importance-mark"
      @click="markImportance"
      @click.stop=""
    ></q-btn>
    <q-btn
      icon="shopping_cart"
      :color="forShopping ? 'primary' : 'grey'"
      round
      dense
      size="10px"
      class="edit-item-shopping-mark"
      @click="markForShopping"
      @click.stop=""
    ></q-btn>

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
        <div class="column" @click.stop="">
          <q-btn
            flat
            dense
            icon="close"
            size="10px"
            @click="packingListManager.RemoveItem(item.id, groupID)"
          />
          <q-btn flat dense icon="more_vert" size="10px">
            <q-menu
              v-model="editingAttributes"
              anchor="top right"
              transition-show="jump-down"
              transition-hide="jump-up"
              class="transparent no-shadow"
              :offset="[-10, 0]"
            >
              <q-list dense>
                <q-item>
                  <q-btn
                    icon="priority_high"
                    round
                    :color="important ? 'red' : 'grey'"
                    size="md"
                    padding="sm"
                    @click="markImportance"
                  />
                </q-item>
                <q-item>
                  <q-btn
                    icon="shopping_cart"
                    round
                    :color="forShopping ? 'primary' : 'grey'"
                    size="md"
                    padding="sm"
                    @click="markForShopping"
                  />
                </q-item>
              </q-list>
            </q-menu>
          </q-btn>
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
const editingAttributes = ref(false)

const selected = computed(
  () => selectedItemIDs.value.indexOf(props.item.id) !== -1
)

const important = computed(() => {
  return props.item.attributes.indexOf("Important") !== -1
})

const forShopping = computed(() => {
  return (
    props.item.attributes.indexOf("ToBuy") !== -1 ||
    props.item.attributes.indexOf("Bought") !== -1
  )
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
  if (props.item.attributes.indexOf("Important") === -1) {
    props.item.attributes.push("Important")
  } else {
    props.item.RemoveAttribute("Important")
  }
}

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
.edit-item-importance-mark {
  position: absolute;
  left: -30px;
  top: 5px;
}
.edit-item-shopping-mark {
  position: absolute;
  left: -30px;
  top: 30px;
}
</style>
