<template>
  <div>
    <q-item>
      <q-item-section>
        <q-input
          v-model="modifiedName"
          autofocus
          outlined
          class="group-name-input"
        />
      </q-item-section>
    </q-item>

    <div class="row">
      <div class="col-12">
        <q-select
          outlined
          use-input
          v-model="modifiedItemType"
          :options="foundItemTypes"
          @filter="searchItemTypes"
          class="group-type-select q-ml-md q-mr-md"
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
            <q-icon
              size="sm"
              class="q-pr-sm"
              :name="getIconByItemType(modifiedItemType)"
            />
            {{ scope.opt }}
          </template>
        </q-select>
      </div>
    </div>

    <q-item class="q-pl-md q-pr-md">
      <q-item-section>
        <q-btn color="green" padding="xs" icon="check" @click="submit" />
      </q-item-section>
      <q-item-section>
        <q-btn flat padding="xs" icon="undo" @click="cancel" />
      </q-item-section>
    </q-item>
  </div>
</template>

<script lang="ts" setup>
import { ref } from "vue"
import type { PackingItemType } from "@/api/models/PackingItemType"
import type { PackingGroup } from "@/models/packing/list/PackingGroup"
import getIconByItemType from "@/methods/getIconForItemType"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"

const { finishEditHeaderForGroup } = useOpenedPackingListStore()

const props = defineProps({
  group: {
    type: Object as () => PackingGroup,
    required: true,
  },
})

const modifiedName = ref(props.group.name)
const modifiedItemType = ref(props.group.type)
const searchingItemTypePhrase = ref(props.group.type.toString())
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

const submit = () => {
  props.group.name = modifiedName.value
  props.group.type = modifiedItemType.value
  finishEditHeaderForGroup(props.group.id)
}

const cancel = () => {
  finishEditHeaderForGroup(props.group.id)
}
</script>

<style lang="scss" scoped>
.group-name-input {
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
.group-type-select {
  :deep(.q-field__control) {
    padding: 0 10px;
  }
  :deep(.q-field__native) {
    font-size: 15px;
    font-weight: bold;
  }
}
.group-item-type {
  font-family: "Ink Free";
  font-size: 16px;
}
</style>
