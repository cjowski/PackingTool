<template>
  <q-card :class="cardClass">
    <q-card-section horizontal class="cursor-pointer">
      <q-item class="col">
        <q-item-section side>
          <q-icon :name="getIconByItemType(group.type)" />
        </q-item-section>

        <q-item-section>
          <q-item-label
            class="text-capitalize text-body1 text-bold non-selectable"
          >
            {{ group.name }}
          </q-item-label>
        </q-item-section>
      </q-item>
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
        <q-checkbox dense v-model="allItemsPacked" color="primary" keep-color />
      </q-item-section>
    </q-item>

    <q-separator v-if="packAllItemsCheckboxVisible" dark />

    <q-card-section>
      <PackPackingItem v-for="item in group.items" :item="item" />
    </q-card-section>
  </q-card>
</template>

<script setup lang="ts">
import { computed } from "vue"
import PackPackingItem from "./PackPackingItem.vue"
import type { PackingGroup } from "@/models/packing/list/PackingGroup"
import getIconByItemType from "@/methods/getIconForItemType"

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

const packAllItemsCheckboxVisible = computed(() => props.group.items.length > 1)

const allItemsPacked = computed({
  get() {
    return props.group.items.every((item) => item.packed)
  },
  set(value: boolean) {
    packAllItems(value)
  },
})

const packAllItems = (value: boolean) => {
  props.group.items.forEach((item) => (item.packed = value))
}
</script>
