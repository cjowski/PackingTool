<template>
  <q-card-section
    horizontal
    @click="select($event)"
    @dblclick="editHeaderForGroup(group.id)"
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

      <q-item-section side style="padding-left: 0px" @click.stop="">
        <q-btn
          v-if="!editing"
          flat
          dense
          icon="edit"
          @click="editGroup(group.id)"
        />

        <q-btn
          v-else
          flat
          dense
          icon="edit_off"
          @click="finishEditGroup(group.id)"
        />
      </q-item-section>

      <q-item-section side style="padding-left: 0px" @click.stop="">
        <q-btn flat dense icon="close" @click="remove" />
      </q-item-section>
    </q-item>
  </q-card-section>
</template>

<script lang="ts" setup>
import { computed } from "vue"
import { storeToRefs } from "pinia"
import type { PackingGroup } from "@/models/packing/list/PackingGroup"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import getIconByItemType from "@/methods/getIconForItemType"

const { packingListManager } = useAllPackingListsStore()
const { setSelectedGroupID, editGroup, editHeaderForGroup, finishEditGroup } =
  useOpenedPackingListStore()
const { editingGroupIDs } = storeToRefs(useOpenedPackingListStore())

const props = defineProps({
  group: {
    type: Object as () => PackingGroup,
    required: true,
  },
})

const editing = computed(
  () => editingGroupIDs.value.indexOf(props.group.id) !== -1
)

const select = (event: MouseEvent) => {
  setSelectedGroupID(props.group.id, event.ctrlKey)
}

const remove = () => {
  finishEditGroup(props.group.id)
  packingListManager.RemoveGroup(props.group.id)
}
</script>

<style lang="scss" scoped>
.display-group-name {
  font-size: 18px;
  font-weight: bold;
}
</style>
