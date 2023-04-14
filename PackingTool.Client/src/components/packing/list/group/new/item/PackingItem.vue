<template>
  <DisplayPackingItem
    v-if="!editing"
    :item="item"
    :groupID="groupID"
    :setEditing="
      () => {
        editing = true
      }
    "
  />
  <EditPackingItem
    v-else
    :item="item"
    :groupID="groupID"
    :onClosing="
      () => {
        editing = false
      }
    "
  />
</template>

<script lang="ts" setup>
import { ref } from "vue"
import type { PackingItem } from "@/models/packing/list/PackingItem"
import DisplayPackingItem from "./DisplayPackingItem.vue"
import EditPackingItem from "./EditPackingItem.vue"

const props = defineProps({
  item: {
    type: Object as () => PackingItem,
    required: true,
  },
  groupID: {
    type: Number,
    required: true,
  },
})

const editing = ref(false)
</script>
