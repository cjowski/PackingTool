<template>
  <q-card :class="cardClass">
    <q-item class="col q-pl-xs q-pr-none">
      <q-item-section horizontal>
        <div class="row q-gutter-xs">
          <q-select
            outlined
            hideBottomSpace
            v-model="group.Type"
            :options="itemTypes"
            class="item-type-select"
          >
            <template v-slot:option="scope">
              <q-item v-bind="scope.itemProps">
                <q-item-section side>
                  <q-icon :name="getIconByItemType(scope.opt)" />
                </q-item-section>

                <q-item-section>
                  <q-item-label>{{ scope.opt }}</q-item-label>
                </q-item-section>
              </q-item>
            </template>

            <template v-slot:selected-item="scope">
              <q-icon :name="getIconByItemType(scope.opt)" size="sm" />
            </template>
          </q-select>
          <q-input
            outlined
            v-model="group.Name"
            label="Name"
            class="group-name-input"
          ></q-input>
        </div>
      </q-item-section>
    </q-item>

    <q-separator dark />

    <q-card-section horizontal>
      <q-item class="q-pl-xs q-pr-xs">
        <div class="row q-col-gutter-xs">
          <div class="col-8">
            <q-input
              v-if="showNewItemNameInput"
              outlined
              v-model="newItemName"
              label="New item"
              autofocus
              @keydown.enter.prevent="submitNewItem"
              class="item-name-input"
            >
            </q-input>
            <!-- fake input: -->
            <q-input
              v-if="!showNewItemNameInput"
              outlined
              v-model="newItemName"
              label="New item"
            >
            </q-input>
          </div>

          <div class="col-4">
            <q-input
              type="number"
              outlined
              v-model="newItemCount"
              @keydown.enter.prevent="submitNewItem"
              class="new-item-count-input"
            ></q-input>
          </div>
        </div>
      </q-item>
    </q-card-section>

    <q-card-section class="item-list-section">
      <PackingItem
        v-for="item in group.Items"
        :item="item"
        :groupID="group.ID"
        :edit="true"
        :packing="false"
        :selected="selectedItemIDs.includes(item.ID)"
        :setSelectedItemID="setSelectedItemID"
      />
    </q-card-section>

    <q-separator />

    <q-card-section horizontal>
      <q-card-actions @click.stop="" style="width: 100%">
        <div class="col">
          <div class="row">
            <q-btn
              color="green"
              padding="xs"
              icon="check"
              class="full-width text-right"
              @click="finishEdit()"
            />
          </div>
        </div>
      </q-card-actions>
    </q-card-section>
  </q-card>
</template>

<script setup lang="ts">
import { ref } from "vue"
import PackingItem from "./item/PackingItem.vue"
import type { PackingGroup } from "@/models/packing/group/PackingGroup"
import { PackingItemType } from "@/models/packing/item/PackingItemType"
import getIconByItemType from "@/methods/getIconForItemType"
import { usePackingListStore } from "@/stores/packingListStore"
import type { PackingItemAttribute } from "@/models/packing/item/PackingItemAttribute"

const { packingListManager } = usePackingListStore()

const props = defineProps({
  group: {
    type: Object as () => PackingGroup,
    required: true,
  },
  isSelected: {
    type: Boolean,
    required: true,
  },
  cardClass: {
    type: String,
    required: true,
  },
  setSelectedGroupID: {
    type: Function,
    required: true,
  },
  setSelectedItemID: {
    type: Function,
    default: () => {},
  },
  selectedItemIDs: {
    type: Array as () => Number[],
    required: true,
  },
  finishEdit: {
    type: Function,
    required: true,
  },
})

const itemTypes = Object.values(PackingItemType)
const showNewItemNameInput = ref(true)
const newItemName = ref("")
const newItemCount = ref(1)

const submitNewItem = () => {
  if (!newItemName.value) return
  packingListManager.AddItem(
    newItemName.value,
    newItemCount.value,
    [] as PackingItemAttribute[],
    props.group.ID
  )
  newItemName.value = ""
  newItemCount.value = 1
  //hack to not loose focus on input:
  showNewItemNameInput.value = false
  setTimeout(() => {
    showNewItemNameInput.value = true
  }, 10)
}
</script>

<style scoped lang="scss">
.group-name-input {
  :deep(.q-field__control) {
    padding: 0 6px;
    height: 56px;
  }
  :deep(.q-field__label) {
    font-size: 14px;
    top: 10px;
  }
  :deep(input) {
    padding-top: 12px;
    font-size: 18px;
  }

  width: calc(100% - 60px);
}
.item-name-input {
  :deep(.q-field__control) {
    padding: 0 6px;
    height: 50px;
  }
  :deep(.q-field__label) {
    font-size: 14px;
    top: 10px;
  }
  :deep(input) {
    padding-top: 10px;
    font-size: 16px;
  }
}
.new-item-count-input {
  :deep(.q-field__control) {
    padding: 0 4px 0 8px;
    height: 50px;
  }
  :deep(input) {
    padding-top: 10px;
    font-size: 16px;
  }
  font-family: "Segoe Print";
}
.item-type-select {
  :deep(.q-field__marginal) {
    display: none;
  }
  width: 48px;
}
.item-list-section {
  padding: 0 0 0 6px;
}
</style>
