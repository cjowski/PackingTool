<template>
  <q-item dense :class="itemClass" clickable>
    <q-item-section side>
      <q-checkbox
        dense
        v-model="item.packed"
        :color="checkboxColor"
        :keep-color="important || toBuy"
      />
    </q-item-section>

    <q-item-section @click="item.packed = !item.packed">
      {{ item.name }}
    </q-item-section>

    <q-item-section v-if="item.count > 1" side>
      <q-badge rounded outline color="indigo-2" :label="item.count" />
    </q-item-section>
  </q-item>
</template>

<script setup lang="ts">
import { computed } from "vue"
import type { PackingItem } from "@/models/packing/list/PackingItem"
import { ExistenceStatus } from "@/models/packing/list/ExistenceStatus"

const props = defineProps({
  item: {
    type: Object as () => PackingItem,
    required: true,
  },
})

const important = computed(
  () => props.item.attributes.indexOf("Important") !== -1
)
const toBuy = computed(() => props.item.attributes.indexOf("ToBuy") !== -1)

const itemClass = computed(() => {
  let output = "q-pa-xs q-pb-none pack-item-font shadow-transition"
  if (props.item.status == ExistenceStatus.New) output += " new-pack-item-label"
  return output
})

const checkboxColor = computed(() => {
  if (important.value) {
    return "red"
  } else if (toBuy.value) {
    return "amber-9"
  }
  return "primary"
})
</script>

<style lang="scss" scoped>
.new-pack-item-label {
  div {
    color: #c6ff00; //lime-13
    font-weight: bold;
  }
}
.pack-item-font {
  font-family: "Segoe Print";
}
</style>
