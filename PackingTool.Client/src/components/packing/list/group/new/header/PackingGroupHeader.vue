<template>
  <div>
    <DisplayPackingGroupHeader v-if="!editing" :group="group" />
    <EditPackingGroupHeader v-else :group="group" />
  </div>
</template>

<script lang="ts" setup>
import { computed } from "vue"
import { storeToRefs } from "pinia"
import DisplayPackingGroupHeader from "./DisplayPackingGroupHeader.vue"
import EditPackingGroupHeader from "./EditPackingGroupHeader.vue"
import type { PackingGroup } from "@/models/packing/list/PackingGroup"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"

const { editingHeaderGroupIDs } = storeToRefs(useOpenedPackingListStore())

const props = defineProps({
  group: {
    type: Object as () => PackingGroup,
    required: true,
  },
})

const editing = computed(
  () => editingHeaderGroupIDs.value.indexOf(props.group.id) !== -1
)
</script>
