<template>
  <q-item dense :class="itemClass" :clickable="true">
    <q-item-section side>
      <q-checkbox
        dense
        v-model="item.packed"
        :color="isImportant ? 'red' : 'primary'"
        :keep-color="isImportant"
      />
    </q-item-section>

    <q-item-section @click="item.packed = !item.packed">
      <div class="row q-gutter-xs non-selectable">
        <div class="col-11 q-ma-none">
          <span>{{ item.name }}</span>
        </div>
        <div class="col-1 q-ma-none text-left">
          <span :style="item.count == 1 ? 'visibility: hidden' : ''">
            {{ item.count }}
          </span>
        </div>
      </div>
    </q-item-section>
  </q-item>
</template>

<script setup lang="ts">
import { computed } from "vue"
import type { PackingItem } from "@/models/packing/list/PackingItem"

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
  return props.item.attributes.indexOf("Important") !== -1
})
</script>
