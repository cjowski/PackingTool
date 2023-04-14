<template>
  <q-card :class="cardClass">
    <PackingGroupHeader :group="group" />

    <q-separator dark />

    <PackingItems :group="group" />

    <q-inner-loading :showing="synchronizing">
      <q-spinner-ball size="100px" color="orange" />
    </q-inner-loading>
  </q-card>
</template>

<script setup lang="ts">
import { ref } from "vue"
import PackingGroupHeader from "./header/PackingGroupHeader.vue"
import PackingItems from "./item/PackingItems.vue"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import type { PackingGroup } from "@/models/packing/list/PackingGroup"

const { packingListManager } = useAllPackingListsStore()
const { setSelectedGroupID } = useOpenedPackingListStore()

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

const synchronizing = ref(false)
const showMoreButtons = ref(false)

const select = (event: MouseEvent) => {
  setSelectedGroupID(props.group.id, event.ctrlKey)
}

const synchronize = async () => {
  showMoreButtons.value = false
  synchronizing.value = true
  await packingListManager.SynchronizeGroup(props.group.id)
  synchronizing.value = false
}

const remove = () => {
  showMoreButtons.value = false
  packingListManager.RemoveGroup(props.group.id)
}
</script>
