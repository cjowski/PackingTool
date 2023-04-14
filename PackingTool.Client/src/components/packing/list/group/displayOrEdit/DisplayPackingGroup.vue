<template>
  <q-card :class="cardClass">
    <PackingGroupHeader :group="group" />
    <EditPackingGroupToolbar :group="group" />

    <q-separator dark />

    <PackingItems :group="group" />
  </q-card>
</template>

<script setup lang="ts">
import PackingGroupHeader from "./header/PackingGroupHeader.vue"
import EditPackingGroupToolbar from "./toolbar/EditPackingGroupToolbar.vue"
import PackingItems from "./item/PackingItems.vue"
import type { PackingGroup } from "@/models/packing/list/PackingGroup"

defineProps({
  group: {
    type: Object as () => PackingGroup,
    required: true,
  },
  cardClass: {
    type: String,
    required: true,
  },
})
</script>
