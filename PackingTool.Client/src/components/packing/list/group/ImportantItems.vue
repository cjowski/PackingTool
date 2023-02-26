<template>
  <q-card v-if="importantItems.length > 0">
    <q-card-section horizontal>
      <q-item class="col">
        <q-item-section side>
          <q-icon name="priority_high" color="red" />
        </q-item-section>

        <q-item-section>
          <q-item-label
            class="text-uppercase text-body1 text-bold non-selectable header-font"
          >
            IMPORTANT
          </q-item-label>
        </q-item-section>
      </q-item>

      <q-card-actions>
        <div class="col">
          <div class="row justify-end">
            <q-btn
              icon="more_vert"
              flat
              dense
              color="blue-grey-2"
              padding="xs"
            />
          </div>
        </div>
      </q-card-actions>
    </q-card-section>

    <q-separator dark />

    <q-item
      v-if="packAllItemsCheckboxVisible"
      dense
      clickable
      @click="packAllItems(!allItemsPacked)"
      style="padding-left: 20px"
      class="cursor-pointer"
    >
      <q-item-section side>
        <q-checkbox dense v-model="allItemsPacked" color="red" keep-color />
      </q-item-section>
    </q-item>

    <q-separator v-if="packAllItemsCheckboxVisible" dark />

    <q-card-section>
      <q-list>
        <PackingItem
          v-for="item in importantItems"
          :item="item"
          :groupID="0"
          :edit="false"
          :packing="packing"
          :importantSection="true"
        />
      </q-list>
    </q-card-section>
  </q-card>
</template>

<script setup lang="ts">
import { computed } from "vue"
import { storeToRefs } from "pinia"
import PackingItem from "./item/PackingItem.vue"
import { usePackingListStore } from "@/stores/packingListStore"

const { importantItems } = storeToRefs(usePackingListStore())

const props = defineProps({
  packing: {
    type: Boolean,
    required: true,
  },
})

const packAllItemsCheckboxVisible = computed(
  () => props.packing && importantItems.value.length > 1
)

const allItemsPacked = computed({
  get() {
    return importantItems.value.every((item) => item.packed)
  },
  set(value: boolean) {
    packAllItems(value)
  },
})

const packAllItems = (value: boolean) => {
  importantItems.value.forEach((item) => (item.packed = value))
}
</script>

<style scoped lang="scss">
.header-font {
  font-family: "Ink Free";
}
</style>
