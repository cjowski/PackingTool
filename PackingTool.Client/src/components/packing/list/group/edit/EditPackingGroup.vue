<template>
  <q-card :class="cardClass">
    <q-item class="col q-pl-xs q-pr-none">
      <q-item-section horizontal>
        <div class="row q-gutter-xs">
          <q-select
            outlined
            hideBottomSpace
            v-model="group.type"
            :options="itemTypes"
            class="item-type-select"
          >
            <template v-slot:option="scope">
              <q-item v-bind="scope.itemProps">
                <q-item-section side>
                  <q-icon :name="getIconByItemType(scope.opt)" />
                </q-item-section>

                <q-item-section>
                  <q-item-label class="item-type-dropdown-label">{{
                    scope.opt
                  }}</q-item-label>
                </q-item-section>
              </q-item>
            </template>

            <template v-slot:selected-item="scope">
              <q-icon :name="getIconByItemType(scope.opt)" size="sm" />
            </template>
          </q-select>
          <q-input
            outlined
            v-model="group.name"
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
      <EditPackingItem
        v-for="item in group.items"
        :item="item"
        :groupID="group.id"
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
              @click="finishEditGroup(group.id)"
            />
          </div>
        </div>
      </q-card-actions>
    </q-card-section>
  </q-card>
</template>

<script setup lang="ts">
import { ref } from "vue"
import EditPackingItem from "./EditPackingItem.vue"
import type { PackingItemType } from "src/api/models/PackingItemType"
import type { PackingItemAttribute } from "@/api/models/PackingItemAttribute"
import type { PackingGroup } from "@/models/packing/list/PackingGroup"
import getIconByItemType from "@/methods/getIconForItemType"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"

const { packingListManager } = useAllPackingListsStore()
const { finishEditGroup } = useOpenedPackingListStore()

const props = defineProps({
  group: {
    type: Object as () => PackingGroup,
    required: true,
  },
  cardClass: {
    type: String,
    required: true,
  },
})

const itemTypes = [
  "Cloth",
  "Electronics",
  "Food",
  "Documents",
  "Shoes",
  "Cosmetics",
  "Medicaments",
  "Other",
] as PackingItemType[] //TODO make it automated
const showNewItemNameInput = ref(true)
const newItemName = ref("")
const newItemCount = ref(1)

const submitNewItem = () => {
  if (!newItemName.value) return
  packingListManager.AddItem(
    newItemName.value,
    newItemCount.value,
    [] as PackingItemAttribute[],
    props.group.id
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
.item-type-dropdown-label {
  font-family: "Ink Free";
  font-size: 16px;
}
</style>
