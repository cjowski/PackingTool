<template>
  <q-card class="add-group-card">
    <q-card-section horizontal>
      <q-item class="col">
        <q-item-section>
          <q-item-label class="q-ml-sm q-mt-xs text-h6 text-bold">
            Add new item group
          </q-item-label>
        </q-item-section>
      </q-item>
    </q-card-section>

    <q-card-section>
      <q-item dense>
        <q-item-section>
          <q-input
            v-model="name"
            autofocus
            outlined
            label="Name"
            @keydown.enter.prevent="add"
            class="new-group-name-input"
          />
        </q-item-section>
      </q-item>
    </q-card-section>

    <q-card-section>
      <q-item dense>
        <q-item-section>
          <q-select
            outlined
            v-model="itemType"
            :options="itemTypes"
            label="Item type"
            @keydown.enter.prevent="add"
            class="new-group-type-select"
          >
            <template v-slot:option="scope">
              <q-item v-bind="scope.itemProps" class="group-item-type">
                <q-item-section>
                  <q-item-label>{{ scope.opt }}</q-item-label>
                </q-item-section>

                <q-item-section side>
                  <q-icon :name="getIconByItemType(scope.opt)" />
                </q-item-section>
              </q-item>
            </template>

            <template v-slot:selected-item="scope">
              {{ scope.opt }}
            </template>
          </q-select>
        </q-item-section>

        <q-item-section side>
          <q-icon :name="getIconByItemType(itemType)" />
        </q-item-section>
      </q-item>
    </q-card-section>

    <q-card-actions vertical>
      <q-item>
        <q-item-section>
          <q-btn size="16px" color="primary" @click="add" :disable="name == ''"
            >Add</q-btn
          >
        </q-item-section>
      </q-item>
    </q-card-actions>
  </q-card>
</template>

<script setup lang="ts">
import { ref } from "vue"
import type { PackingItemType } from "@/api/models/PackingItemType";
import { usePackingListStore } from "@/stores/packingListStore"
import getIconByItemType from "@/methods/getIconForItemType"

const { packingListManager } = usePackingListStore()

const props = defineProps({
  closeAddGroupDialog: {
    type: Function,
    required: true,
  },
})

const name = ref("")
const itemType = ref("Other" as PackingItemType)
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

const add = () => {
  if (name.value) {
    packingListManager.AddGroup(name.value, itemType.value as PackingItemType)
    name.value = ""
    itemType.value = "Other"
    props.closeAddGroupDialog()
  }
}
</script>

<style scoped lang="scss">
.add-group-card {
  min-width: 350px;
  font-family: "Ink Free";
}
.group-item-type {
  font-family: "Ink Free";
  font-size: 16px;
}
.new-group-name-input {
  :deep(.q-field__control) {
    padding: 0 10px;
    height: 50px;
  }
  :deep(.q-field__label) {
    font-size: 16px;
    top: 10px;
  }
  :deep(input) {
    padding-top: 10px;
    font-size: 18px;
  }
}
.new-group-type-select {
  :deep(.q-field__control) {
    padding: 0 10px;
  }
  :deep(.q-field__native) {
    font-size: 18px;
  }
}
</style>
