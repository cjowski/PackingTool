<template>
  <div>
    <DisplayPackingGroupHeader
      v-if="!editingHeader"
      :group="group"
      :setEditingHeader="
        () => {
          editingHeader = true
        }
      "
    />
    <EditPackingGroupHeader
      v-else
      :group="group"
      :onClosing="
        () => {
          editingHeader = false
        }
      "
    />
  </div>
</template>

<script lang="ts" setup>
import { ref } from "vue"
import DisplayPackingGroupHeader from "./DisplayPackingGroupHeader.vue"
import EditPackingGroupHeader from "./EditPackingGroupHeader.vue"
import type { PackingGroup } from "@/models/packing/list/PackingGroup"

defineProps({
  group: {
    type: Object as () => PackingGroup,
    required: true,
  },
})

const editingHeader = ref(false)
</script>
