<template>
  <q-card-section>
    <PackingItem
      v-for="item in group.items"
      :item="item"
      :groupID="group.id"
    />
  </q-card-section>
</template>

<script lang="ts" setup>
import type { PackingGroup } from "@/models/packing/list/PackingGroup"
import PackingItem from "./PackingItem.vue"

defineProps({
  group: {
    type: Object as () => PackingGroup,
    required: true,
  },
})
</script>
