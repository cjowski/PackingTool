<template>
  <q-item dense :class="itemClass" :clickable="true">
    <q-item-section side>
      <q-checkbox
        dense
        v-model="item.Packed"
        :color="isImportant ? 'red' : 'primary'"
        :keep-color="isImportant"
      />
    </q-item-section>

    <q-item-section @click="item.Packed = !item.Packed">
      <div class="row q-gutter-xs non-selectable">
        <div class="col-11 q-ma-none">
          <span>{{ item.Name }}</span>
        </div>
        <div class="col-1 q-ma-none text-left">
          <span :style="item.Count == 1 ? 'visibility: hidden' : ''">
            {{ item.Count }}
          </span>
        </div>
      </div>
    </q-item-section>
  </q-item>
</template>

<script setup lang="ts">
import { computed } from "vue"
import type { PackingItem } from "@/models/packing/item/PackingItem"
import { PackingItemAttribute } from "@/models/packing/item/PackingItemAttribute"

const props = defineProps({
  item: {
    type: Object as () => PackingItem,
    required: true,
  },
  itemClass: {
    type: String,
    required: true,
  },
})

const isImportant = computed(() => {
  return props.item.Attributes.indexOf(PackingItemAttribute.Important) !== -1
})
</script>
