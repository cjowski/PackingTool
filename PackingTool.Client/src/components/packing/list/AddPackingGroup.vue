<template>
  <q-card class="add-group-card q-mt-xl">
    <q-card-section horizontal :class="$q.screen.lt.md ? 'q-mt-md' : ''">
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
      <q-select
        options-dense
        outlined
        label="Group type"
        v-model="itemType"
        :options="foundItemTypes"
        @filter="searchItemTypes"
        @keydown.enter.prevent="add"
        class="new-group-type-select q-ml-md q-mr-md"
      >
        <template v-slot:option="scope">
          <q-item v-bind="scope.itemProps" class="group-item-type">
            <q-item-section side>
              <q-icon :name="getIconByItemType(scope.opt)" />
            </q-item-section>
            <q-item-section>
              <q-item-label class="text-bold">{{ scope.opt }}</q-item-label>
            </q-item-section>
          </q-item>
        </template>

        <template v-slot:selected-item="scope">
          <q-icon
            size="sm"
            class="q-pr-sm"
            :name="getIconByItemType(itemType)"
          />
          {{ scope.opt }}
        </template>
      </q-select>
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
import type { PackingItemType } from "@/api/models/PackingItemType"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import getIconByItemType from "@/methods/getIconForItemType"

const { packingListManager } = useAllPackingListsStore()
const { editGroup, addItemsForGroup } = useOpenedPackingListStore()

const props = defineProps({
  closeAddGroupDialog: {
    type: Function,
    required: true,
  },
})

const name = ref("")
const itemType = ref("Other" as PackingItemType)
const searchingItemTypePhrase = ref("")
const foundItemTypes = ref([] as PackingItemType[])

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

const searchItemTypes = (
  text: string,
  doneFn: (callbackFn: () => void) => void
) => {
  searchingItemTypePhrase.value = text
  doneFn(() => {
    if (searchingItemTypePhrase.value) {
      foundItemTypes.value = itemTypes.filter((itemType) =>
        itemType
          .toLowerCase()
          .startsWith(searchingItemTypePhrase.value.toLowerCase())
      )
    } else {
      foundItemTypes.value = itemTypes
    }
  })
}

const add = () => {
  if (name.value) {
    const group = packingListManager.AddGroup(
      name.value,
      itemType.value as PackingItemType
    )
    addItemsForGroup(group.id)
    editGroup(group.id)

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
    font-weight: bold;
  }
  :deep(input) {
    padding-top: 10px;
    font-size: 18px;
    font-weight: bold;
  }
  font-family: "Ink Free";
}
.new-group-type-select {
  :deep(.q-field__control) {
    padding: 0 10px;
  }
  :deep(.q-field__native) {
    font-size: 18px;
    font-weight: bold;
  }
}
</style>
