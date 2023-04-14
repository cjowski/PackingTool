<template>
  <q-card-section
    horizontal
    @click="select($event)"
    @dblclick="setEditingHeader()"
    class="cursor-pointer"
  >
    <q-item class="col">
      <q-item-section side>
        <q-icon :name="getIconByItemType(group.type)" />
      </q-item-section>

      <q-item-section>
        <q-item-label class="display-group-name non-selectable">
          {{ group.name }}
        </q-item-label>
      </q-item-section>
    </q-item>
  </q-card-section>
</template>

<script lang="ts" setup>
import type { PackingGroup } from "@/models/packing/list/PackingGroup"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import getIconByItemType from "@/methods/getIconForItemType"

const { setSelectedGroupID } = useOpenedPackingListStore()

const props = defineProps({
  group: {
    type: Object as () => PackingGroup,
    required: true,
  },
  setEditingHeader: {
    type: Function,
    required: true,
  },
})

const select = (event: MouseEvent) => {
  setSelectedGroupID(props.group.id, event.ctrlKey)
}
</script>

<style lang="scss" scoped>
.display-group-name {
  font-size: 18px;
  font-weight: bold;
}
</style>
